import axios from 'axios';

const createAxiosInstance = () => {
  const instance = axios.create({
    baseURL: "http://localhost:5000",//window.cashServiceAPIUrl,
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    },
    withCredentials: true,
    timeout: 10000, // Timeout after 10s
  });

  // Request interceptor
  instance.interceptors.request.use(
    (config) => {
      // Add auth token if exists
      const token = localStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  // Response interceptor
  instance.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response) {
        // Handle specific status codes
        switch (error.response.status) {
          case 401:
            // Handle unauthorized
            break;
          case 404:
            // Handle not found
            break;
          case 500:
            // Handle server error
            break;
        }
      }
      return Promise.reject(error);
    }
  );

  return instance;
};

export const api = createAxiosInstance(); 