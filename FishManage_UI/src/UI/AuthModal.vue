<template>
  <div>
    <div v-if="isLogin" @submit.prevent="handleLogin">
      <div
        class="limiter container-login100"
        style="background-image: url(&quot;images/fish_back.gif&quot;)"
      >
        <div class="container-login100-form-btn m-t-17 exit-btn">
          <a href="https://localhost:5173/">
            <button type="submit" class="login100-form-btn">Back</button>
          </a>
        </div>
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

    <div v-else @submit.prevent="handleRegister">
      <div
        class="limiter container-login100"
        style="background-image: url(&quot;images/fish_back.gif&quot;)"
      >
        <div class="container-login100-form-btn m-t-17 exit-btn">
          <a href="https://localhost:5173/">
            <button type="submit" class="login100-form-btn">Back</button>
          </a>
        </div>
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
    <!-- Registration Form -->
  </div>
</template>
<style>
.container-login100-form-btn.m-t-17.exit-btn {
  flex-direction: row-reverse;
}
</style>
<script setup>
import api from "@/js/api_auth.js";
import { ref, onMounted } from "vue";

const showModal = ref(false);
const isLogin = ref(true);
const isAuthenticated = ref(!!localStorage.getItem("token"));
const isAdmin = ref(false);
const userId = ref(null);

const loginData = ref({ userName: "", password: "" });
const registerData = ref({ email: "", name: "", userName: "", password: "" });
const errorMessage = ref("");

const closeModal = () => {
  showModal.value = false;
  clearForm();
  errorMessage.value = "";
};

const clearForm = () => {
  loginData.value = { userName: "", password: "" };
  registerData.value = { email: "", name: "", userName: "", password: "" };
};

const handleLogin = async () => {
  try {
    const response = await api.post("User/login", loginData.value);

    if (response.data.isSuccess && response.data.result) {
      const { token, user } = response.data.result;
      const role = user?.role?.toLowerCase() || "";

      if (!token) {
        throw new Error("Token is missing from response");
      }

      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));
      localStorage.setItem("role", role);
      localStorage.setItem("userId", user.id);

      // Update component state
      isAdmin.value = role === "admin";
      isAuthenticated.value = !!token;
      userId.value = user.id;

      const queryParams = new URLSearchParams({
        userId: encodeURIComponent(user.id),
        isAdmin: isAdmin.value.toString(),
        isAuthenticated: isAuthenticated.value.toString(),
      }).toString();

      window.history.pushState({}, "", `../?${queryParams}`);
      closeModal();
      window.location.reload();
    } else {
      throw new Error(response.data.errorMessages?.[0] || "Invalid login response");
    }
  } catch (error) {
    errorMessage.value =
      error.response?.data?.errorMessages?.[0] || "An error occurred during login";
  }
};

const handleRegister = async () => {
  try {
    const payload = {
      userName: registerData.value.userName,
      name: registerData.value.name,
      password: registerData.value.password,
      role: "customer",
      email: registerData.value.email,
    };

    const response = await api.post("/register", payload);
    const data = response.data;

    window.location.href = `https://localhost:5173/?userId=${encodeURIComponent(data.userId)}&isAuthenticated=true&isAdmin=false`;
    clearForm();
  } catch (error) {
    errorMessage.value = error.response?.data?.errorMessages?.[0] || "An error occurred";
    alert(errorMessage.value);
  }
};

const loadFacebookSDK = () => {
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
};

const socialLogin = (provider) => {
  if (provider === "facebook") {
    if (typeof FB === "undefined") {
      return;
    }

    FB.login(
      (response) => {
        if (response.authResponse) {
          let accessToken = response.authResponse.accessToken;
          console.log(accessToken);
          handleFacebookLogin(accessToken);
        } else {
          console.error("Facebook login failed", response);
        }
      },
      { scope: "email,public_profile" }
    );
  }
};

const handleFacebookLogin = async (accessToken) => {
  try {
    const apiResponse = await fetch("https://localhost:7229/api/User/login-facebook", {
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

    if (data.token) {
      localStorage.setItem("token", data.token);
      isAuthenticated.value = true;

      window.location.href = `https://localhost:5173/?userId=${encodeURIComponent(data.userId)}&isAuthenticated=true&isAdmin=${data.isAdmin}`;
    } else {
      console.error("Login failed", data);
    }
  } catch (error) {
    console.error("Error logging in with Facebook:", error);
  }
};

onMounted(loadFacebookSDK);
</script>

