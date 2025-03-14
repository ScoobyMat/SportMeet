import AuthenticationService from "@/services/AuthenticationService";
import TokenService from "@/services/TokenService";
import { usePresenceStore } from "@/stores/presenceStore";
import { useUserStore } from "@/stores/user";
import { defineStore } from "pinia";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: TokenService.getToken(),
    userId: TokenService.getUserId(),
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    getUserId: (state) => state.userId,
  },

  actions: {
    setToken(token) {
      this.token = token;
      TokenService.setToken(token);
      this.userId = TokenService.getUserId();

      if (!this.userId) {
        console.error("Invalid token: Missing user ID");
        this.logout();
      }
    },

    async login(model) {
      try {
        const token = await AuthenticationService.login(model);
        this.setToken(token);
        useUserStore().fetchUser(this.userId);

        const presenceStore = usePresenceStore();
        await presenceStore.initializeConnection();
      } catch (error) {
        console.error("Login failed:", error);
        throw error;
      }
    },

    logout() {
      this.token = null;
      this.userId = null;
      TokenService.removeToken();
      useUserStore().clearUser();

      const presenceStore = usePresenceStore();
      presenceStore.stopConnection();
    },

    loadToken() {
      const token = TokenService.getToken();
      if (token) {
        this.setToken(token);

        const userStore = useUserStore();
        userStore.fetchUser(this.userId).then(() => {
          const presenceStore = usePresenceStore();
          presenceStore.initializeConnection();
        });
    }
  },
}});
