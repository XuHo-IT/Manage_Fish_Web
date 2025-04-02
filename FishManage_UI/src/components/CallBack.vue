<template>
  <div class="container-login100-form-btn m-t-17 exit-btn">
    <a href="https://localhost:5173/">
      <button type="submit" class="login100-form-btn">Back</button>
    </a>
  </div>
  <div class="payment-status">
    <h1 :class="{ 'text-green-500': isSuccess }">
      {{ isSuccess ? "Payment Successful!" : "Payment Failed!" }}
    </h1>
    <div class="order-details">
      <p><strong>Order ID:</strong> {{orderId ||  orderMOMOId }}</p>
      <p><strong>User Name:</strong> {{  userNameMOMO ||   userName }}</p>
      <p><strong>Address:</strong> {{ addressMOMO ||  address }}</p>
      <p><strong>Phone Number:</strong> {{  phoneNumberMOMO ||  phoneNumber}}</p>
      <p><strong>Email:</strong> {{  emailMOMO || email  }}</p>
      <p><strong>Order Date:</strong> {{ orderDate || new Date().toLocaleString() }}</p>
      <p><strong>Total Amount:</strong> {{ amountMOMO ||amount }} VND</p>
      <p><strong>Payment Method:</strong> {{payTypeMOMO || payType }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";

// Extracting parameters from URL dynamically
const params = new URLSearchParams(window.location.search);
const orderMOMOId = params.get("orderId") || "";
const amountMOMO = params.get("amount") || "";
const payTypeMOMO = params.get("payType") || "";
const isSuccessMOMO = params.get("status") === "0"; // Assume MOMO success status is "0"
const userNameMOMO = "Your name payment kept screet throughout MOMO";
const addressMOMO = "Your address kept screet throughout MOMO";
const phoneNumberMOMO = "Your phone number kept screet throughout MOM";
const emailMOMO = "Your email kept kept screet throughout MOM";
// Reactive state variables
const orderId = ref("");
const userName = ref("");
const address = ref("");
const phoneNumber = ref("");
const email = ref("");
const orderDate = ref("");
const amount = ref("");
const payType = ref("");
const isSuccess = ref(false);

onMounted(() => {
  const storedData = sessionStorage.getItem("callbackData");

  if (storedData) {
    const callbackData = JSON.parse(storedData);
    orderId.value = callbackData.orderId;
    userName.value = callbackData.name;
    address.value = callbackData.address;
    phoneNumber.value = callbackData.phoneNumber;
    email.value = callbackData.email;
    orderDate.value = callbackData.orderDate;
    amount.value = callbackData.amount;
    payType.value = callbackData.payType;
    isSuccess.value = callbackData.status === "success";

    console.log("Retrieved Normal Payment Data:", callbackData);
  } else if (orderMOMOId) {
    // **MOMO Payment Flow**
    orderId.value = orderMOMOId;
    amount.value = amountMOMO;
    payType.value = payTypeMOMO;
    isSuccess.value = isSuccessMOMO;

    console.log("Retrieved MOMO Payment Data:", {
      orderMOMOId,
      amountMOMO,
      payTypeMOMO,
      isSuccessMOMO,
    });
  } else {
    console.log("No payment data found.");
  }
});
</script>

<style scoped>
.payment-status {
  max-width: 600px;
  margin: 50px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background: #f9f9f9;
  text-align: center;
}
.order-details {
  margin-top: 20px;
  text-align: left;
}
.text-green-500 {
  color: green;
}
</style>
