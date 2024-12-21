import axios from "axios";

const API_URL = "https://localhost:7147/api/Group";

const GroupService = {
  async getGroupMembers(groupId) {
    try {
      const response = await axios.get(`${API_URL}/${groupId}`);
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
