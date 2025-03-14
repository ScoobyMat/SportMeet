<template>
  <div class="container my-5">
    <form @submit.prevent="createEvent" novalidate>
      <div class="row">

        <div class="col-md-4">
          <div class="small-card">
            <div class="card-body">
              <h4 class="card-title mb-3">Podstawowe informacje</h4>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-calendar-event"></i></span>
                <div class="form-floating flex-grow-1">
                  <input type="text" id="eventName" v-model="event.eventName" class="form-control" placeholder="Nazwa Eventu" required
                    :class="{ 'is-invalid': submitted && !event.eventName, 'is-valid': submitted && event.eventName }" />
                  <label for="eventName">Nazwa Eventu</label>
                  <div class="invalid-feedback position-absolute">Proszę wprowadzić nazwę eventu.</div>
                </div>
              </div>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-trophy"></i></span>
                <div class="form-floating flex-grow-1">
                  <select id="sportType" v-model="event.sportType" class="form-select" required
                    :class="{ 'is-invalid': submitted && !event.sportType, 'is-valid': submitted && event.sportType }">
                    <option value="" disabled>Wybierz sport</option>
                    <option v-for="sport in availableSports" :key="sport" :value="sport">
                      {{ sport }}
                    </option>
                  </select>
                  <label for="sportType">Typ Sportu</label>
                  <div class="invalid-feedback position-absolute">Proszę wybrać sport.</div>
                </div>
              </div>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-card-text"></i></span>
                <div class="form-floating flex-grow-1">
                  <textarea id="description" v-model="event.description" class="form-control" rows="5" placeholder="Opis eventu" required
                    :class="{ 'is-invalid': submitted && !event.description, 'is-valid': submitted && event.description }"></textarea>
                  <label for="description">Opis eventu</label>
                  <div class="invalid-feedback position-absolute">Proszę opisać event.</div>
                </div>
              </div>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-people"></i></span>
                <div class="form-floating flex-grow-1">
                  <input type="number" id="groupSize" v-model="event.groupSize" class="form-control" min="0" placeholder="Liczba uczestników" required
                    :class="{ 'is-invalid': submitted && (!event.groupSize || event.groupSize < 1), 'is-valid': submitted && event.groupSize && event.groupSize >= 1 }" />
                  <label for="groupSize">Liczba uczestników</label>
                  <div class="invalid-feedback position-absolute">Proszę podać liczbę uczestników (min. 1).</div>
                </div>
              </div>
              
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="small-card">
            <div class="card-body">
              <h4 class="card-title mb-3">Lokalizacja i termin</h4>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-geo-alt"></i></span>
                <div class="form-floating flex-grow-1">
                  <input type="text" id="address" v-model="event.address" class="form-control" placeholder="Adres" required
                    :class="{ 'is-invalid': submitted && !event.address, 'is-valid': submitted && event.address }" />
                  <label for="address">Adres</label>
                  <div class="invalid-feedback position-absolute">Proszę podać adres.</div>
                </div>
              </div>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-building"></i></span>
                <div class="form-floating flex-grow-1">
                  <input type="text" id="city" v-model="event.city" class="form-control" placeholder="Miasto" required
                    :class="{ 'is-invalid': submitted && !event.city, 'is-valid': submitted && event.city }" />
                  <label for="city">Miasto</label>
                  <div class="invalid-feedback position-absolute">Proszę podać miasto.</div>
                </div>
              </div>

              <div class="row mt-4">
                <div class="col-6 input-group position-relative">
                  <div class="form-floating flex-grow-1">
                    <input type="date" id="date" v-model="event.date" class="form-control" required
                      :class="{ 'is-invalid': submitted && !event.date, 'is-valid': submitted && event.date }" />
                    <label for="date">Data</label>
                    <div class="invalid-feedback position-absolute">Proszę wybrać datę.</div>
                  </div>
                </div>
                <div class="col-6 input-group position-relative">
                  <div class="form-floating flex-grow-1">
                    <input type="time" id="time" v-model="event.time" class="form-control" required
                      :class="{ 'is-invalid': submitted && !event.time, 'is-valid': submitted && event.time }" />
                    <label for="time">Czas</label>
                    <div class="invalid-feedback position-absolute">Proszę wybrać czas.</div>
                  </div>
                </div>
              </div>
              
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="small-card shadow-sm mb-4">
            <div class="card-body">
              <h4 class="card-title mb-3">Zdjęcie eventu</h4>
              <p>Dodaj własne zdjęcie, aby wyróżnić swoje wydarzenie lub zachowaj domyślny obraz</p>

              <div class="input-group mt-4 position-relative">
                <span class="input-group-text fs-4"><i class="bi bi-image"></i></span>
                <div class="form-floating flex-grow-1">
                  <input type="file" id="photo" ref="fileInput" @change="handleFileChange" accept="image/*" class="form-control" />
                  <label for="photo">Dodaj zdjęcie</label>
                </div>
              </div>

              <div v-if="displayImageUrl" class="text-center position-relative mt-3">
                <img :src="displayImageUrl" alt="Podgląd zdjęcia" class="img-fluid rounded mb-2" />
                <button v-if="imageUrl" type="button" class="btn btn-sm btn-outline-danger" @click="removePhoto">
                  Usuń zdjęcie
                </button>
              </div>

            </div>
          </div>
        </div>
      </div>
      <button type="submit" class="btn btn-primary btn-lg w-100 mt-4">Zapisz Event</button>
    </form>
  </div>
