<template>
    <div class="container d-flex justify-content-center align-items-center">
        <div class="col-md-6">
            <h2 class="text-center">Logowanie</h2>
            <hr>
            <form @submit.prevent="login">
            <div class="form-floating mb-3">
                <input
                id="email"
                v-model="loginData.email"
                class="form-control"
                placeholder="Email"
                type="email"
                />
                <label for="email">Email</label>
            </div>

            <div class="form-floating mb-3">
                <input
                id="password"
                v-model="loginData.password"
                class="form-control"
                type="password"
                placeholder="Hasło"
                />
                <label for="password">Hasło</label>
            </div>

            <button type="submit" class="btn btn-primary">Zaloguj się</button>
            </form>

            <p v-if="errorMessage" class="text-danger mt-3">{{ errorMessage }}</p>
        </div>
    </div>
</template>

<script setup>
import accountService from '@/services/accountService';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const loginData = ref({
    email: '',
    password: ''
});

const errorMessage = ref('');

const router = useRouter();

const login = async () => {
    errorMessage.value = '';

    try {
        const user = await accountService.login(loginData.value);
        if (user) {
        router.push('/');
        }
    } catch (error) {
        errorMessage.value = 'Błąd logowania. Sprawdź dane i spróbuj ponownie.';
    }
};
</script>

<style scoped>
</style>