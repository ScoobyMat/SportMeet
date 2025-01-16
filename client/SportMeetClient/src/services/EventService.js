import axios from "axios";

const API_URL = "https://localhost:7147/api/Event";

const EventService = {
  async GetEvents() {
    try {
      const response = await axios.get(`${API_URL}`);
      return response.data;
    } catch (error) {
      console.error("error:", error.response || error.message);
      throw error;
    }
  },

  async GetEventById(eventId) {
    try {
      const response = await axios.get(`${API_URL}/${eventId}`);
      return response.data;
    } catch (error) {
      console.error("error:", error.response || error.message);
      throw error;
    }
  },

  async GetUpcomingEvents(userId) {
    try {
      const response = await axios.get(`${API_URL}/upcoming-events/${userId}`);
      return response.data;
    } catch (error) {
      console.error("error:", error.response || error.message);
      throw error;
    }
  },

  async FilterEvents(filters) {
    try {
      const params = new URLSearchParams();
      if (filters.location) params.append("location", filters.location);
      if (filters.sportType) params.append("sportType", filters.sportType);
      if (filters.startDate) params.append("startDate", filters.startDate);
      if (filters.endDate) params.append("endDate", filters.endDate);

      const response = await axios.get(
        `${API_URL}/filter?${params.toString()}`
      );

      // Jeśli odpowiedź jest pusta (brak wyników), rzucamy specjalny błąd lub zwracamy pustą tablicę
      if (response.data.length === 0) {
        throw new Error('No events found for the applied filters');
      }

      return response.data; // Zwracamy dane, jeśli są dostępne
    } catch (error) {
      console.error("Error filtering events:", error.message);
      throw error;  // Rzucamy błąd, aby obsłużyć go w komponencie
    }
  },

  async createEvent(formData) {
    try {
      const response = await axios.post(`${API_URL}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      return response.data;
    } catch (error) {
      console.error("Error creating event:", error.message);
      throw error;
    }
  },
};

export default EventService;
