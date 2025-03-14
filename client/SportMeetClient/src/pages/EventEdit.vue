<template>
  <div class="event-edit container">
    <h2>Edytuj wydarzenie</h2>
      <form @submit.prevent="updateEvent">
        <div class="mb-3">
          <label for="eventName" class="form-label">Nazwa wydarzenia</label>
          <input v-model="event.eventName" type="text" id="eventName" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="description" class="form-label">Opis</label>
          <textarea v-model="event.description" id="description" class="form-control"></textarea>
        </div>
        <div class="mb-3">
          <label for="sportType" class="form-label">Sport</label>
          <input v-model="event.sportType" type="text" id="sportType" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="maxParticipants" class="form-label">Maksymalna liczba uczestników</label>
          <input v-model.number="event.maxParticipants" type="number" id="maxParticipants" min="1" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="address" class="form-label">Adres</label>
          <input v-model="event.address" type="text" id="address" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="city" class="form-label">Miasto</label>
          <input v-model="event.city" type="text" id="city" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="date" class="form-label">Data</label>
          <input v-model="event.date" type="date" id="date" class="form-control" required />
        </div>
        <div class="mb-3">
          <label for="time" class="form-label">Godzina</label>
          <input v-model="event.time" type="time" id="time" class="form-control" required />
        </div>
        <button type="submit" class="btn btn-primary">Zapisz zmiany</button>
        <button type="button" class="btn btn-secondary ms-2" @click="cancelEdit">Anuluj</button>
      </form>
    </div>
</template>

<script setup>
import EventService from "@/services/EventService";
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useToast } from "vue-toastification";

const route = useRoute();
const router = useRouter();
const toast = useToast();

const event = ref({
  id: 0,
  eventName: "",
  description: "",
  sportType: "",
  maxParticipants: 0,
  address: "",
  city: "",
  date: "",
  time: "",
  photoUrl: null,
  photoPublicId: null,
  createdByUserId: 0,
});

const fetchEvent = async () => {
  try {
    const eventId = parseInt(route.params.eventId);
    const data = await EventService.getEventById(eventId);
    event.value = data;
  } catch (error) {
    console.error("Błąd pobierania wydarzenia:", error);
    toast.error("Nie udało się pobrać danych wydarzenia.");
  }
};

const updateEvent = async () => {
  try {
    event.value.time = formatTimeForBackend(event.value.time);
    
    await EventService.updateEvent(event.value);
    toast.success("Wydarzenie zostało zaktualizowane.");
    router.push("/myEvents");
  } catch (error) {
    console.error("Błąd aktualizacji wydarzenia:", error);
    toast.error("Nie udało się zaktualizować wydarzenia.");
  }
};

const cancelEdit = () => {
  router.push("/myEvents");
};

const formatTimeForBackend = (timeStr) => {
  if (timeStr && timeStr.split(':').length === 2) {
    return timeStr + ":00";
  }
  return timeStr;
};

onMounted(() => {
  fetchEvent();
});
</script>

<style scoped>
.event-edit {
  max-width: 600px;
  margin: 2rem auto;
  padding: 1rem;
}

.loading {
  text-align: center;
  font-size: 1.2rem;
  margin-top: 2rem;
}
</style>
