import apiClient from "@/apiClient";

const API_URL = "/Event";

const EventService = {
  async GetEvents() {
    try {
      const response = await apiClient.get(API_URL, {
        requiresAuth: false,
      });
      return response.data;
    } catch (error) {
      console.error("Error fetching events:", error);
      throw error;
    }
  },

  async GetEventById(eventId) {
    try {
      const response = await apiClient.get(`${API_URL}/${eventId}`, {
        requiresAuth: false,
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching event with ID ${eventId}:`, error);
      throw error;
    }
  },

  async GetUpcomingEvents(userId) {
    try {
      const response = await apiClient.get(
        `${API_URL}/upcoming-events/${userId}`,
        {
          requiresAuth: true,
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching upcoming events:", error);
      throw error;
    }
  },

  async FilterEvents(filters) {
    const params = new URLSearchParams();
    if (filters.location) params.append("location", filters.location);
    if (filters.sportType) params.append("sportType", filters.sportType);
    if (filters.startDate) params.append("startDate", filters.startDate);
    if (filters.endDate) params.append("endDate", filters.endDate);

    try {
      const response = await apiClient.get(
        `${API_URL}/filter?${params.toString()}`,
        { requiresAuth: false }
      );
      return response.data;
    } catch (error) {
      console.error("Error fetching filtered events:", error);
      throw error;
    }
  },

  async createEvent(formData) {
    try {
      const response = await apiClient.post(API_URL, formData, {
        headers: { "Content-Type": "multipart/form-data" },
        requiresAuth: true,
      });
      return response.data;
    } catch (error) {
      console.error("Error creating event:", error);
      throw error;
    }
  },
};

export default EventService;
