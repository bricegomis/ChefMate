<template>
  <div class="q-pa-md">
    <q-table
      class="my-sticky-header-column-table"
      :columns="allColumns"
      :rows="products"
      row-key="id"
      separator="cell"
      flat
      bordered
      dense
      title="Produits"
      :rows-per-page-options="[20, 50, 100]"
    >
      <!-- <template v-slot:header="props">
        <q-tr :props="props">
          <q-th auto-width />
          <q-th v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.label }}
          </q-th>
        </q-tr>
      </template>
      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td auto-width>
            <q-btn
              size="sm"
              color="accent"
              round
              flat
              dense
              @click="props.expand = !props.expand"
              :icon="props.expand ? 'remove' : 'add'"
            />
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
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
      </template> -->
      <template v-slot:body-cell-name="props">
        <q-td
          :props="props"
          @click="props.expand = !props.expand"
          class="cursor-pointer"
        >
          <!-- <q-icon
            :name="props.expand ? 'expand_less' : 'expand_more'"
            class="q-mr-sm"
          /> -->
          {{ props.row.name }}
        </q-td>
      </template>
      <template v-slot:body-cell-labels="props">
        <q-td :props="props">
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
      </template>
    </q-table>
  </div>
</template>
<script setup lang="ts">
import { Product } from 'src/models/Product';

const props = defineProps<{
  products: Product[];
}>();

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
  { name: 'comments', label: 'Comments', field: 'comments', sortable: true },
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
// const subColumns = [
//   {
//     name: 'store',
//     label: 'Store',
//     field: 'storeName',
//     sortable: true,
//     style: 'width: 80px',
//   },
//   {
//     name: 'price',
//     label: 'Price',
//     field: 'price',
//     sortable: true,
//     style: 'width: 50px',
//   },
//   {
//     name: 'date',
//     label: 'Date',
//     field: 'dateBuying',
//     sortable: true,
//     style: 'width: 100px',
//   },
// ];

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
