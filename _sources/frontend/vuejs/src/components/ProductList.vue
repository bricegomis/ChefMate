<template>
  <div class="q-pa-md">
    <q-table
      class="my-sticky-header-column-table"
      :columns="allColumns"
      :rows="filteredProducts"
      row-key="id"
      separator="cell"
      flat
      bordered
      dense
      title="Produits"
      :rows-per-page-options="[20, 50, 100]"
    >
      <template v-slot:top>
        <div class="row">
          <div class="col q-table__title">Products</div>
          <div class="col-auto">
            <q-input
              borderless
              dense
              debounce="300"
              color="primary"
              v-model="filter"
            >
              <template v-slot:append>
                <q-icon name="search" />
              </template>
            </q-input>
          </div>
        </div>
        <div class="row">
          <q-expansion-item expand-separator icon="filter_list" label="Filters">
            <q-toggle
              v-model="selectAllTypes"
              label="Select All"
            />
            <q-toggle
              v-for="type in types"
              :key="type"
              v-model="visibleTypes"
              :val="type"
              :label="type"
            />
          </q-expansion-item>
        </div>
      </template>
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td
            :props="props"
            key="name"
            @click="props.expand = !props.expand"
            class="cursor-pointer"
          >
            <span class="">{{ props.row.name }}</span>
            <span class="text-italic text-caption">{{
              props.row.comments
            }}</span>
          </q-td>
          <q-td :props="props" key="labels">
            <q-chip
              v-for="(label, index) in props.row.labels"
              :key="index"
              color="primary"
              text-color="white"
              class="q-mr-sm"
            >
              {{ label }}
            </q-chip>
          </q-td>
          <q-td
            v-for="col in props.cols.slice(2)"
            :key="col.name"
            :props="props"
          >
            {{ col.value }}
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td colspan="100%">
            <div class="text-left">
              <q-table
                :rows="props.row.prices"
                row-key="storeName"
                :columns="subColumns"
              />
            </div>
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>

<script setup lang="ts">
import { Product } from 'src/models/Product';
import { computed, ref, watch } from 'vue';

const props = defineProps<{
  products: Product[];
}>();

const filter = ref<string>('');

const columns = [
  {
    name: 'name',
    label: 'Name',
    field: 'name',
    sortable: true,
    style: 'width: 250px',
  },
  {
    name: 'labels',
    label: 'Labels',
    field: 'labels',
  },
  // { name: 'comments', label: 'Comments', field: 'comments', sortable: true },
  { name: 'type', label: 'Type', field: 'type', sortable: true },
  { name: 'tags', label: 'Tags', field: 'tags', sortable: true },
  {
    name: 'lowestPrice',
    label: ' BestPrice',
    field: 'lowestPrice',
    sortable: true,
    format: (val: unknown) => {
      return val ? `${val}€` : 'N/A';
    },
  },
  { name: 'unit', label: 'Unit', field: 'unit', sortable: true },
];
const subColumns = [
  {
    name: 'store',
    label: 'Store',
    field: 'storeName',
    sortable: true,
    style: 'width: 80px',
  },
  {
    name: 'price',
    label: 'Price',
    field: 'price',
    sortable: true,
    style: 'width: 50px',
  },
  {
    name: 'date',
    label: 'Date',
    field: 'dateBuying',
    sortable: true,
    style: 'width: 100px',
  },
];

// Create the list of stores
const stores = Array.from(
  new Set(
    props.products.flatMap(
      (product) => product.prices?.map((price) => price.storeName) || []
    )
  )
);

// Création des colonnes dynamiquement pour chaque magasin
const storeColumns = stores.map((store) => ({
  name: store,
  label: store,
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  field: (row: any) => row.storePrices?.[store] ?? null,
  format: (val: unknown) => {
    return val ? `${val}€` : '';
  },
  sortable: true,
}));

// Ajout des colonnes des magasins aux colonnes existantes
const allColumns = [...columns, ...storeColumns];

const products = props.products.map((product) => {
    const lowestPrice = product.prices
      ? Math.min(...product.prices.map((priceItem) => priceItem.price))
      : null;
    const storePrices = stores.reduce(
      (acc: { [key: string]: number | null }, store: string) => {
        const priceItem = product.prices?.find(
          (price) => price.storeName === store
        );
        acc[store] = priceItem ? priceItem.price : null;
        return acc;
      },
      {}
    );
    return {
      ...product,
      lowestPrice,
      storePrices,
    };
  });

const types = computed(() => {
  return Array.from(new Set(products.flatMap((product) => product.type || [])));
});

const visibleTypes = ref(types.value);
const selectAllTypes = ref(true);

watch(selectAllTypes, (newValue) => {
  visibleTypes.value = newValue ? types.value : [];
});

// Filter products based on visibleTypes
const filteredProducts = computed(() => {
  return products.filter(product => product.type && visibleTypes.value.includes(product.type));
});
</script>
<style lang="sass">
.my-sticky-header-column-table
  td:nth-child(2)
    background-color: $accent
    z-index: 1
    position: sticky
    left: 0
  tr th
    position: sticky
    /* higher than z-index for td below */
    z-index: 2
    /* bg color is important; just specify one */
    background: $primary
  thead tr:last-child th
    /* height of all previous header rows */
    top: 48px
    /* highest z-index */
    z-index: 3
  thead tr:first-child th
    top: 0
    z-index: 1
  tr:first-child th:first-child
    /* highest z-index */
    z-index: 3
  td:nth-child(2), th:first-child
    position: sticky
    left: 0

  /* prevent scrolling behind sticky top row on focus */
  tbody
    /* height of all previous header rows */
    scroll-margin-top: 48px
</style>
