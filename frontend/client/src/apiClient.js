import axios from "axios";
import { useRouter } from "vue-router";

const API_BASE_URL = "https://localhost:7147/api";

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    timeout: 10000,
});

const getToken = () => {
    return localStorage.getItem("token");
};

apiClient.interceptors.request.use(
    (config) => {
        if (config.requiresAuth) {
            const token = getToken();
            if (token) {
                config.headers["Authorization"] = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    async (error) => {
        const originalRequest = error.config;

        if (error.response && error.response.status === 401) {
                localStorage.removeItem("token");

                const router = useRouter();
                router.push({ name: "Login" });

                return Promise.reject("Token is invalid. Redirecting to login.");
        }
        return Promise.reject(error);
    }
);

export default apiClient;
