import apiClient from "@/apiClient";

const API_URL = "/Friendships";

const FriendService = {
  async sendFriendRequest(dto) {
    try {
      const response = await apiClient.post(`${API_URL}`, dto, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error sending friend request:", error);
      throw error;
    }
  },

  async getReceivedInvitations(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/received/${userId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching invitations:", error);
      throw error;
    }
  },

  async getSentInvitations(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/sent/${userId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching sent invitations:", error);
      throw error;
    }
  },

  async acceptInvitation(invitationId) {
    try {
      const response = await apiClient.post(
        `${API_URL}/respond`,
        { friendshipId: invitationId, accept: true },
        { requiresAuth: true }
      );
      return response.data;
    } catch (error) {
      console.error("Error accepting invitation:", error);
      throw error;
    }
  },

  async rejectInvitation(invitationId) {
    try {
      const response = await apiClient.post(
        `${API_URL}/respond`,
        { friendshipId: invitationId, accept: false },
        { requiresAuth: true }
      );
      return response.data;
    } catch (error) {
      console.error("Error rejecting invitation:", error);
      throw error;
    }
  },

  async getFriends(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error fetching friends:", error);
      throw error;
    }
  },

  async deleteFriendship(friendshipId) {
    try {
      const response = await apiClient.delete(`${API_URL}/${friendshipId}`, { requiresAuth: true });
      return response.data;
    } catch (error) {
      console.error("Error deleting friendship:", error);
      throw error;
    }
  },
};

export default FriendService;
