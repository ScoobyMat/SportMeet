<template>
    <div v-if="!isLoggedIn" class="card">
        <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo" />
        <div class="app-description">
            <h2>Witaj na platformie SportMeet.</h2>
            <h4>Tutaj w łatwy sposób możesz znaleść partnerów do wspólnych aktywności.</h4>
            <h4>Szukaj i zapisuj się na wydarzenia sportowe w twojej okolicy.</h4>
        </div>
    </div>

    <div v-else>
        <div class="row">
            <div class="col-md-8 card">
                <h2>Twoje nadchodzące wydarzenia</h2>
                <p>Treść związana z nadchodzącymi wydarzeniami</p>
            </div>

            <div class="col-md-4">
                <div class="small-card">
                    <h2>Przeglądaj wydarzenia</h2>
                    <router-link to="/events" class="btn btn-primary">
                        Szukaj wydarzeń
                    </router-link>
                </div>
                <div class="small-card">
                    <h2>Zorganizuj własne wydarzenie</h2>
                    <div class="btn btn-primary">
                        Stwórz wydarzenie
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed, ref } from 'vue';

const currentUser = ref(JSON.parse(localStorage.getItem('user')));

const isLoggedIn = computed(() => {
    return currentUser.value !== null;
});
</script>

<style scoped>
/* .navbar-logo {
    background: rgba(255, 255, 255, 0.25);
    border-radius: 20%;
    padding: 2px;
} */
</style>
