<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 card">
        <img :src="user.photoUrl ? user.photoUrl : userImage" alt="userPhoto" class="card-img-top" />

  <div class="card-body">
    <strong v-if="user.gender === 'Mężczyzna'">
      Pan:
    </strong>
    <strong v-else="user.gender === 'Kobieta'">
      Pani:
    </strong>
    <p>{{ user.firstName }} {{ user.lastName }}</p>

    <strong>Email:</strong>
    <p>{{ user.email }}</p>

    <strong>Wiek:</strong>
    <p>{{ (user.age) }}</p>

    <strong>Płeć:</strong>
    <p>{{ user.gender }}</p>

    <strong>Mieszka w:</strong>
    <p>{{ user.city }}, {{ user.country }}</p>
  </div>
        <button class="btn btn-primary btn-lg mb-5" @click="goToEditProfile">
          Edytuj profil
        </button>
      </div>

      <div class="col-md-6">
        <div class="small-card">
          <tab heading="About">
            <h4>O mnie:</h4>
            <blockquote class="user-description">
              <p>{{ user.description || 'Brak opisu.' }}</p>
            </blockquote>
          </tab>
        </div>

        <div class="small-card">
          <h4>Zainteresowania</h4>
          <ul class="list-unstyled">
            <li v-if="user.preferredSports && user.preferredSports.length > 0">
              <ul class="preferred-sports-list">
                <li v-for="(sport, index) in user.preferredSports" :key="index">
                  <i class="bi bi-check-circle"></i> {{ sport }}
                </li>
              </ul>
            </li>
            <li v-else>Brak zdefiniowanych zainteresowań.</li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import userImage from '@/assets/image/user.png';
import userService from "@/services/userService";
import { useUserStore } from '@/stores/userStore';
import { onMounted, ref } from "vue";
import { useRouter } from 'vue-router';

const router = useRouter();
const userStore = useUserStore();

const user = ref({
  photoUrl: '',
  description: '',
  preferredSports: [],
  firstName: '',
  lastName: '',
  email: '',
  age: '',
  gender: '',
  city: '',
  country: ''
});

const goToEditProfile = () => {
  router.push({ name: 'UserProfileEdit' });
};

onMounted(async () => {
  if (!userStore.currentUser) return;

  try {
    const userData = await userService.GetUser(userStore.currentUser.id);
    user.value = {
      photoUrl: userData.photoUrl || userImage,
      description: userData.description || '',
      preferredSports: userData.preferredSports ? userData.preferredSports.split(',').map(sport => sport.trim()) : [],
      firstName: userData.firstName || '',
      lastName: userData.lastName || '',
      email: userData.email || '',
      age: userData.age || '',
      gender: userData.gender || '',
      city: userData.city || '',
      country: userData.country || ''
    };
  } catch (error) {
    console.error("Nie udało się pobrać danych użytkownika:", error.message);
  }
});
</script>

<style scoped>

.user-description {
  font-style: italic;
  border-left: 4px solid #007bff;
  padding: 10px 15px;
  margin-top: 10px;
}

.user-description p {
  margin: 0;
  font-size: 1.5rem;
  color: #555;
}

.preferred-sports-list {
  list-style-type: none;
  font-style: italic;
}

.preferred-sports-list li {
  font-size: 1.5rem;
}

.preferred-sports-list i {
  color: #28a745;
  margin-right: 8px;
}

ul {
  padding-left: 0;
}

ul li {
  font-size: 1rem;
  color: #555;
}

.card-img-top {
  padding: 25px;
  border-radius: 50%;
  max-width: 80%;
  max-height: 450px;
}

.card-body strong{
  font-size: 1.2rem;
}

.card-body p {
  margin: 0;
  font-size: 1.5rem;
  font-family: italic;
  color: #555;
}
</style>
