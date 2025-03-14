import { createHubConnection, stopConnection } from "@/utils/signalRHelper";
import { defineStore } from "pinia";
import { ref } from "vue";

export const usePresenceStore = defineStore("presence", () => {
  const onlineUsers = ref([]);
  let connection = null;

  const initializeConnection = async () => {
    const token = localStorage.getItem("token");
    if (!token) {
      console.error("Brak tokenu użytkownika!");
      return;
    }

    if (connection) return;

    const eventHandlers = {
      UserIsOnline: (username) => {
        if (!onlineUsers.value.includes(username)) {
          onlineUsers.value.push(username);
        }
      },
      UserIsOffline: (username) => {
        onlineUsers.value = onlineUsers.value.filter((user) => user !== username);
      },
      GetOnlineUsers: (users) => {
        onlineUsers.value = users;
      },
    };
    

    connection = createHubConnection(
      "presence",
      token,
      eventHandlers
    );

    try {
      await connection.start();
      console.log("Połączenie SignalR nawiązane");
    } catch (error) {
      console.error("Błąd połączenia SignalR:", error);
    }
  };

  const stopConnectionHandler = async () => {
    if (connection) {
      await stopConnection(connection);
      connection = null;
    }
  };

  return {
    onlineUsers,
    initializeConnection,
    stopConnection: stopConnectionHandler,
  };
});
