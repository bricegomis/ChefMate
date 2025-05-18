interface AuthState {
  showAuthPopup: boolean;
}

interface AuthMutations {
  showAuthPopup(state: AuthState, value: boolean): void;
}

export default {
  namespaced: true,
  state: <AuthState>{
    showAuthPopup: false,
  },
  mutations: <AuthMutations>{
    showAuthPopup(state: AuthState, value: boolean) {
      state.showAuthPopup = value;
    },
  },
};
