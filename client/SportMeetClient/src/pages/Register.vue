<template>
    <div class="d-flex justify-content-center align-items-center">
        <div class="card">
            <img src="/logo.png" alt="Logo" width="50" />
            <h2>Rejestracja</h2>

            <form @submit.prevent="register" class="needs-validation" novalidate>
                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-person"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="text" id="firstName" v-model="form.firstName" class="form-control"
                            placeholder="Imię" required
                            :class="{ 'is-invalid': !form.firstName && submitted, 'is-valid': form.firstName && submitted }" />
                        <label for="firstName">Imię</label>
                        <div class="invalid-feedback position-absolute">Proszę podać imię.</div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-person"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="text" id="lastName" v-model="form.lastName" class="form-control"
                            placeholder="Nazwisko" required
                            :class="{ 'is-invalid': !form.lastName && submitted, 'is-valid': form.lastName && submitted }" />
                        <label for="lastName">Nazwisko</label>
                        <div class="invalid-feedback position-absolute">Proszę podać nazwisko.</div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-gender-ambiguous"></i></span>
                    <select id="gender" v-model="form.gender" class="form-select" required
                        :class="{ 'is-invalid': !form.gender && submitted, 'is-valid': form.gender && submitted }">
                        <option value="" disabled selected>Wybierz płeć</option>
                        <option value="Mężczyzna">Mężczyzna</option>
                        <option value="Kobieta">Kobieta</option>
                    </select>
                    <div class="invalid-feedback position-absolute">Proszę wybrać płeć.</div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-calendar"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="date" id="dateOfBirth" v-model="form.dateOfBirth" class="form-control"
                            placeholder="Data urodzenia" required
                            :class="{ 'is-invalid': !form.dateOfBirth && submitted, 'is-valid': form.dateOfBirth && submitted }" />
                        <label for="dateOfBirth">Data urodzenia</label>
                        <div class="invalid-feedback position-absolute">Proszę podać datę urodzenia.</div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-building"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="text" id="city" v-model="form.city" class="form-control" placeholder="Miasto"
                            required
                            :class="{ 'is-invalid': !form.city && submitted, 'is-valid': form.city && submitted }" />
                        <label for="city">Miasto</label>
                        <div class="invalid-feedback position-absolute">Proszę podać miasto.</div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-globe"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="text" id="country" v-model="form.country" class="form-control"
                            placeholder="Państwo" required
                            :class="{ 'is-invalid': !form.country && submitted, 'is-valid': form.country && submitted }" />
                        <label for="country">Państwo</label>
                        <div class="invalid-feedback position-absolute">Proszę podać państwo.</div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-envelope"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="email" id="email" v-model="form.email" class="form-control" placeholder="Email"
                            required pattern="[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            :class="{ 'is-invalid': (!form.email && submitted) || (form.email && !form.email.match(/[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/) && submitted), 'is-valid': form.email && form.email.match(/[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/) && submitted }" />
                        <label for="email">Email</label>
                        <div class="invalid-feedback position-absolute">Proszę podać poprawny adres email.</div>
                    </div>
                </div>


                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-lock"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="password" id="password" v-model="form.password" class="form-control"
                            placeholder="Hasło" required minlength="8" :class="{
                                'is-invalid': (submitted && (!form.password || form.password.length < 8)),
                                'is-valid': (submitted && form.password && form.password.length >= 8)
                            }" />
                        <label for="password">Hasło</label>
                        <div class="invalid-feedback position-absolute">
                            Hasło musi mieć co najmniej 8 znaków.
                        </div>
                    </div>
                </div>

                <div class="input-group mt-4 position-relative">
                    <span class="input-group-text fs-4"><i class="bi bi-lock"></i></span>
                    <div class="form-floating flex-grow-1">
                        <input type="password" id="confirmPassword" v-model="form.confirmPassword" class="form-control"
                            placeholder="Potwierdź Hasło" required :class="{
                                'is-invalid': (submitted && (checkPassword || !form.confirmPassword)),
                                'is-valid': (submitted && !checkPassword && form.confirmPassword)
                            }" />
                        <label for="confirmPassword">Potwierdź Hasło</label>
                        <div class="invalid-feedback position-absolute">
                            Hasła się nie zgadzają!
                        </div>
                    </div>
                </div>

                <button type="submit" class="btn btn-primary btn-lg w-100 mt-4" :disabled="isSubmitting">
                    Utwórz konto
                </button>
            </form>
        </div>
    </div>
</template>

<script setup>
import AuthenticationService from '@/services/AuthenticationService';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

const router = useRouter();
const toast = useToast();

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

const isSubmitting = ref(false);
const submitted = ref(false);

const checkPassword = computed(() => form.value.password !== form.value.confirmPassword);

const isFormValid = computed(() => {
    return (
        form.value.firstName &&
        form.value.lastName &&
        form.value.gender &&
        form.value.dateOfBirth &&
        form.value.city &&
        form.value.country &&
        form.value.email &&
        form.value.password &&
        !checkPassword.value &&
        form.value.password.length >= 8
    );
});

const register = async () => {
    submitted.value = true;

    if (!isFormValid.value) {
        toast.error('Proszę poprawić błędy w formularzu');
        return;
    }

    isSubmitting.value = true;

    try {
        await AuthenticationService.register(form.value);
        toast.success('Rejestracja zakończona sukcesem. Możesz się teraz zalogować.');
        router.push('/login');
    } catch (error) {
        toast.error(error.message);
    } finally {
        isSubmitting.value = false;
    }
};
</script>


<style scoped>
.card {
    width: 60%;
}
</style>
