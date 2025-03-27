<template>
  <div>
    <div class="d-flex flex-wrap align-items-center p-2 rounded-4">
      <!-- Search Bar -->
      <div class="col-md-4 d-none d-md-block p-2">
        <Voice />
        <div class="col-md-12" style="box-shadow: red">
          <form @submit.prevent="handleSearch" id="search-form" class="text-center">
            <input
              v-model="searchTerm"
              type="text"
              class="form-control border-0 bg-transparent"
              placeholder="Search for products name"
            />
          </form>
        </div>
      </div>

      <!-- Sort Order Dropdown -->
      <div class="col-md-4 p-2">
        <label for="sortOrder" class="form-label">Sort by:</label>
        <select v-model="sortOrder" @change="fetchSortedProducts" class="form-select">
          <option value="default">Default</option>
          <option value="asc">Price: Low to High</option>
          <option value="desc">Price: High to Low</option>
        </select>
      </div>

      <!-- Price Range Slider -->
      <div class="col-md-4 p-2">
        <p>Price Range: ${{ minPrice }} - ${{ maxPrice }}</p>
        <div class="slider d-flex">
          <input
            type="range"
            v-model="minPrice"
            min="0"
            max="50"
            @input="fetchSortedProducts"
            class="form-range me-2"
          />
          <input
            type="range"
            v-model="maxPrice"
            min="51"
            max="100"
            @input="fetchSortedProducts"
            class="form-range"
          />
        </div>
      </div>
    </div>

    <ul class="d-flex justify-content-end list-unstyled m-0">
      <!-- Separate Section for Auth Modal -->
      <li></li>
      <li>
        <a href="#" class="p-2 mx-1" @click="toggleCart">
          <svg width="24" height="24">
            <use xlink:href="#shopping-bag"></use>
          </svg>
        </a>
      </li>
    </ul>

    <!-- Offcanvas cart section -->
    <div
      class="offcanvas offcanvas-end"
      :class="{ show: isCartVisible }"
      tabindex="-1"
      id="offcanvasCart"
      aria-labelledby="offcanvasCartLabel"
    >
      <div class="offcanvas-header">
        <h5 id="offcanvasCartLabel">Shopping Cart</h5>
        <button type="button" class="btn-close" @click="toggleCart" aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <div v-for="(item, index) in cart" :key="index" class="cart-item">
          <h6>{{ item.productName }}</h6>
          <p>Quantity: {{ item.quantity }}</p>
          <p>Price: ${{ item.price }}</p>
          <p>Total: ${{ (item.price * item.quantity).toFixed(2) }}</p>
        </div>
        <div class="d-flex justify-content-between">
          <h5>Total:</h5>
          <h5>${{ cartTotal.toFixed(2) }}</h5>
        </div>
        <button class="btn btn-primary w-100 mt-3" @click="checkout">Proceed to Checkout</button>
      </div>
    </div>

    <button v-if="isAuthenticated && isAdmin" @click="showCreateProduct" class="btn btn-primary">
      Create Product
    </button>

    <button v-if="isAuthenticated && isAdmin" @click="showCreateCoupon" class="btn btn-primary">
      Create Coupon
    </button>

    <Modal :visible="isModalVisible" @close="hideModal">
      <template v-if="isEditMode">
        <EditProduct :product="currentProduct" />
      </template>
      <template v-else-if="isCreatingProduct">
        <CreateProduct />
      </template>
      <template v-else="isCreatingCoupon">
        <CreateCoupon />
      </template>
    </Modal>

    <div class="row row-product">
      <div class="col-md-12">
        <div class="swiper">
          <div class="swiper-wrapper">
            <div class="product-item swiper-slide">
              <div
                class="product-grid row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-3 row-cols-xl-4 row-cols-xxl-5"
              >
                <div v-for="(product, index) in products" :key="index" class="col">
                  <div
                    :class="{ disabled: product.quantity === 0 && !isAdmin }"
                    class="product-item"
                  >
                    <figure>
                      <a href="index.html">
                        <img
                          :src="imageSrc(product)"
                          :alt="product.productName"
                          class="tab-image"
                        />
                      </a>
                    </figure>
                    <div class="d-flex flex-column text-center">
                      <h3 class="fs-6 fw-normal">{{ product.productName }}</h3>
                      <div class="d-flex justify-content-center align-items-center gap-2">
                        <span class="text-dark fw-semibold">${{ product.price }}</span>
                      </div>

                      <div class="d-flex justify-content-center align-items-center gap-2">
                        <span class="text-dark fw-semibold quantity-check">
                          {{
                            product.quantity === 0
                              ? "Out of Stock"
                              : `Quantity Available: ${product.quantity}`
                          }}
                        </span>
                      </div>
                      <div class="button-area p-3 pt-0">
                        <div class="row g-1 mt-2">
                          <input
                            type="number"
                            class="form-control border-dark-subtle input-number quantity"
                            v-model.number="quantities[product.productId]"
                            min="0"
                            value="0"
                            placeholder="0"
                          />

                          <div class="col-5 btn-buy-user">
                            <button
                              @click="addToCart(product)"
                              class="btn btn-primary rounded-1 p-2 fs-7 btn-cart"
                            >
                              <svg class="svg-buy" width="18" height="18">
                                <use xlink:href="#cart"></use>
                              </svg>
                            </button>
                          </div>
                          <div class="col-2">
                            <div v-if="isAuthenticated && isAdmin">
                              <a
                                href="#"
                                @click="deleteProduct(product.productId)"
                                class="btn btn-danger btn-outline-dark rounded-1 p-2 fs-6 btn-del-pr"
                              >
                                <svg width="18" height="18">
                                  <use xlink:href="#trash"></use>
                                </svg>
                              </a>
                            </div>
                          </div>
                          <div class="col-2">
                            <div v-if="isAuthenticated && isAdmin">
                              <a
                                href="#"
                                @click="showEditProduct(product)"
                                class="btn btn-danger edit-pr-btn"
                              >
                                <svg
                                  class="svg-edit"
                                  xmlns="http://www.w3.org/2000/svg"
                                  viewBox="0 0 512 512"
                                >
                                  <path
                                    d="M362.7 19.3L314.3 67.7 444.3 197.7l48.4-48.4c25-25 25-65.5 0-90.5L453.3 19.3c-25-25-65.5-25-90.5 0zm-71 71L58.6 323.5c-10.4 10.4-18 23.3-22.2 37.4L1 481.2C-1.5 489.7 .8 498.8 7 505s15.3 8.5 23.7 6.1l120.3-35.4c14.1-4.2 27-11.8 37.4-22.2L421.7 220.3 291.7 90.3z"
                                  />
                                </svg>
                              </a>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <ChatBot />
