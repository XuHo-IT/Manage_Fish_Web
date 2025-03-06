<template>
  <table>
    <thead>
      <tr>
        <th>ORDER ID</th>
        <th>PRDUCT ID</th>
        <th>QUANTITY</th>
        <th>PAYMENT</th>
        <th>ORDER DATE</th>
        <th class="text-end">TOTAL AMOUNT</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(order, index) in orders" :key="index">
        <td>
          <a href="#" class="text-muted">{{ order.orderId }}</a>
        </td>
        <td>{{ order.productId }}</td>
        <td>{{ order.quantity }}</td>
        <td>{{ order.paymentMethod }}</td>
        <td>{{ order.orderDate }}</td>
        <td class="text-end">${{ order.totalAmount }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import axios from "axios";
import { ref, onMounted } from "vue";

export default {
  name: "TotalReport",
  setup() {
    const orders = ref([]);
    const apiOrder = "https://localhost:7229/api/FishOrderAPI";

    const getTotalOrders = async () => {
      try {
        const token = localStorage.getItem("token");
        console.log("token: " + token);
        const response = await axios.get(apiOrder, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        orders.value = response.data.result;
      } catch (error) {
        console.error("Error fetching total orders:", error);
      }
    };

    onMounted(getTotalOrders);

    return {
      orders,
    };
  },
};
</script>
