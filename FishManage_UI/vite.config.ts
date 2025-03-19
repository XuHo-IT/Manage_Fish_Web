import { defineConfig, loadEnv } from "vite";
import vue from "@vitejs/plugin-vue";
import path from "path";

export default defineConfig(({ mode }) => {
  // Load the `okta.env` file manually
  const env = loadEnv(mode, process.cwd(), "VITE_");

  return {
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
    define: {
      "process.env": env,  // Make env variables available globally
    },
  };
});
