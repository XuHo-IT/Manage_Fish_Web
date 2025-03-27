<template>
  <div class="container-login100-form-btn m-t-17 exit-btn">
    <a href="https://localhost:5173/">
      <button type="submit" class="login100-form-btn">Back</button>
    </a>
  </div>
  <div class="payment-status">
    <h1 :class="{ 'text-green-500': isSuccess }">
      {{ "Payment Successful!" }}
    </h1>
    <div class="order-details">
      <p><strong>Order ID:</strong> {{ orderId }}</p>
      <p><strong>User ID:</strong> {{ userId }}</p>
      <p><strong>Order Date:</strong> {{ new Date().toLocaleString() }}</p>
      <p><strong>Total Amount:</strong> {{ amount }}vnd</p>
      <p><strong>Payment Method:</strong> {{ payType }}</p>
    </div>
  </div>
</template>

<script setup>
const params = new URLSearchParams(window.location.search);
const orderId = params.get("orderId");
const userId = params.get("requestId") ||params.get("userId") ;
const amount = params.get("amount");
const payType = params.get("payType");

async function handleRedirect() {
  try {
    const tokens = await authClient.token.parseFromUrl();
    authClient.tokenManager.setTokens(tokens);
    console.log("User logged in:", tokens);
    window.location.href = "/";  // Redirect to home
  } catch (error) {
    console.error("Error processing login:", error);
  }
}

Onmounted = () => {
  handleRedirect();
}


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
</style>
