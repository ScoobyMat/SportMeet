<template>
  <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
    <div class="container-fluid">
      <a class="navbar-brand d-flex align-items-center" href="/" router-link-active="active">
        <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo me-2" height="50" />
        SportMeet
      </a>

      <ul class="navbar-nav me-auto mb-md-0">
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="eventsDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
            Wydarzenia
          </a>
          <ul class="dropdown-menu" aria-labelledby="eventsDropdown">
            <li>
              <router-link to="/events" class="dropdown-item">Przeglądaj wydarzenia w okolicy</router-link>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>
            <li>
              <router-link to="/event/create" class="dropdown-item">Zorganizuj wydarzenie</router-link>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>
            <li>
              <router-link to="/my-events" class="dropdown-item">Moje wydarzenia</router-link>
            </li>
          </ul>
        </li>
      </ul>

      <div v-if="isLoggedIn" class="dropdown">
        <img v-if="currentUser.photoUrl" :src="currentUser.photoUrl" alt="user main image" width="40" height="40" class="me-3" />
        <img v-else src="@/assets/image/user.png" alt="User profile image" width="40" height="40" class="me-3" />

        <a class="dropdown-toggle text-light text-decoration-none" href="#" role="button" id="dropdownMenuLink" data-bs-toggle="dropdown" aria-expanded="false">
          {{ currentUser.firstName }} {{ currentUser.lastName }}
        </a>

        <ul class="dropdown-menu mt-3" aria-labelledby="dropdownMenuLink">
          <li>
            <router-link to="/profile" class="dropdown-item">Mój profil <i class="bi bi-person-square"></i></router-link>
          </li>
          <li>
            <hr class="dropdown-divider">
          </li>
          <li>
            <a class="dropdown-item" href="#" @click="logout">Wyloguj <i class="bi bi-box-arrow-left "></i></a>
          </li>
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
import { useUserStore } from '@/stores/userStore';
import { computed } from 'vue';

const userStore = useUserStore();

const currentUser = computed(() => userStore.currentUser);
const isLoggedIn = computed(() => userStore.currentUser !== null);

const logout = () => {
  userStore.logout();
};
</script>

<style scoped>
.dropdown-menu {
  background-color: #f8f9fa;
}

.dropdown-item:hover {
  background-color: #343a40;
  color: white;
}
</style>
