import { createApp } from "vue";
import App from "./App.vue";
import router from "./router/index.js";
import store from "./js/store.js";
import VueApexCharts from "vue3-apexcharts";

// Import all components
import ProductList from "./components/ProductList.vue";
import AuthModal from "./components/AuthModal.vue";
import ResetPass from "./components/ResetPassword.vue";
import ForgetPass from "./components/ForgetPass.vue";
import CreateProduct from "./components/CreateProduct.vue";
import EditProduct from "./components/EditProduct.vue";
import Checkout from "./components/CheckOut.vue";
import TotalReport from "./components/TotalReport.vue";
import RecentOrder from "./components/RecentOrder.vue";
import CreateCoupon from "./components/CreateCoupon.vue";
import ViewResponse from "./components/View_Response.vue";
import MailFirstCoupon from "./components/MailFirstCoupon.vue";
import NewList from "./components/NewList.vue";
import ButtonLogin from "./components/ButtonLogin.vue";
import Voice from "./components/Voice.vue";
import Profile from "./components/Profile.vue";
import CallBack from "./components/CallBack.vue";
import Recognition from "./components/Recognition.vue";

// Create the main Vue app
const app = createApp(App);
app.use(router);
app.use(store);
app.use(VueApexCharts);

// Register components globally
const components = {
  ProductList,
  AuthModal,
  ResetPass,
  ForgetPass,
  CreateProduct,
  EditProduct,
  Checkout,
  TotalReport,
  RecentOrder,
  CreateCoupon,
  ViewResponse,
  MailFirstCoupon,
  NewList,
  ButtonLogin,
  Voice,
  Profile,
  CallBack,
  Recognition
};

Object.entries(components).forEach(([name, component]) => {
  app.component(name, component);
});

// Mount the main app to `#app`
app.mount("#app");

// Mount additional components dynamically
document.querySelectorAll("[data-component]").forEach((el) => {
  const componentName = el.getAttribute("data-component");
  if (components[componentName]) {
    createApp(components[componentName]).mount(el);
  }
});