</template>

<style scoped>
.disabled {
  pointer-events: none;
  opacity: 0.5;
}
.btn .btn-danger {
  width: 100%;
}
input.form-control.border-dark-subtle.input-number.quantity {
  height: 43px;
  width: 50px;
}
.row.row-product {
  width: 100%;
  padding: 0;
  margin: 0;
}
a.btn.btn-danger.btn-outline-dark.rounded-1.p-2.fs-6.btn-del-pr {
  height: 43px;
}
a.btn.btn-danger.edit-pr-btn {
  height: 43px;
  width: 35.6px;
}
svg.svg-edit {
  width: 13px;
  margin-top: 5px;
}
svg.svg-buy {
  margin-left: 40px;
}
.btn-buy-user {
  width: calc(100% / 1.4);
}
</style>

<script setup>
document.addEventListener("DOMContentLoaded", function () {
  const quantityElement = document.querySelector(".quantity-check");
  const productItem = document.querySelector(".product-item");

  if (quantityElement && productItem) {
    // Extract number from text
    const quantityText = quantityElement.textContent.match(/\d+/);
    // Convert to number
    const quantity = quantityText ? parseInt(quantityText[0], 10) : 0;

    if (quantity === 0) {
      productItem.classList.add("disabled");
      productItem.style.pointerEvents = "none";
      productItem.style.opacity = "0.5";
    }
  }
});

import { ref, onMounted, computed, onBeforeUnmount, watch } from "vue";
import api from "@/js/api_auth.js";
import Modal from "./Modal.vue";
import CreateProduct from "./CreateProduct.vue";
import EditProduct from "./EditProduct.vue";
import CreateCoupon from "./CreateCoupon.vue";
import Voice from "./Voice.vue";
import ChatBot from "./ChatBot.vue";
import { eventBus, eventBus2 } from "@/js/eventBus.js";
import { useAlertStore } from "@/js/useAlertStore";

const alertStore = useAlertStore;
const minPrice = ref(0);
const maxPrice = ref(100);
const sortOrder = ref("default");
const products = ref([]);
const quantities = ref({});
const isModalVisible = ref(false);
const isCartVisible = ref(false);
const isEditMode = ref(false);
const isCreatingProduct = ref(false);
const isCreatingCoupon = ref(false);
const currentProduct = ref(null);
const cart = ref([]);
const searchTerm = ref([]);
const token = ref(null);

const isAuthenticated = ref(false);
const isAdmin = ref(false);
const userId = ref(null);

const loadAuthState = () => {
  const authenticated = localStorage.getItem("token");
  isAuthenticated.value = !!authenticated;
  const userIdParam = localStorage.getItem("userId");
  userId.value = userIdParam;
  const role = localStorage.getItem("role");
  if (role == "admin") isAdmin.value = "true";
};

onMounted(() => {
  loadAuthState();
});

const addToCart = (product) => {
  const quantity = quantities.value[product.productId] || 0;
  if (quantity < 0) {
    alertStore.showAlert("Please select a valid product quantity!");
    return;
  }

  const existingProduct = cart.value.find((item) => item.productId === product.productId);

  if (existingProduct) {
    existingProduct.quantity -= quantity;
  } else {
    cart.value.push({
      ...product,
      quantity: quantity,
    });
    alertStore.showAlert("Registration successful!", "success");

    // alertStore.showAlert("Product has been added to your cart!");
  }

  quantities.value[product.productId] = 0;
};

