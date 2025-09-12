import { translateErrorMessage } from "@/errorMessages";
import axios from "axios";

const API_URL = "https://localhost:7147/api/Auth";

const AuthenticationService = {
  async login(model) {
    try {
      const response = await axios.post(`${API_URL}/login`, model, {
        Authorization: false,
      });
      return response.data.token;
    } catch (error) {
      const errorMessage = error.response?.data?.message || "Wystąpił błąd podczas logowania.";
      throw new Error(translateErrorMessage(errorMessage));
    }
  },

  async register(model) {
    try {
      const response = await axios.post(`${API_URL}/register`, model, {
        Authorization: false,
      });

        return response.data;
    } catch (error) {
      const errorMessage = error.response?.data?.message || "Wystąpił błąd podczas rejestracji.";
      throw new Error(translateErrorMessage(errorMessage));
    }
  },
};

export default AuthenticationService;
