<template>
  <div>
    <div v-if="isLogin" @submit.prevent="handleLogin">
      <div class="limiter">
        <div class="container-login100" style="background-image: url(&quot;images/bg-01.jpg&quot;)">
          <div class="wrap-login100 p-l-110 p-r-110 p-t-62 p-b-33">
            <form class="login100-form validate-form flex-sb flex-w">
              <span class="login100-form-title p-b-53"> Sign With </span>

              <a href="#" class="btn-face m-b-20" @click.prevent="socialLogin('facebook')">
                <i class="fa fa-facebook-official"></i>
                Facebook
              </a>

              <a href="#" class="btn-google m-b-20" @click="socialLogin('google')">
                <img src="/Login/images/icons/icon-google.png" alt="GOOGLE" />
                Google
              </a>

              <div class="p-t-31 p-b-9">
                <span class="txt1"> Username </span>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Username is required">
                <input
                  v-model="loginData.userName"
                  type="text"
                  placeholder="Username"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-blue-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>

              <div class="p-t-13 p-b-9">
                <span class="txt1"> Password </span>

                <a href="/Login/ForgotPass.html" class="txt2 bo1 m-l-5"> Forgot? </a>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Password is required">
                <input
                  v-model="loginData.password"
                  type="password"
                  placeholder="Password"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-blue-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>

              <div class="container-login100-form-btn m-t-17">
                <button type="submit" class="login100-form-btn">Sign In</button>
              </div>

              <div class="w-full text-center p-t-55">
                <span class="txt2"> Not a member? </span>

                <a @click="isLogin = false" href="#" class="txt2 bo1"> Sign up now </a>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>

    <div v-else @submit.prevent="handleRegister">
      <div class="limiter">
        <div class="container-login100" style="background-image: url(&quot;images/bg-01.jpg&quot;)">
          <div class="wrap-login100 p-l-110 p-r-110 p-t-62 p-b-33">
            <form class="login100-form validate-form flex-sb flex-w">
              <span class="login100-form-title p-b-53"> Sign Up</span>
              <div class="p-t-31 p-b-9">
                <span class="txt1"> Email </span>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Username is required">
                <input
                  v-model="registerData.email"
                  type="text"
                  placeholder="Email address"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>
              <div class="p-t-31 p-b-9">
                <span class="txt1"> Full Name </span>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Username is required">
                <input
                  v-model="registerData.name"
                  type="text"
                  placeholder="Username"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>
              <div class="p-t-31 p-b-9">
                <span class="txt1"> Username </span>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Username is required">
                <input
                  v-model="registerData.userName"
                  type="text"
                  placeholder="Username"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>

              <div class="p-t-13 p-b-9">
                <span class="txt1"> Password </span>
                <a href="/Login/ForgotPass.html" class="txt2 bo1 m-l-5"> Forgot? </a>
              </div>
              <div class="wrap-input100 validate-input" data-validate="Password is required">
                <input
                  v-model="registerData.password"
                  type="password"
                  placeholder="Password"
                  class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                  required
                />
                <span class="focus-input100"></span>
              </div>
              <div class="container-login100-form-btn m-t-17">
                <button type="submit" class="login100-form-btn">Sign Up</button>
              </div>
              <div class="w-full text-center p-t-55">
                <span class="txt2"> Already a member? </span>
                <a @click="isLogin = true" href="#" class="txt2 bo1"> Sign in now </a>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- Registration Form -->
  </div>
</template>

