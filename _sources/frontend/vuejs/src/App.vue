<template>
  <router-view />
  <q-ajax-bar />
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useProductStore } from 'src/stores/product-store';

const productStore = useProductStore();

defineOptions({
  name: 'App',
});

const loop = async () => {
  // try {
  await productStore.fetchProfile();
  await productStore.fetchProducts();
  // } catch (error) {
  //   // console.error('Error fetching items:', error);
  // } finally {
  //   setTimeout(loop, 5000);
  // }
};

onMounted(() => {
  loop();
});
</script>
