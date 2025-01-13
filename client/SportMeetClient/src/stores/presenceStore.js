import { HubConnectionBuilder } from "@microsoft/signalr";
import { defineStore } from "pinia";
import { ref } from "vue";
import { useUserStore } from "./userStore";

export const usePresenceStore = defineStore("presence", () => {
  const onlineUsers = ref([]);
  let connection = null;

  const initializeConnection = async () => {
    const userStore = useUserStore();
    const token = userStore.currentUser?.token;

    if (!token) {
      console.error("Brak tokenu użytkownika!");
      return;
    }

    if (connection) return;

    connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7147/hubs/presence", {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    connection.on("UserIsOnline", (username) => {
      if (!onlineUsers.value.includes(username)) {
        onlineUsers.value.push(username);
      }
    });

    connection.on("UserIsOffline", (username) => {
      onlineUsers.value = onlineUsers.value.filter((user) => user !== username);
    });

    connection.on("GetOnlineUsers", (users) => {
      onlineUsers.value = users;
    });

    try {
      await connection.start();
      console.log("Połączenie SignalR nawiązane");
    } catch (error) {
      console.error("Błąd połączenia SignalR:", error);
    }
  };

  const stopConnection = async () => {
    if (connection) {
      try {
        await connection.stop();
        connection = null;
        console.log("Połączenie SignalR zakończone");
      } catch (error) {
        console.error("Błąd podczas zamykania połączenia SignalR:", error);
      }
    }
  };

  return {
    onlineUsers,
    initializeConnection,
    stopConnection,
  };
});
