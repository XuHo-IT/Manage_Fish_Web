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
                autocomplete="off"
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
                autocomplete="off"
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
                autocomplete="off"
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
                autocomplete="off"
                v-model="registerData.name"
                type="text"
                placeholder="Fullname"
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
                autocomplete="off"
                v-model="registerData.userName"
                type="text"
                placeholder="Username"
                class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                required
              />
              <span class="focus-input100"></span>
            </div>
            <div class="p-t-31 p-b-9">
              <span class="txt1"> PhoneNumber </span>
            </div>
            <div class="wrap-input100 validate-input" data-validate="PhoneNumer is required">
              <input
                autocomplete="off"
                v-model="registerData.phoneNumber"
                type="text"
                placeholder="PhoneNumer"
                class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
                required
              />
              <span class="focus-input100"></span>
            </div>
            <div class="p-t-31 p-b-9">
              <span class="txt1">Gender</span>
            </div>
            <div class="wrap-input100">
              <select
                v-model="registerData.gender"
                class="w-full px-3 py-2 border rounded focus:ring focus:ring-green-300"
              >
                <option :value="true">Male</option>
                <option :value="false">Female</option>
              </select>
            </div>

            <div class="p-t-13 p-b-9">
              <span class="txt1"> Province </span>
            </div>
            <div class="wrap-input100 validate-input" data-validate="Password is required">
              <select id="province" class="form-control" v-model="selectedProvince">
                <option v-for="province in provinces" :key="province.id" :value="province.id">
                  {{ province.full_name }}
                </option>
              </select>
            </div>

            <div class="p-t-13 p-b-9">
              <span class="txt1"> District </span>
            </div>
            <div class="wrap-input100 validate-input" data-validate="Password is required">
              <select id="district" class="form-control" v-model="selectedDistrict">
                <option v-for="district in districts" :key="district.id" :value="district.id">
                  {{ district.full_name }}
                </option>
              </select>
            </div>
            <div class="p-t-13 p-b-9">
              <span class="txt1"> Ward </span>
            </div>
            <div class="wrap-input100 validate-input" data-validate="Password is required">
              <select id="ward" class="form-control" v-model="selectedWard">
                <option v-for="ward in wards" :key="ward.id" :value="ward.id">
                  {{ ward.full_name }}
                </option>
              </select>
            </div>
            <div class="p-t-13 p-b-9">
              <span class="txt1"> Date Of Birth </span>
            </div>
            <div class="wrap-input100 validate-input" data-validate="DateOfBirth is required">
              <input
                autocomplete="off"
                v-model="registerData.DateOfBirth"
                type="date"
                placeholder="DateOfBirth"
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
                autocomplete="off"
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
import { ref, onMounted, watch } from "vue";

const showModal = ref(false);
const isLogin = ref(true);
const isAuthenticated = ref(!!localStorage.getItem("token"));
const isAdmin = ref(false);
const userId = ref(null);

const loginData = ref({ userName: "", password: "" });


const registerData = ref({
  email: "",
  name: "",
  userName: "",
  password: "",
  phoneNumber: "",
  gender: "Female",
  address: "",
  DateOfBirth: "",
});
const errorMessage = ref("");

const provinces = ref([]);
const districts = ref([]);
const wards = ref([]);
const selectedProvince = ref("0");
const selectedDistrict = ref("0");
const selectedWard = ref("0");

const closeModal = () => {
  showModal.value = false;
  clearForm();
  errorMessage.value = "";
};

const clearForm = () => {
  loginData.value = { userName: "", password: "" };
  registerData.value = {
    email: "",
    name: "",
    userName: "",
    password: "",
    phoneNumber: "",
    gender: "",
    address: "",
    DateOfBirth: "",
  };
};
const fetchProvinces = async () => {
  try {
    const response = await fetch("https://esgoo.net/api-tinhthanh/1/0.htm");
    const data = await response.json();
    if (data.error === 0) {
      provinces.value = data.data;
    }
  } catch (error) {
    console.error("Error fetching provinces:", error);
  }
};

watch(selectedProvince, async (newVal) => {
  if (newVal !== "0") {
    try {
      const response = await fetch(`https://esgoo.net/api-tinhthanh/2/${newVal}.htm`);
      const data = await response.json();
      if (data.error === 0) {
        districts.value = data.data;
        wards.value = []; // Reset wards
      }
    } catch (error) {
      console.error("Error fetching districts:", error);
    }
  }
});

watch(selectedDistrict, async (newVal) => {
  if (newVal !== "0") {
    try {
      const response = await fetch(`https://esgoo.net/api-tinhthanh/3/${newVal}.htm`);
      const data = await response.json();
      if (data.error === 0) {
        wards.value = data.data;
      }
    } catch (error) {
      console.error("Error fetching wards:", error);
    }
  }
});
watch([selectedProvince, selectedDistrict, selectedWard], () => {
  const province = provinces.value.find((p) => p.id === selectedProvince.value)?.full_name || "";
  const district = districts.value.find((d) => d.id === selectedDistrict.value)?.full_name || "";
  const ward = wards.value.find((w) => w.id === selectedWard.value)?.full_name || "";

  registerData.value.address = [ward, district, province].filter(Boolean).join(", ");
});

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
      localStorage.setItem("role", role);
      localStorage.setItem("userId", user.id);
      window.history.pushState({}, "", `../`);
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
      UserName: registerData.value.userName,
      FullName: registerData.value.name,
      Password: registerData.value.password,
      Role: "customer",
      Email: registerData.value.email,
      PhoneNumber: registerData.value.phoneNumber,
      Gender: registerData.value.gender,
      Address: registerData.value.address,
      DateOfBirth: new Date(registerData.value.DateOfBirth).toISOString(),
      imageFile:"",
    };
  console.log(payload);
    const response = await api.post("User/register", payload,{
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
      localStorage.setItem("token", token);
      localStorage.setItem("role", role);
      localStorage.setItem("userId", user.id);
    window.location.href = "https://localhost:5173/";
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
      { scope: "email,public_profile" },
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

onMounted(() => {
  loadFacebookSDK();
  fetchProvinces();
});
</script>
