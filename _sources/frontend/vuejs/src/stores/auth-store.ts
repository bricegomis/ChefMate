import { defineStore } from 'pinia';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    showAuthPopup: false,
    token: null as string | null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    initializeToken() {
      const token = localStorage.getItem('jwt_token');
      if (token) {
        this.token = token;
      }
    },
    setShowAuthPopup(value: boolean) {
      this.showAuthPopup = value;
    },
    setToken(token: string | null) {
      this.token = token;
      if (token) {
        localStorage.setItem('jwt_token', token);
      } else {
        localStorage.removeItem('jwt_token');
      }
    },
  },
});
