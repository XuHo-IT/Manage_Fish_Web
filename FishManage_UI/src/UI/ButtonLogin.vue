<template>
  <li v-if="!isAuthenticated">
    <a href="/Login/login.html" @click="openModal" class="p-2 mx-1">
      <svg width="30" height="30" class="text-gray-600 hover:text-blue-500">
        <use xlink:href="#user"></use>
      </svg>
    </a>
  </li>

  <li v-if="isAuthenticated">
    <a href="/profile.html" class="p-2 mx-1">
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512">
        <path d="M224 16c-6.7 0-10.8-2.8-15.5-6.1C201.9 5.4 194 0 176 0c-30.5 0-52 43.7-66 89.4C62.7 98.1 32 112.2 32 128c0 14.3 25 27.1 64.6 35.9c-.4 4-.6 8-.6 12.1c0 17 3.3 33.2 9.3 48l-59.9 0C38 224 32 230 32 237.4c0 1.7 .3 3.4 1 5l38.8 96.9C28.2 371.8 0 423.8 0 482.3C0 498.7 13.3 512 29.7 512l388.6 0c16.4 0 29.7-13.3 29.7-29.7c0-58.5-28.2-110.4-71.7-143L415 242.4c.6-1.6 1-3.3 1-5c0-7.4-6-13.4-13.4-13.4l-59.9 0c6-14.8 9.3-31 9.3-48c0-4.1-.2-8.1-.6-12.1C391 155.1 416 142.3 416 128c0-15.8-30.7-29.9-78-38.6C324 43.7 302.5 0 272 0c-18 0-25.9 5.4-32.5 9.9c-4.8 3.3-8.8 6.1-15.5 6.1zm56 208l-12.4 0c-16.5 0-31.1-10.6-36.3-26.2c-2.3-7-12.2-7-14.5 0c-5.2 15.6-19.9 26.2-36.3 26.2L168 224c-22.1 0-40-17.9-40-40l0-14.4c28.2 4.1 61 6.4 96 6.4s67.8-2.3 96-6.4l0 14.4c0 22.1-17.9 40-40 40zm-88 96l16 32L176 480 128 288l64 32zm128-32L272 480 240 352l16-32 64-32z"/></svg>
    </a>
  </li>
  <li v-if="isAuthenticated && isAdmin">
    <a href="/src/dist/dashboard/index.html" class="p-2 mx-1">
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

<script setup>
import { ref, onMounted } from 'vue';
  const isAuthenticated = ref(false);
  const isAdmin = ref(false);
  const userId =  ref(false);

    const loadAuthState = () => {
      const authenticated = localStorage.getItem("token");
      isAuthenticated.value = !!authenticated;
      const userIdParam = localStorage.getItem("userId");
      userId.value = userIdParam;
      const role =localStorage.getItem("role");
      if (role == "admin") isAdmin.value = "true";  
    };
    const logout = () => {
      localStorage.removeItem("token");
      isAuthenticated.value = false;
      isAdmin.value = false;
      userId.value = null;
      window.history.pushState({}, "", "/");
      window.location.reload();
  };
  onMounted(loadAuthState);
</script>
