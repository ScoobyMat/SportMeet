import apiClient from "@/apiClient";

const API_URL = "/Notifications";

const NotificationService = {
  async getNotifications(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching notifications:", error);
      throw error;
    }
  },

  async getUnreadNotifications(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}/unread`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching unread notifications:", error);
      throw error;
    }
  },

  async markAsRead(notificationId) {
    try {
      const response = await apiClient.patch(`${API_URL}/${notificationId}/read`, null, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error marking notification as read:", error);
      throw error;
    }
  },

  async markAllAsRead(userId) {
    try {
      const response = await apiClient.patch(`${API_URL}/user/${userId}/readall`, null, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error marking all notifications as read:", error);
      throw error;
    }
  }
};

export default NotificationService;
