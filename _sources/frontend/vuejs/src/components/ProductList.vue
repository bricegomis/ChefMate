<template>
  <q-table
    class="my-sticky-header-column-table"
    :columns="columns"
    :rows="productsWithMeta"
    row-key="id"
    separator="cell"
    flat
    bordered
    dense
    title="Produits"
    :rows-per-page-options="[20, 50, 100]"
    @row-click="openProduct"
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
  </q-table>
</template>

<script setup lang="ts">
import { createDefaultProduct, Product } from 'src/models/Product';
import { computed } from 'vue';

const props = defineProps<{
  filteredProducts: Product[];
  allTypes: string[];
  selectedTags: string[];
  showStoreColumns?: boolean;
}>();

const emit = defineEmits<{
  (event: 'delete-product', product: Product): void;
  (event: 'open-product', product: Product): void;
}>();

function openProduct(evt: Event, product: Product) {
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
  {
    name: 'usage',
    label: 'usage',
    field: 'usages',
    sortable: true,
    style: 'width: 250px',
  },
  { name: 'tags', label: 'Tags', field: 'tags', sortable: true },
  // {
  //   name: 'lowestPriceItem',
  //   label: 'Lowest Price',
  //   field: 'lowestPriceItem',
  //   sortable: true,
  //   format: (val: { value: number; unit: ProductQuantityUnit | null }) => {
  //     return val ? `${val.value}€/${val.unit}` : '-';
  //   },
  // },
];

const productsWithMeta = computed(() => {
  return props.filteredProducts.map((product) => {
    const lowestPriceItem =
      (product.prices ?? []).length > 0
        ? product.prices?.reduce((min, current) =>
            current.price < min.price ? current : min
          )
        : null;

    return {
      ...product,
      lowestPriceItem,
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
