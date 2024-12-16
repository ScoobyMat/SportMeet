<template>
    <div class="d-flex justify-content-center align-items-center">
        <div class="card">
                <img src="/logo.png" alt="Logo"/>
            <h2 class="text-center">Logowanie</h2>

            <form @submit.prevent="login">
                <div class="input-group mt-4 mb-4">
                    <span class="input-group-text fs-4"><i class="bi bi-envelope"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input id="email" v-model="loginData.email" class="form-control" placeholder="Email"
                            type="email" />
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
            <p v-if="errorMessage" class="text-danger mt-3 text-center">{{ errorMessage }}</p>
        </div>
    </div>
</template>



<script setup>
import AuthenticationService from '@/services/AuthenticationService';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const loginData = ref({
    email: '',
    password: ''
});

const errorMessage = ref('');
const showPassword = ref(false);

const router = useRouter();

const login = async () => {
    errorMessage.value = '';

    try {
        const user = await AuthenticationService.login(loginData.value);
        if (user) {
            router.push('/');
        }
    } catch (error) {
        errorMessage.value = error.message;
    }
};

const togglePasswordVisibility = () => {
    showPassword.value = !showPassword.value;
};
</script>

<style scoped>
.card{
    width: 60%;
}
</style>
