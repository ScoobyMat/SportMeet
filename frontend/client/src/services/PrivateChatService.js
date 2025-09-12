import { createHubConnection, startConnection, stopConnection } from "@/utils/signalRHelper";

class PrivateChatService {
  constructor() {
    this.connection = null;
    this.currentUserId = null;
    this.recipientId = null;
  }

  async initialize(currentUserId, recipientId, messageReceived, historyReceived) {
    this.currentUserId = Number(currentUserId);
    this.recipientId = Number(recipientId);
    const token = localStorage.getItem("token");
    if (!token) {
      console.error("Brak tokena!");
      return;
    }
    
    const eventHandlers = {
      ReceiveMessage: messageReceived,
      ReceiveChatHistory: historyReceived,
    };
    
    this.connection = createHubConnection("privateChatHub", token, eventHandlers);
    await startConnection(this.connection);
    
    if (this.connection.state !== "Connected") {
      console.error("Connection is not in Connected state");
      return;
    }
    
    await this.connection.invoke("JoinChat", this.currentUserId, this.recipientId);
  }
  
  async sendMessage(message) {
    if (this.connection) {
      try {
        await this.connection.invoke("SendMessage", this.currentUserId, this.recipientId, message);
        console.log("Wiadomość prywatna wysłana");
      } catch (err) {
        console.error("Nie udało się wysłać wiadomości:", err);
      }
    }
  }
  
  async disconnect() {
    await stopConnection(this.connection);
    this.connection = null;
  }
}

export default new PrivateChatService();
