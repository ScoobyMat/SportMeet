<template>
  <div class="row">
    <div class="col-md-8 card p-3 mb-4">
      <div class="FilterBox">
        <h2>Filtruj wydarzenia</h2>
        <div class="row">
          <div class="col-md-3 mb-3">
            <label for="location" class="form-label">Miasto</label>
            <input v-model="filters.location" type="text" id="location" class="form-control" placeholder="Wpisz nazwę miasta" />
          </div>
          <div class="col-md-3 mb-3">
            <label for="sportType" class="form-label">Sport</label>
            <select v-model="filters.sportType" id="sportType" class="form-select">
              <option value="" disabled>Wybierz sport</option>
              <option v-for="sport in availableSports" :key="sport" :value="sport">{{ sport }}</option>
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
        <button @click="applyFilters" class="btn btn-primary">Filtruj wydarzenia</button>
      </div>

      <div v-if="noEventsFound" class="alert alert-warning mt-3">
        <strong>Brak wydarzeń</strong> - Nie znaleziono żadnych wydarzeń pasujących do wybranych filtrów.
      </div>

      <div class="row">
        <div v-for="event in events" :key="event.id" class="col-md-3 mt-2">
          <div :class="['event-card', { highlighted: event.id === highlightedEvent }]" @click="openEventDetails(event)">
            <img :src="event.photoUrl || sportImages[event.sportType]" class="card-img-top" alt="Event Image">
            <h5>{{ event.eventName }}</h5>
            <p>Data: {{ formatDate(event.date) }}</p>
            <p>Godzina: {{ event.time }}</p>
            <p>Miasto: {{ event.city }}</p>
            <button v-if="!event.isMember" class="btn btn-primary" @click.stop="joinEvent(event.id)">Dołącz</button>
            <button v-else class="btn btn-success" @click.stop="goToEvent(event.id)">Przejdź do wydarzenia</button>
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
        <img :src="selectedEvent?.photoUrl || sportImages[selectedEvent.sportType]" alt="Event Photo" class="event-img">
        <div class="event-details">
          <h3>{{ selectedEvent.eventName }}</h3>
          <p><strong>Sport:</strong> {{ selectedEvent.sportType }}</p>
          <p><strong>Opis:</strong> {{ selectedEvent.description }}</p>
          <p><strong>Gdzie:</strong> {{ selectedEvent.address }}, {{ selectedEvent.city }}</p>
          <p><strong>Kiedy:</strong> {{ formatDate(selectedEvent.date) }} | {{ selectedEvent.time }}</p>
          <p><strong>Liczba miejsc:</strong> {{ selectedEvent.currentMembers }}/{{ selectedEvent.maxMembers }}</p>
          <p><strong>Organizator:</strong> {{ selectedEvent.createdByUser }}</p>
          <button v-if="!selectedEvent.isMember" class="btn btn-primary" @click.stop="joinEvent(selectedEvent.id)">Dołącz</button>
          <button v-else class="btn btn-success" @click.stop="goToEvent(selectedEvent.id)">Przejdź do wydarzenia</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { sportImages } from '@/constants/sports';
import EventService from "@/services/EventService";
import GroupMemberService from '@/services/GroupMemberService';
import GroupService from '@/services/GroupService';
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import HereMap from "../components/Map/HereMap.vue";
import { useAuthStore } from '../stores/auth';

const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

const filters = ref({
  location: "",
  sportType: '',
  startDate: "",
  endDate: "",
});

const events = ref([]);
const selectedEvent = ref(null);
const highlightedEvent = ref(null);
const noEventsFound = ref(false);

const apikey = 'LlRhGrawpujpHpgpxdaTjmwDqM5LRxmhJqIruMeZJNk';

const center = ref({
  lat: 52.2296756,
  lng: 21.0122287,
});

const fetchEvents = async () => {
  try {
    const allEvents = await EventService.GetEvents();
    const userId = parseInt(authStore.getUserId);

    for (const event of allEvents) {
      const members = await GroupService.getGroupMembers(event.groupId);
      event.isMember = members.some(member => member.userId === userId);
    }

    events.value = allEvents;
  } catch (error) {
    console.error("Error fetching all events:", error.message);
  }
};

const applyFilters = async () => {
  try {
    const filteredEvents = await EventService.FilterEvents(filters.value);

    events.value = filteredEvents;

    const userId = parseInt(authStore.getUserId);
    for (const event of events.value) {
      const members = await GroupService.getGroupMembers(event.groupId);
      event.isMember = members.some(member => member.userId === userId);
    }

    noEventsFound.value = events.value.length === 0;

    if (filters.value.location) {
      const cityCoordinates = await geocodeCity(filters.value.location);
      center.value = cityCoordinates;
    }
  } catch (error) {
    console.error("Error applying filters:", error.message);
  }
};


const geocodeCity = async (city) => {
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
  try {
    const event = events.value.find(e => e.id === eventId);

    const userId = parseInt(authStore.getUserId);
    const role = 'Member';

    await GroupMemberService().addMember(event.groupId, userId, role);

    toast.success('Dołączyłeś do grupy wydarzenia!');
    router.push({ name: 'EventGroup', params: { eventId } });
  } catch (error) {
    toast.error('Wystąpił błąd przy dołączaniu do grupy.');
  }
};


const goToEvent = (eventId) => {
  router.push({ name: "EventGroup", params: { eventId } });
};

const openEventDetails = (event) => {
  selectedEvent.value = event;
};

const closeModal = () => {
  selectedEvent.value = null;
};

const highlightEvent = (eventId) => {
  highlightedEvent.value = eventId;
  console.log("Podświetlone wydarzenie:", highlightedEvent.value);
};

const mapMarkers = computed(() => {
  return events.value
    .filter(event => event.latitude && event.longitude)
    .map(event => ({
      lat: event.latitude,
      lng: event.longitude,
      id: event.id,
    }));
});

const formatDate = (date) => {
  return new Date(date).toLocaleDateString();
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
  height: 100px;
}

.event-card .btn {
  margin-top: auto;
}

.FilterBox {
  border: 1px solid white;
  padding: 1rem;
  width: 100%;
}

.highlighted {
  background-color: aqua;
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
</style>
