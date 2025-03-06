import { createStore } from "vuex";

export default createStore({
  state: {
    isAuthenticated: !!localStorage.getItem("token"),
    isAdmin: localStorage.getItem("role") === "admin",
    userId: localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user")).id : null,
  },
  mutations: {
    login(state, { token, user, role }) {
      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));
      localStorage.setItem("role", role);

      state.isAuthenticated = true;
      state.isAdmin = role === "admin";
      state.userId = user.id;
    },
    logout(state) {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      localStorage.removeItem("role");

      state.isAuthenticated = false;
      state.isAdmin = false;
      state.userId = null;
    },
  },
});
