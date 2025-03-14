import { createRouter, createWebHistory } from "vue-router";
import ProductList from "@/UI/ProductList.vue";
import AuthModal from "@/UI/AuthModal.vue";
import CreateProduct from "@/UI/CreateProduct.vue";
import EditProduct from "@/UI/EditProduct.vue";
import CheckOut from "@/UI/CheckOut.vue";
import CallBack from "@/UI/CallBack.vue";
import CreateCoupon from "@/UI/CreateCoupon.vue";
import ChatBot from "@/UI/ChatBot.vue";
import Voice from "@/UI/Voice.vue";

const routes = [
  { path: "/productList", component: ProductList },
  { path: "/auth", component: AuthModal },
  { path: "/create", component: CreateProduct },
  { path: "/edit", component: EditProduct },
  { path: "/checkout", component: CheckOut },
  { path: "/callBack", name: "CallBack", component: CallBack },
  { path: "/createCoupon", component: CreateCoupon },
  { path: "/voice", component: Voice },
  { path: "/ChatBot", component: ChatBot },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
