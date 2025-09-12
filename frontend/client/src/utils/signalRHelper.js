import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const BASE_URL = "https://localhost:7147/hubs";

export function createHubConnection(hubName, token, eventHandlers = {}) {
  if (!token) {
    console.error("Brak tokena! Nie można połączyć z SignalR.");
    return null;
  }

  const url = `${BASE_URL}/${hubName}`;

  const connection = new HubConnectionBuilder()
    .withUrl(url, { accessTokenFactory: () => token })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  for (const [event, handler] of Object.entries(eventHandlers)) {
    connection.on(event, handler);
  }
  return connection;
}

export async function startConnection(connection) {
  if (!connection) return;
  try {
    await connection.start();
    console.log(`Połączono z SignalR: ${connection.baseUrl}`);
  } catch (error) {
    setTimeout(() => startConnection(connection), 5000);
  }
}

export async function stopConnection(connection) {
  if (connection) {
    try {
      await connection.stop();
    } catch (error) {
      console.error("Błąd podczas zamykania połączenia SignalR:", error);
    }
  }
}

