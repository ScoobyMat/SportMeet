<template>
  <div>
    <div class="search-box mb-3">
      <input
        v-model="searchText"
        type="text"
        class="form-control"
        placeholder="Szukaj użytkowników po imieniu lub nazwisku"
      />
    </div>

    <div v-if="filteredUsers.length > 0">
      <h3>Użytkownicy:</h3>
      <div class="user-cards">
        <div v-for="user in filteredUsers" :key="user.id" class="user-card">
          <div>
            <img
              :src="user.photoUrl || defaultUserImage"
              alt="User Profile"
              class="user-image"
            />
          </div>
          <div class="user-details">
            <h4>{{ user.firstName }} {{ user.lastName }}</h4>
            <p>{{ user.userName }}</p>
            <div class="actions">
              <button class="btn btn-info btn-sm" @click="goToProfile(user.userName)">
                Zobacz profil
              </button>
              <button
                v-if="user.id !== currentUserId"
                class="btn btn-primary btn-sm"
                @click="sendFriendRequest(user.id)"
              >
                Wyślij zaproszenie
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else>
      <p>Brak użytkowników spełniających kryteria wyszukiwania.</p>
    </div>
  </div>
</template>

<script setup>
import FriendService from "@/services/FriendService";
import { useAuthStore } from "@/stores/auth";
import { computed, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";
import UserService from "../services/UserService";


const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

const currentUserId = parseInt(authStore.getUserId);
const defaultUserImage = "@/assets/image/user.png";

const users = ref([]); 
const searchText = ref("");

const fetchUsers = async () => {
  try {
    users.value = await UserService.getUsers();
  } catch (error) {
    console.error("Błąd podczas pobierania użytkowników", error);
  }
};

onMounted(() => {
  fetchUsers();
});

const filteredUsers = computed(() => {
  if (!searchText.value) {
    return users.value;
  }
  const term = searchText.value.toLowerCase();
  return users.value.filter((user) => {
    return (
      (user.firstName && user.firstName.toLowerCase().includes(term)) ||
      (user.lastName && user.lastName.toLowerCase().includes(term))
    );
  });
});

const goToProfile = (userName) => {
  router.push({ name: "UserProfile", params: { username: userName } });
};

const sendFriendRequest = async (userId) => {
  try {
    const requestDto = {
      requestorId: parseInt(authStore.getUserId),
      addresseeId: userId,
    };
    await FriendService.sendFriendRequest(requestDto);
    toast.success("Zaproszenie zostalo wyslane");
  } catch (error) {
    console.error("Błąd wysyłania zaproszenia:", error);
    toast.error("Zaproszenie już wysłane");
  }
};
</script>

<style scoped>
.user-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
  margin-top: 1rem;
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

.actions {
  margin-top: 10px;
  display: flex;
  justify-content: center;
  gap: 5px;
}

.search-box input {
  max-width: 400px;
  margin: 0 auto;
  display: block;
}
</style>
