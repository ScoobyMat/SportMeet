<template>
  <div v-if="authStore.isAuthenticated" class="container">
    <div class="row">
      <div class="col-md-6 card">
        <img
          :src="userStore.getUserPhoto || userImage"
          alt="Zdjęcie użytkownika"
          class="card-img-top"
        />
        <div class="card-body">
          <strong v-if="user.gender === 'Mężczyzna'">Pan:</strong>
          <strong v-else-if="user.gender === 'Kobieta'">Pani:</strong>
          <p>{{ user.firstName || 'Nieznane' }} {{ user.lastName || 'Nieznane' }}</p>
          <p>({{ user.userName }})</p>
          <strong>Wiek:</strong>
          <p>{{ user.age || 'Brak informacji' }}</p>
          <strong>Email:</strong>
          <p>{{ user.email }}</p>
          <strong>Płeć:</strong>
          <p>{{ user.gender || 'Brak informacji' }}</p>
          <strong>Mieszka w:</strong>
          <p>
            {{ user.city || 'Brak informacji' }},
            {{ user.country || 'Brak informacji' }}
          </p>
        </div>
        <button class="btn btn-primary btn-lg mb-3" @click="goToEditProfile">
          Edytuj profil
        </button>
        <button class="btn btn-secondary btn-lg" @click="goToCredentialsEdit">
          Zmień dane logowania
        </button>
      </div>

      <div class="col-md-6">
        <div class="small-card">
          <h4>O mnie:</h4>
          <blockquote class="user-description">
            <p>{{ user.description || 'Brak opisu.' }}</p>
          </blockquote>
        </div>

        <div class="small-card">
          <h4>Zainteresowania:</h4>
          <ul class="preferred-sports-list">
            <li v-if="user.preferredSports.length > 0" v-for="(sport, index) in user.preferredSports" :key="index">
              <i class="bi bi-check-circle"></i> {{ sport }}
            </li>
            <li v-else>Brak zdefiniowanych zainteresowań.</li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import userImage from "@/assets/image/user.png";
import { useAuthStore } from "@/stores/auth";
import { useUserStore } from "@/stores/user";
import { computed, onMounted } from "vue";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const userStore = useUserStore();
const router = useRouter();

onMounted(() => {
  if (authStore.isAuthenticated && authStore.getUserId) {
    userStore.fetchUser(authStore.getUserId);
  }
});

const user = computed(() => {
  const userData = userStore.getUser || {};
  return {
    ...userData,
    preferredSports: Array.isArray(userData.preferredSports)
      ? userData.preferredSports
      : (userData.preferredSports || "").split(",").map((s) => s.trim()),
  };
});

const goToEditProfile = () => {
  router.push({ name: "UserProfileEdit" });
};

const goToCredentialsEdit = () => {
  router.push({ name: "UserCredentialsEdit" });
};
</script>

<style scoped>
.user-description {
  font-style: italic;
  border-left: 4px solid #007bff;
  padding: 10px 15px;
  margin-top: 10px;
  font-size: 1.2rem;
  color: #555;
}

.card {
  align-items: center;
}

.preferred-sports-list {
  list-style-type: none;
  padding-left: 0;
  margin-top: 10px;
}

.preferred-sports-list li {
  font-size: 1.2rem;
  color: #555;
}

.preferred-sports-list i {
  color: #28a745;
  margin-right: 8px;
}

.card-img-top {
  padding: 25px;
  border-radius: 50%;
  max-width: 80%;
  max-height: 300px;
  object-fit: cover;
}

.card-body strong {
  font-size: 1.2rem;
}

.card-body p {
  margin: 0;
  font-size: 1.2rem;
  color: #555;
}

ul {
  margin: 0;
  padding: 0;
}
</style>
