import { createApp } from "vue";
import App from "./App.vue";
import ProductList from "./UI/ProductList.vue";
import AuthModal from "./UI/AuthModal.vue";
import CreateProduct from "./UI/CreateProduct.vue";
import EditProduct from "./UI/EditProduct.vue";
import Checkout from "./UI/Checkout.vue";
import TotalReport from "./UI/TotalReport.vue";
import RecentOrder from "./UI/RecentOrder.vue";
import CreateCoupon from "./UI/CreateCoupon.vue";
import ViewResponse from "./UI/View_Response.vue";
import MailFirstCoupon from "./UI/MailFirstCoupon.vue";
import NewList from "./UI/NewList.vue";
import VueApexCharts from "vue3-apexcharts";
import router from "./router";

// Mount main Vue app
const app = createApp(App);
app.use(router);

app.mount("#app");

const productListApp = createApp(ProductList);
productListApp.mount("#product-list");

const authModalApp = createApp(AuthModal);
authModalApp.mount("#auth-modal");

const createProductApp = createApp(CreateProduct);
createProductApp.mount("#create-product");

const editProductApp = createApp(EditProduct);
createProductApp.mount("#edit-product");

const appCheckOut = createApp(Checkout);
appCheckOut.use(router);
appCheckOut.mount("#checkout-app");

const appTotalReport = createApp(TotalReport);
appTotalReport.mount("#total-report");

const recentOrder = createApp(RecentOrder);
recentOrder.mount("#recent-order");

const viewResponse = createApp(ViewResponse);
viewResponse.use(VueApexCharts);
viewResponse.mount("#view-response");

const createCoupon = createApp(CreateCoupon);
createCoupon.mount("#create-coupon");

const newList = createApp(NewList);
newList.mount("#new-list");

const mailFirstCoupon = createApp(MailFirstCoupon);
mailFirstCoupon.mount("#mailCoupon");
