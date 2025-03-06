import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import path from "path";

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "src"), 
    },
  },
  server: {
    https: {
      key: path.resolve(__dirname, "cert/key.pem"),
      cert: path.resolve(__dirname, "cert/cert.pem"),
    },
    host: "localhost",
  },
});
