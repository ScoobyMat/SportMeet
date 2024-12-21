<template>
    <div class="card">
        <div class="card" ref="messagesContainer">
            <div v-for="(message, index) in messages" :key="index" class="message">
                <div
                    :class="{ 'sent-message': message.isSentByCurrentUser, 'received-message': !message.isSentByCurrentUser }">
                    <div class="message-header">
                        <strong>{{ message.senderName }}</strong>
                    </div>
                    <p>{{ message.content }}</p>
                    <small>{{ formatTimestamp(message.timestamp) }}</small>
                </div>
            </div>
        </div>
        <div class="chat-input-container">
            <input type="text" v-model="newMessage" placeholder="Type your message..."
                @keyup.enter="sendMessageToGroup" />
            <button @click="sendMessageToGroup">Send</button>
        </div>
    </div>
</template>

<script setup>
import { onMounted, ref } from 'vue';

const messages = ref([]);
const newMessage = ref('');
const currentUserId = 1; // Replace with the actual logged-in user ID.

const fetchMessages = async () => {
    try {
        messages.value = await ChatService.getMessages(); // API call to fetch messages.
    } catch (error) {
        console.error('Error fetching messages:', error);
    }
};

const sendMessage = async () => {
    if (newMessage.value.trim() === '') return;

    const message = {
        userId: currentUserId,
        userName: 'Your Name', // Replace with dynamic user name.
        content: newMessage.value.trim(),
    };

    try {
        const sentMessage = await ChatService.sendMessage(message); // API call to send the message.
        messages.value.push(sentMessage);
        newMessage.value = '';
    } catch (error) {
        console.error('Error sending message:', error);
    }
};

// Fetch messages when the component is mounted.
onMounted(() => {
    fetchMessages();

    // Optionally implement WebSocket or polling for real-time updates.
    // Example:
    // setInterval(fetchMessages, 5000);
});
</script>

<style scoped>


.message {
    margin-bottom: 10px;
    word-wrap: break-word;
    word-break: break-word;
}

.message-header {
    font-weight: bold;
}

.sent-message,
.received-message {
    padding: 10px;
    border-radius: 12px;
    max-width: 50%;
}

.sent-message {
    background-color: var(--sent-message-color);
    margin-left: auto;
}

.received-message {
    background-color: var(--received-message-color);
}

.chat-input-container {
    display: flex;
    align-items: center;
    border-top: 1px solid #ccc;
    padding-top: 10px;
    background-color: var(--background-color);
}

.chat-input-container input {
    flex-grow: 1;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 8px;
    margin-right: 10px;
}

.chat-input-container button {
    padding: 10px 15px;
    background-color: var(--navbar-color);
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
}
</style>