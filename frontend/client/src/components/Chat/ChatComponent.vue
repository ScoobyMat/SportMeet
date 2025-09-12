<template>
  <div class="chat-container">
    <div class="messages-container" ref="messagesContainer">
      <div
        v-for="message in messages"
        :key="message.id"
        class="message"
        :class="{
          'my-message': message.senderId === currentUser.id,
          'other-message': message.senderId !== currentUser.id
        }"
      >
        <div class="message-bubble">
          <div class="message-header">
            <div class="sender-info">
              <img
                class="sender-photo"
                :src="message.senderPhotoUrl"
                alt="Zdjęcie użytkownika"
              />
              <span class="sender-name">{{ message.senderName }}</span>
            </div>
            <span class="timestamp">{{ formatTimestamp(message.created) }}</span>
          </div>
          <p class="message-content">{{ message.content }}</p>
        </div>
      </div>
    </div>

    <form @submit.prevent="sendMessage">
      <input v-model="newMessage" placeholder="Napisz wiadomość..." required />
      <button type="submit" class="btn btn-primary">
        <i class="bi bi-send"></i>
      </button>
    </form>
  </div>
</template>


<script setup>
import EventChatService from "@/services/EventChatService";
import PrivateChatService from "@/services/PrivateChatService";
import { useUserStore } from "@/stores/user";
import { computed, defineProps, nextTick, onMounted, ref, watch } from "vue";

const props = defineProps({
  chatType: {
    type: String,
    required: true,
  },
  chatId: {
    type: [Number, String],
    required: true,
  },
});

const userStore = useUserStore();
const currentUser = computed(() => userStore.getUser);

const messages = ref([]);
const newMessage = ref("");
const messagesContainer = ref(null);

const formatTimestamp = (timestamp) => {
  const date = new Date(timestamp);
  return date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit", second: "2-digit" });
};

const initializeChat = async () => {
  try {
    if (!currentUser.value || !currentUser.value.id) {
      console.error("User not authenticated. Please log in.");
      return;
    }
    if (props.chatType === "group") {
      await EventChatService.initialize(props.chatId, handleMessageReceived, handleHistoryReceived);
    } else if (props.chatType === "private") {
      await PrivateChatService.initialize(
        Number(currentUser.value.id),
        Number(props.chatId),
        handleMessageReceived,
        handleHistoryReceived
      );
    } else {
      console.error("Invalid chat type:", props.chatType);
    }
  } catch (err) {
    console.error("Failed to initialize chat:", err);
  }
};

const handleMessageReceived = (message) => {
  messages.value.push(message);
  scrollToBottom();
};

const handleHistoryReceived = (history) => {
  messages.value = history;
  scrollToBottom();
};

const sendMessage = async () => {
  if (!newMessage.value.trim()) return;

  const messageDto = {
    content: newMessage.value,
    senderId: Number(currentUser.value.id),
    senderName: `${currentUser.value.firstName} ${currentUser.value.lastName}`
  };

  try {
    if (props.chatType === "group") {
      await EventChatService.sendMessage(messageDto);
    } else if (props.chatType === "private") {
      await PrivateChatService.sendMessage(newMessage.value);
    }
    newMessage.value = "";
  } catch (err) {
    console.error("Failed to send message:", err);
  }
  scrollToBottom();
};

const scrollToBottom = () => {
  nextTick(() => {
    if (messagesContainer.value) {
      messagesContainer.value.scrollTop = messagesContainer.value.scrollHeight;
    }
  });
};

watch(
  () => currentUser.value,
  async (newUser) => {
    if (newUser && newUser.id) {
      await initializeChat();
    }
  },
  { immediate: true }
);

watch(
  () => props.chatId,
  async () => {
    await initializeChat();
  },
  { immediate: true }
);

onMounted(() => initializeChat());
</script>



<style scoped>
.chat-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  background-color: white;
  padding: 20px;
}

.messages-container {
  display: flex;
  flex-direction: column;
  padding: 10px;
  border: 1px solid gray;
  border-radius: 8px;
  background-color: white;
  overflow-y: auto;
  max-height: calc(100vh - 150px);
  flex-grow: 1;
}

.message {
  display: flex;
  justify-content: flex-start;
  margin-bottom: 10px;
}

.message-bubble {
  width: 50%;
  padding: 5px;
  border-radius: 12px;
  word-wrap: break-word;
  word-break: break-word;
}

.my-message {
  justify-content: flex-end;
}

.my-message .message-bubble {
  background-color: dodgerblue;
  color: white;
}

.other-message .message-bubble {
  background-color: darkslategrey;
  color: white;
}

.message-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-weight: bold;
  margin-bottom: 5px;
}

.sender-info {
  display: flex;
  align-items: center;
}

.sender-photo {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  object-fit: cover;
  margin-right: 8px;
}

.sender-name {
  font-weight: bold;
  color: white;
}

.timestamp {
  font-size: 0.8em;
  color: white;
}

form {
  display: flex;
  margin-top: 10px;
}

input {
  flex: 1;
  padding: 5px;
  margin-right: 1rem;
}

p {
  text-align: left;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  font-size: 1.2em;
}
</style>

