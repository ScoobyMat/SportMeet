import axios from 'axios';

const API_URL = 'https://localhost:7147/api/Users';

const UserService = {

    async GetUser(userId) {
        try {
            const response = await axios.get(`${API_URL}/${userId}`);
            return response.data;
        } catch (error) {
            console.error('Register error:', error.response || error.message);
            throw error;
        }
    }
}
export default UserService;