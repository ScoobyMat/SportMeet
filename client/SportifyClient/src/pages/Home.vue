<template>
    <div class="card">
        <img src="../assets/image/logo2.png" alt="Logo" class="navbar-logo" />
        <div class="app-description">
            <h2>Witaj na platformie Sportify.</h2>
            <h4>Tutaj w łatwy sposób możesz znaleść partnerów do wspólnych aktywności.</h4>
            <h4>Szukaj i zapisuj się na wydarzenia sportowe w twojej okolicy.</h4>
        </div>
        <div class="row mt-5">
            <div class="activities col-12 col-md-6">
                <h2>Twoje nadchodzące wydarzenia</h2>
                <p>Treść związana z nadchodzącymi wydarzeniami</p>
            </div>

            <div class="menu col-12 col-md-6">
                <h2>Przeglądaj wydarzenia</h2>
                    <router-link to="/events" class="btn btn-primary">
                        Szukaj wydarzeń
                    </router-link>
                <hr>
                <h2>Zorganizuj własne wydarzenie</h2>
                <div class="btn btn-primary">
                    Stwórz wydarzenie
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
</script>

<style scoped>
/* .navbar-logo {
    background: rgba(255, 255, 255, 0.25);
    border-radius: 20%;
    padding: 2px;
} */
</style>
