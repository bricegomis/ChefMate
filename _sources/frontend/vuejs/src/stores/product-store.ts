import { defineStore } from 'pinia';
import { ref } from 'vue';
import { Product } from 'src/models/Product';
import { Profile } from 'src/models/Profile';
import { api } from 'boot/axios';
import { TagInfo } from 'src/models/tagInfo';

const localStorageProductsKey = 'products';
const localStorageTagsKey = 'tags';

export const useProductStore = defineStore('ProductStore', () => {
  const profile = ref<Profile>();
  const products = ref<Product[]>([]);
  const tags = ref<TagInfo[]>([]);

  const loadFromLocalStorage = () => {
    const storeProducts = localStorage.getItem(localStorageProductsKey);
    if (storeProducts) {
      try {
        products.value = JSON.parse(storeProducts);
      } catch (e) {
        products.value = [];
      }
    }
    const storedTags = localStorage.getItem(localStorageTagsKey);
    if (storedTags) {
      try {
        products.value = JSON.parse(storedTags);
      } catch (e) {
        products.value = [];
      }
    }
  };

  const saveToLocalStorage = () => {
    localStorage.setItem(
      localStorageProductsKey,
      JSON.stringify(products.value)
    );
    localStorage.setItem(localStorageTagsKey, JSON.stringify(products.value));
  };

  loadFromLocalStorage();

  const fetchProducts = async () => {
    try {
      const response = await api.get('product');
      products.value = response.data;
    } catch (error) {
      console.error('Error fetching Products:', error);
    }
  };

  const fetchAll = async () => {
    await fetchProducts();
    await fetchTags();
    saveToLocalStorage();
  };

  const fetchTags = async () => {
    try {
      const response = await api.get('product/tags');
      tags.value = response.data;
    } catch (error) {
      console.error('Error fetching tags:', error);
    }
  };

  const createProduct = async (product: Product) => {
    try {
      await api.post('product', product);
      await fetchAll();
    } catch (error) {
      console.error('Error creating Product:', error);
    }
  };

  const updateProduct = async (product: Product): Promise<boolean> => {
    try {
      if (product.id) {
        const encodedId = encodeURIComponent(product.id); // Encode the ID to handle special characters like '/'
        await api.put(`product/${encodedId}`, product);
      } else {
        await api.post('product', product);
      }
      await fetchAll();
      return true;
    } catch (error) {
      console.error('Error when editing a product:', error);
      return false;
    }
  };

  const deleteProduct = async (product: Product): Promise<boolean> => {
    try {
      await api.delete(`product/${product.id}`);
      await fetchAll();
      return true;
    } catch (error) {
      console.error('Error deleting Product:', error);
      return false;
    }
  };

  return {
    products,
    tags,
    profile,
    fetchAll,
    createProduct,
    updateProduct,
    deleteProduct,
  };
});
