<template>
    <div v-if="invitations.length > 0" class="small-card">
        <h2>Oczekujące zaproszenia</h2>
        <div class="invitations-scroll">
            <div v-for="inv in invitations" :key="inv.id" class="user-card">
                <img :src="inv.requestorPhotoUrl || defaultUserImage" alt="Profil zapraszającego" class="user-image" />
                <p>{{ inv.requestorName }}</p>
                <small>{{ formatDate(inv.created) }}</small>
                <div class="invitation-actions">
                    <button class="btn btn-success btn-sm" @click="acceptInvitation(inv.id)"><i class="bi bi-person-fill-add"></i></button>
                    <button class="btn btn-danger btn-sm" @click="rejectInvitation(inv.id)"><i class="bi bi-person-fill-x"></i></button>
                </div>
            </div>
        </div>
    </div>

    <div class="small-card">
        <h2>Twoi znajomi</h2>
        <div v-if="friends.length > 0" class="user-cards">
            <div v-for="friend in friends" :key="friend.id" class="user-card">
                <img :src="friend.photoUrl || defaultUserImage" alt="Profil użytkownika" class="user-image" />
                <div class="user-details">
                    <h4>{{ friend.name }}</h4>
                    <p>{{ friend.username }}</p>
                    <p>
                        Status:
                        <span :class="{ 'text-success': friend.isOnline, 'text-muted': !friend.isOnline }">
                            {{ friend.isOnline ? 'Online' : 'Offline' }}
                        </span>
                    </p>
                    <div class="btn-group">
                        <button class="btn btn-info btn-sm" @click="goToProfile(friend.username)">
                            Zobacz profil
                        </button>
                        <button class="btn btn-primary btn-sm" @click="goToChat(friend.id)">
                            <i class="bi bi-chat-dots"></i> Czat
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div v-else class="alert alert-warning">
            <p>Brak znajomych.</p>
        </div>
    </div>
</template>


<script setup>
import defaultUserImage from "@/assets/image/user.png";
import FriendService from "@/services/FriendService";
import { useAuthStore } from "@/stores/auth";
import { usePresenceStore } from "@/stores/presenceStore";
import { computed, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";

const authStore = useAuthStore();
const router = useRouter();
const toast = useToast();
const presenceStore = usePresenceStore();

const invitations = ref([]);
const friendships = ref([]);

const fetchInvitations = async () => {
    try {
        const userId = parseInt(authStore.getUserId);
        invitations.value = await FriendService.getReceivedInvitations(userId) || [];
    } catch (error) {
        console.error("Błąd pobierania zaproszeń:", error);
        toast.error("Błąd pobierania zaproszeń.");
    }
};

const fetchFriends = async () => {
    try {
        const userId = parseInt(authStore.getUserId);
        friendships.value = await FriendService.getFriends(userId) || [];
    } catch (error) {
        console.error("Błąd pobierania znajomych:", error);
        toast.error("Błąd pobierania listy znajomych.");
    }
};

onMounted(() => {
    fetchInvitations();
    fetchFriends();
});

const friends = computed(() => {
    const currentUserId = parseInt(authStore.getUserId);
    const onlineUsers = presenceStore.onlineUsers;
    return (friendships.value || []).map(rel => {
        let friendId, friendName, friendUsername, friendPhotoUrl;
        if (rel.requestorId === currentUserId) {
            friendId = rel.addresseeId;
            friendName = rel.addresseeName;
            friendUsername = rel.addresseeNickName || rel.addresseeName;
            friendPhotoUrl = rel.addresseePhotoUrl || defaultUserImage;
        } else {
            friendId = rel.requestorId;
            friendName = rel.requestorName;
            friendUsername = rel.requestorNickName || rel.requestorName;
            friendPhotoUrl = rel.requestorPhotoUrl || defaultUserImage;
        }
        const isOnline = onlineUsers.map(u => u.toLowerCase()).includes(friendName.toLowerCase());

        return {
            id: friendId,
            name: friendName,
            username: friendUsername,
            photoUrl: friendPhotoUrl,
            status: rel.status,
            created: rel.created,
            isOnline: isOnline,
        };
    });
});

const goToProfile = (username) => {
    router.push({ name: "UserProfile", params: { username } });
};

const goToChat = (friendId) => {
    router.push({ name: "PrivateChat", params: { friendId } });
};

const acceptInvitation = async (invitationId) => {
    try {
        await FriendService.acceptInvitation(invitationId);
        toast.success("Zaproszenie zaakceptowane.");
        await fetchInvitations();
        await fetchFriends();
    } catch (error) {
        console.error("Błąd akceptacji zaproszenia:", error);
        toast.error("Błąd akceptacji zaproszenia.");
    }
};

const rejectInvitation = async (invitationId) => {
    try {
        await FriendService.rejectInvitation(invitationId);
        toast.success("Zaproszenie odrzucone.");
        await fetchInvitations();
    } catch (error) {
        console.error("Błąd odrzucenia zaproszenia:", error);
        toast.error("Błąd odrzucenia zaproszenia.");
    }
};

const formatDate = (dateStr) => {
    return new Date(dateStr).toLocaleDateString();
};
</script>


<style scoped>
.user-card {
    border: 1px solid #ddd;
    padding: 20px;
    border-radius: 8px;
    text-align: center;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    width: 180px;
    background-color: #fff;
    position: relative;
}

.user-image {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    object-fit: cover;
}

.btn-group {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
}

.btn {
    flex: 1;
    margin: 0 5px;
}

.user-image {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    object-fit: cover;
}
</style>
