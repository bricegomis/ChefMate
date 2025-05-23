import { ref } from 'vue';
import { defineStore } from 'pinia';
import { api } from 'boot/axios';

export const useProfileStore = defineStore('profile', () => {
  const profile = ref(null);
  const isOnline = ref(false);
  const token = ref<string | null>(null);

  const isAuthenticated = () => !!token.value;

  const initialize = () => {
    const storedToken = localStorage.getItem('jwt_token');
    if (storedToken) {
      setToken(storedToken);
      api.defaults.headers.common['Authorization'] = `Bearer ${storedToken}`;
      fetchProfile();
    }
  };

  const setToken = (newToken: string | null) => {
    token.value = newToken;
    if (newToken) {
      localStorage.setItem('jwt_token', newToken);
      api.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
    } else {
      localStorage.removeItem('jwt_token');
      delete api.defaults.headers.common['Authorization'];
    }
  };

  const login = async (email: string, password: string) => {
    try {
      const response = await api.post('/auth/login', { email, password });
      setToken(response.data.token);
      fetchProfile();
    } catch (error) {
      console.error('Login failed:', error);
      throw error;
    }
  };

  const logout = () => {
    setToken(null);
  };

  const fetchProfile = async () => {
    try {
      const response = await api.get('profile');
      isOnline.value = true;
      profile.value = response.data;
    } catch (error) {
      isOnline.value = false;
    }
  };

  return {
    profile,
    isOnline,
    token,
    isAuthenticated,
    initialize,
    setToken,
    login,
    logout,
    fetchProfile,
  };
});
