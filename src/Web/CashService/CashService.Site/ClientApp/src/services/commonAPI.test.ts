import { describe, it, expect, vi, beforeEach } from 'vitest';
import { baseAPI } from './commonAPI';
import { api } from './axios';
import { AxiosResponse } from 'axios';

// Mock the axios api
vi.mock('./axios', () => ({
  api: {
    get: vi.fn()
  }
}));

describe('baseAPI', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getCountryCodes', () => {
    it('should successfully fetch country codes', async () => {
      const mockResponse = {
        data: ['US', 'GB', 'CA'],
        error: null,
        response: {
          data: ['US', 'GB', 'CA'],
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await baseAPI.getCountryCodes();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('../Payments/GetCountryCodes');
    });

    it('should handle error when fetching country codes', async () => {
      const mockError = {
        data: undefined,
        error: { code: 500, message: 'Server error' },
        response: undefined
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockError);

      const result = await baseAPI.getCountryCodes();
      expect(result).toEqual(mockError);
      expect(api.get).toHaveBeenCalledWith('../Payments/GetCountryCodes');
    });
  });
}); 