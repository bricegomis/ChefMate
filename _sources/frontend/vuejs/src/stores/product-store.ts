import { defineStore } from 'pinia';
import { Product } from 'src/models/Product';
import { Profile } from 'src/models/Profile';
import { api } from 'boot/axios';
import { reactive } from 'vue';

export const useProductStore = defineStore('ProductStore', {
  state: () => ({
    products: [] as Product[],
    types: [] as {
      name: string;
      isSelected: boolean;
      nbOccurrence: number;
    }[],
    profile: {} as Profile,
    isOnline: false,
  }),
  getters: {
    types: (state) => {
      Object.entries(
        state.products.reduce((acc: Record<string, number>, product) => {
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
        );
    },
  },
  actions: {
    async fetchProfile() {
      try {
        const response = await api.get('profile');
        this.isOnline = true;
        this.profile = response.data;
      } catch (error) {
        //console.error('Error fetching profile:', error);
        this.isOnline = false;
      }
    },
    async fetchProducts() {
      try {
        const response = await api.get('product/all');
        this.products = response.data;
      } catch (error) {
        //console.error('Error fetching Products:', error);
      }
    },
    async createProduct(product: Product) {
      try {
        await api.post('product', product);
        await this.fetchProducts();
      } catch (error) {
        console.error('Error creating Product:', error);
      }
    },
    async updateProduct(product: Product) {
      try {
        if (product.id)
          // TODO use product ?
          // => editing
          await api.put('product', product);
        // => new
        else await api.post('Product', product);
        await this.fetchProducts();
      } catch (error) {
        console.error('Error when editing a product :', error);
      }
    },
    async deleteProduct(product: Product) {
      try {
        await api.delete(`product/${product.id}`);
        await this.fetchProducts();
      } catch (error) {
        console.error('Error deleting Product:', error);
      }
    },
  },
  persist: {
    enabled: true,
  },
});
