<template>
  <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
    <div class="container-fluid">
      <a class="navbar-brand d-flex align-items-center" href="/" router-link-active="active">
        <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo me-2" height="50" />
        SportMeet
      </a>

      <ul class="navbar-nav me-auto mb-md-0">
        <li class="nav-item">
          <router-link to="/events" router-link-active="active" class="nav-link">
            Wydarzenia
          </router-link>
        </li>
      </ul>

      <div v-if="isLoggedIn" class="dropdown">
        
        <img v-if="currentUser.photoUrl"
          :src="currentUser.photoUrl"
          alt="user main image"
          width="40"
          height="40"
          class="me-3"
        />
        <img v-else
          src="@/assets/image/user.png"
          alt="user main image"
          width="40"
          height="40"
          class="me-3"
        />

        <a class="dropdown-toggle text-light text-decoration-none" href="#" role="button" id="dropdownMenuLink"
          data-bs-toggle="dropdown" aria-expanded="false">
          {{ currentUser.firstName }} {{ currentUser.lastName }}
        </a>
        
        <ul class="dropdown-menu mt-3" aria-labelledby="dropdownMenuLink">
          <li><router-link to="/profile" class="dropdown-item">Mój profil <i class="bi bi-person-square"></i></router-link></li>
          <li>
            <hr class="dropdown-divider">
          </li>
          <li><a class="dropdown-item" href="#" @click="logout">Wyloguj <i class="bi bi-box-arrow-left "></i></a></li>
        </ul>
      </div>

      <ul v-else class="navbar-nav">
        <li class="nav-item">
          <router-link to="/login" class="nav-link">Zaloguj się</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/register" class="nav-link">Rejestracja</router-link>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script setup>
import { computed, ref } from 'vue';

const currentUser = ref(JSON.parse(localStorage.getItem('user')));

const isLoggedIn = computed(() => {
  return currentUser.value !== null;
});

const logout = () => {
  localStorage.removeItem('user');
  currentUser.value = null;
};
</script>

<style scoped>
</style>
