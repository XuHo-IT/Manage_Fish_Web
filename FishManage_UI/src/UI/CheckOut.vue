<template>
  <div class="row">
    <form @submit.prevent="createPayment" class="col-6">
      <div class="mb-3">
        <label for="name" class="form-label">Name</label>
        <input type="text" class="form-control" v-model="userInfo.FullName" required />
      </div>
      <div class="mb-3">
        <label for="phoneNumber" class="form-label">Phone Number</label>
        <input type="text" class="form-control" v-model="userInfo.phoneNumber" required />
      </div>

      <div class="mb-3">
        <label for="city" class="form-label">City</label>
        <input type="text" class="form-control" v-model="userInfo.city" required />
      </div>
      <div class="mb-3">
        <label for="zipCode" class="form-label">Zip Code</label>
        <input type="text" class="form-control" v-model="userInfo.zipCode" required />
      </div>
      <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input type="email" class="form-control" v-model="userInfo.email" required />
      </div>
      <div class="mb-3">
        <label for="province" class="form-label">Province</label>
        <select id="province" class="form-control" v-model="selectedProvince">
          <option v-for="province in provinces" :key="province.id" :value="province.id">
            {{ province.full_name }}
          </option>
        </select>
      </div>
      <div class="mb-3">
        <label for="district" class="form-label">District</label>
        <select id="district" class="form-control" v-model="selectedDistrict">
          <option v-for="district in districts" :key="district.id" :value="district.id">
            {{ district.full_name }}
          </option>
        </select>
      </div>
      <div class="mb-3">
        <label for="ward" class="form-label">Ward</label>
        <select id="ward" class="form-control" v-model="selectedWard">
          <option v-for="ward in wards" :key="ward.id" :value="ward.id">
            {{ ward.full_name }}
          </option>
        </select>
      </div>
      <div class="mb-3" style="display: none">
        <label for="address" class="form-label">Address</label>
        <input type="text" class="form-control" v-model="userInfo.address" required />
      </div>

      <button
        type="submit"
        class="btn btn-primary btn-block btn-lg"
        style="background-color: palevioletred"
      >
        Pay By MoMo
      </button>
    </form>

    <!-- Order Recap -->
    <div class="col-6">
      <div v-for="(item, index) in cart" :key="index">
        <h6>{{ item.productName }}</h6>
        <p>Quantity: {{ item.quantity }}</p>
        <p>Price: ${{ item.price }}</p>
      </div>
      <div class="d-flex justify-content-between">
        <h5>Bill total:</h5>
        <h5>${{ total.toFixed(2) }}</h5>
      </div>
      <div class="d-flex justify-content-between">
        <h5>Shipping:</h5>
        <h5 v-if="shippingPrice !== null">${{ shippingPrice }}</h5>
      </div>
      <div class="mb-3">
        <label for="couponCode" class="form-label">Coupon Code</label>
        <input type="text" class="form-control" v-model="selectedCouponCode" required />
        <div v-if="selectedCoupon">
          <p>{{ selectedCoupon.couponDescription }}</p>
          <div class="d-flex justify-content-between">
            <h5>Total total:</h5>
            <h5>${{ newTotal }}</h5>
          </div>
        </div>
        <div v-else>
          <p>No Coupon Code Available</p>
          <div class="d-flex justify-content-between">
            <h5>Total total:</h5>
            <h5>${{ newTotal = parseFloat(total.toFixed(2)) + parseFloat(shippingPrice) }}</h5>
          </div>
        </div>

        <button
          type="button"
          class="btn btn-primary btn-block btn-lg"
          style="background-color: green"
          @click="checkCoupon"
        >
          Check Coupon
        </button>
      </div>

      <button
        type="button"
        class="btn btn-primary btn-block btn-lg"
        style="background-color: green"
        @click="createPaymentByCOD"
      >
        Pay By COD
      </button>

      <button type="submit" class="btn btn-primary btn-block btn-lg">Pay By VNPay</button>
      <h6>
        <a href="https://localhost:5173/">Cancel and return to website</a>
      </h6>
      <router-view></router-view>
    </div>
  </div>
</template>

<style>
.css_select_div {
  text-align: center;
}
.css_select {
  display: inline-table;
  width: 25%;
  padding: 5px;
  margin: 5px 2%;
  border: solid 1px #686868;
  border-radius: 5px;
}
</style>

<script setup>
import { ref, onMounted, watch } from "vue";
import { useRouter } from "vue-router";
import api from "@/js/api_auth.js";
const isAuthenticated = ref(false);
const isAdmin = ref(false);
const userId = ref(null);

