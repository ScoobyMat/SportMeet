<template>
  <div class="row">
    <div class="col-md-8 card p-3 mb-4">
      <div class="FilterBox">
        <h2>Filtruj wydarzenia</h2>
        <div class="row">

          <div class="col-md-3 mb-3">
            <label for="city" class="form-label">Miasto</label>
            <input v-model="filters.city" type="text" id="city" class="form-control" placeholder="Wpisz nazwę miasta" />
          </div>

          <div class="col-md-3 mb-3">
            <label for="sportType" class="form-label">Sport</label>
            <select v-model="filters.sportType" id="sportType" class="form-select">
              <option value="" disabled>Wybierz sport</option>
              <option v-for="sport in sportsList" :key="sport" :value="sport">
                {{ sport }}
              </option>
            </select>
          </div>

          <div class="col-md-3 mb-3">
            <label for="startDate" class="form-label">Data początkowa</label>
            <input v-model="filters.startDate" type="date" id="startDate" class="form-control" />
          </div>

          <div class="col-md-2 mb-3">
            <label for="endDate" class="form-label">Data końcowa</label>
            <input v-model="filters.endDate" type="date" id="endDate" class="form-control" />
          </div>

        </div>
        <div class="d-flex gap-2">
          <button @click="applyFilters" class="btn btn-primary">Filtruj wydarzenia</button>
          <button @click="clearFilters" class="btn btn-secondary">Wyczyść filtry</button>
        </div>
      </div>

      <div v-if="noEventsFound" class="alert alert-warning mt-3">
        <strong>Brak wydarzeń</strong> - Nie znaleziono żadnych wydarzeń pasujących do wybranych filtrów.
      </div>

      <div class="row mt-3">
        <div v-for="event in events" :key="event.id" class="col-md-3 mt-2">
          <div :class="['event-card', { highlighted: event.id === highlightedEvent }]" @click="openEventDetails(event)">
            <img :src="event.photoUrl || sportImages[event.sportType]" class="card-img-top" alt="Event Image" />

            <h5>{{ event.eventName }}</h5>
            <p>Data: {{ formatDate(event.date) }}</p>
            <p>Godzina: {{ event.time }}</p>
            <p>Miasto: {{ event.city }}</p>

            <div v-if="event.isMember">
              <p class="text-success">Jesteś zapisany</p>
              <button class="btn btn-secondary" @click.stop="goToEvent(event.id)">
                Przejdź do wydarzenia
              </button>
            </div>
            <div v-else>
              <div v-if="event.currentMembers >= event.maxParticipants">
                <p class="text-danger">Wydarzenie pełne</p>
                <button class="btn btn-danger" disabled>Wydarzenie pełne</button>
              </div>
              <div v-else>
                <button class="btn btn-primary" @click.stop="joinEvent(event.id)">
                  Dołącz
                </button>
              </div>

            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="col-md-4 card p-3 mb-4">
      <h2 class="text-center p-2">Mapa wydarzeń:</h2>
      <HereMap :center="center" :markers="mapMarkers" @marker-click="highlightEvent" />
    </div>

    <div v-if="selectedEvent" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <img :src="selectedEvent.photoUrl || sportImages[selectedEvent.sportType]" alt="Event Photo"
          class="event-img" />
        <div class="event-details">
          <h3>{{ selectedEvent.eventName }}</h3>
          <p><strong>Sport:</strong> {{ selectedEvent.sportType }}</p>
          <p><strong>Opis:</strong> {{ selectedEvent.description }}</p>
          <p><strong>Gdzie:</strong> {{ selectedEvent.address }}, {{ selectedEvent.city }}</p>
          <p><strong>Kiedy:</strong> {{ formatDate(selectedEvent.date) }} | {{ selectedEvent.time }}</p>
          <p>
            <strong>Liczba miejsc:</strong>
            {{ selectedEvent.currentMembers }}/{{ selectedEvent.maxParticipants }}
          </p>
          <p><strong>Organizator:</strong> {{ selectedEvent.createdByUserName }}</p>
          <div v-if="selectedEvent.isMember">
            <button class="btn btn-secondary" @click.stop="goToEvent(selectedEvent.id)">
              Przejdź do wydarzenia
            </button>
          </div>
          <div v-else>
            <div v-if="selectedEvent.currentMembers >= selectedEvent.maxParticipants">
              <p class="text-danger">Wydarzenie pełne</p>
              <button class="btn btn-danger" disabled>Wydarzenie pełne</button>
            </div>
            <div v-else>
              <button class="btn btn-primary" @click.stop="joinEvent(selectedEvent.id)">
                Dołącz
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";

import HereMap from "@/components/Map/HereMap.vue";
import { availableSports, sportImages } from "@/constants/sports";
import EventService from "@/services/EventService";
import { useAuthStore } from "@/stores/auth";
import EventAttendeesService from "../services/EventAttendeesService";

const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

const sportsList = ref(availableSports);
const filters = ref({
  city: "",
  sportType: "",
  startDate: "",
  endDate: "",
});

