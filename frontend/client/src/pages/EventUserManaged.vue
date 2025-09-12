<template>
  <div class="managed-events container">
    <h2>Twoje zarządzane wydarzenia</h2>
      <div v-if="managedEvents.length > 0" class="row">
        <div v-for="event in managedEvents" :key="event.id" class="col-md-4 mb-3">
          <div class="small-card event-card">
            <img :src="event.photoUrl || sportImages[event.sportType]" class="card-img-top" alt="Event Image" />
            <div class="card-body">
              <h5 class="card-title">{{ event.eventName }}</h5>
              <p class="card-text">
                Data: {{ formatDate(event.date) }}<br>
                Godzina: {{ event.time }}<br>
                Miasto: {{ event.city }}
              </p>
              <div class="d-flex justify-content-between">
                <button class="btn btn-success" @click="groupEvent(event.id)">Przejdz do wydarzenia</button>
                <button class="btn btn-primary" @click="editEvent(event.id)">Edytuj</button>
                <button class="btn btn-danger" @click="deleteEvent(event.id)">Usuń</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div v-else class="alert alert-warning">
        Nie masz żadnych wydarzeń, którymi zarządzasz.
      </div>
    </div>
</template>

<script setup>
import { sportImages } from "@/constants/sports";
import EventService from "@/services/EventService";
import { useAuthStore } from "@/stores/auth";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";

const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

const managedEvents = ref([]);

const fetchManagedEvents = async () => {
  try {
    const userId = parseInt(authStore.getUserId);
    const events = await EventService.getManagedEvents(userId);
    managedEvents.value = events;
  } catch (error) {
    console.error("Błąd pobierania zarządzanych wydarzeń:", error);
    toast.error("Nie udało się pobrać wydarzeń.");
  }
};

const editEvent = (eventId) => {
  router.push({ name: "EventEdit", params: { eventId } });
};

const groupEvent = (eventId) => {
  router.push({ name: "EventGroup", params: { eventId } });
};

const deleteEvent = async (eventId) => {
  try {
    if (confirm("Czy na pewno chcesz usunąć to wydarzenie?")) {
      await EventService.deleteEvent(eventId);
      toast.success("Wydarzenie zostało usunięte.");
      await fetchManagedEvents();
    }
  } catch (error) {
    console.error("Błąd usuwania wydarzenia:", error);
    toast.error("Nie udało się usunąć wydarzenia.");
  }
};

const formatDate = (date) => {
  return new Date(date).toLocaleDateString();
};

onMounted(() => {
  if (authStore.isAuthenticated) {
    fetchManagedEvents();
  }
});
</script>

<style scoped>
.managed-events {
  padding: 1rem;
}

.loading {
  text-align: center;
  font-size: 1.2rem;
  margin-top: 2rem;
}

.event-card {
  cursor: pointer;
  transition: transform 0.2s;
}

.event-card:hover {
  transform: scale(1.02);
}

.card-img-top {
  height: 150px;
  object-fit: cover;
}
</style>
