import axios from 'axios';

const API_URL = 'https://localhost:7147/api/Account/';

const AccountService = {
  currentUser: JSON.parse(localStorage.getItem('user')) || null,

  async login(model) {
    try {
      const response = await axios.post(`${API_URL}login`, model);
      if (response && response.data) {
        this.setCurrentUser(response.data);
        return response.data;
      }
    } catch (error) {
      console.error('Login error:', error.response || error.message);
      throw error;
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
      console.error('Register error:', error.response || error.message);
      throw error;
    }
  },

  setCurrentUser(user) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser = user;
  },

  logout() {
    localStorage.removeItem('user');
    this.currentUser = null;
  }
};

export default AccountService;
