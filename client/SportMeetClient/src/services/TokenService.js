import { jwtDecode } from "jwt-decode";

const TokenService = {
  setToken(token) {
    localStorage.setItem("token", token);
  },

  getToken() {
    return localStorage.getItem("token");
  },

  removeToken() {
    localStorage.removeItem("token");
  },

  decodeToken() {
    const token = TokenService.getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch (error) {
      console.error("Failed to decode token:", error);
      return null;
    }
  },

  getUserId() {
    const decoded = TokenService.decodeToken();
    return decoded?.sub || null;
  }
};

export default TokenService;
