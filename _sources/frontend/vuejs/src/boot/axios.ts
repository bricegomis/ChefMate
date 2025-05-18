import { boot } from 'quasar/wrappers';
import axios, { AxiosInstance } from 'axios';
import { useAuthStore } from 'src/stores/auth-store';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance;
    $api: AxiosInstance;
    $auth: {
      login: (email: string, password: string) => Promise<void>;
      logout: () => void;
    };
  }
}

// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'api/',
});

const auth = {
  async login(email: string, password: string) {
    try {
      const response = await api.post('/auth/login', { email, password });
      const token = response.data.token;
      const authStore = useAuthStore();
      authStore.setToken(token);
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } catch (error) {
      console.error('Login failed:', error);
      throw error;
    }
  },
  logout() {
    delete api.defaults.headers.common['Authorization'];
    const authStore = useAuthStore();
    authStore.setToken(null);
  },
};

export default boot(({ app }) => {
  const authStore = useAuthStore();
  authStore.initializeToken(); // Centralize token initialization

  if (authStore.token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${authStore.token}`;
  }

  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axios;
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file

  app.config.globalProperties.$api = api;
  // ^ ^ ^ this will allow you to use this.$api (for Vue Options API form)
  //       so you can easily perform requests against your app's API

  app.config.globalProperties.$auth = auth;

  // Add Axios response interceptor
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        // Trigger authentication popup
        const authStore = useAuthStore();
        authStore.setShowAuthPopup(true);
      }
      return Promise.reject(error);
    }
  );
});

export { api, auth };