const loadAuthState = () => {
      const authenticated = localStorage.getItem("token");
      isAuthenticated.value = !!authenticated;
      const userIdParam = localStorage.getItem("userId");
      userId.value = userIdParam;
      const role =localStorage.getItem("role");
      if (role == "admin") isAdmin.value = "true";  
    };
const cart = ref([]);
const total = ref(0);
const userInfo = ref({
  userId: localStorage.getItem("userId") || "",
  FullName: "",
  phoneNumber: "",
  address: "",
  zipCode: "",
  email: "",
});
const shippingPrice = ref(0);

const router = useRouter();

const provinces = ref([]);
const districts = ref([]);
const wards = ref([]);
const selectedProvince = ref("0");
const selectedDistrict = ref("0");
const selectedWard = ref("0");

const coupons = ref([]);
const selectedCouponCode = ref("");
const selectedCoupon = ref(null);
const newTotal = ref(0);
const users = ref([]);
const fetchCoupons = async () => {
  try {
    const response = await api.get("CouponModel");
    const data = await response.data;
    if (data.isSuccess) {
      coupons.value = data.result;
      console.log(coupons.value);
    }
  } catch (error) {
    console.error("Error fetching coupons:", error);
  }
};

const fetchProvinces = async () => {
  try {
    const response = await fetch("https://esgoo.net/api-tinhthanh/1/0.htm");
    const data = await response.json();
    if (data.error === 0) {
      provinces.value = data.data;
    }
  } catch (error) {
    console.error("Error fetching provinces:", error);
  }
};

watch(selectedProvince, async (newVal) => {
  if (newVal !== "0") {
    try {
      const response = await fetch(`https://esgoo.net/api-tinhthanh/2/${newVal}.htm`);
      const data = await response.json();
      if (data.error === 0) {
        districts.value = data.data;
        wards.value = []; // Reset wards
      }
    } catch (error) {
      console.error("Error fetching districts:", error);
    }
  }
});

watch(selectedDistrict, async (newVal) => {
  if (newVal !== "0") {
    try {
      const response = await fetch(`https://esgoo.net/api-tinhthanh/3/${newVal}.htm`);
      const data = await response.json();
      if (data.error === 0) {
        wards.value = data.data;
      }
    } catch (error) {
      console.error("Error fetching wards:", error);
    }
  }
});
watch([selectedProvince, selectedDistrict, selectedWard], () => {
  const province = provinces.value.find((p) => p.id === selectedProvince.value)?.full_name || "";
  const district = districts.value.find((d) => d.id === selectedDistrict.value)?.full_name || "";
  const ward = wards.value.find((w) => w.id === selectedWard.value)?.full_name || "";

  userInfo.value.address = [ward, district, province].filter(Boolean).join(", ");
});

watch(
  userInfo,
  (newVal) => {
    if (Object.values(newVal).every((field) => field.trim() !== "")) {
      shippingPrice.value = (Math.random() * (2 - 0.25) + 0.25).toFixed(2);
    }
  },
  { deep: true },
);
const checkCoupon = async () => {
  selectedCoupon.value =
    coupons.value.find((c) => c.couponCode === selectedCouponCode.value) || null;

  if (!selectedCoupon.value || !selectedCoupon.value.couponId) {
    alert("Please enter a valid coupon code.");
    return;
  }

  const totalAmount = total.value ? Math.round(total.value) : 0;
  const shippingAmount = shippingPrice.value ? parseFloat(shippingPrice.value) : 0;

  const moneyDouble = totalAmount + shippingAmount;
  const requestData = {
    money: moneyDouble,
    couponId: selectedCoupon.value.couponId,
  };

  try {
    const response = await api.post("CouponModel/ApplyDiscount", requestData);

    const data = await response.data;

    if (response.status === 200) {
      newTotal.value = data.newTotal.toFixed(2).toString();
    } else {
      alert("Failed to apply the coupon.");
    }
  } catch (error) {
    console.error("Error during coupon validation:", error);
    alert("An error occurred while applying the coupon.");
  }
};
const fetchUserById = async () => {
  try {
    const response = await api.get(`User/${localStorage.getItem("userId")}`);
    const data = response.data; 
    if (!data) {
      console.error("Invalid user data:", data);
      return null;
    }
    return data.result; 
  } catch (error) {
    console.error("Error fetching user:", error);
    return null; 
  }
};

