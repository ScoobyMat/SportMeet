import { defineStore } from 'pinia';
import { ref } from 'vue';
import { usePresenceStore } from './PresenceStore';

export const useUserStore = defineStore('user', () => {
  const currentUser = ref(JSON.parse(localStorage.getItem('user')) || null);

  const setUser = (user) => {
    const sanitizedUser = {
      id: user.id,
      firstName: user.firstName,
      lastName: user.lastName,
      token: user.token,
      photoUrl: user.photoUrl || null,
    };

    currentUser.value = sanitizedUser;
    localStorage.setItem('user', JSON.stringify(sanitizedUser));
  };

  const updatePhoto = (photoUrl) => {
    if (currentUser.value) {
      const sanitizedUser = {
        id: currentUser.value.id,
        firstName: currentUser.value.firstName,
        lastName: currentUser.value.lastName,
        token: currentUser.value.token,
        photoUrl,
      };

      currentUser.value = sanitizedUser;
      localStorage.setItem('user', JSON.stringify(sanitizedUser));
    } else {
      console.error('Nie można zaktualizować zdjęcia. Użytkownik nie jest zalogowany.');
    }
  };

  const logout = async () => {
    currentUser.value = null;
    localStorage.removeItem('user');
    const presenceStore = usePresenceStore();
    await presenceStore.stopConnection();
    console.log('Użytkownik wylogowany');
  };

  return {
    currentUser,
    setUser,
    updatePhoto,
    logout,
  };
});
