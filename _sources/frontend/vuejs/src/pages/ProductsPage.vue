<template>
  <q-page title="Products">
    <q-card class="q-pa-md">
      <ProductsFilter :types="types" />
    </q-card>
    <ProductList
      v-if="products?.length > 0"
      :filteredProducts="filteredProducts"
      :allTypes="types.map((_) => _.name)"
      :selectedTypes="selectedTypes.map((_) => _.name)"
      :stores="stores"
    />
  </q-page>
</template>

<script setup lang="ts">
import ProductsFilter from 'src/components/ProductsFilter.vue';
import ProductList from 'src/components/ProductList.vue';

import { useProductStore } from 'src/stores/product-store';
import { computed } from 'vue';

const productStore = useProductStore();

const products = computed(() => {
  return productStore.products;
});

// Filter products based on selected types
const filteredProducts = computed(() => {
  return selectedTypes.value.length
    ? products.value.filter(
        (product) =>
          product.type &&
          selectedTypes.value.map((_) => _.name).includes(product.type)
      )
    : [];
});

const types = computed(() => {
  return productStore.types;
});

const selectedTypes = computed(() => types.value.filter((_) => _.isSelected));

const stores = computed(() => {
  return productStore.stores;
});

defineOptions({
  name: 'ProductsPage',
});
</script>
