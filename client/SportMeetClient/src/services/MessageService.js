import * as signalR from "@microsoft/signalr";

class MessageService {
  constructor() {
    this.connection = null;
    this.groupId = null;
  }

  async initialize(groupId, onMessageReceived, onHistoryReceived, onError) {
    this.groupId = groupId;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:7147/chathub`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.on("ReceiveMessage", onMessageReceived);
    this.connection.on("ReceiveHistory", onHistoryReceived);

    try {
      await this.connection.start();
      console.log("SignalR connected");

      await this.joinGroup(groupId);
    } catch (err) {
      console.error("Connection failed:", err);
      onError(err.message);
    }
  }

  async joinGroup(groupId) {
    if (this.connection) {
      groupId = Number(groupId);

      if (isNaN(groupId)) {
        console.error("Invalid groupId:", groupId);
        return;
      }

      try {
        console.log(`Attempting to join group: ${groupId}`);
        await this.connection.invoke("JoinGroup", groupId);
      } catch (err) {
        console.error("Failed to join group:", err);
      }
    }
  }

  async sendMessage(messageDto) {
    if (this.connection) {
        console.log("Sending message:", messageDto);

        try {
            await this.connection.invoke("SendMessage", this.groupId, messageDto);
            console.log("Message sent successfully");
        } catch (err) {
            console.error("Failed to send message:", err);
            throw err;
        }
    }
}

  async leaveGroup(groupId) {
    if (this.connection) {
      try {
        await this.connection.invoke("LeaveGroup", groupId);
        console.log(`Left group ${groupId}`);
      } catch (err) {
        console.error("Failed to leave group:", err);
        throw err;
      }
    }
  }

  async disconnect() {
    if (this.connection) {
      try {
        await this.connection.stop();
        console.log("SignalR disconnected");
      } catch (err) {
        console.error("Failed to disconnect:", err);
      }
      this.connection = null;
    }
  }
}

export default new MessageService();
