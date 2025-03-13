import { defineStore } from 'pinia';
import { Product } from 'src/models/Product';
import { Profile } from 'src/models/Profile';
import { api } from 'boot/axios';

export const useProductStore = defineStore('ProductStore', {
  state: () => ({
    products: [] as Product[],
    isEditingProductDialogVisible: false,
    editingProduct: {} as Product,
    profile: {} as Profile,
    isOnline: false,
  }),
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
    async getProduct(id: string) {
      try {
        console.log('api.getProduct', id);
        const response = await api.get(`product/${id}`);
        this.editingProduct = response.data; //TODO check if it's the right name
        console.log('this.getProduct', this.editingProduct);
      } catch (error) {
        console.error('Error fetching Product:', error);
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
    async finishProduct(product: Product) {
      try {
        await api.put('product/finish', product);
        await this.fetchProducts();
      } catch (error) {
        console.error('Error when finishing a product :', error);
      }
    },
  },
  persist: {
    enabled: true,
  },
});
