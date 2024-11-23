import axios from 'axios';

const baseUrl = 'https://localhost:7147/api/';

const AccountService = {
  currentUser: JSON.parse(localStorage.getItem('user')) || null,

  async login(model) {
    try {
      const response = await axios.post(`${baseUrl}Account/login`, model);
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
      const response = await axios.post(`${baseUrl}Account/register`, model);
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
