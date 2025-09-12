import UserService from "@/services/UserService";
import { defineStore } from "pinia";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: null,
  }),
  getters: {
    getUser: (state) => state.user,
    getUserPhoto: (state) => state.user?.photoUrl || null,
  },
  actions: {
    async fetchUser(userId) {
      try {
        this.user = await UserService.getUser(userId);
      } catch (error) {
        console.error("Failed to fetch user data:", error);
        throw error;
      }
    },

    updateUserPhoto(photoUrl) {
      this.user.photoUrl = photoUrl;
    },

    clearUser() {
      this.user = null;
    },
  },
});