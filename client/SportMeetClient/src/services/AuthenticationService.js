import { usePresenceStore } from '@/stores/presenceStore';
import { useUserStore } from '@/stores/userStore';
import axios from 'axios';

const API_URL = 'https://localhost:7147/api/Auth/';

const AuthenticationService = {
  async login(model) {
    try {
      const response = await axios.post(`${API_URL}login`, model);
      if (response && response.data) {
        const userStore = useUserStore();
        userStore.setUser(response.data);
        localStorage.setItem('user_token', response.data.token);
        const presenceStore = usePresenceStore();
        presenceStore.initializeConnection();
        
  
        return response.data;
      }
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Wystąpił błąd podczas logowania.';
      throw new Error(errorMessage);
    }
  },

  async register(model) {
    try {
        const response = await axios.post(`${API_URL}register`, model);
        if (response && response.data) {
            this.setCurrentUser(response.data);
        }
        return response.data;
    } catch (error) {
        const errorMessage = error.response?.data?.message || 'Wystąpił błąd podczas rejestracji.';
        throw new Error(errorMessage);
    }
},

};

export default AuthenticationService;