<script>
import api from "@/js/api_auth.js"; // Axios instance
export default {
  data() {
    return {
      showModal: false,
      isLogin: true, // Toggle between login and register
      isAuthenticated: !!localStorage.getItem("token"),
      isAdmin: false,
      token: null,
      userId: null,
      loginData: { userName: "", password: "" },
      registerData: { email: "", name: "", userName: "", password: "" },
      errorMessage: "",
    };
  },
  mounted() {
    this.loadFacebookSDK();
  },
  methods: {
    openModal() {
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
      this.clearForm();
      this.errorMessage = "";
    },
    clearForm() {
      this.loginData = { userName: "", password: "" };
      this.registerData = { email: "", name: "", userName: "", password: "" };
    },
    async handleLogin() {
      try {
        const response = await api.post("/login", this.loginData);
        console.log("Full API Response:", response.data.result);

        if (response.data.isSuccess && response.data.result) {
          const { token, user } = response.data.result || {};
          const role = user?.role?.toLowerCase() || "";

          console.log("Token Received:", token);

          if (!token) {
            throw new Error("Token is missing from response");
          }

          // Save authentication details
          localStorage.setItem("token", token);
          localStorage.setItem("user", JSON.stringify(user));
          localStorage.setItem("role", role);
          localStorage.setItem("userId", user.id);

          // Set component state
          this.isAdmin = role === "admin";
          this.isAuthenticated = !!token;
          this.userId = user.id;

          alert("Login Success:", { userId: this.userId, isAdmin: this.isAdmin });

          // Construct URL
          const queryParams = new URLSearchParams({
            userId: encodeURIComponent(this.userId),
            isAdmin: this.isAdmin.toString(),
            isAuthenticated: this.isAuthenticated.toString(),
          }).toString();

          // Update URL and reload
          window.history.pushState({}, "", `../?${queryParams}`);
          this.closeModal();
          window.location.reload();
        } else {
          throw new Error(response.data.errorMessages?.[0] || "Invalid login response");
        }
      } catch (error) {
        console.error("Login Error:", error);
        this.errorMessage =
          error.response?.data?.errorMessages?.[0] || "An error occurred during login";
      }
    },

    // async handleLogin() {
    //   try {
    //     const response = await api.post("../UserAuth/login", this.loginData);
    //     const { token, user, role } = response.data.result;

    //     this.$store.commit("login", { token, user, role });

    //     window.location.href = `../?userId=${user.id}`;
    //   } catch (error) {
    //     this.errorMessage = error.response?.data?.errorMessages?.[0] || "An error occurred";
    //   }
    // },

    async handleRegister() {
  try {
    const payload = {
      userName: this.registerData.userName,
      name: this.registerData.name,
      password: this.registerData.password,
      role: "customer",
      email: this.registerData.email,
    };

    console.log("Sending payload:", payload);

    const response = await api.post("/register", payload);
    const data = response.data; // Extract response data

    // Show success alert
    alert("Registration successful! Redirecting...");

    // Redirect after alert
    window.location.href = `https://localhost:5173/?userId=${encodeURIComponent(data.userId)}&isAuthenticated=true&isAdmin=false`;

    // Clear form fields
    this.clearForm();
  } catch (error) {
    // Show error alert if registration fails
    this.errorMessage = error.response?.data?.errorMessages?.[0] || "An error occurred";
    alert(this.errorMessage);
  }
},


    loadFacebookSDK() {
      window.fbAsyncInit = function () {
        FB.init({
          appId: "1722472512021344",
          cookie: true,
          xfbml: true,
          version: "v18.0",
        });
      };

      if (!document.getElementById("facebook-jssdk")) {
        let js = document.createElement("script");
        js.id = "facebook-jssdk";
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        document.body.appendChild(js);
      }
    },

    socialLogin(provider) {
      if (provider === "facebook") {
        if (typeof FB === "undefined") {
          console.error("Facebook SDK not loaded");
          return;
        }

        FB.login(
          (response) => {
            if (response.authResponse) {
              let accessToken = response.authResponse.accessToken;
              console.log(accessToken);
              this.handleFacebookLogin(accessToken);
            } else {
              console.error("Facebook login failed", response);
            }
          },
          { scope: "email,public_profile" },
        );
      }
    },

    async handleFacebookLogin(accessToken) {
      try {
        let apiResponse = await fetch("https://localhost:7229/api/User/login-facebook", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ accessToken }),
        });

        if (!apiResponse.ok) {
          throw new Error(`HTTP error! Status: ${apiResponse.status}`);
        }

        let data = await apiResponse.json();
        console.log("User Data:", data);

        if (data.token) {
          localStorage.setItem("token", data.token);
          this.isAuthenticated = true;

          window.location.href = `https://localhost:5173/?userId=${encodeURIComponent(data.userId)}&isAuthenticated=true&isAdmin=${data.isAdmin}`;
        } else {
          console.error("Login failed", data);
        }
      } catch (error) {
        console.error("Error logging in with Facebook:", error);
      }
    },
  },
};
</script>
