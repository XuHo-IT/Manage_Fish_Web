<template>
  <div class="container" style="max-width: 100%">
    <div class="main-body">
      <!-- Breadcrumb -->
      <nav aria-label="breadcrumb" class="main-breadcrumb">
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a href="index.html">Home</a></li>
          <li class="breadcrumb-item"><a href="javascript:void(0)">User</a></li>
          <li class="breadcrumb-item active" aria-current="page">User Profile</li>
        </ol>
      </nav>
      <!-- /Breadcrumb -->

      <div class="row gutters-sm">
        <form class="col-md-6 mb-3" @submit.prevent="updateUser">
          <div class="col-md-12 mb-3">
            <div class="card">
              <div class="card-body">
                <div class="d-flex flex-column align-items-center text-center">
                  <img :src="user.imageUrl" alt="Admin" class="rounded-circle" width="150" />
                  <div class="mt-3">
                    <h4>Your Name: </h4>
                    <input
                      type="text"
                      name="name"
                      id="name"
                      class="formbold-form-input"
                      v-model="user.name"
                      required
                    />
                    <p class="text-secondary mb-1">Number No: {{ userId }}</p>
                    <div>
                      <label for="imageURl" class="formbold-form-label mr-3">Upload your img</label>
                      <input
                        type="file"
                        name="imageURl"
                        id="imageURl"
                        class="formbold-form-input"
                        @change="handleFileUploadUpdate"
                        required
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-md-12">
            <div class="card mb-3">
              <div class="card-body">
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Full Name</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="text"
                      name="userName"
                      id="userName"
                      class="formbold-form-input"
                      v-model="user.userName"
                      required
                    />
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Email</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="text"
                      name="userEmail"
                      id="userEmail"
                      class="formbold-form-input"
                      v-model="user.userEmail"
                      required
                    />
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Phone</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="text"
                      name="phone"
                      id="phone"
                      class="formbold-form-input"
                      v-model="user.phone"
                      required
                    />
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Gender</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <select v-model="genderComputed">
                      <option value="Male">Male</option>
                      <option value="Female">Female</option>
                    </select>
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Address</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="text"
                      name="address"
                      id="address"
                      class="formbold-form-input"
                      v-model="user.address"
                      required
                    />
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">Date Of Birth</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="date"
                      name="dateOfBirth"
                      id="dateOfBirth"
                      class="formbold-form-input"
                      v-model="user.dateOfBirth"
                      required
                    />       {{ user.dateOfBirth }}
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-3">
                    <h6 class="mb-0">New Password</h6>
                  </div>
                  <div class="col-sm-9 text-secondary">
                    <input
                      type="text"
                      name="newPassword"
                      id="newPassword"
                      class="formbold-form-input"
                      v-model="user.newPassword"
                      required
                    />
                  </div>
                </div>
                <hr />
                <div class="row">
                  <div class="col-sm-12">
                    <button class="btn btn-info" type="submit">Edit</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </form>
        <div class="col-md-6 mb-3">
          <div class="card">
            <div class="card-body">
              <div class="d-flex flex-column align-items-center text-center">
                <div class="mt-3">
                  <h2>Your Order</h2>
                  <table>
                    <thead>
                      <tr>
                        <th>Order ID</th>
                        <th>Order Date</th>
                        <th>Total Amount</th>
                        <th>Payment Method</th>
                        <th>Product Name</th>
                        <th>Price</th>
                        <th>Quantity</th>
                        <th>Image</th>
                      </tr>
                    </thead>
                    <tbody v-for="order in orders" :key="order.orderId">
                      <tr v-for="(product, index) in order.products" :key="product.productId">
                        <td v-if="index === 0" :rowspan="order.products.length">
                          {{ order.orderId }}
                        </td>
                        <td v-if="index === 0" :rowspan="order.products.length">
                          {{ new Date(order.orderDate).toLocaleString() }}
                        </td>
                        <td v-if="index === 0" :rowspan="order.products.length">
                          {{ order.totalAmount }} VND
                        </td>
                        <td v-if="index === 0" :rowspan="order.products.length">
                          {{ order.paymentMethod }}
                        </td>
                        <td>{{ product.productName }}</td>
                        <td>{{ product.price }}</td>
                        <td>{{ product.quantity }}</td>
                        <td>
                          <img :src="product.imageURl" alt="Product Image" width="50" />
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import api from "@/js/api_auth.js";
import { ref, onMounted, computed } from "vue";

const userId = ref(localStorage.getItem("userId"));
const user = ref({
  id: userId.value,
  userName: "",
  userEmail: "",
  name: "",
  role: "",
  phone: "",
  gender: false, 
  dateOfBirth: "",
  address: "",
  newPassword: "",
  imageUrl: null,
});
const orders = ref([]);
const imageFile = ref(null);
const message = ref(""); // Store update messages

// Fetch total orders for the user
const getTotalOrders = async () => {
  try {
    if (!userId.value) return;
    const response = await api.get(`FishOrderAPI/user/${userId.value}`);
    orders.value = Array.isArray(response.data.result)
      ? response.data.result
      : [response.data.result];
  } catch (error) {
    console.error("Error fetching total orders:", error);
  }
};

// Computed property for gender selection
const genderComputed = computed({
  get: () => (user.value.gender ? "Male" : "Female"),
  set: (newValue) => {
    user.value.gender = newValue === "Male";
  },
});

// Fetch user details by ID
const fetchUserById = async () => {
  try {
    if (!userId.value) return;
    const response = await api.get(`User/${userId.value}`);
    if (response.data.result) {
      user.value = {
        id: response.data.result.id,
        userName: response.data.result.userName,
        userEmail: response.data.result.email,
        name: response.data.result.name,
        phone: response.data.result.phoneNumber,
        gender: response.data.result.gender,
        dateOfBirth: response.data.result.dateOfBirth,
        address: response.data.result.address,
        role: response.data.result.role,
        imageUrl: response.data.result.imageUrl,
      };
    }
  } catch (error) {
    console.error("Error fetching user:", error);
  }
};

// Handle image file selection
const handleFileUploadUpdate = (event) => {
  const selectedFile = event.target.files[0];
  if (selectedFile) {
    imageFile.value = selectedFile;
  }
};

// Update user information
const updateUser = async () => {
  try {
    if (!user.value.id) {
      message.value = "User ID is missing.";
      return;
    }

    const formData = new FormData();
    formData.append("Id", user.value.id);
    formData.append("UserName", user.value.userName);
    formData.append("Name", user.value.name);
    formData.append("Email", user.value.userEmail);
    formData.append("PhoneNumber", user.value.phone);
    formData.append("Gender", user.value.gender);
    formData.append("Address", user.value.address);
    formData.append("DateOfBirth", user.value.dateOfBirth);
    formData.append("Role", user.value.role);

    if (user.value.newPassword) {
      formData.append("NewPassword", user.value.newPassword);
    }
    if (imageFile.value) {
      formData.append("imageFile", imageFile.value);
    }

    const response = await api.put(`User/${user.value.id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });

    if (response.data.isSuccess) {
      message.value = "User updated successfully!";
      await fetchUserById(); // Refresh data
    } else {
      message.value = response.data.errorMessages.join(", ");
    }
  } catch (error) {
    message.value = "An error occurred while updating the profile.";
    console.error(error);
  }
};

// Load data on mount
onMounted(() => {
  fetchUserById();
  getTotalOrders();
});
</script>