const events = ref([]);
const selectedEvent = ref(null);
const highlightedEvent = ref(null);
const noEventsFound = ref(false);

const center = ref({
  lat: 52.2296756,
  lng: 21.0122287,
});

const fetchEvents = async () => {
  try {
    const fetchedEvents = await EventService.getUpcomingEvents();
    const userId = parseInt(authStore.getUserId);
    await Promise.all(
      fetchedEvents.map(async (ev) => {
        const attendees = await EventAttendeesService.getEventAttendees(ev.id);
        ev.isMember = attendees.some((a) => a.userId === userId);
      })
    );
    events.value = fetchedEvents;
  } catch (error) {
    console.error("Error fetching events:", error);
  }
};

const applyFilters = async () => {
  try {
    const params = {
      city: filters.value.city,
      sportType: filters.value.sportType,
      from: filters.value.startDate,
      to: filters.value.endDate,
    };
    const filteredEvents = await EventService.searchEvents(params);
    const userId = parseInt(authStore.getUserId);
    await Promise.all(
      filteredEvents.map(async (ev) => {
        const attendees = await EventAttendeesService.getEventAttendees(ev.id);
        ev.isMember = attendees.some((a) => a.userId === userId);
      })
    );
    events.value = filteredEvents;
    noEventsFound.value = events.value.length === 0;

    if (filters.value.city) {
      const cityCoordinates = await geocodeCity(filters.value.city);
      center.value = cityCoordinates;
    }
  } catch (error) {
    console.error("Error applying filters:", error);
  }
};

const clearFilters = () => {
  filters.value = {
    city: "",
    sportType: "",
    startDate: "",
    endDate: "",
  };
  fetchEvents();
};

const geocodeCity = async (city) => {
  const apikey = "LlRhGrawpujpHpgpxdaTjmwDqM5LRxmhJqIruMeZJNk";
  const url = `https://geocode.search.hereapi.com/v1/geocode?q=${encodeURIComponent(city)}&apiKey=${apikey}`;
  const response = await fetch(url);
  const data = await response.json();
  const firstResult = data.items[0];
  if (firstResult) {
    return { lat: firstResult.position.lat, lng: firstResult.position.lng };
  }
  return { lat: 52.2296756, lng: 21.0122287 };
};

const joinEvent = async (eventId) => {
  const currentEvent = events.value.find(ev => ev.id === eventId);
  if (currentEvent && currentEvent.currentMembers >= currentEvent.maxParticipants) {
    toast.error("Wydarzenie jest pełne.");
    return;
  }
  try {
    await EventAttendeesService.joinEvent(eventId, parseInt(authStore.getUserId));
    toast.success("Dołączyłeś do wydarzenia!");
    goToEvent(eventId);
  } catch (error) {
    toast.error("Wystąpił błąd przy dołączaniu do wydarzenia.");
  }
};

const goToEvent = (eventId) => {
  router.push({ name: "EventGroup", params: { eventId } });
};

const openEventDetails = (ev) => {
  selectedEvent.value = ev;
};

const closeModal = () => {
  selectedEvent.value = null;
};

const highlightEvent = (eventId) => {
  const foundEvent = events.value.find(ev => ev.id === eventId);
  if (foundEvent) {
    openEventDetails(foundEvent);
  }
};

const mapMarkers = computed(() => {
  return events.value
    .filter(ev => ev.latitude && ev.longitude)
    .map(ev => ({
      lat: ev.latitude,
      lng: ev.longitude,
      id: ev.id,
    }));
});

const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleDateString();
};

onMounted(() => {
  if (authStore.isAuthenticated) {
    fetchEvents();
  }
});

watch(
  () => authStore.isAuthenticated,
  (newVal) => {
    if (newVal) {
      fetchEvents();
    } else {
      events.value = [];
    }
  }
);
</script>

<style scoped>
.FilterBox {
  border: 1px solid white;
  padding: 1rem;
  width: 100%;
  margin-bottom: 1rem;
}

.event-card {
  background-color: azure;
  border: 1px solid black;
  padding: 15px;
  cursor: pointer;
  transition: transform 0.2s;
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
}

.event-card:hover {
  transform: scale(1.02);
}

.card-img-top {
  object-fit: cover;
  height: 200px;
  width: 100%;
}

.alert-warning {
  margin-top: 1rem;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background-color: #fff;
  border-radius: 10px;
  overflow: hidden;
  max-width: 800px;
  width: 100%;
  display: flex;
  flex-direction: row;
}

.event-img {
  width: 50%;
  object-fit: cover;
}

.event-details {
  padding: 20px;
  width: 50%;
  display: flex;
  flex-direction: column;
}

.members-list {
  list-style: none;
  padding: 0;
}

.members-list li {
  display: flex;
  align-items: center;
  padding: 5px;
  border-bottom: 1px solid #ddd;
}

.member-img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 10px;
}

.event-group {
  padding: 1rem;
}

.navbar-logo {
  width: 200px;
  margin: 0 auto;
  display: block;
}

.app-description {
  text-align: center;
  margin: 20px 0;
}
</style>