const createPaymentByCOD = async () => {
  if (isAuthenticated.value = true) {
    try {
      const user = await fetchUserById();
      console.log("User"+user);
      const cartItems = JSON.parse(localStorage.getItem("cart") || "[]");
      console.log(cartItems);
      const response = await api.post(
        "PaymentAPI/CreatePaymentCOD",
     {
          orderId: new Date().getTime().toString(),
          userId: localStorage.getItem("userId"),
          name: user.name,
          address: user.address,
          email: user.email,
          phoneNumber: user.phoneNumber,
          totalAmount: Math.round(newTotal.value * 25.475 * 10000).toString(),
          paymentMethod: "COD",
          products: cartItems.map((item) => ({
            productId: item.productId,
            quantity: item.quantity,
          })),
        },
      );
      const textResponse = await response.data;
      router.push({
        name: "CallBack",
        query: {
          orderId: textResponse.result.orderId,
          userId: textResponse.result.userId,
          amount: newTotal.value.toString(),
          payType: textResponse.result.paymentMethod,
          status: textResponse.isSuccess ? "success" : "failed",
        },
      });

      if (!response.ok) {
        throw new Error(textResponse);
      }
    } catch (error) {
      console.error("Error creating payment:", error);
    }
  } else {
    try {
      const cartItems = JSON.parse(localStorage.getItem("cart") || "[]");
      console.log(cartItems);
      const response = await api.post(
        "PaymentAPI/CreatePaymentCOD",
        JSON.stringify({
          orderId: new Date().getTime().toString(),
          userId: new Date().getTime().toString() + Math.floor(Math.random() * 1000),
          name: userInfo.value.FullName,
          address: userInfo.value.address,
          email: userInfo.value.email,
          phoneNumber: userInfo.value.phoneNumber,
          orderDate: new Date().toISOString(),
          totalAmount: Math.round(newTotal.value * 25.475 * 10000).toString(),
          paymentMethod: "COD",
          products: cartItems.map((item) => ({
            productId: item.productId,
            quantity: item.quantity,
          })),
        }),
      );
      const textResponse = await response.data;
      router.push({
        name: "CallBack",
        query: {
          orderId: textResponse.result.orderId,
          userId: textResponse.result.userId,
          amount: newTotal.value.toString(),
          payType: textResponse.result.paymentMethod,
          status: textResponse.isSuccess ? "success" : "failed",
        },
      });

      if (!response.ok) {
        throw new Error(textResponse);
      }
    } catch (error) {
      console.error("Error creating payment:", error);
    }
  }
};

const createPayment = async () => {
  if (isAuthenticated.value = true) {
    try {
      const user = await fetchUserById();
      const cartItems = JSON.parse(localStorage.getItem("cart") || "[]");
      const response = await api.post("PaymentAPI/CreatePaymentMomo", {
        orderId: new Date().getTime().toString(),
        userId: localStorage.getItem("userId"),
        name: user.name,
        address: user.address,
        email: user.email,
        phoneNumber: user.phoneNumber,
        orderDate: new Date().toLocaleString(),
        totalAmount: Math.round(newTotal.value * 25.475 * 10000).toString(),
        paymentMethod: "MoMo",
        products: cartItems.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      });

      console.log("Server response:", response.data);

      if (!response.data || !response.data.payUrl) {
        throw new Error("Invalid response from server");
      }
      window.location.href = response.data.payUrl;
    } catch (error) {
      console.error("Error creating payment:", error);
    }
  } else {
    try {
      const cartItems = JSON.parse(localStorage.getItem("cart") || "[]");
      const response = await api.post("PaymentAPI/CreatePaymentMomo", {
        orderId: new Date().getTime().toString(),
        userId: new Date().getTime().toString(),
        name: userInfo.value.FullName,
        address: userInfo.value.address,
        email: userInfo.value.email,
        phoneNumber: userInfo.value.phoneNumber,
        orderDate: new Date().toLocaleString(),
        totalAmount: Math.round(newTotal.value * 25.475 * 10000).toString(),
        paymentMethod: "MoMo",
        products: cartItems.map((item) => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      });

      console.log("Server response:", response.data);

      if (!response.data || !response.data.payUrl) {
        throw new Error("Invalid response from server");
      }
      window.location.href = response.data.payUrl;
    } catch (error) {
      console.error("Error creating payment:", error);
    }
  }
};

onMounted(() => {
  const cartData = localStorage.getItem("cart");
  const totalData = localStorage.getItem("total");
  if (cartData && totalData) {
    cart.value = JSON.parse(cartData);
    total.value = parseFloat(totalData);
  }
  fetchCoupons();
  fetchProvinces();
  loadAuthState();
});
</script>