const cartTotal = computed(() => {
  return cart.value.reduce((total, item) => total + item.price * item.quantity, 0);
});

const toggleCart = () => {
  isCartVisible.value = !isCartVisible.value;
};

const imageSrc = (product) => {
  if (product && product.imageURl) {
    if (product.imageURl.startsWith("http")) {
      return product.imageURl;
    } else {
      return `src/assets/images/${product.imageURl}`;
    }
  }
  return "path/to/default-image.png";
};
const numberWords = {
  one: 1,
  two: 2,
  three: 3,
  four: 4,
  five: 5,
  six: 6,
  seven: 7,
  eight: 8,
  nine: 9,
  ten: 10,
};

const processVoiceCommand = () => {
  const transcript = eventBus.result;
  const regex =
    /\b(\d+|one|two|three|four|five|six|seven|eight|nine|ten)\s+([\w\s]+?)\s*(?:fish|item|piece|unit|bag|box)?$/i;
  const match = transcript.match(regex);

  if (match) {
    let quantity = match[1];
    const productName = match[2].trim();

    // Convert word numbers to digits if necessary
    if (isNaN(quantity)) {
      quantity = numberWords[quantity]; // Convert "one" to 1, etc.
    } else {
      quantity = parseInt(quantity);
    }

    const product = products.value.find((p) =>
      p.productName.toLowerCase().includes(productName.toLowerCase()),
    );

    console.log(product);
    if (product) {
      quantities.value[product.productId] = quantity;
      addToCart(product);
    } else {
      alert(`Product "${productName}" not found.`);
    }
  } else {
    alert("Could not recognize the product and quantity.");
  }
};

const handleSearch = () => {
  fetchSortedProducts();
};
const fetchSortedProducts = async () => {
  try {
    let apiEndpoint = "FishProductAPI/GetProductInRange";

    switch (sortOrder.value) {
      case "asc":
        apiEndpoint = "FishProductAPI/GetProductAsc";
        break;
      case "desc":
        apiEndpoint = "FishProductAPI/GetProductDesc";
        break;
      case "oldest":
        apiEndpoint = "FishProductAPI/GetProductOldest";
        break;
      case "newest":
        apiEndpoint = "FishProductAPI/GetProductNewest";
        break;
    }

    const apiUrl = `${apiEndpoint}?min=${minPrice.value}&max=${maxPrice.value}&searchTerm=${encodeURIComponent(searchTerm.value) || eventBus.result}`;
    const response = await api.get(apiUrl);

    if (response.data.isSuccess) {
      products.value = response.data.result;
      console.log("data:", products.value);
    } else {
      console.error("Error fetching data:", response.data.errorMessages);
    }
  } catch (error) {
    console.error("Error fetching data:", error);
  }
};

const deleteProduct = async (productId) => {
  try {
    await api.delete(`/FishProductAPI/${productId}`);
    window.location.reload();
    alert("Product deleted successfully!");
    handleSearch();
  } catch (error) {
    console.error("Error deleting product:", error);
    alert("Error deleting product. Please try again.");
  }
};

const checkout = () => {
  const cartData = JSON.stringify(cart.value);
  const totalData = cartTotal.value;

  localStorage.setItem("cart", cartData);
  localStorage.setItem("total", totalData);

  window.location.href = "checkout.html";
};
const trackView = async () => {
  try {
    await api.post("Analytics/TrackView");
  } catch (error) {
    console.error("Error tracking view:", error);
  }
};

const startSession = async () => {
  try {
    await api.post("Analytics/StartSession");
  } catch (error) {
    console.error("Error starting session:", error);
  }
};

const endSession = async () => {
  try {
    await axios.post(`${api}/Analytics/EndSessions`);
  } catch (error) {
    console.error("Error ending session:", error);
  }
};

const showCreateProduct = () => {
  isEditMode.value = false;
  isModalVisible.value = true;
  isCreatingProduct.value = true;
  isCreatingCoupon.value = false;
  document.querySelector(".swiper").style.display = "none";
};

const showCreateCoupon = () => {
  isEditMode.value = false;
  isModalVisible.value = true;
  isCreatingProduct.value = false;
  isCreatingCoupon.value = true;
  document.querySelector(".swiper").style.display = "none";
};

const showEditProduct = (product) => {
  isEditMode.value = true;
  currentProduct.value = product;
  isModalVisible.value = true;
  document.querySelector(".swiper").style.display = "none";
};

const hideModal = () => {
  isModalVisible.value = false;
  document.querySelector(".swiper").style.display = "block";
};

onBeforeUnmount(() => {
  window.removeEventListener("beforeunload", endSession);
});
watch(
  () => eventBus2.result,
  (newValue) => {
    if (newValue === "Victory") {
      handleSearch();
    }
    if (newValue === "ILoveYou") {
      processVoiceCommand();
    }
  },
);
onMounted(() => {
  trackView();
  loadAuthState();
  startSession();
  handleSearch();
  window.addEventListener("beforeunload", endSession);
});
</script>

<style src="@/assets/css/style.css"></style>
<style src="@/assets/css/vendor.css"></style>
<style>
@import url("https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css");
</style>
