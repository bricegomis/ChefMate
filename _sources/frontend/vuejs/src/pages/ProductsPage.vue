<template>
  <q-page title="Products">
    <!-- <q-tabs v-model="currentTag" inline-label class="text-white shadow-2">
      <q-tab
        v-for="tag in tags"
        :key="tag.name"
        :name="tag.name"
        :icon="tag.icon"
        :label="tag.label"
        :class="tag.classes"
      />
    </q-tabs> -->
    <ProductList v-if="products?.length > 0" :products="products" />
  </q-page>
</template>

<script setup lang="ts">
import ProductList from 'src/components/ProductList.vue';

import { useProductStore } from 'src/stores/product-store';
import { computed } from 'vue';

const productStore = useProductStore();

const products = computed(() => {
  let result = productStore.products;

  // if (currentTag.value != 'all') {
  //   result = result.filter(
  //     (product) =>
  //       product.tags && product.tags.indexOf(currentTag.value) >= 0
  //   );
  // }

  // result.sort((a, b) => {
  //   // console.log(Number(b.isReward))
  //   return Number(b.isReward) - Number(a.isReward);
  // });

  // result.sort((a, b) => {
  //   const comparison = (a.points ?? 0) - (b.points ?? 0);
  //   return selectedOrder.value.value === 'asc' ? comparison : -comparison;
  // });

  return result;
});

defineOptions({
  name: 'ProductsPage',
});
</script>
