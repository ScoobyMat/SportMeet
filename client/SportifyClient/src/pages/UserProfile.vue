<template>
    <div class="container">
      <div class="row">
        <div class="col-md-6 card">
          <UserCard :user="user" />
        </div>
  
        <div class="col-md-6">
          <div class="small-card">
            <tab heading="About">
              <h4>Description</h4>
            </tab>
          </div>

          <div class="small-card">
              <h4>Interests</h4>
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