</template>

<script setup>
import { availableSports, sportImages } from '@/constants/sports.js';
import EventService from '@/services/eventService';
import { useAuthStore } from '@/stores/auth';
import { computed, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';

const authStore = useAuthStore();
const toast = useToast();
const router = useRouter();

const event = ref({
  eventName: '',
  description: '',
  sportType: '',
  groupSize: 0,
  address: '',
  city: '',
  date: '',
  time: '',
  createdByUserId: authStore.getUserId,
  photo: null,
});

const imageUrl = ref(null);
const fileInput = ref(null);
const submitted = ref(false);

const handleFileChange = (e) => {
  const file = e.target.files[0];
  if (file) {
    event.value.photo = file;
    imageUrl.value = URL.createObjectURL(file);
  }
};

const removePhoto = () => {
  event.value.photo = null;
  imageUrl.value = null;
  if (fileInput.value) {
    fileInput.value.value = "";
  }
};

const displayImageUrl = computed(() => {
  if (imageUrl.value) return imageUrl.value;
  if (event.value.sportType && sportImages[event.value.sportType]) {
    return sportImages[event.value.sportType];
  }
  return null;
});

const isFormValid = computed(() => {
  return ( event.value.eventName && event.value.sportType && event.value.description && event.value.groupSize
            && event.value.address && event.value.city && event.value.date && event.value.time
  );
});

const createEvent = async () => {
  submitted.value = true;

  if (!isFormValid.value) {
    toast.error('Wypełnij wymagane pola');
    return;
  }

  const formData = new FormData();
  formData.append('EventName', event.value.eventName);
  formData.append('Description', event.value.description || '');
  formData.append('SportType', event.value.sportType);
  formData.append('MaxParticipants', event.value.groupSize);
  formData.append('Address', event.value.address);
  formData.append('City', event.value.city);
  formData.append('Date', event.value.date);
  formData.append('Time', event.value.time);
  formData.append('CreatedByUserId', event.value.createdByUserId);

  if (event.value.photo) {
    formData.append('Photo', event.value.photo);
  }

  try {
    const newEvent = await EventService.createEvent(formData);
    if (newEvent) {
      toast.success('Wydarzenie zostało utworzone');
      router.push('/events');
    }
  } catch (error) {
    toast.error('Wystąpił błąd podczas dodawania wydarzenia. Spróbuj ponownie póżniej');
    console.error('Błąd przy tworzeniu eventu:', error.message);
  }
};
</script>

<style scoped>
.small-card {
  min-height: 65vh;
}
.card-body {
  padding: 1.5rem;
}
.card-title {
  border-bottom: 1px solid #ddd;
  padding-bottom: 0.5rem;
  margin-bottom: 1rem;
}
img {
  height: 100%;
  max-height: 250px;
  width: 100%;
  object-fit: cover;
}
</style>
