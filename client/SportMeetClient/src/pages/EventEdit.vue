<template>
  <div class="event-edit container">
    <h2>Edytuj wydarzenie</h2>
    <form @submit.prevent="updateEvent">
      <!-- Pola formularza -->
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
      
      <!-- Obsługa zdjęcia -->
      <div class="mb-3">
        <label for="photo" class="form-label">Zdjęcie wydarzenia</label>
        <input type="file" id="photo" ref="fileInput" @change="handleFileChange" accept="image/jpeg, image/png" class="form-control" />
      </div>
      <div v-if="displayImageUrl" class="mt-2 text-center imageArea">
        <img :src="displayImageUrl" alt="Podgląd zdjęcia" class="dostosowany-obraz mb-2" />
        <button type="button" class="btn btn-sm btn-outline-danger" @click="removePhoto">
          Usuń zdjęcie
        </button>
      </div>
      
      <button type="submit" class="btn btn-primary">Zapisz zmiany</button>
      <button type="button" class="btn btn-secondary ms-2" @click="cancelEdit">Anuluj</button>
    </form>
  </div>
</template>

<script setup>
import { sportImages } from "@/constants/sports";
import EventService from "@/services/EventService";
import { computed, onMounted, ref } from "vue";
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
  photoFile: null,
});

// Zmienna imageUrl przechowuje URL nowo wybranego zdjęcia
const imageUrl = ref(null);
const fileInput = ref(null);

// Computed property, które wybiera zdjęcie do wyświetlenia:
// Preferowane: nowe zdjęcie > aktualne zdjęcie z danych > domyślny obraz dla wybranego sportu
const displayImageUrl = computed(() => {
  if (imageUrl.value) return imageUrl.value;
  if (event.value.photoUrl) return event.value.photoUrl;
  if (event.value.sportType && sportImages[event.value.sportType]) return sportImages[event.value.sportType];
  return null;
});

const fetchEvent = async () => {
  try {
    const eventId = parseInt(route.params.eventId);
    const data = await EventService.getEventById(eventId);
    event.value = data;
    // Ustawienie aktualnego zdjęcia, jeśli istnieje
    imageUrl.value = data.photoUrl || null;
  } catch (error) {
    console.error("Błąd pobierania wydarzenia:", error.message);
    toast.error("Nie udało się pobrać danych wydarzenia.");
  }
};

const handleFileChange = (e) => {
  const file = e.target.files[0];
  if (file) {
    event.value.photoFile = file;
    imageUrl.value = URL.createObjectURL(file);
  }
};

const removePhoto = () => {
  // Usuń zarówno nowe, jak i aktualne zdjęcie
  event.value.photoFile = null;
  imageUrl.value = null;
  // Ustawienie photoUrl na null, by backend przyjął, że zdjęcie zostało usunięte
  event.value.photoUrl = null;
  if (fileInput.value) {
    fileInput.value.value = "";
  }
};

const cancelEdit = () => {
  router.push("/myEvents");
};

const updateEvent = async () => {
  try {
    // Formatowanie czasu, jeśli potrzeba (dodanie ":00" dla sekund)
    event.value.time = formatTimeForBackend(event.value.time);
    
    const formData = new FormData();
    formData.append("Id", event.value.id);
    formData.append("EventName", event.value.eventName);
    formData.append("Description", event.value.description || "");
    formData.append("SportType", event.value.sportType);
    formData.append("MaxParticipants", event.value.maxParticipants);
    formData.append("Address", event.value.address);
    formData.append("City", event.value.city);
    formData.append("Date", event.value.date);
    formData.append("Time", event.value.time);
    formData.append("CreatedByUserId", event.value.createdByUserId);

    if (event.value.photoFile) {
      formData.append("Photo", event.value.photoFile);
    }
    
    await EventService.updateEvent(formData);
    toast.success("Wydarzenie zostało zaktualizowane.");
    router.push("/myEvents");
  } catch (error) {
    console.error("Błąd aktualizacji wydarzenia:", error.message);
    toast.error("Nie udało się zaktualizować wydarzenia.");
  }
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
.imageArea {
  border: 1px solid #ddd;
  height: 200px;
  width: 100%;
  overflow: hidden;
  position: relative;
}
.dostosowany-obraz {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>
