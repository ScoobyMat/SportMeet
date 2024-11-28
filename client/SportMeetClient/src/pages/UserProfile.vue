<template>
  <div class="container">
    <div class="row">

      <div class="col-md-6 card">
        <UserCard :user="user" />
        <button class="btn btn-primary mb-5">
          Edytuj profil
        </button>
      </div>

      <div class="col-md-6">
        <div class="small-card">
          <tab heading="About">
            <h4>Opis</h4>
            <p>{{ user.description }}</p>
          </tab>
        </div>

        <div class="small-card">
          <h4>Zainteresowania</h4>
          <p>{{ user.preferredSports }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>

import UserCard from "@/components/user/ProfileCard.vue";
import userService from "@/services/userService";
import { computed, onMounted, ref } from "vue";

const currentUser = ref(JSON.parse(localStorage.getItem("user")));
const isLoggedIn = computed(() => currentUser.value !== null);

const user = ref({});

const userId = computed(() => currentUser.value?.id);

onMounted(async () => {
  if (!isLoggedIn.value) return;

  try {
    const userData = await userService.GetUser(userId.value);
    user.value = userData;
  } catch (error) {
    console.error("Nie udało się pobrać danych użytkownika:", error.message);
  }
});
</script>

<style scoped>
</style>