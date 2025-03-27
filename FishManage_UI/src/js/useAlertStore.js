import { reactive } from "vue";

export const useAlertStore = reactive({
  isVisible: true,
  message: "",
  type: "success",

  showAlert(msg, type = "success") {
    this.message = msg;
    this.type = type;
    this.isVisible = true;
  },

  hideAlert() {
    this.isVisible = false;
  },
});
