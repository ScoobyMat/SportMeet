import { translateErrorMessage } from "@/errorMessages";
import apiClient from "../apiClient";

const API_URL = "/Auth";

const AuthenticationService = {
  async login(model) {
    try {
      const response = await apiClient.post(`${API_URL}/login`, model, {
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
      const response = await apiClient.post(`${API_URL}/register`, model, {
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
