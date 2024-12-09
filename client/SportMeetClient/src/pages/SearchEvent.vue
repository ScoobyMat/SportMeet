<template>
    <div class="row">
        <div class="col-md-8 card p-3 mb-4">
            <h2>Filtruj wydarzenia</h2>

            <div class="row">
                <div class="col-md-4 mb-3">
                    <label for="location" class="form-label">Miasto</label>
                    <input v-model="filters.location" type="text" id="location" class="form-control"
                        placeholder="Wpisz nazwę miasta" />
                </div>
                <div class="col-md-4 mb-3">
                    <label for="startDate" class="form-label">Data początkowa</label>
                    <input v-model="filters.startDate" type="date" id="startDate" class="form-control" />
                </div>
                <div class="col-md-4 mb-3">
                    <label for="endDate" class="form-label">Data końcowa</label>
                    <input v-model="filters.endDate" type="date" id="endDate" class="form-control" />
                </div>
            </div>

            <button @click="applyFilters" class="btn btn-primary">Filtruj wydarzenia</button>

            <div class="row">
                <div class="col-md-2 mt-2" v-for="event in events" :key="event.id">
                    <div class="event-card" @click="openEventDetails(event)">
                        <img :src="event.imageUrl || 'https://cdn.open-pr.com/V/a/Va07962746_g.jpg'"
                            class="card-img-top" alt="Event Image">
                        <h5>{{ event.eventName }}</h5>
                        <p>Data: {{ event.date }}</p>
                        <p>Godzina: {{ event.time }}</p>
                        <p>Miasto: {{ event.city }}</p>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4 card p-3 mb-4">
            <h2 class="text-center p-2">Mapa wydarzeń:</h2>
            <HereMap :center="center" />
        </div>

        <EventModal v-if="selectedEvent" :event="selectedEvent" @close="selectedEvent = null" />
    </div>
</template>

<script setup>
import EventService from "@/services/EventService";
import { onMounted, ref } from "vue";
import EventModal from "../components/EventDetails.vue";
import HereMap from "../components/Map/HereMap.vue";

const filters = ref({
    location: "",
    startDate: "",
    endDate: "",
});

const events = ref([]);
const selectedEvent = ref(null);

const center = ref({
    lat: 52.2296756,
    lng: 21.0122287,
});

const fetchEvents = async () => {
    try {
        const allEvents = await EventService.GetEvents();
        events.value = allEvents;
    } catch (error) {
        console.error("Error fetching all events:", error.message);
    }
};

const applyFilters = async () => {
    try {
        const filteredEvents = await EventService.FilterEvents(filters.value);
        events.value = filteredEvents;
    } catch (error) {
        console.error("Error applying filters:", error.message);
    }
};

const openEventDetails = (event) => {
    selectedEvent.value = event;
};

onMounted(() => {
    fetchEvents();
});
</script>

<style scoped>
.event-card {
    background-color: azure;
    border: 1px solid black;
    padding: 15px;
    cursor: pointer;
    transition: transform 0.2s;
}

.event-card:hover {
    transform: scale(1.02);
}

.card-img-top {
    width: 125px;
    height: 125px;
    margin-bottom: 10px;
}
</style>