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
    <template v-slot:top-right>
      <q-btn
        color="primary"
        icon-right="add"
        label="Create new"
        no-caps
        @click="createNewProduct"
      />
    </template>
    <!-- BODY SLOT -->
    <template v-slot:body="props">
      <q-tr :props="props">
        <!-- NAME -->
        <q-td
          :props="props"
          key="name"
          @click="openProduct(props.row)"
          class="cursor-pointer"
        >
          <span class="">{{ props.row.name }}</span>
          <q-btn
            flat
            dense
            size="sm"
            icon="delete"
            color="negative"
            @click.stop="deleteProduct(props.row)"
          /><br />
          <span class="text-italic text-caption text-left">{{
            props.row.description
          }}</span>
        </q-td>
        <q-td :props="props" key="actions"> </q-td>
        <q-td :props="props" key="tags">
          {{ props.row.tags.join(', ') }}
        </q-td>
        <q-td :props="props" key="lowestPriceItem">
          {{
            props.row.lowestPriceItem
              ? `${props.row.lowestPriceItem.price}€/${props.row.lowestPriceItem.unit}`
              : 'N/A'
          }}
        </q-td>
        <q-td v-for="col in props.cols.slice(3)" :key="col.name" :props="props">
          <span>{{ col.value }}</span>
        </q-td>
      </q-tr>
    </template>
  </q-table>
</template>

<script setup lang="ts">
import { createDefaultProduct, Product } from 'src/models/Product';
import { ProductQuantityUnit } from 'src/models/ProductQuantityUnit';
import { computed } from 'vue';

const props = defineProps<{
  filteredProducts: Product[];
  allTypes: string[];
  selectedTags: string[];
  stores: string[];
  showStoreColumns?: boolean;
}>();

const emit = defineEmits<{
  (event: 'delete-product', product: Product): void;
  (event: 'open-product', product: Product): void;
}>();

function deleteProduct(product: Product) {
  emit('delete-product', product);
}

function openProduct(product: Product) {
  emit('open-product', product);
}

function createNewProduct() {
  emit('open-product', createDefaultProduct());
}

const columns = [
  {
    name: 'name',
    label: 'Name',
    field: 'name',
    sortable: true,
    style: 'width: 250px',
  },
  { name: 'tags', label: 'Tags', field: 'tags', sortable: true },
  {
    name: 'lowestPriceItem',
    label: 'Lowest Price',
    field: 'lowestPriceItem',
    sortable: true,
  },
];
// Add a column for each store
const storeColumns = props.stores.map((store) => ({
  name: store,
  label: store,
  field: store, // Use a unique string identifier for the field
  format: (val: { value: number; unit: ProductQuantityUnit | null }) => {
    return val ? `${val.value}€/${val.unit}` : '-';
  },
  sortable: true,
}));

const allColumns = computed(() => {
  return props.showStoreColumns ? columns.concat(storeColumns) : columns;
});

const productsWithMeta = computed(() => {
  return props.filteredProducts.map((product) => {
    const lowestPriceItem =
      (product.prices ?? []).length > 0
        ? product.prices?.reduce((min, current) =>
            current.price < min.price ? current : min
          )
        : null;

    const storePrices = props.stores.reduce(
      (
        acc: {
          [key: string]: {
            value: number;
            unit: ProductQuantityUnit | null;
          } | null;
        },
        store: string
      ) => {
        const priceItem = product.prices?.find(
          (price) => price.storeId === store
        );
        if (!priceItem) {
          acc[store] = null;
          return acc;
        }
        acc[store] = {
          value: priceItem.price,
          unit: priceItem.unit,
        };
        return acc;
      },
      {}
    );

    return {
      ...product,
      lowestPriceItem,
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
