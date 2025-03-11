<template>
  <div class="formbold-main-wrapper">
    <div class="formbold-form-wrapper">
      <form @submit.prevent="createCoupon">
        <h2 class="formbold-form-title">Create coupon</h2>

        <div class="formbold-input-flex">
          <div>
            <label for="couponName" class="formbold-form-label">Coupon Code</label>
            <input
              type="text"
              name="couponName"
              id="couponName"
              class="formbold-form-input"
              v-model="newCoupon.couponCode"
              required
            />
            <button @click="generateCouponCode">Generate Code</button>
          </div>
        </div>

        <div class="formbold-input-flex">
          <div>
            <label for="couponDescription" class="formbold-form-label">Coupon Description</label>
            <input
              type="text"
              name="couponDescription"
              id="couponDescription"
              class="formbold-form-input"
              v-model="newCoupon.couponDescription"
              required
            />
          </div>
        </div>

        <div class="formbold-input-flex">
          <div>
            <label for="quantity" class="formbold-form-label">Quanity</label>
            <input
              type="number"
              name="quantity"
              id="quantity"
              class="formbold-form-input"
              v-model="newCoupon.quantity"
              required
            />
          </div>
        </div>

        <div>
          <label for="dateStart" class="formbold-form-label">Date Start</label>
          <input
            type="date"
            name="dateStart"
            id="dateStart"
            class="formbold-form-input"
            v-model="newCoupon.dateStart"
            required
          />
        </div>
        <div>
          <label for="dateEnd" class="formbold-form-label">Date End</label>
          <input
            type="date"
            name="dateEnd"
            id="dateEnd"
            class="formbold-form-input"
            v-model="newCoupon.dateEnd"
            required
          />
        </div>

        <button class="formbold-btn btn-primary">Create</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import api from "@/js/api_auth.js";


const newCoupon = ref({
  couponCode: "",
  couponDescription: "",
  quantity: null,
  dateStart: null,
  dateEnd: null,
});


const generateCouponCode = () => {
  newCoupon.value.couponCode = Math.random().toString(36).substring(2, 7).toUpperCase();
};


const createCoupon = async () => {
  const formData = {
    couponCode: newCoupon.value.couponCode,
    couponDescription: newCoupon.value.couponDescription,
    quantity: newCoupon.value.quantity,
    dateStart: newCoupon.value.dateStart,
    dateEnd: newCoupon.value.dateEnd,
  };

  try {
    await api.post("CouponModel/CreateCoupon", formData);
    alert("Coupon created successfully!");
  } catch (error) {
    alert("Coupon creation failed!");
    console.error("Error:", error);
  }
};
</script>

<style src="@/assets/css/style.css"></style>
<style src="@/assets/css/vendor.css"></style>
<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap");
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  width: 100%;
}
body {
  font-family: "Inter", sans-serif;
}
.formbold-main-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 48px;
}

.formbold-form-wrapper {
  margin: 0 auto;
  max-width: 550px;
  width: 100%;
  background: white;
}

.formbold-event-wrapper span {
  font-weight: 500;
  font-size: 16px;
  line-height: 24px;
  letter-spacing: 2.5px;
  color: #6a64f1;
  display: inline-block;
  margin-bottom: 12px;
}
.formbold-event-wrapper h3 {
  font-weight: 700;
  font-size: 28px;
  line-height: 34px;
  color: #07074d;
  width: 60%;
  margin-bottom: 15px;
}
.formbold-event-wrapper h4 {
  font-weight: 600;
  font-size: 20px;
  line-height: 24px;
  color: #07074d;
  width: 60%;
  margin: 25px 0 15px;
}
.formbold-event-wrapper p {
  font-size: 16px;
  line-height: 24px;
  color: #536387;
}

.formbold-event-details {
  background: #fafafa;
  border: 1px solid #dde3ec;
  border-radius: 5px;
  margin: 25px 0 30px;
}
.formbold-event-details h5 {
  color: #07074d;
  font-weight: 600;
  font-size: 18px;
  line-height: 24px;
  padding: 15px 25px;
}
.formbold-event-details ul {
  border-top: 1px solid #edeef2;
  padding: 25px;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  row-gap: 14px;
}
.formbold-event-details ul li {
  color: #536387;
  font-size: 16px;
  line-height: 24px;
  width: 50%;
  display: flex;
  align-items: center;
  gap: 10px;
}

.formbold-form-title {
  color: #07074d;
  font-weight: 600;
  font-size: 28px;
  line-height: 35px;
  width: 60%;
  margin-bottom: 30px;
}

.formbold-input-flex {
  display: flex;
  gap: 20px;
  margin-bottom: 15px;
}
.formbold-input-flex > div {
  width: 50%;
}
.formbold-form-input {
  text-align: center;
  width: 100%;
  padding: 13px 22px;
  border-radius: 5px;
  border: 1px solid #dde3ec;
  background: #ffffff;
  font-weight: 500;
  font-size: 16px;
  color: #536387;
  outline: none;
  resize: none;
}
.formbold-form-input:focus {
  border-color: #6a64f1;
  box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.05);
}
.formbold-form-label {
  color: #536387;
  font-size: 14px;
  line-height: 24px;
  display: block;
  margin-bottom: 10px;
}

.formbold-policy {
  font-size: 14px;
  line-height: 24px;
  color: #536387;
  width: 70%;
  margin-top: 22px;
}
.formbold-policy a {
  color: #6a64f1;
}
.formbold-btn {
  text-align: center;
  width: 100%;
  font-size: 16px;
  border-radius: 5px;
  padding: 14px 25px;
  border: none;
  font-weight: 500;
  background-color: #6a64f1;
  color: white;
  cursor: pointer;
  margin-top: 25px;
}
.formbold-btn:hover {
  box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.05);
}
</style>
