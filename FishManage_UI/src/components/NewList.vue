<template>
  <div class="relative w-full overflow-hidden p-4">
    <div class="flex justify-between mb-2">
      <button @click="prev" :disabled="startIndex === 0" class="p-2 bg-gray-200 rounded">◀</button>
      <button
        @click="next"
        :disabled="startIndex + itemsPerPage >= newsData.length"
        class="p-2 bg-gray-200 rounded"
      >
        ▶
      </button>
    </div>
    <div class="flex gap-4 overflow-x-auto">
      <div
        v-for="(news, index) in visibleNews"
        :key="index"
        class="w-72 shrink-0 border rounded-lg overflow-hidden"
      >
        <img :src="news.urlToImage" :alt="news.title" class="w-full h-40 object-cover" />
        <div class="p-4">
          <h2 class="font-bold text-lg">{{ news.title }}</h2>
          <p class="text-sm text-gray-500">
            {{ news.source.name }} - {{ formatDate(news.publishedAt) }}
          </p>
          <a :href="news.url" target="_blank" class="text-blue-600 hover:underline">Read More</a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from "vue";

export default {
  setup() {
    const newsData = ref([]);
    const startIndex = ref(0);
    const itemsPerPage = 7;

    const fetchNews = async () => {
      try {
        const yesterday = new Date();
        yesterday.setDate(yesterday.getDate() - 1); 
        const formattedDate = yesterday.toISOString().split("T")[0]; 

        const response = await fetch(
          `https://newsapi.org/v2/everything?q=Apple&from=${formattedDate}&apiKey=bd58ad8404154d16a3b28813ff007938`,
        );

        const data = await response.json();
        if (data.articles) {
          newsData.value = data.articles;
        }
      } catch (error) {
        console.error("Error fetching news:", error);
      }
    };

    const visibleNews = computed(() =>
      newsData.value.slice(startIndex.value, startIndex.value + itemsPerPage),
    );

    const next = () => {
      if (startIndex.value + itemsPerPage < newsData.value.length) {
        startIndex.value += itemsPerPage;
      }
    };

    const prev = () => {
      if (startIndex.value > 0) {
        startIndex.value -= itemsPerPage;
      }
    };

    const formatDate = (dateString) => {
      return new Date(dateString).toLocaleDateString();
    };

    onMounted(fetchNews);

    return { newsData, visibleNews, startIndex, itemsPerPage, next, prev, formatDate };
  },
};
</script>
