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
      @delete-product="handleDeleteProduct"
      @open-product="handleOpenProduct"
      :stores="stores"
    />
    <ProductDetail
      :isOpen="popupDetailOpen"
      :product="editingProduct || {} as Product"
      :types="types.map((_) => _.name)"
      @close="popupDetailOpen = false"
      @save="saveProduct"
    />
  </q-page>
</template>

<script setup lang="ts">
import ProductsFilter from 'src/components/ProductsFilter.vue';
import ProductList from 'src/components/ProductList.vue';
import ProductDetail from 'src/components/ProductDetail.vue';

import { useQuasar } from 'quasar';
import { useProductStore } from 'src/stores/product-store';
import { computed, ref } from 'vue';
import { Product } from 'src/models/Product';

const productStore = useProductStore();
const $q = useQuasar();

const products = computed(() => {
  return productStore.products;
});

const popupDetailOpen = ref(false);
const editingProduct = ref<Product>();

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

function handleDeleteProduct(product: Product) {
  $q.dialog({
    title: 'Confirm',
    message: 'Are you sure',
    cancel: true,
    persistent: true,
  }).onOk(() => {
    productStore.deleteProduct(product);
    $q.notify({
      message: 'Product deleted',
      caption: product.name,
      color: 'green',
    });
  });
}

function handleOpenProduct(product: Product) {
  popupDetailOpen.value = true;
  editingProduct.value = product;
}

function saveProduct(product: Product) {
  productStore.updateProduct(product);
  $q.notify({
    caption: 'Product updated successfully',
    message: product.name,
    color: 'green',
  });
  popupDetailOpen.value = false;
}

defineOptions({
  name: 'ProductsPage',
});
</script>
