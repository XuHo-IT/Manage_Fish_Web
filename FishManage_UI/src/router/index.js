import { createRouter, createWebHistory } from "vue-router";
import ProductList from "@/components/ProductList.vue";
import AuthModal from "@/components/AuthModal.vue";
import CreateProduct from "@/components/CreateProduct.vue";
import EditProduct from "@/components/EditProduct.vue";
import CheckOut from "@/components/CheckOut.vue";
import CallBack from "@/components/CallBack.vue";
import CreateCoupon from "@/components/CreateCoupon.vue";
import ChatBot from "@/components/ChatBot.vue";
import Voice from "@/components/Voice.vue";

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
