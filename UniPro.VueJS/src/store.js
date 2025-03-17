import { createStore } from 'vuex';

export default createStore({
  state: {
    isAuthenticated: false, // или true, если пользователь уже авторизован
  },
  mutations: {
    setAuth(state, payload) {
      state.isAuthenticated = payload;
    },
  },
  actions: {},
  modules: {},
});
