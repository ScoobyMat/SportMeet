<script setup>
import UserService from '@/services/UserService';
import { usePresenceStore } from '@/stores/presenceStore';
import { onMounted, ref } from 'vue';

const presenceStore = usePresenceStore();
const users = ref([]);

const fetchUsers = async () => {
  try {
    users.value = await UserService.getUsers();
    console.log('Pobrani użytkownicy:', users.value);
  } catch (error) {
    console.error('Błąd podczas pobierania użytkowników', error);
  }
};

onMounted(() => {
  fetchUsers();
});
</script>

<template>
  <div>
    <div v-if="users.length > 0">
      <h3>Użytkownicy:</h3>
      <div class="user-cards">
        <div v-for="user in users" :key="user.id" class="user-card">
          <img :src="user.photoUrl" alt="User Profile" class="user-image" />
          <div class="user-details">
            <h4>{{ user.firstName }}</h4>
            <p>{{ user.email }}</p>
            <span v-if="presenceStore.onlineUsers.includes(user.firstName)" class="online-status">Online</span>
            <span v-else class="offline-status">Offline</span>
          </div>
        </div>
      </div>
    </div>
    <div v-else>
      <p>Brak użytkowników.</p>
    </div>
  </div>
</template>

<style scoped>
.user-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
}

.user-card {
  border: 1px solid #ddd;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.user-image {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
}

.user-details {
  margin-top: 10px;
}

.online-status {
  color: green;
  font-weight: bold;
}

.offline-status {
  color: red;
  font-weight: bold;
}
</style>
