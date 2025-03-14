
import apiClient from "@/apiClient";

const API_URL = "/EventAttendees";

const EventAttendeesService = {

  async getEventAttendees(eventId) {
    try {
      const response = await apiClient.get(`${API_URL}/event/${eventId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error(`Error fetching attendees for event ${eventId}:`, error);
      throw error;
    }
  },

  async joinEvent(eventId, userId) {
    try {
      const response = await apiClient.post(
        `${API_URL}/event/${eventId}/user/${userId}`,
        {},
        { requiresAuth: true }
      );
      return response.data;
    } catch (error) {
      console.error(`Error joining event ${eventId} as user ${userId}:`, error);
      throw error;
    }
  },

  async leaveEvent(eventId, userId) {
    try {
      const response = await apiClient.delete(`${API_URL}/event/${eventId}/user/${userId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error(`Error leaving event ${eventId} as user ${userId}:`, error);
      throw error;
    }
  },
};

export default EventAttendeesService;