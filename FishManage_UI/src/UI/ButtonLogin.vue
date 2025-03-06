<template>
  <li v-if="!isAuthenticated">
    <a href="/Login/login.html" @click="openModal" class="p-2 mx-1">
      <svg width="30" height="30" class="text-gray-600 hover:text-blue-500">
        <use xlink:href="#user"></use>
      </svg>
    </a>
  </li>
  <li v-if="isAuthenticated && isAdmin">
    <a href="#" @click="dashboard" class="p-2 mx-1">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" class="w-6 h-6">
        <path
          fill="currentColor"
          d="M151.6 42.4C145.5 35.8 137 32 128 32s-17.5 3.8-23.6 10.4l-88 96c-11.9 13-11.1 33.3 2 45.2s33.3 11.1 45.2-2L96 146.3 96 448c0 17.7 14.3 32 32 32s32-14.3 32-32l0-301.7 32.4 35.4c11.9 13 32.2 13.9 45.2 2s13.9-32.2 2-45.2l-88-96z"
        />
      </svg>
    </a>
  </li>
  <li v-if="isAuthenticated">
    <a href="#" @click="logout" class="p-2 mx-1">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="w-6 h-6">
        <path
          fill="currentColor"
          d="M377.9 105.9L500.7 228.7c7.2 7.2 11.3 17.1 11.3 27.3s-4.1 20.1-11.3 27.3L377.9 406.1c-6.4 6.4-15 9.9-24 9.9c-18.7 0-33.9-15.2-33.9-33.9l0-62.1-128 0c-17.7 0-32-14.3-32-32l0-64c0-17.7 14.3-32 32-32l128 0 0-62.1c0-18.7 15.2-33.9 33.9-33.9c9 0 17.6 3.6 24 9.9z"
        />
      </svg>
    </a>
  </li>
</template>

<script>
export default {
  data() {
    return {
      isAuthenticated: false,
      isAdmin: false,
      userId: null,
    };
  },
  mounted() {
    this.loadAuthState();
  },
  methods: {
    loadAuthState() {
      const params = new URLSearchParams(window.location.search);
      const authenticated = params.get("isAuthenticated");
      const userIdParam = params.get("userId");
      const role = params.get("isAdmin"); // This is coming as a string
      this.isAuthenticated = !!authenticated; // Check if token exists
      this.isAdmin = role === "true"; // Convert string "true"/"false" to boolean
      this.userId = userIdParam;
      console.log("buttonlogin token:"+ localStorage.getItem("token"));
    },
    dashboard() {
      const queryParams = new URLSearchParams({
        userId: this.userId ? encodeURIComponent(this.userId) : "",
        isAdmin: this.isAdmin.toString(),
        isAuthenticated: this.isAuthenticated.toString(),
      }).toString();

      window.location.href = `/src/dist/dashboard/index.html?${queryParams}`;
    },

    logout() {
      localStorage.removeItem("token");
      this.isAuthenticated = false;
      this.isAdmin = false;
      this.userId = null;
      window.history.pushState({}, "", "/");
      window.location.reload();
    },
  },
};
</script>
