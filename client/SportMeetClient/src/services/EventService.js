import axios from "axios";

const API_URL = "https://localhost:7147/api/Event";

const EventService = {
    async GetEvents() {
        try {
        const response = await axios.get(`${API_URL}`);
        return response.data;
        } catch (error) {
        console.error("error:", error.response || error.message);
        throw error;
        }
    },

    async GetEventById() {
        try {
            const response = await axios.get(`${API_URL}/${eventId}`);
            return response.data;
        } catch (error) {
        console.error("error:", error.response || error.message);
        throw error;
        }
    },

    async GetUpcomingEvents(userId) {
        try {
        const response = await axios.get(`${API_URL}/upcoming-events/${userId}`);
        return response.data;
        } catch (error) {
        console.error("error:", error.response || error.message);
        throw error;
        }
    },

    async FilterEvents(filters) {
        try {
        const params = new URLSearchParams();
        if (filters.location) params.append("location", filters.location);
        if (filters.startDate) params.append("startDate", filters.startDate);
        if (filters.endDate) params.append("endDate", filters.endDate);

        const response = await axios.get(
            `${API_URL}/filter?${params.toString()}`
        );
        return response.data;
        } catch (error) {
        console.error("Error filtering events:", error.message);
        throw error;
        }
    },
};

export default EventService;
