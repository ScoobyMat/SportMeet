<template>
    <div class="container my-5">
      <h2>Zmień hasło</h2>
      <form @submit.prevent="updatePassword">
        <div class="mb-3">
          <label for="newPassword" class="form-label">Nowe hasło</label>
          <input
            type="password"
            id="newPassword"
            v-model="form.newPassword"
            class="form-control"
            required
            minlength="8"
          />
        </div>
        <div class="mb-3">
          <label for="confirmPassword" class="form-label">Potwierdź nowe hasło</label>
          <input
            type="password"
            id="confirmPassword"
            v-model="form.confirmPassword"
            class="form-control"
            required
            minlength="8"
          />
        </div>
        <div v-if="passwordMismatch" class="alert alert-danger">
          Hasła nie zgadzają się!
        </div>
        <button type="submit" class="btn btn-primary">Zapisz zmiany</button>
        <button type="button" class="btn btn-secondary ms-2" @click="cancelEdit">
          Anuluj
        </button>
      </form>
    </div>
  </template>
  
  <script setup>
  import UserService from '@/services/UserService';
import { useAuthStore } from '@/stores/auth';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
  
  const router = useRouter();
  const toast = useToast();
  const authStore = useAuthStore();
  
  const form = ref({
    newPassword: '',
    confirmPassword: ''
  });
  
  const passwordMismatch = computed(() => form.value.newPassword !== form.value.confirmPassword);
  
  const updatePassword = async () => {
    if (passwordMismatch.value) {
      toast.error("Hasła nie zgadzają się!");
      return;
    }
  
    try {
      const payload = {
        Id: authStore.getUserId,
        NewPassword: form.value.newPassword
      };
      await UserService.changeCredentials(payload);
      toast.success("Hasło zostało zaktualizowane.");
      router.push("/profile");
    } catch (error) {
      toast.error(error.message || "Wystąpił błąd podczas aktualizacji hasła.");
    }
  };
  
  const cancelEdit = () => {
    router.push("/profile");
  };
  </script>
  
  <style scoped>
  .container {
    max-width: 500px;
  }
  </style>
  