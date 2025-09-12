<template>
  <div class="d-flex justify-content-center align-items-center">
    <div class="card">
      <img src="/logo.png" alt="Logo" />
      <h2 class="text-center">Logowanie</h2>

      <form @submit.prevent="login">
        <div class="input-group mt-4 mb-4">
          <span class="input-group-text fs-4"><i class="bi bi-envelope"></i></span>
          <div class="form-floating flex-grow-1">
            <input id="email" v-model="loginData.email" class="form-control" placeholder="Email" type="email" />
            <label for="email">Email</label>
          </div>
        </div>

        <div class="input-group mb-4">
          <span class="input-group-text fs-4"><i class="bi bi-lock"></i></span>
          <div class="form-floating flex-grow-1">
            <input id="password" v-model="loginData.password" class="form-control"
              :type="showPassword ? 'text' : 'password'" placeholder="Hasło" />
            <label for="password">Hasło</label>
          </div>
          <span class="input-group-text fs-4 cursor-pointer" @click="togglePasswordVisibility">
            <i :class="showPassword ? 'bi bi-eye-slash' : 'bi bi-eye'"></i>
          </span>
        </div>

        <button type="submit" class="btn btn-primary btn-lg w-100 mb-4">Zaloguj się</button>

        <p class="text-center">
          <a href="/register" class="text-decoration-none">Nie masz konta? Zarejestruj się</a>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup>
import { useAuthStore } from "@/stores/auth";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";

const toast = useToast();

const authStore = useAuthStore();
const router = useRouter();

const loginData = ref({
  email: "",
  password: "",
});
const showPassword = ref(false);

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};

const login = async () => {
  try {
    await authStore.login(loginData.value);
    toast.success("Zalogowano pomyślnie");
    router.push("/");
  } catch (error) {
    toast.error(error.message);
  }
};
</script>

<style scoped>
.card {
  align-items: center;
  width: 60%;
}

img {
  width: 150px;
  height: 150px;
}
</style>
