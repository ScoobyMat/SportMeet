<template>
    <div class="container">
        <div class="card">
            <h2>Tworzenie Eventu</h2>
            <form @submit.prevent="createEvent">
                <div class="mb-3">
                    <label for="eventName" class="form-label">Nazwa Eventu</label>
                    <input type="text" id="eventName" v-model="event.eventName" class="form-control" required />
                </div>

                <div class="mb-3">
                    <label for="description" class="form-label">Opis</label>
                    <textarea id="description" v-model="event.description" class="form-control" rows="4"></textarea>
                </div>

                <div class="mb-3">
                    <label for="sportType" class="form-label">Typ Sportu</label>
                    <select id="sportType" v-model="event.sportType" class="form-select" required>
                        <option value="" disabled selected>Wybierz sport</option>
                        <option v-for="sport in availableSports" :key="sport" :value="sport">
                            {{ sport }}
                        </option>
                    </select>
                </div>

                <div class="mb-3">
                    <label for="groupSize" class="form-label">Liczba uczestników</label>
                    <input type="number" id="groupSize" v-model="event.groupSize" class="form-control" min="1"
                        required />
                </div>

                <div class="mb-3">
                    <label for="address" class="form-label">Adres</label>
                    <input type="text" id="address" v-model="event.address" class="form-control" required />
                </div>

                <div class="mb-3">
                    <label for="city" class="form-label">Miasto</label>
                    <input type="text" id="city" v-model="event.city" class="form-control" required />
                </div>

                <div class="mb-3">
                    <label for="date" class="form-label">Data</label>
                    <input type="date" id="date" v-model="event.date" class="form-control" required />
                </div>

                <div class="mb-3">
                    <label for="time" class="form-label">Czas</label>
                    <input type="time" id="time" v-model="event.time" class="form-control" required />
                </div>

                <div class="mb-3">
                    <label for="photo" class="form-label">Zdjęcie Eventu</label>
                    <input type="file" id="photo" @change="handleFileChange" accept="image/*" class="form-control" />
                </div>

                <button type="submit" class="btn btn-primary btn-lg">Zapisz Event</button>
            </form>
        </div>
    </div>
</template>

<script setup>
import { availableSports } from '@/constants/sports.js';
import EventService from '@/services/eventService';
import { computed, ref } from 'vue';

import { useUserStore } from '@/stores/userStore';
import { useRouter } from 'vue-router';

const userStore = useUserStore();
const currentUser = computed(() => userStore.currentUser);

const createdUser = currentUser.value.id;

const router = useRouter();

const event = ref({
    eventName: '',
    description: '',
    sportType: '',
    groupSize: 1,
    address: '',
    city: '',
    date: '',
    time: '',
    createdByUserId: createdUser,
    photo: null,
});

const imageUrl = ref(null);


const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
        event.value.photo = file;
        imageUrl.value = URL.createObjectURL(file);
    }
};

const createEvent = async () => {
    const formData = new FormData();

    formData.append('EventName', event.value.eventName);
    formData.append('Description', event.value.description || '');
    formData.append('SportType', event.value.sportType);
    formData.append('GroupSize', event.value.groupSize);
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
            router.push('/events');
        }
    } catch (error) {
        console.error('Błąd przy tworzeniu eventu:', error.message);
    }
};
</script>

<style scoped>
button {
    margin-top: 1rem;
}
</style>
