<template>
  <div class="row">
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Users</h6>
          <h4 class="mb-3">
            {{ totalUsers }}
            <span class="badge bg-light-success border border-success"
              ><i class="ti ti-trending-up"></i> 70.5%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra <span class="text-success">8,900</span> this year
          </p>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Product</h6>
          <h4 class="mb-3">
            {{ totalProducts }}
            <span class="badge bg-light-warning border border-warning"
              ><i class="ti ti-trending-down"></i> 27.4%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra <span class="text-warning">1,943</span> this year
          </p>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Sales</h6>
          <h4 class="mb-3" v-if="isAdmin">
            {{ totalOrders }}
            <span class="badge bg-light-danger border border-danger"
              ><i class="ti ti-trending-down"></i> 27.4%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra
            <span class="text-danger">$20,395</span> this year
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref,onMounted } from "vue";
import api from "@/js/api_auth.js";

const totalUsers = ref(0);
const totalProducts = ref(0);
const totalOrders = ref(0);
const isAuthenticated = ref(false);
const isAdmin = ref(false);
const userId = ref(null);

const loadAuthState = () => {
  const authenticated = localStorage.getItem("token");
  isAuthenticated.value = !!authenticated;
  const userIdParam = localStorage.getItem("userId");
  userId.value = userIdParam;
  const role = localStorage.getItem("role");
  if (role == "admin") isAdmin.value = "true";
};
const getTotalUsers = async () => {
  try {
    const response = await api.get("/User");
    totalUsers.value = response.data.result.length;
  } catch (error) {
    console.error("Error fetching total users:", error);
  }
};
const getTotalProducts = async () => {
  try {
    const response = await api.get("FishProductAPI");
    totalProducts.value = response.data.result.length;
  } catch (error) {
    console.error("Error fetching total products:", error);
  }
};
const getTotalOrders = async () => {
  try {
    const response = await api.get("FishOrderAPI");
    totalOrders.value = response.data.result.length;
  } catch (error) {
    console.error("Error fetching total orders:", error);
  }
};
onMounted(() => {
  loadAuthState();
  getTotalUsers();
  getTotalProducts();
  getTotalOrders();
});
</script>
