import { defineStore } from 'pinia';
import { ref, reactive, computed } from 'vue';
import { Product } from 'src/models/Product';
import { Profile } from 'src/models/Profile';
import { api } from 'boot/axios';

export const useProductStore = defineStore('ProductStore', () => {
  const profile = ref<Profile>();
  const isOnline = ref(false);
  const products = ref<Product[]>([]);
  const tags = computed(() =>
    Object.entries(
      products.value.reduce((acc: Record<string, number>, product) => {
        for (const tag of product.tags || []) {
          acc[tag] = (acc[tag] || 0) + 1; // Count occurrences
        }
        return acc;
      }, {})
    )
      .sort((a, b) => b[1] - a[1]) // Sort by occurrences in descending order
      .map(([name, nbOccurrence]) =>
        reactive({
          name,
          isSelected: false,
          nbOccurrence,
        })
      )
  );
  // Create the list of stores
  const stores = computed(() =>
    Array.from(
      new Set(
        products.value.flatMap(
          (product) => product.prices?.map((price) => price.storeId) || []
        )
      )
    )
  );

  const fetchProducts = async () => {
    try {
      const response = await api.get('product');
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

  const updateProduct = async (product: Product): Promise<boolean> => {
    try {
      if (product.id) {
        const encodedId = encodeURIComponent(product.id); // Encode the ID to handle special characters like '/'
        await api.put(`product/${encodedId}`, product);
      } else {
        await api.post('product', product);
      }
      await fetchProducts();
      return true;
    } catch (error) {
      console.error('Error when editing a product:', error);
      return false;
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
    tags,
    stores,
    profile,
    isOnline,
    fetchProducts,
    createProduct,
    updateProduct,
    deleteProduct,
  };
});
