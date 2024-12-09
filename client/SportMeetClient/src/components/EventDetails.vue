<template>
    <div class="modal-overlay " @click.self="close">
        <div class="modal-content">
            <img src="@/assets/image/logo.png" alt="Logo" class="event-img" />
            <div class="event-details">
                <h5>{{ event.eventName }}</h5>
                <p><strong>Twórca:</strong> {{ event.creatorName }}</p>
                <p><strong>Sport:</strong> {{ event.sportType }}</p>
                <p><strong>Opis:</strong> {{ event.description }}</p>
                <p><strong>Lokalizacja:</strong> {{ event.location }}, {{ event.city }}</p>
                <p><strong>Data:</strong> {{ event.date }} | <strong>Godzina:</strong> {{ event.time }}</p>
                <p><strong>Liczba miejsc:</strong> {{ event.availableSeats }}</p>

                {{ event.group }}

                <button class="btn btn-primary" @click="joinEvent">Dołącz</button>
            </div>
        </div>
    </div>
</template>

<script setup>

const props = defineProps({
    event: Object,
});
const emit = defineEmits(["close"]);


const close = () => {
    emit("close");
};

const joinEvent = () => {
    console.log("Dołączono do wydarzenia:", props.event);
    alert("Dołączyłeś do wydarzenia!");
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