import { HubConnectionBuilder } from '@microsoft/signalr';

class PresenceService {
  constructor() {
    this.connection = null;
  }

  // Funkcja do tworzenia połączenia SignalR
  async createHubConnection(token) {
    try {
      this.connection = new HubConnectionBuilder()
        .withUrl('https://localhost:7147/hubs/presence', {
          accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build();

      // Nasłuchiwanie na dołączenie użytkownika
      this.connection.on('UserJoined', (user) => {
        const presenceStore = usePresenceStore();
        presenceStore.addUserToOnline(user); // Dodajemy użytkownika do listy online
      });

      // Nasłuchiwanie na opuszczenie użytkownika
      this.connection.on('UserLeft', (user) => {
        const presenceStore = usePresenceStore();
        presenceStore.removeUserFromOnline(user); // Usuwamy użytkownika z listy online
      });

      // Pobranie wszystkich użytkowników online na początku
      this.connection.on('GetOnlineUsers', (users) => {
        const presenceStore = usePresenceStore();
        presenceStore.setOnlineUsers(users);
        console.log('Aktualni użytkownicy online:', users);
      });

      // Rozpoczynamy połączenie
      await this.connection.start();
      console.log('Połączenie SignalR zostało nawiązane.');
    } catch (error) {
      console.error('Błąd podczas łączenia z SignalR:', error);
    }
  }

  // Funkcja do zatrzymania połączenia
  async stopConnection() {
    if (connection) {
      try {
        await connection.stop();
        connection = null; // Resetujemy połączenie
        console.log('Połączenie SignalR zakończone');
      } catch (error) {
        console.error('Błąd podczas zamykania połączenia SignalR:', error);
      }
    }
  };
}

export default new PresenceService();
