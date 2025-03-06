<template>
  <div>
    <div>
      <div class="limiter">
        <div class="container-login100" style="background-image: url(&quot;images/bg-01.jpg&quot;)">
          <div class="wrap-login100 p-l-110 p-r-110 p-t-62 p-b-33">
            <form @submit.prevent="sendForgotPasswordRequest" class="login100-form validate-form flex-sb flex-w">
              <span class="login100-form-title p-b-53"> Forgot Password</span>
              <div class="p-t-13 p-b-9">
                <span class="txt1"> Your Email </span>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Password is required">
                <input
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-blue-300"
                 v-model="email" placeholder="Enter your email" />
                <span class="focus-input100"></span>
              </div>

              <div class="container-login100-form-btn m-t-17">
                <div class="container-login100-form-btn m-t-17">
                <button type="submit" class="login100-form-btn">Submit</button>
                </div>  
              </div>
              <div class="w-full text-center p-t-55">
                <span class="txt2"> Back to Log in </span>

                <a href="/Login/login.html" class="txt2 bo1"> Log in </a>
              </div>
              <p v-if="message">{{ message }}</p>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      email: "",
      message: "",
    };
  },
  methods: {
    async sendForgotPasswordRequest() {
  try {
    await axios.post("https://localhost:7229/api/User/forgotPass", {
      email: this.email,
    });
    alert("Password reset link sent to your email. Please check your inbox.");
  } catch (error) {
    this.message = error.response?.data?.errorMessages?.[0] || "An error occurred. Please try again.";
    alert(this.message);
  }
}
  }
};
</script>
