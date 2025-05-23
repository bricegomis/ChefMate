import { boot } from 'quasar/wrappers';
import axios from 'axios';
import { useProfileStore } from 'src/stores/profile-store';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'api/',
});

export default boot(({ app, router }) => {
  const profileStore = useProfileStore();
  profileStore.initialize(); // Centralize token initialization

  app.config.globalProperties.$axios = axios;
  app.config.globalProperties.$api = api;

  // Add Axios response interceptor
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        profileStore.logout();
        router.push('/profile');
      }
      return Promise.reject(error);
    }
  );
});

export { api };
