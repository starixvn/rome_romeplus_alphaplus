import axios from 'axios';

const createAxiosInstance = () => {
  const instance = axios.create({
    baseURL: "",
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    },
    withCredentials: true,
    timeout: 120000, // Timeout after 120s
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
      try {
        if (error.response) {
          // Handle specific status codes
          switch (error.response.status) {
            case 401:
              // Handle unauthorized
              console.error('Unauthorized access');
              // Optionally redirect to login page or show a message
              break;
            case 404:
              // Handle not found
              console.error('Resource not found');
              // Optionally show a message to the user
              break;
            case 500:
              // Handle server error
              console.error('Server error');
              // Optionally show a message to the user
              break;
            default:
              console.error('An error occurred:', error.response.data);
          }
        } else if (error.request) {
          // The request was made but no response was received
          console.error('No response received:', error.request);
        } else {
          // Something happened in setting up the request
          console.error('Error setting up the request:', error.message);
        }
      } catch (e) {
        console.error('Error handling response:', e);
      }
      return Promise.reject(error);
    }
  );

  return instance;
};

const createAPI = () => {
  const api = createAxiosInstance();

  return {
    get: async <T = any>(url: string, config?: any, onError?: any) => { 
      try {
        const cfg = config || { method: 'GET' };
        const response = await api.get<T>(url, cfg);
        return response.data;
      } catch (error) {
        console.error(`Error get: ${url}`, error);
        onError && onError(error);
      }

      return null;
    },
    post: async <T = any>(url: string, data?: any, onError?: any) => {
      try {
        const response = await api.post<T>(url, data);
        return response.data;
      } catch (error) {
        console.error(`Error post: ${url}`, error);
        onError && onError(error);
      }

      return undefined;
    },

    put: async <T = any>(url: string, data?: any, onError?: any) => {
      try {
        const response = await api.put<T>(url, data);
        return response.data;
      } catch (error) {
        console.error(`Error put: ${url}`, error);
        onError && onError(error);
      }

      return undefined;
    },    

  };
};

export const axiosInstance = createAxiosInstance(); 
export const api = createAPI(); 