<template>
    <div class="modal-overlay " @click.self="close">
        <div class="modal-content">
            <img :src="event?.photoUrl || sportImages[event.sportType]" alt="Event Photo" class="event-img">
            <div class="event-details">
                <h3>{{ event.eventName }}</h3>
                <p><strong>Sport:</strong> {{ event.sportType }}</p>
                <p><strong>Opis:</strong> {{ event.description }}</p>
                <p><strong>Gdzie:</strong> {{ event.address }}, {{ event.city }}</p>
                <p><strong>Kiedy:</strong> {{ event.date }} | {{ event.time }}</p>
                <p><strong>Liczba miejsc:</strong> {{ event.currentMembers}}/{{ event.maxMembers }}</p>
                <p><strong>Orgaznizator:</strong> {{ event.createdByUser }}</p>
                {{ event.group }}
                <button class="btn btn-primary"  @click="joinEvent(event.id)">Dołącz</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { sportImages } from '@/constants/sports';
import { useRouter } from 'vue-router';


const router = useRouter();



const props = defineProps({
    event: Object,
});
const emit = defineEmits(["close"]);


const close = () => {
    emit("close");
};

const joinEvent = (eventId) => {
    router.push({ name: 'EventGroup',params: {eventId: eventId} });
};
</script>

<style scoped>
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