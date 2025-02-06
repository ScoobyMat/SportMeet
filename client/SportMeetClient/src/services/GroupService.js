import apiClient from "@/apiClient";

const API_URL = "/Group";

const GroupService = {
  async getGroupMembers(groupId) {
    try {
      const response = await apiClient.get(`${API_URL}/${groupId}`, {
        requiresAuth: true,
      });
      return response.data.members;
    } catch (error) {
      console.error(
        "Error fetching group members:",
        error.response || error.message
      );
      return [];
    }
  },
};

export default GroupService;