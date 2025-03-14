import apiClient from "@/apiClient";

const API_URL = "/Events";

const EventService = {
  async getAllEvents() {
    try {
      const response = await apiClient.get(API_URL, { requiresAuth: false });
      return response.data;
    } catch (error) {
      console.error("Error fetching events:", error);
      throw error;
    }
  },

  async getEventById(eventId) {
    try {
      const response = await apiClient.get(`${API_URL}/${eventId}`, { requiresAuth: false });
      return response.data;
    } catch (error) {
      console.error(`Error fetching event with ID ${eventId}:`, error);
      throw error;
    }
  },

  async getUpcomingEvents(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}/upcoming`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching upcoming events:", error);
      throw error;
    }
  },

  async getManagedEvents(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}/managed`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching managed events:", error);
      throw error;
    }
  },

  async searchEvents(filters) {
    const params = new URLSearchParams();
    if (filters.city) params.append("city", filters.city);
    if (filters.from) params.append("from", filters.from);
    if (filters.to) params.append("to", filters.to);
    if (filters.sportType) params.append("sportType", filters.sportType);
    
    try {
      const response = await apiClient.get(`${API_URL}/search?${params.toString()}`, { requiresAuth: false });
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

  async updateEvent(eventData) {
    try {
      const response = await apiClient.put(`${API_URL}/${eventData.id}`, eventData, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error updating event:", error);
      throw error;
    }
  },

  async deleteEvent(eventId) {
    try {
      const response = await apiClient.delete(`${API_URL}/${eventId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error(`Error deleting event with ID ${eventId}:`, error);
      throw error;
    }
  },
};

export default EventService;
