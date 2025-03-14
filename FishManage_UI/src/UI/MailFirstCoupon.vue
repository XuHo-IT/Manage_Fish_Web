<template>
  <section>
    <div class="container-lg">
      <div
        class="bg-secondary text-light py-5 my-5"
        style="
          background: url(&quot;src/assets/images/banner-newsletter.jpg&quot;) no-repeat;
          background-size: cover;
        "
      >
        <div class="container">
          <div class="row justify-content-center">
            <div class="col-md-5 p-3">
              <div class="section-header">
                <h2 class="section-title display-5 text-light">
                  Get Discount on your first purchase
                </h2>
              </div>
              <p>Just Sign Up & Register now to become a member.</p>
            </div>
            <div class="col-md-5 p-3">
              <form @submit.prevent="requestCoupon">
                <div class="mb-3">
                  <label for="name" class="form-label d-none">Name</label>
                  <input
                    v-model="name"
                    type="text"
                    class="form-control form-control-md rounded-0"
                    name="name"
                    id="name"
                    placeholder="Name"
                    required
                  />
                </div>
                <div class="mb-3">
                  <label for="email" class="form-label d-none">Email</label>
                  <input
                    v-model="email"
                    type="email"
                    class="form-control form-control-md rounded-0"
                    name="email"
                    id="email"
                    placeholder="Email Address"
                    required
                  />
                </div>
                <div class="d-grid gap-2">
                  <button
                    type="submit"
                    class="btn btn-dark btn-md rounded-0"
                    :disabled="isSubmitting"
                  >
                    {{ isSubmitting ? "Processing..." : "Submit" }}
                  </button>
                </div>
              </form>
              <p v-if="message" class="mt-3 text-success">{{ message }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import api from "@/js/api_auth.js";
import { ref, onMounted } from "vue";

const name = ref();
const email = ref();
const message = ref();
const isSubmitting = ref(false);

const requestCoupon = async () => {
  if (isSubmitting.value) return;
  isSubmitting.value = "true";

  if (document.cookie.includes("couponReceived=true")) {
    message.value = "You have already received a coupon.";
    isSubmitting.value = "false";
    return;
  }

  try {
    const response = await api.get("CouponModel");
    const coupons = await response.data;

    if (!coupons.isSuccess) {
      message.value = "No coupons available at the moment.";
      isSubmitting.value = "false";
      return;
    }

    const validCoupons = coupons.result.filter((coupon) => coupon.quantity > 0);

    if (validCoupons.length === 0) {
      message.value = "No valid coupons available.";
      return;
    }

    const randomCoupon = validCoupons[Math.floor(Math.random() * validCoupons.length)].couponCode;

    console.log(randomCoupon);

    const sendResponse = await api.post(
      "CouponModel/FirstCoupon",
      JSON.stringify({
        email: this.email,
        subject: "Hi" + this.name + "Here is your first coupon! Enjoy your discount!",
        message: `Your coupon code is: ${randomCoupon}`,
      }),
    );

    const result = await sendResponse.data();

    if (sendResponse.ok) {
      this.message = "Coupon sent successfully! Check your email.";
      document.cookie = "couponReceived=true; max-age=31536000"; // 1 year expiry
    } else {
      this.message = result.message || "Failed to send coupon.";
    }
  } catch (error) {
    console.error("Error:", error);
    this.message = "An error occurred while processing your request.";
  }

  isSubmitting.value = false;
};
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
