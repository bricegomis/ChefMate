<template>
  <q-page title="Products">
    <ProductList v-if="products?.length > 0" :products="products" />
  </q-page>
</template>

<script setup lang="ts">
import ProductList from 'src/components/ProductList.vue';

import { useProductStore } from 'src/stores/product-store';
import { computed } from 'vue';

const productStore = useProductStore();

const products = computed(() => {
  return productStore.products;
});

defineOptions({
  name: 'ProductsPage',
});
</script>
