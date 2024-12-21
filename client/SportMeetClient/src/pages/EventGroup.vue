<template>
    <div class="container">
        <div class="row gx-4">
            <div v-if="showMembers" class="col-lg-3 col-md-12 card">
                <button class="btn btn-sm btn-secondary mb-3 w-100" @click="toggleMembers">Ukryj listę członków</button>
                <ul>
                    <li v-for="member in members" :key="member.userId">
                        <span>{{ member.firstName }} {{ member.lastName }} - {{ member.role }}</span>
                    </li>
                </ul>
            </div>

            <div :class="showMembers ? 'col-lg-6 col-md-12' : 'col-lg-9 col-md-12'" class="event-chat card">
                <button v-if="!showMembers" class="btn btn-sm btn-secondary mb-3 w-100" @click="toggleMembers">Pokaż
                    listę członków</button>
                <h4>Czat grupowy</h4>
                <GroupChat />
            </div>

            <div class="col-lg-3 col-md-12 card">
                <div v-if="event">
                    <div class="event-card">
                        <img :src="event.eventPhotoUrl || sportImages[event.sportType]" alt="Event Photo"
                            class="event-img">
                        <h3>{{ event.eventName }}</h3>
                        <p><strong>Sport:</strong> {{ event.sportType }}</p>
                        <p><strong>Opis:</strong> {{ event.description }}</p>
                        <p><strong>Gdzie:</strong> {{ event.address }}, {{ event.city }}</p>
                        <p><strong>Kiedy:</strong> {{ event.date }} | {{ event.time }}</p>
                        <p><strong>Liczba miejsc:</strong> {{ event.currentMembers }} / {{ event.maxMembers }}</p>
                        <p><strong>Organizator:</strong> {{ event.createdByUser }}</p>
                    </div>
                    <HereMap :center="center" :mapHeight="'400px'" :mapWidth="'100%'" />
                </div>

                <div v-else>
                    <p>Ładowanie szczegółów wydarzenia...</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import GroupChat from '@/components/Chat/GroupChat.vue';
import HereMap from '@/components/Map/HereMap.vue';
import { sportImages } from '@/constants/sports';
import EventService from '@/services/EventService';
import GroupService from '@/services/GroupService';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

const event = ref(null);
const members = ref([]);
const center = ref({ lat: 52.2296756, lng: 21.0122287 });
const showMembers = ref(true);
const route = useRoute();
const eventId = route.params.eventId;

const fetchEvent = async () => {
    try {
        const fetchedEvent = await EventService.GetEventById(eventId);
        event.value = fetchedEvent;
        center.value = { lat: fetchedEvent.latitude || 52.2296756, lng: fetchedEvent.longitude || 21.0122287 };
        fetchGroup(fetchedEvent.groupId);
    } catch (error) {
        console.error('Błąd ładowania szczegółów eventu:', error);
    }
};

const fetchGroup = async (groupId) => {
    try {
        const groupMembers = await GroupService.getGroupMembers(groupId);
        members.value = groupMembers;
    } catch (error) {
        console.error('Błąd ładowania członków grupy:', error);
    }
};

const toggleMembers = () => {
    showMembers.value = !showMembers.value;
};

onMounted(() => {
    fetchEvent();
});
</script>

<style scoped>
.card {
    justify-content: start;
    align-items: none;
}

.event-members ul {
    list-style: none;
    padding: 0;
}

.event-members li {
    padding: 5px;
    border-bottom: 1px solid #ddd;
}

.event-chat .chat-box {
    height: 100%;
    width: 100%;
    border: 1px solid #ddd;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #f8f8f8;
}

.event-card img {
    max-width: 100%;
    height: auto;
    margin-bottom: 1rem;
}

.event-card h3 {
    font-size: 1.5rem;
    margin-bottom: 1rem;
}

.event-card {
    padding: 1rem;
    border: 1px solid #ddd;
    margin: 1rem 0;
  }
</style>