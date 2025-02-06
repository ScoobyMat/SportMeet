<template>
  <div class="group-chat">
    <div class="messages-container" ref="messagesContainer">
      <div v-for="message in messages" :key="message.id" class="message" :class="{
        'my-message': message.senderId === currentUser.id,
        'other-message': message.senderId !== currentUser.id
      }">
        <div class="message-bubble">
          <div class="message-header">
            <span class="sender-name">{{ message.senderName }}</span>
            <span class="timestamp">{{ formatTimestamp(message.sentAt) }}</span>
          </div>
          <p class="message-content">{{ message.content }}</p>
        </div>
      </div>
    </div>

    <form @submit.prevent="sendMessage">
      <input v-model="newMessage" placeholder="Type your message..." required />
      <button type="submit" class="btn btn-primary"><i class="bi bi-send"></i></button>
    </form>
  </div>
</template>

<script setup>
import MessageService from "@/services/MessageService";
import { useUserStore } from "@/stores/user";
import { computed, defineProps, nextTick, onMounted, ref, watch } from 'vue';

const props = defineProps({
  groupId: {
    type: String,
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
  return `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
};

const initializeChat = async () => {
  try {
    const groupIdAsNumber = Number(props.groupId);
    if (isNaN(groupIdAsNumber)) {
      console.error("Invalid groupId:", props.groupId);
      return;
    }

    await MessageService.initialize(
      groupIdAsNumber,
      handleMessageReceived,
      handleHistoryReceived,
      handleError
    );
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

const handleError = (error) => {
  console.error("Chat error:", error);
};

const sendMessage = async () => {
  const groupIdAsNumber = Number(props.groupId);
  if (isNaN(groupIdAsNumber)) {
    console.error("Invalid groupId:", props.groupId);
    return;
  }

  if (!currentUser.value || !currentUser.value.id) {
    console.error("User not logged in or user id is missing");
    return;
  }

  const messageDto = {
    content: newMessage.value,
    senderId: Number(currentUser.value.id),
    senderName: `${currentUser.value.firstName} ${currentUser.value.lastName}`,
    groupId: groupIdAsNumber,
  };

  try {
    await MessageService.sendMessage(messageDto);
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
  () => props.groupId,
  async (newGroupId) => {
    const newGroupIdAsNumber = Number(newGroupId);
    if (isNaN(newGroupIdAsNumber)) {
      console.error("Invalid groupId:", newGroupId);
      return;
    }
    await initializeChat();
  },
  { immediate: true }
);

onMounted(() => {
  if (currentUser.value && currentUser.value.id) {
    initializeChat();
  } else {
    console.error("User not authenticated. Please log in.");
  }
});
</script>



<style scoped>
.group-chat {
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
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  font-size: 1.2em;
}
</style>
