<template>
  <div>
    <h2>Orders List</h2>
    <table>
      <thead>
        <tr>
          <th>Order ID</th>
          <th>User ID</th>
          <th>Order Date</th>
          <th>Total Amount</th>
          <th>Payment Method</th>
          <th>Product Name</th>
          <th>Price</th>
          <th>Quantity</th>
          <th>Image</th>
        </tr>
      </thead>
      <tbody>
        <template v-for="order in orders" :key="order.orderId">
          <tr v-for="(product, index) in order.products" :key="product.productId">
            <td v-if="index === 0" :rowspan="order.products.length">{{ order.orderId }}</td>
            <td v-if="index === 0" :rowspan="order.products.length">{{ order.userId }}</td>
            <td v-if="index === 0" :rowspan="order.products.length">
              {{ new Date(order.orderDate).toLocaleString() }}
            </td>
            <td v-if="index === 0" :rowspan="order.products.length">{{ order.totalAmount }} VND</td>
            <td v-if="index === 0" :rowspan="order.products.length">{{ order.paymentMethod }}</td>
            <td>{{ product.productName }}</td>
            <td>{{ product.price }}</td>
            <td>{{ product.quantity }}</td>
            <td>
              <img :src="product.imageURl" alt="Product Image" width="50" />
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import api from "@/js/api_auth.js";
import { ref, onMounted } from "vue";

const orders = ref([]);

const getTotalOrders = async () => {
  try {
    const response = await api.get("FishOrderAPI");
    orders.value = response.data.result;
  } catch (error) {
    console.error("Error fetching total orders:", error);
  }
};
onMounted(getTotalOrders);
</script>
