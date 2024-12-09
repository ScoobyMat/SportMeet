<template>
    <div v-if="!isLoggedIn" class="card">
        <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo" />
        <div class="app-description">
            <h2>Witaj na platformie SportMeet.</h2>
            <h4>Tutaj w łatwy sposób możesz znaleźć partnerów do wspólnych aktywności.</h4>
            <h4>Szukaj i zapisuj się na wydarzenia sportowe w twojej okolicy.</h4>
        </div>
    </div>

    <div v-else>
        <div class="row">
            <div class="col-md-8 card">
                <h2>Twoje nadchodzące wydarzenia</h2>
                <div v-if="events.length" class="carousel-wrapper">
                    <button class="btn btn-outline-primary left" @click="scrollLeft" :disabled="currentIndex === 0">
                        &#8249;
                    </button>
                    <div class="carousel">
                        <div v-for="event in pagedEvents" :key="event.id" class="eventBox">
                            <img :src="event.imageUrl || 'https://cdn.open-pr.com/V/a/Va07962746_g.jpg'" class="card-img-top" alt="Event Image">
                            <h5>{{ event.eventName }}</h5>
                            <br>
                            <p>Data: {{ event.date }}</p>
                            <p>Godzina: {{ event.time }} </p>
                            <br>
                            <p>Adres: {{ event.address }} </p>
                            <p>Miasto: {{ event.city }}</p>
                        </div>
                    </div>
                    <button class="btn btn-outline-primary right" @click="scrollRight"
                        :disabled="currentIndex + maxVisibleCards >= events.length">
                        &#8250;
                    </button>
                </div>
            <div v-else>Nie masz nadchodzących wydarzeń</div>
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
import { ref, computed, onMounted } from "vue";
import EventService from "@/services/EventService";

const events = ref([]);
const maxVisibleCards = 3;
const currentIndex = ref(0);

const pagedEvents = computed(() => {
    return events.value.slice(currentIndex.value, currentIndex.value + maxVisibleCards);
});

const scrollLeft = () => {
    if (currentIndex.value > 0) {
        currentIndex.value -= 1;
    }
};

const scrollRight = () => {
    if (currentIndex.value + maxVisibleCards < events.value.length) {
        currentIndex.value += 1;
    }
};

const currentUser = ref(JSON.parse(localStorage.getItem("user")));
const isLoggedIn = computed(() => currentUser.value !== null);

const fetchEvents = async () => {
    if (!currentUser.value || !currentUser.value.id) {
        console.warn("No user logged in.");
        return;
    }

    try {
        events.value = await EventService.GetUpcomingEvents(currentUser.value.id);
    } catch (error) {
        console.error("Failed to load events:", error.message);
    }
};

onMounted(() => {
    fetchEvents();
});
</script>

<style scoped>

.carousel-wrapper {
    display: flex;
    align-items: center;
    width: 100%;
    position: relative;
}

.carousel {
    display: flex;
    gap: 0.5rem;
    width: 100%;
    flex-grow: 1;
    padding: 1rem;
    overflow: hidden;
}

.eventBox {
    background: rgba(255, 255, 255, 0.25);
    border-radius: 15px;
    padding: 2rem;
    flex: 0 0 auto;
    min-width: 33.33%;
    min-height: 30vh;
    box-sizing: border-box;
}

.right {
    margin-left: 1rem;
}

.card-img-top {
    width: 150px;
    height: 150px;
    margin-bottom: 20px;
}

h2{
    margin-top: 150px;
}
</style>
