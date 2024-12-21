import { useUserStore } from "@/stores/userStore";
import axios from "axios";

const API_URL = "https://localhost:7147/api/Users";

const UserService = {
  async GetUser(userId) {
    try {
      const response = await axios.get(`${API_URL}/${userId}`);
      return response.data;
    } catch (error) {
      console.error("GetUser error:", error.response || error.message);
      throw error;
    }
  },

  async UpdateUser(formData) {
    try {
      const response = await axios.put(`${API_URL}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      if (response.data && response.data.photoUrl) {

        const userStore = useUserStore();

        userStore.updatePhoto(response.data.photoUrl);

        const currentUser = JSON.parse(localStorage.getItem("user")) || {};
        currentUser.photoUrl = response.data.photoUrl;

        if (response.data.token) {
          currentUser.token = response.data.token;
        }

        localStorage.setItem("user", JSON.stringify(currentUser));
      }

      return response.data;
    } catch (error) {
      console.error("UpdateUser error:", error.response || error.message);
      throw error;
    }
  },
};

export default UserService;
