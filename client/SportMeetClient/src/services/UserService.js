import apiClient from "@/apiClient";
import { useUserStore } from "../stores/user";

const API_URL = "/Users";

const UserService = {
  async getUsers() {
    try {
      const response = await apiClient.get(`${API_URL}`, {
        requiresAuth: true,
      });
      return response.data;
    } catch (error) {
      console.error("GetUsers error:", error.response || error.message);
      throw error;
    }
  },

  async getUser(userId) {
    try {
      const response = await apiClient.get(`${API_URL}/${userId}`, {
        requiresAuth: true,
      });
      return response.data;
    } catch (error) {
      console.error("GetUser error:", error.response || error.message);
      throw error;
    }
  },

  async updateUser(formData) {
    try {
      const response = await apiClient.put(`${API_URL}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
        requiresAuth: true,
      });
  
      const userStore = useUserStore();
  
      if (response.data.photoUrl) {
        userStore.user.photoUrl = response.data.photoUrl;
      }
  
      return response.data;
    } catch (error) {
      console.error("UpdateUser error:", error.response || error.message);
      throw error;
    }
  }
};

export default UserService;
