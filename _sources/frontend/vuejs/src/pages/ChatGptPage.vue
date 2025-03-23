<template>
  <q-page title="ChatGpt">
    <textarea v-model="productListStr" />
  </q-page>
</template>

<script setup lang="ts">
import { useProductStore } from 'src/stores/product-store';
import { computed } from 'vue';

const productStore = useProductStore();

const productListStr = computed(() => {
  return productStore.products.map((_) => _.name).join(', ');
});

defineOptions({
  name: 'ChatGptPage',
});
</script>
