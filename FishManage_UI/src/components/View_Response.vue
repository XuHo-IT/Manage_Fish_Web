<template>
  <div class="col-md-12 col-xl-8" style="width: 100%;">
    <div class="d-flex align-items-center justify-content-between mb-3">
      <h5 class="mb-0">Unique Visitor</h5>
      <ul class="nav nav-pills justify-content-end mb-0" id="chart-tab-tab" role="tablist">
        <li class="nav-item" role="presentation">
          <button
            class="nav-link"
            id="chart-tab-home-tab"
            data-bs-toggle="pill"
            data-bs-target="#chart-tab-home"
            type="button"
            role="tab"
            aria-controls="chart-tab-home"
            aria-selected="true"
          >
            Month
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button
            class="nav-link active"
            id="chart-tab-profile-tab"
            data-bs-toggle="pill"
            data-bs-target="#chart-tab-profile"
            type="button"
            role="tab"
            aria-controls="chart-tab-profile"
            aria-selected="false"
          >
            Week
          </button>
        </li>
      </ul>
    </div>
    <div class="card">
      <div class="card-body">
        <div class="tab-content" id="chart-tab-tabContent">
          <div
            class="tab-pane"
            id="chart-tab-home"
            role="tabpanel"
            aria-labelledby="chart-tab-home-tab"
            tabindex="0"
          >
            <div id="visitor-chart-1"></div>
          </div>
          <div
            class="tab-pane show active"
            id="chart-tab-profile"
            role="tabpanel"
            aria-labelledby="chart-tab-profile-tab"
            tabindex="0"
          >
            <div class="d-flex justify-content-between mb-3">
              <div><strong>Total Views:</strong> {{ totalViews }}</div>
              <div><strong>Total Sessions:</strong> {{ totalSessions }}</div>
            </div>
            <div id="visitor-chart">
              <VueApexCharts
                v-if="isChartReady"
                type="bar"
                height="450"
                :options="chartOptions"
                :series="series"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="card-body">
        <div class="tab-content" id="chart-tab-tabContent">
          <table class="table">
            <thead>
              <tr>
                <!-- <th>Order ID</th> -->
                <th>User ID</th>
                <th>Username</th>
                <th>Name</th>
                <th>Email</th>
                <th>PhoneNumber</th>
                <th>Gender</th>
                <th>DateOfBirth</th>
                <th>Address</th>
                <th>Current Role</th>
                <th>Change Role</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(user, index) in users" :key="index">
                <!-- <td>
                    <a href="#" class="text-muted">{{ user.orderId }}</a>
                  </td> -->
                <td>{{ user.id }}</td>
                <td>{{ user.userName }}</td>
                <td>{{ user.name }}</td>
                <td>{{ user.email }}</td>
                <td>{{ user.phoneNumber }}</td>
                <td>{{ user.gender }}</td>
                <td>{{ user.dateOfBirth }}</td>
                <td>{{ user.address }}</td>
                <td>{{ user.role }}</td>
                <td class="text-end">
                  <select v-model="user.role" @change="updateUserRole(user)">
                    <option
                      v-for="selectedRole in roles"
                      :key="selectedRole"
                      :value="selectedRole || user.role"
                    >
                      {{ selectedRole }}
                    </option>
                  </select>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, defineAsyncComponent, nextTick } from "vue";
import api from "@/js/api_auth.js";

// Lazy-load VueApexCharts to ensure it's available
const VueApexCharts = defineAsyncComponent(() => import("vue3-apexcharts"));

const users = ref([]);
const totalViews = ref(0);
const totalSessions = ref(0);
const series = ref([]);
const isChartReady = ref(false);
const roles = ref(["Admin", "Customer"]);
import Swal from "sweetalert2";

const chartOptions = ref({
  chart: {
    type: "bar",
    height: 350,
  },
  plotOptions: {
    bar: {
      horizontal: false,
    },
  },
  xaxis: {
    categories: ["Total Views", "Active Sessions"],
  },
  dataLabels: {
    enabled: true,
  },
});

const updateUserRole = async (user) => {
  try {
    const updatedRole = { role: user.role };

    await api.put(`User/UserRole/${user.id}`, updatedRole, {
      headers: { "Content-Type": "application/json" },
    });
    Swal.fire({
    icon: "success",
    title: "Added to Cart!",
    text: `Updated role for ${user.userName} to ${user.role}`,
    timer: 2000,
    showConfirmButton: false,
  }); 
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "Invalid Quantity",
      text: "Error updating user role: " + error.response?.data?.message || error.message,
    }); 
  }
};

const fetchVisitorData = async () => {
  try {
    const response = await api.get("Analytics/GetAnalytics");
    if (response.data) {
      totalViews.value = response.data.totalViews;
      totalSessions.value = response.data.activeSessions;

      series.value = [
        {
          name: "Metrics",
          data: [totalViews.value, totalSessions.value],
        },
      ];

      await nextTick();
      isChartReady.value = true;
    }
  } catch (error) {
    console.error("Error fetching visitor stats:", error);
  }
};

// Fetch user data
const fetchUser = async () => {
  try {
    const response = await api.get("User");
    if (response.data) {
      users.value = response.data.result;
    }
  } catch (error) {
    console.error("Error fetching users:", error);
  }
};

// Fetch data on mounted
onMounted(() => {
  fetchVisitorData();
  fetchUser();
});
</script>
