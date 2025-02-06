import { createHubConnection, stopConnection } from "@/utils/signalRHelper";

class MessageService {
  constructor() {
    this.connection = null;
    this.groupId = null;
  }

  async initialize(groupId, onMessageReceived, onHistoryReceived, onError) {
    this.groupId = groupId;

    try {
      this.connection = createHubConnection(
        "https://localhost:7147/chathub",
        localStorage.getItem("user_token"),
        {
          ReceiveMessage: onMessageReceived,
          ReceiveHistory: onHistoryReceived,
        }
      );

      await this.connection.start();
      console.log("SignalR connected");

      await this.joinGroup(groupId);
    } catch (err) {
      console.error("Connection failed:", err);
      if (onError) onError(err.message);
    }
  }

  async joinGroup(groupId) {
    if (this.connection) {
      try {
        await this.connection.invoke("JoinGroup", Number(groupId));
        console.log(`Joined group: ${groupId}`);
      } catch (err) {
        console.error("Failed to join group:", err);
      }
    }
  }

  async sendMessage(messageDto) {
    if (this.connection) {
      try {
        await this.connection.invoke("SendMessage", this.groupId, messageDto);
        console.log("Message sent");
      } catch (err) {
        console.error("Failed to send message:", err);
        throw err;
      }
    }
  }

  async disconnect() {
    await stopConnection(this.connection);
    this.connection = null;
  }
}

export default new MessageService();
