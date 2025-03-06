<template>
  <div class="col-md-12 col-xl-8">
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
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, defineAsyncComponent, nextTick } from "vue";
import axios from "axios";

// Lazy-load VueApexCharts to ensure it's available
const VueApexCharts = defineAsyncComponent(() => import("vue3-apexcharts"));

const totalViews = ref(0);
const totalSessions = ref(0);
const series = ref([]);
const isChartReady = ref(false);
const apiBaseUrl = "https://localhost:7229/api/Analytics";

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

const fetchVisitorData = async () => {
  try {
    const response = await axios.get(`${apiBaseUrl}/GetAnalytics`);

    if (response.data) {
      totalViews.value = response.data.totalViews;
      totalSessions.value = response.data.activeSessions;

      series.value = [
        {
          name: "Metrics",
          data: [totalViews.value, totalSessions.value],
        },
      ];

      // Wait for DOM updates before setting chart ready
      await nextTick();
      isChartReady.value = true;
    }
  } catch (error) {
    console.error("Error fetching visitor stats:", error);
  }
};

onMounted(fetchVisitorData);
</script>
