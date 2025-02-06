import apiClient from "@/apiClient";

const API_URL = "/GroupMember";

const GroupMemberService = () => {
  const addMember = async (groupId, userId, role) => {
    try {
      const addMemberDto = { groupId, userId, role };
      console.log('Dane do dodania:', addMemberDto);

      const response = await apiClient.post(
        `${API_URL}/AddMember`,
        addMemberDto,
        {
          requiresAuth: true,
        }
      );

      return response.data;
    } catch (error) {
      console.error("Error adding member to group:", error);
      throw error;
    }
  };

  return {
    addMember,
  };
};

export default GroupMemberService;
