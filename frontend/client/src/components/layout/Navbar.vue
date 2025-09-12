<template>
  <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
    <div class="container-fluid">
      <a class="navbar-brand d-flex align-items-center" href="/" router-link-active="active">
        <img src="@/assets/image/logo.png" alt="Logo" class="navbar-logo me-2" height="50" />
        SportMeet
      </a>

      <ul v-if="authStore.isAuthenticated" class="navbar-nav me-auto mb-md-0">
        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="eventsDropdown" role="button" data-bs-toggle="dropdown"
            aria-expanded="false">
            Wydarzenia
          </a>
          <ul class="dropdown-menu" aria-labelledby="eventsDropdown">
            <li>
              <router-link to="/events" class="dropdown-item">Przeglądaj wydarzenia w okolicy</router-link>
            </li>
            <li>
              <hr class="dropdown-divider" />
            </li>
            <li>
              <router-link to="/event/create" class="dropdown-item">Zorganizuj wydarzenie</router-link>
            </li>
            <li>
              <hr class="dropdown-divider" />
            </li>
            <li>
              <router-link to="/myEvents" class="dropdown-item">Moje wydarzenia</router-link>
            </li>
          </ul>
        </li>

        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="communityDropdown" role="button" data-bs-toggle="dropdown"
            aria-expanded="false">
            Społeczność
          </a>
          <ul class="dropdown-menu" aria-labelledby="communityDropdown">
            <li>
              <router-link to="/users" class="dropdown-item">Użytkownicy</router-link>
            </li>
            <li>
              <hr class="dropdown-divider" />
            </li>
            <li>
              <router-link to="/friends" class="dropdown-item">Znajomi</router-link>
            </li>
          </ul>
        </li>
      </ul>

      <ul v-if="authStore.isAuthenticated" class="navbar-nav ms-auto mb-md-0">
        <li class="nav-item dropdown me-3">
          <a class="nav-link dropdown-toggle position-relative" href="#" id="notificationsDropdown" role="button"
            data-bs-toggle="dropdown" aria-expanded="false">
            <i class="bi bi-bell" style="font-size: 1.5rem;"></i>
            <span v-if="unreadCount > 0"
              class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
              {{ unreadCount }}
              <span class="visually-hidden">unread notifications</span>
            </span>
          </a>
          <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="notificationsDropdown" style="width: 300px;">
            <li class="dropdown-header d-flex justify-content-between align-items-center">
              <span>Powiadomienia</span>
              <button v-if="unreadCount > 0" class="btn btn-link btn-sm" @click="markAllAsRead">Oznacz
                wszystkie</button>
            </li>
            <li v-if="notifications.length === 0" class="dropdown-item text-center">Brak powiadomień</li>
            <li v-for="notif in notifications" :key="notif.id">
              <div class="dropdown-item d-flex flex-column">
                <p class="mb-1 fw-bold">{{ notif.message }}</p>
                <small class="text-muted">{{ formatDate(notif.created) }}</small>
                <button @click="markNotificationAsRead(notif.id)" class="btn btn-sm btn-primary mt-1">
                  Oznacz jako przeczytane
                </button>
              </div>
              <hr class="dropdown-divider" />
            </li>
          </ul>
        </li>

        <li class="nav-item dropdown">
          <img v-if="userStore.getUserPhoto" :src="userStore.getUserPhoto" alt="User profile image" width="40"
            height="40" class="me-3 rounded-circle" />
          <img v-else src="@/assets/image/user.png" alt="Default user profile image" width="40" height="40"
            class="me-3 rounded-circle" />
          <a class="dropdown-toggle text-light text-decoration-none" href="#" role="button" id="dropdownMenuLink"
            data-bs-toggle="dropdown" aria-expanded="false">
            {{ userStore.getUser?.firstName }} {{ userStore.getUser?.lastName }}
          </a>
          <ul class="dropdown-menu mt-3" aria-labelledby="dropdownMenuLink">
            <li>
              <router-link to="/profile" class="dropdown-item">
                Mój profil <i class="bi bi-person-square"></i>
              </router-link>
            </li>
            <li>
              <hr class="dropdown-divider" />
            </li>
            <li>
              <a class="dropdown-item" href="#" @click="logout">
                Wyloguj <i class="bi bi-box-arrow-left"></i>
              </a>
            </li>
          </ul>
        </li>
      </ul>

      <ul v-else class="navbar-nav">
        <li class="nav-item">
          <router-link to="/login" class="nav-link">Zaloguj się</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/register" class="nav-link">Rejestracja</router-link>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script setup>
import NotificationService from "@/services/NotificationService";
import { useAuthStore } from "@/stores/auth";
import { useUserStore } from "@/stores/user";
import { computed, onMounted, ref } from "vue";
import { useToast } from "vue-toastification";

const toast = useToast();
const authStore = useAuthStore();
const userStore = useUserStore();

const logout = () => {
  authStore.logout();
  toast.success("Zostałeś wylogowany");
};

const notifications = ref([]);
const fetchNotifications = async () => {
  try {
    const userId = parseInt(authStore.getUserId);
    notifications.value = await NotificationService.getUnreadNotifications(userId);
  } catch (error) {
    toast.error("Błąd pobierania powiadomień:", error);
  }
};

const markNotificationAsRead = async (id) => {
  try {
    await NotificationService.markAsRead(id);
    notifications.value = notifications.value.filter((n) => n.id !== id);
    toast.success("Powiadomienie oznaczone jako przeczytane");
  } catch (error) {
    toast.error("Nie udało się oznaczyć powiadomienia jako przeczytane");
  }
};

const markAllAsRead = async () => {
  try {
    const userId = parseInt(authStore.getUserId);
    await NotificationService.markAllAsRead(userId);
    notifications.value = [];
    toast.success("Wszystkie powiadomienia zostały oznaczone jako przeczytane");
  } catch (error) {
    toast.error("Nie udało się oznaczyć wszystkich powiadomień jako przeczytane");
  }
};

const formatDate = (dateStr) => {
  return new Date(dateStr).toLocaleString();
};

const unreadCount = computed(() => notifications.value.length);

onMounted(() => {
  if (authStore.isAuthenticated) {
    fetchNotifications();
  }
});
</script>

<style scoped>
.dropdown-menu {
  background-color: #f8f9fa;
}

.dropdown-item:hover {
  background-color: #343a40;
  color: white;
}

.position-relative .badge {
  font-size: 0.75rem;
}
</style>
