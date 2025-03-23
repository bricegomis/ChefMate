import { defineStore } from 'pinia';
import { ref, reactive, computed } from 'vue';
import { Product } from 'src/models/Product';
import { Profile } from 'src/models/Profile';
import { api } from 'boot/axios';

export const useProductStore = defineStore('ProductStore', () => {
  const products = ref<Product[]>([]);
  const types = computed(() =>
    Object.entries(
      products.value.reduce((acc: Record<string, number>, product) => {
        if (product.type) {
          acc[product.type] = (acc[product.type] || 0) + 1; // Count occurrences
        }
        return acc;
      }, {})
    )
      .sort((a, b) => b[1] - a[1]) // Sort by occurrences in descending order
      .map(([name, nbOccurrence]) =>
        reactive({
          name,
          isSelected: true,
          nbOccurrence,
        })
      )
  );
  const profile = ref<Profile>();
  const isOnline = ref(false);

  const fetchProfile = async () => {
    try {
      const response = await api.get('profile');
      isOnline.value = true;
      profile.value = response.data;
    } catch (error) {
      isOnline.value = false;
    }
  };

  const fetchProducts = async () => {
    try {
      const response = await api.get('product/all');
      products.value = response.data;
    } catch (error) {
      console.error('Error fetching Products:', error);
    }
  };

  const createProduct = async (product: Product) => {
    try {
      await api.post('product', product);
      await fetchProducts();
    } catch (error) {
      console.error('Error creating Product:', error);
    }
  };

  const updateProduct = async (product: Product) => {
    try {
      if (product.id) {
        await api.put('product', product);
      } else {
        await api.post('Product', product);
      }
      await fetchProducts();
    } catch (error) {
      console.error('Error when editing a product:', error);
    }
  };

  const deleteProduct = async (product: Product) => {
    try {
      await api.delete(`product/${product.id}`);
      await fetchProducts();
    } catch (error) {
      console.error('Error deleting Product:', error);
    }
  };

  return {
    products,
    types,
    profile,
    isOnline,
    fetchProfile,
    fetchProducts,
    createProduct,
    updateProduct,
    deleteProduct,
  };
});
