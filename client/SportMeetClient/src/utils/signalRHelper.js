import { HubConnectionBuilder } from "@microsoft/signalr";

export function createHubConnection(url, token, eventHandlers = {}) {
    const connection = new HubConnectionBuilder()
        .withUrl(url, { accessTokenFactory: () => token })
        .withAutomaticReconnect()
        .configureLogging("info")
        .build();

    for (const [event, handler] of Object.entries(eventHandlers)) {
        connection.on(event, handler);
    }

    return connection;
}

export async function stopConnection(connection) {
    if (connection) {
        try {
            await connection.stop();
            console.log("Połączenie SignalR zakończone");
        } catch (error) {
            console.error("Błąd podczas zamykania połączenia SignalR:", error);
        }
    }
}
