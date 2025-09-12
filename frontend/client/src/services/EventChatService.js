import {createHubConnection, startConnection, stopConnection} from "@/utils/signalRHelper";

class EventChatService {
  constructor() {
    this.connection = null;
    this.eventId = null;
  }

  async initialize(eventId, onMessageReceived, onHistoryReceived) {
    this.eventId = eventId;
    const token = localStorage.getItem("token");

    const eventHandlers = {
      ReceiveMessage: onMessageReceived,
      ReceiveHistory: onHistoryReceived,
    };

    this.connection = createHubConnection("eventChatHub", token, eventHandlers);
    await startConnection(this.connection);
    await this.joinEvent(eventId);
  }

  async joinEvent(eventId) {
    if (this.connection) {
      try {
        await this.connection.invoke("JoinEventChat", eventId);
        console.log(`Dołączono do czatu eventu ${eventId}`);
      } catch (err) {
        console.error("Nie udało się dołączyć do eventu:", err);
      }
    }
  }

  async sendMessage(messageDto) {
    if (this.connection) {
      try {
        await this.connection.invoke("SendMessage", this.eventId, messageDto.senderName, messageDto.content);
        console.log("Wiadomość wysłana");
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

export default new EventChatService();
