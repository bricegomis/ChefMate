import { defineStore } from 'pinia';
import { ref } from 'vue';
import { api } from 'boot/axios';
import { Store } from 'src/models/Store';

export const useStoreStore = defineStore('StoreStore', () => {
  const stores = ref<Store[]>([]);

  const fetchStores = async () => {
    try {
      const response = await api.get('store');
      stores.value = response.data;
    } catch (error) {
      console.error('Error fetching stores:', error);
    }
  };

  const saveStore = async (store: Store) => {
    try {
      if (store.id) {
        await api.put(`store/${store.id}`, store);
      } else {
        await api.post('store', store);
      }
      return true;
    } catch (error) {
      console.error('Error saving store:', error);
      return false;
    }
  };

  return {
    stores,
    fetchStores,
    saveStore,
  };
});
