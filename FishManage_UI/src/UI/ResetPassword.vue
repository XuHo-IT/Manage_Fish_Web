<template>
    <div>
      <h2>Reset Password</h2>
      <input v-model="password" placeholder="Enter new password" type="password" />
      <button @click="resetPassword">Reset</button>
      <p v-if="message">{{ message }}</p>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  export default {
    data() {
      return {
        password: "",
        message: "",
      };
    },
    methods: {
      async resetPassword() {
        const urlParams = new URLSearchParams(window.location.search);
        const email = urlParams.get("email");
        const token = urlParams.get("token");
  
        try {
          await axios.post("https://localhost:7229/api/User/resetPass", {
            email,
            token,
            newPassword: this.password,
          });
          this.message = "Password successfully reset!";
        } catch (error) {
          this.message = "Error: " + (error.response?.data?.errorMessages || "Try again.");
        }
      },
    },
  };
  </script>
  