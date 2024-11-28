<template>
    <div class="container d-flex justify-content-center align-items-center">
        <div class="col-md-6">
            <h2 class="text-center">Rejestracja</h2>
            <hr />

            <form @submit.prevent="register">
                <div class="form-floating mb-3">
                    <input type="text" id="firstName" v-model="form.firstName" class="form-control" placeholder="Imię"
                        required />
                    <label for="firstName">Imię</label>
                </div>

                <div class="form-floating mb-3">
                    <input type="text" id="lastName" v-model="form.lastName" class="form-control" placeholder="Nazwisko"
                        required />
                    <label for="lastName">Nazwisko</label>
                </div>

                <div class="mb-3">
                    <label class="form-label d-block">Płeć</label>
                    <div>
                        <input type="radio" id="male" value="Mężczyzna" v-model="form.gender" class="form-check-input"
                            required />
                        <label for="male" class="form-check-label ms-2 me-5">Mężczyzna</label>

                        <input type="radio" id="female" value="Kobieta" v-model="form.gender" class="form-check-input"
                            required />
                        <label for="female" class="form-check-label ms-2">Kobieta</label>
                    </div>
                </div>

                <div class="form-floating mb-3">
                    <input type="date" id="dateOfBirth" v-model="form.dateOfBirth" class="form-control"
                        placeholder="Data urodzenia" required />
                    <label for="dateOfBirth">Data urodzenia</label>
                </div>

                <div class="form-floating mb-3">
                    <input type="text" id="city" v-model="form.city" class="form-control" placeholder="Miasto"
                        required />
                    <label for="city">Miasto</label>
                </div>

                <div class="form-floating mb-3">
                    <input type="text" id="country" v-model="form.country" class="form-control" placeholder="Państwo"
                        required />
                    <label for="country">Państwo</label>
                </div>

                <div class="form-floating mb-3">
                    <input type="email" id="email" v-model="form.email" class="form-control" placeholder="Email"
                        required />
                    <label for="email">Email</label>
                </div>

                <div class="form-floating mb-3">
                    <input type="password" id="password" v-model="form.password" class="form-control"
                        placeholder="Hasło" required />
                    <label for="password">Hasło</label>
                    <div class="form-text">Hasło powinno mieć minimum 8 znaków.</div>
                </div>

                <div class="form-floating mb-3">
                    <input type="password" id="confirmPassword" v-model="form.confirmPassword" class="form-control"
                        placeholder="Potwierdź Hasło" required />
                    <label for="confirmPassword">Potwierdź Hasło</label>
                </div>

                <div v-if="checkPassword" class="alert alert-danger">
                    Hasła się nie zgadzają!
                </div>

                <button type="submit" class="btn btn-primary w-100" :disabled="isSubmitting">
                    Utwórz konto
                </button>
            </form>

            <div v-if="errorMessage" class="alert alert-danger mt-3">
                {{ errorMessage }}
            </div>
        </div>
    </div>
</template>


<script setup>
import accountService from '@/services/accountService';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const form = ref({
    firstName: '',
    lastName: '',
    gender: '',
    dateOfBirth: '',
    city: '',
    country: '',
    email: '',
    password: '',
    confirmPassword: '',
});

const errorMessage = ref('');
const isSubmitting = ref(false);

const checkPassword = computed(() => {
    return form.value.password !== form.value.confirmPassword;
});

const register = async () => {
    errorMessage.value = '';
    isSubmitting.value = true;

    if (checkPassword.value) {
        errorMessage.value = 'Hasła się nie zgadzają!';
        isSubmitting.value = false;
        return;
    }

    try {
        const response = await accountService.register(form.value);
        router.push('/login');
    } catch (error) {
        errorMessage.value = 'Wystąpił błąd. Spróbuj ponownie!';
    } finally {
        isSubmitting.value = false;
    }
};
</script>

<style scoped></style>
