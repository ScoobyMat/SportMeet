<template>
    <div class="container">
      <div v-if="user">
        <div class="row">
          <div class="col-md-6 card">
            <img :src="user.photoUrl || defaultUserImage" alt="Zdjęcie użytkownika" class="card-img-top" />
            <div class="card-body">
              <h2>{{ user.firstName }} {{ user.lastName }}</h2>
              <h2>({{ user.userName }})</h2>
              <hr>
              <p><strong>Wiek:</strong> {{ user.age }}</p>
              <p><strong>Płeć:</strong> {{ user.gender }}</p>
              <p><strong>Mieszka w:</strong> {{ user.city }}, {{ user.country }}</p>
            </div>
          </div>
  
          <div class="col-md-6">
            <div class="small-card">
              <h4>O mnie:</h4>
              <blockquote class="user-description">
                <p>{{ user.description || 'Brak opisu' }}</p>
              </blockquote>
            </div>
  
            <div class="small-card">
              <h4>Zainteresowania:</h4>
              <ul class="preferred-sports-list">
                <li v-if="parsedPreferredSports.length > 0" v-for="(sport, index) in parsedPreferredSports" :key="index">
                  <i class="bi bi-check-circle"></i> {{ sport }}
                </li>
                <li v-else>Brak zdefiniowanych zainteresowań.</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div v-else>
        <p>Profil nie został znaleziony.</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import defaultUserImage from "@/assets/image/user.png";
import UserService from "@/services/UserService";
import { computed, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
  
  const route = useRoute();
  const user = ref(null);
  
  const fetchUserProfile = async () => {
    try {
      const username = route.params.username;
      user.value = await UserService.getUserByUsername(username);
    } catch (error) {
      console.error("Błąd pobierania profilu użytkownika:", error);
    }
  };
  
  onMounted(() => {
    fetchUserProfile();
  });
  
  const parsedPreferredSports = computed(() => {
    if (!user.value || !user.value.preferredSports) return [];
    return Array.isArray(user.value.preferredSports)
      ? user.value.preferredSports
      : user.value.preferredSports.split(",").map(s => s.trim());
  });
  </script>
  
  <style scoped>
  .card-img-top {
    padding: 25px;
    border-radius: 50%;
    max-width: 80%;
    max-height: 300px;
    object-fit: cover;
  }
  
  .card {
    align-items: center;
  }
  
  .card-body {
    padding: 1rem;
  }
  
  .user-description {
    font-style: italic;
    border-left: 4px solid #007bff;
    padding: 10px 15px;
    margin-top: 10px;
    font-size: 1.2rem;
    color: #555;
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
  </style>
  