<template>
  <q-table
    class="my-sticky-header-column-table"
    :columns="allColumns"
    :rows="productsWithMeta"
    row-key="id"
    separator="cell"
    flat
    bordered
    dense
    title="Produits"
    :rows-per-page-options="[20, 50, 100]"
  >
    <!-- BODY SLOT -->
    <template v-slot:body="props">
      <q-tr :props="props">
        <!-- NAME -->
        <q-td
          :props="props"
          key="name"
          @click="props.expand = !props.expand"
          class="cursor-pointer"
        >
          <span class="">{{ props.row.name }}</span
          ><br />
          <span class="text-italic text-caption">{{ props.row.comments }}</span>
        </q-td>
        <q-td :props="props" key="actions">
          <q-btn
            flat
            dense
            size="sm"
            icon="delete"
            color="negative"
            @click="deleteProduct(props.row)"
          />
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
        <q-td :props="props" key="type">
          <q-select
            filled
            dense
            v-model="props.row.type"
            :options="selectedTypes"
            options-dense
          />
        </q-td>
        <q-td v-for="col in props.cols.slice(4)" :key="col.name" :props="props">
          {{ col.value }}
        </q-td>
      </q-tr>
    </template>
  </q-table>
</template>

<script setup lang="ts">
import { Product } from 'src/models/Product';
import { computed } from 'vue';

const props = defineProps<{
  filteredProducts: Product[];
  allTypes: string[];
  selectedTypes: string[];
  stores: string[];
}>();

const emit = defineEmits<{
  (event: 'delete-product', product: Product): void;
}>();

function deleteProduct(product: Product) {
  emit('delete-product', product);
}

const columns = [
  {
    name: 'name',
    label: 'Name',
    field: 'name',
    sortable: true,
    style: 'width: 250px',
  },
  {
    name: 'actions',
    label: '',
    field: '',
    sortable: false,
  },
  {
    name: 'labels',
    label: 'Labels',
    field: 'labels',
  },
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

// Création des colonnes dynamiquement pour chaque magasin
const storeColumns = props.stores.map((store) => ({
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

const productsWithMeta = computed(() => {
  return props.filteredProducts.map((product) => {
    const lowestPrice = product.prices
      ? Math.min(...product.prices.map((priceItem) => priceItem.price))
      : null;
    const storePrices = props.stores.reduce(
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
});
</script>

<style lang="sass">
.my-sticky-header-column-table
  td:first-child
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
  td:first-child, th:first-child
    position: sticky
    left: 0

  /* prevent scrolling behind sticky top row on focus */
  tbody
    /* height of all previous header rows */
    scroll-margin-top: 48px
.q-chip
  width: 100px
</style>
