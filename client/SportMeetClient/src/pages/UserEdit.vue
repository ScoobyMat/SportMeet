<template>
    <div class="container">
      <div class="row">
        <div class="col-md-6 card">
          <h2>Edycja Zdjęcia i Danych Profilu</h2>
          <div class="profileImageContainer">
            <label class="profileImage mb-5" for="fileInput">
              <img v-if="imageUrl" :src="imageUrl" alt="Profile Image" />
              <div v-else class="addImageIcon">+</div>
              <input type="file" id="fileInput" @change="handleFileChange" accept="image/jpeg, image/png" />
            </label>
  
            <button v-if="imageUrl" class="btn btn-primary btn-lg mt-5" @click.prevent="removeImage">
              Usuń zdjęcie
            </button>
          </div>
  
          <h5 class="mt-4">Dane Profilu</h5>
          <form @submit.prevent="updateProfile">
            <div class="mb-3">
              <label for="email" class="form-label">Email</label>
              <input type="email" id="email" v-model="user.email" class="form-control" disabled />
            </div>
            <div class="mb-3">
              <label for="city" class="form-label">Miasto</label>
              <input type="text" id="city" v-model="user.city" class="form-control" />
            </div>
            <div class="mb-3">
              <label for="country" class="form-label">Kraj</label>
              <input type="text" id="country" v-model="user.country" class="form-control" />
            </div>
            <button type="submit" class="btn btn-primary btn-lg mt-2">Zapisz Zmiany</button>
          </form>
        </div>
  
        <div class="col-md-6">
          <div class="small-card">
            <h2>Opis</h2>
            <hr />
            <textarea v-model="user.description" class="form-control mt-4" rows="14" placeholder="Opisz siebie..."></textarea>
          </div>
  
          <div class="small-card">
            <h2>Zainteresowania</h2>
            <hr />
            <div>
              <label for="preferred-sports" class="form-label">Preferowane sporty:</label>
              <select id="preferred-sports" class="form-select" v-model="newSport">
                <option value="" disabled selected>Wybierz sport</option>
                <option v-for="sport in availableSports" :key="sport" :value="sport">{{ sport }}</option>
              </select>
              <button class="btn btn-success btn-lg w-100 mt-3" @click="addSport">Dodaj</button>
            </div>
            <ul class="list-group mt-3">
              <p>Lista zainteresowań:</p>
              <li v-for="(sport, index) in user.preferredSports" :key="index" class="list-group-item">
                {{ sport }}
                <button class="btn btn-sm btn-danger btn-lg float-end" @click="removeSport(index)">Usuń</button>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { availableSports } from "@/constants/sports";
import UserService from "@/services/userService";
import { useUserStore } from "@/stores/userStore";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
  
  const router = useRouter();
  const userStore = useUserStore();
  
  const user = ref({
    email: "",
    city: "",
    country: "",
    description: "",
    preferredSports: [],
  });
  
  const newSport = ref("");
  const imageUrl = ref(null);
  const originalPhotoUrl = ref(null);
  
  const fetchUserData = async () => {
    try {
      const userData = await UserService.GetUser(userStore.currentUser.id);
      Object.assign(user.value, {
        email: userData.email || "",
        city: userData.city || "",
        country: userData.country || "",
        description: userData.description || "",
        preferredSports: userData.preferredSports
          ? userData.preferredSports.split(",").map((s) => s.trim())
          : [],
      });
      imageUrl.value = userData.photoUrl || null;
      originalPhotoUrl.value = userData.photoUrl || null;
    } catch (error) {
      console.error("Błąd przy pobieraniu danych:", error.message);
    }
  };
  
  const handleFileChange = (event) => {
    const file = event.target.files[0];
    if (file) {
      imageUrl.value = URL.createObjectURL(file);
      user.value.photoFile = file;
    }
  };
  
  const removeImage = () => {
    imageUrl.value = null;
    user.value.photoFile = null;
    originalPhotoUrl.value = null;
  };
  
  const addSport = () => {
    if (newSport.value && !user.value.preferredSports.includes(newSport.value)) {
      user.value.preferredSports.push(newSport.value);
      newSport.value = "";
    }
  };
  
  const removeSport = (index) => {
    user.value.preferredSports.splice(index, 1);
  };
  
  const updateProfile = async () => {
  try {
    const formData = new FormData();
    formData.append("Id", userStore.currentUser.id);
    formData.append("City", user.value.city);
    formData.append("Country", user.value.country);
    formData.append("PreferredSports", user.value.preferredSports.join(","));
    formData.append("Description", user.value.description);

    if (user.value.photoFile) {
      formData.append("Photo", user.value.photoFile);
    }

    const updatedUser = await UserService.UpdateUser(formData);

    if (updatedUser) {
      // Zachowaj tylko wymagane pola przy aktualizacji użytkownika
      userStore.setUser({
        id: updatedUser.id,
        firstName: updatedUser.firstName,
        lastName: updatedUser.lastName,
        token: updatedUser.token,
        photoUrl: updatedUser.photoUrl || null,
      });

      imageUrl.value = updatedUser.photoUrl || null;
      originalPhotoUrl.value = updatedUser.photoUrl || null;
      router.push("/profile");
    }
  } catch (error) {
    console.error("Nie udało się zaktualizować danych:", error.message);
    alert("Wystąpił błąd podczas zapisywania zmian.");
  }
};



  
  onMounted(() => {
    if (userStore.currentUser) fetchUserData();
  });
  </script>
  
  <style scoped>
  .profileImageContainer {
    width: 100%;
    align-items: center;
  }
  
  .profileImage {
    display: flex;
    justify-content: center;
    cursor: pointer;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    overflow: hidden;
    position: relative;
    border: 1px solid black;
    border-radius: 50%;
    max-width: 80%;
    height: 400px;
    background: rgba(255, 255, 255, 0.25);
    align-items: flex-start;
  }
  
  .addImageIcon {
    font-size: 75px;
    color: #555;
    margin: auto;
  }
  
  input[type="file"] {
    display: none;
  }
  </style>
  