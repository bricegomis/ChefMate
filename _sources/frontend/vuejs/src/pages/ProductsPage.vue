<template>
  <q-page title="Products">
    <q-card class="q-pa-md">
      <ProductsFilter
        :tags="tags"
        :usages="Object.values(ProductUsageType)"
        :search="searchFilter"
        :showStoreColumns="showStoreColumns"
        @search="onSearch"
        @showStoreColumns="onShowStoreColumns"
      />
    </q-card>
    <ProductList
      v-if="products?.length > 0"
      :filteredProducts="filteredProducts"
      :allTypes="tags.map((_) => _.name)"
      :selectedTags="selectedTags.map((_) => _.name)"
      @delete-product="handleDeleteProduct"
      @open-product="handleOpenProduct"
      :stores="stores"
      :show-store-columns="showStoreColumns"
    />
    <ProductDetail
      :isOpen="popupDetailOpen"
      :product="editingProduct || {} as Product"
      :types="tags.map((_) => _.name)"
      @close="popupDetailOpen = false"
      @save="saveProduct"
    />
  </q-page>
</template>

<script setup lang="ts">
import ProductsFilter from 'src/components/ProductsFilter.vue';
import ProductList from 'src/components/ProductList.vue';
import ProductDetail from 'src/components/ProductDetail.vue';
import { ProductUsageType } from 'src/models/ProductUsageType';

import { useQuasar } from 'quasar';
import { useProductStore } from 'src/stores/product-store';
import { computed, ref, onMounted } from 'vue';
import { Product } from 'src/models/Product';

const productStore = useProductStore();
const $q = useQuasar();

const products = computed(() => {
  return productStore.products;
});

const searchFilter = ref<string>('');
const showStoreColumns = ref(false);

const popupDetailOpen = ref(false);
const editingProduct = ref<Product>();

// Fetch products when the page is loaded
onMounted(() => {
  productStore.fetchProducts();
});

function onSearch(searchQuery: string) {
  searchFilter.value = searchQuery;
}

function onShowStoreColumns(show: boolean) {
  showStoreColumns.value = show;
}

// Filter products based on selected tags
const filteredProducts = computed(() => {
  return selectedTags.value.length
    ? products.value.filter(
        (product) =>
          (product.tags == null ||
            product.tags?.length < 1 ||
            product.tags?.some((tag) =>
              selectedTags.value.map((_) => _.name).includes(tag)
            )) &&
          (searchFilter.value.length === 0 ||
            product.name
              .toUpperCase()
              .includes(searchFilter.value.toUpperCase()))
      )
    : [];
});

const tags = computed(() => {
  return productStore.tags;
});

const selectedTags = computed(() => tags.value.filter((_) => _.isSelected));

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

async function saveProduct(product: Product) {
  const success = await productStore.updateProduct(product);
  if (success) {
    $q.notify({
      caption: 'Product updated successfully',
      message: product.name,
      color: 'green',
    });
    popupDetailOpen.value = false;
  } else {
    $q.notify({
      caption: 'Failed to update product',
      message: product.name,
      color: 'red',
    });
  }
}

defineOptions({
  name: 'ProductsPage',
});
</script>
