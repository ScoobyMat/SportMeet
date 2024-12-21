import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useUserStore = defineStore('user', () => {
  const currentUser = ref(JSON.parse(localStorage.getItem('user')) || null);

  const setUser = (user) => {
    currentUser.value = user;
    localStorage.setItem('user', JSON.stringify(user));
  };

  const updatePhoto = (photoUrl) => {
    if (currentUser.value) {
      currentUser.value.photoUrl = photoUrl;
      localStorage.setItem('user', JSON.stringify(currentUser.value));
    } else {
      console.error('Nie można zaktualizować zdjęcia. Użytkownik nie jest zalogowany.');
    }
  };

  const logout = () => {
    currentUser.value = null;
    localStorage.removeItem('user');
  };

  return {
    currentUser,
    setUser,
    updatePhoto,
    logout,
  };
});
