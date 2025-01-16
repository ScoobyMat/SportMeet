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
          <div
            class="col-md-3 mt-2"
            v-for="event in events"
            :key="event.id"
            :class="{ highlighted: highlightedEvent === event.id }"
          >
            <div class="event-card">
              <div class="Details" @click="openEventDetails(event)">
                <img :src="event.photoUrl || sportImages[event.sportType]" class="card-img-top" alt="Event Image">
                <h5>{{ event.eventName }}</h5>
                <p>Data: {{ formatDate(event.date) }}</p>
                <p>Godzina: {{ event.time }}</p>
                <p>Miasto: {{ event.city }}</p>
              </div>
              <button class="btn btn-primary" @click="joinEvent(event.id)"> Dołącz </button>
            </div>
          </div>
        </div>
      </div>
  
      <div class="col-md-4 card p-3 mb-4">
        <h2 class="text-center p-2">Mapa wydarzeń:</h2>
        <HereMap :center="center" :markers="mapMarkers" @marker-click="highlightEvent" />
      </div>
  
      <EventDetails v-if="selectedEvent" :event="selectedEvent" @close="selectedEvent = null" />
    </div>
  </template>
  
  <script setup>
  import { availableSports, sportImages } from '@/constants/sports';
import EventService from "@/services/EventService";
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import EventDetails from "../components/EventDetails.vue";
import HereMap from "../components/Map/HereMap.vue";
  
  const router = useRouter();
  
  const filters = ref({
    location: "",
    sportType: '',
    startDate: "",
    endDate: "",
  });
  
  const events = ref([]);
  const selectedEvent = ref(null);
  const highlightedEvent = ref(null);
  const noEventsFound = ref(false);  // Nowa zmienna, która przechowuje status braku wydarzeń
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
      // Filtruj wydarzenia
      const filteredEvents = await EventService.FilterEvents(filters.value);
      events.value = filteredEvents;
  
      // Jeśli nie ma żadnych wydarzeń, ustaw flagę `noEventsFound`
      if (events.value.length === 0) {
        noEventsFound.value = true;
      } else {
        noEventsFound.value = false;
      }
  
      // Geokodowanie miasta (np. Gdańsk)
      if (filters.value.location) {
        const cityCoordinates = await geocodeCity(filters.value.location);
        center.value = cityCoordinates;  // Uaktualnij środek mapy na współrzędne miasta
      }
  
    } catch (error) {
      console.error("Error applying filters:", error.message);
    }
  };
  
  const geocodeCity = async (city) => {
    const url = `https://geocode.search.hereapi.com/v1/geocode?q=${encodeURIComponent(city)}&apiKey=LlRhGrawpujpHpgpxdaTjmwDqM5LRxmhJqIruMeZJNk`;
    const response = await fetch(url);
    const data = await response.json();
    const firstResult = data.items[0];
  
    if (firstResult) {
      return { lat: firstResult.position.lat, lng: firstResult.position.lng };
    }
  
    return { lat: 52.2296756, lng: 21.0122287 };  // Domyślne współrzędne (Warszawa) jeśli brak danych
  };
  
  const joinEvent = (eventId) => {
    router.push({ name: 'EventGroup', params: { eventId } });
  };
  
  const openEventDetails = (event) => {
    selectedEvent.value = event;
  };
  
  const highlightEvent = (eventId) => {
    highlightedEvent.value = eventId;
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
  
//   const formatTime = (time) => {
//     return new Date(`1970-01-01T${time}Z`).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
//   };
  
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
  
  .highlighted .event-card {
    background-color: aqua;
  }
  
  .alert-warning {
    margin-top: 1rem;
  }
  </style>
  