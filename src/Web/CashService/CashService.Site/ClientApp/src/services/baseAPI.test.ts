import { describe, it, expect, vi, beforeEach } from 'vitest';
import { baseAPI } from './baseAPI';
import { api } from '../config/axios';

// Mock the axios api
vi.mock('../config/axios', () => ({
  api: {
    get: vi.fn(),
  },
}));

describe('baseAPI', () => {
  beforeEach(() => {
    // Clear all mocks before each test
    vi.clearAllMocks();
  });

  describe('getCountryCodes', () => {
    it('should return country codes when API call is successful', async () => {
      // Arrange
      const mockCountryCodes = ['+1', '+44', '+49'];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockCountryCodes });

      // Act
      const result = await baseAPI.getCountryCodes();

      // Assert
      expect(api.get).toHaveBeenCalledWith('../Payments/GetCountryCodes', { method: 'GET' });
      expect(result).toEqual(mockCountryCodes);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await baseAPI.getCountryCodes();

      // Assert
      expect(api.get).toHaveBeenCalledWith('../Payments/GetCountryCodes', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });
}); 