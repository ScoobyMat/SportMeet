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
              <div v-if="user.id !== currentUserId">
                <button
                  v-if="!isFriend(user.id) && !isInvitationSent(user.id)"
                  class="btn btn-primary btn-sm"
                  @click="sendFriendRequest(user.id)"
                >
                  Wyślij zaproszenie
                </button>
                <button
                  v-else-if="isInvitationSent(user.id)"
                  class="btn btn-warning btn-sm"
                  @click="cancelFriendRequest(user.id)"
                >
                  Anuluj zaproszenie
                </button>
              </div>
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
import defaultUserImage from "@/assets/image/user.png";
import FriendService from "@/services/FriendService";
import UserService from "@/services/UserService";
import { useAuthStore } from "@/stores/auth";
import { computed, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";

const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();

const currentUserId = parseInt(authStore.getUserId);
const searchText = ref("");
const users = ref([]);

const friendships = ref([]);
const sentInvitations = ref([]);

const fetchUsers = async () => {
  try {
    users.value = await UserService.getUsers();
  } catch (error) {
    console.error("Błąd podczas pobierania użytkowników:", error);
  }
};

const fetchFriendships = async () => {
  try {
    friendships.value = await FriendService.getFriends(currentUserId);
  } catch (error) {
    console.error("Błąd podczas pobierania znajomych:", error);
  }
};

const fetchSentInvitations = async () => {
  try {
    sentInvitations.value = await FriendService.getSentInvitations(currentUserId);
  } catch (error) {
    console.error("Błąd podczas pobierania wysłanych zaproszeń:", error);
  }
};

onMounted(async () => {
  await fetchUsers();
  await fetchFriendships();
  await fetchSentInvitations();
});

const filteredUsers = computed(() => {
  if (!searchText.value) {
    return users.value;
  }
  const term = searchText.value.toLowerCase();
  return users.value.filter(user => 
    (user.firstName && user.firstName.toLowerCase().includes(term)) ||
    (user.lastName && user.lastName.toLowerCase().includes(term))
  );
});

const isFriend = (userId) => {
  return friendships.value.some(f =>
    (f.requestorId === currentUserId && f.addresseeId === userId) ||
    (f.requestorId === userId && f.addresseeId === currentUserId)
  );
};

const isInvitationSent = (userId) => {
  return sentInvitations.value.some(inv => 
    inv.addresseeId === userId && inv.status === "Pending"
  );
};

const goToProfile = (username) => {
  router.push({ name: "UserProfile", params: { username } });
};

const sendFriendRequest = async (userId) => {
  try {
    const requestDto = {
      requestorId: currentUserId,
      addresseeId: userId,
    };
    await FriendService.sendFriendRequest(requestDto);
    toast.success("Zaproszenie zostało wysłane.");

    await fetchSentInvitations();
  } catch (error) {
    console.error("Błąd wysyłania zaproszenia:", error);
    toast.error("Nie udało się wysłać zaproszenia.");
  }
};

const cancelFriendRequest = async (userId) => {
  try {
    const invitation = sentInvitations.value.find(inv => inv.addresseeId === userId && inv.status === "Pending");
    if (invitation) {
      await FriendService.deleteFriendship(invitation.id);
      toast.success("Zaproszenie zostało anulowane.");
      await fetchSentInvitations();
    } else {
      toast.info("Brak wysłanego zaproszenia dla tego użytkownika.");
    }
  } catch (error) {
    console.error("Błąd anulowania zaproszenia:", error);
    toast.error("Nie udało się anulować zaproszenia.");
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
  text-align: center;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  background-color: #fff;
  position: relative;
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
