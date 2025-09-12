import { usePresenceStore } from "@/stores/presence";
import { createHubConnection, stopConnection } from "@/utils/signalRHelper";

class PresenceService {
  constructor() {
    this.connection = null;
  }

  async createHubConnection(token) {
    const presenceStore = usePresenceStore();

    try {
      const eventHandlers = {
        UserJoined: (user) => presenceStore.addUserToOnline(user),
        UserLeft: (user) => presenceStore.removeUserFromOnline(user),
        GetOnlineUsers: (users) => presenceStore.setOnlineUsers(users),
      };

      this.connection = createHubConnection(
        "https://localhost:7147/hubs/presence",
        token,
        eventHandlers
      );

      await this.connection.start();
      console.log("SignalR connected.");
    } catch (error) {
      console.error("SignalR connection failed:", error);
    }
  }

  async stopConnection() {
    await stopConnection(this.connection);
    this.connection = null;
  }
}

export default new PresenceService();
