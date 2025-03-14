<template>
    <div class="event-group">
        <div class="row">
            <div v-if="showMembers" class="col-lg-3 col-md-12 card">
                <button class="btn btn-sm btn-secondary mb-3 w-100" @click="toggleMembers">
                    {{ showMembers ? 'Ukryj listę członków' : 'Pokaż listę członków' }}
                </button>
                <ul class="members-list">
                    <li v-for="member in members" :key="member.userId">
                        <img :src="member.photoUrl || defaultUserImage" alt="User photo" class="member-img" />
                        <span>
                            {{ member.firstName }} {{ member.lastName }}
                            <i v-if="member.role === 'Owner'" class="bi bi-star-fill" title="Administrator"></i>
                            <i v-else class="bi bi-person-fill" title="Członek"></i>
                        </span>
                    </li>
                </ul>
            </div>

            <div :class="showMembers ? 'col-lg-6 col-md-12' : 'col-lg-9 col-md-12'" class="event-chat card">
                <button v-if="!showMembers" class="btn btn-sm btn-secondary mb-3 w-100" @click="toggleMembers">
                    Pokaż listę członków
                </button>
                <h4>Czat grupowy</h4>
                <ChatComponent chatType="group" :chatId="Number(eventId)" />
            </div>

            <div class="col-lg-3 col-md-12 card">
                <div v-if="event">
                    <div class="event-card">
                        <img :src="event.photoUrl || sportImages[event.sportType]" alt="Event Photo" class="event-img mb-2">
                        <h4>{{ event.eventName }}</h4>
                        <p><strong>Sport:</strong> {{ event.sportType }}</p>
                        <p><strong>Opis:</strong> {{ event.description }}</p>
                        <p><strong>Gdzie:</strong> {{ event.address }}, {{ event.city }}</p>
                        <p><strong>Kiedy:</strong> {{ event.date }} | {{ event.time }}</p>
                        <p><strong>Uczestnicy:</strong> {{ members.length }}/{{ event.maxParticipants }}</p>
                    </div>
                    <HereMap :center="center" :markers="[center]" :mapHeight="'350px'" :mapWidth="'100%'" />
                </div>
                <div v-else>
                    <p>Ładowanie szczegółów wydarzenia...</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import defaultUserImage from '@/assets/image/user.png';
import ChatComponent from '@/components/Chat/ChatComponent.vue';
import HereMap from '@/components/Map/HereMap.vue';
import { sportImages } from '@/constants/sports';
import EventService from '@/services/EventService';
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import EventAttendeesService from '../services/EventAttendeesService';

const route = useRoute();
const eventId = route.params.eventId;
const event = ref(null);
const members = ref([]);
const center = ref({ lat: 52.2296756, lng: 21.0122287 });
const showMembers = ref(true);

const fetchEvent = async () => {
    try {
        const fetchedEvent = await EventService.getEventById(eventId);
        event.value = fetchedEvent;
        center.value = { lat: fetchedEvent.latitude, lng: fetchedEvent.longitude };
        fetchMembers(eventId);
    } catch (error) {
        console.error('Błąd ładowania szczegółów eventu:', error);
    }
};

const fetchMembers = async (eventId) => {
    try {
        const attendees = await EventAttendeesService.getEventAttendees(eventId);
        members.value = attendees;
    } catch (error) {
        console.error('Błąd ładowania uczestników:', error);
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
.event-group {
    padding: 1rem;
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

.event-img {
    width: 50%;
    object-fit: cover;
}

.event-card {
    padding: 1rem;
    border: 1px solid #ddd;
    margin: 1rem 0;
}
</style>
