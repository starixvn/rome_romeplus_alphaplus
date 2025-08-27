import { describe, it, expect, vi, beforeEach } from 'vitest';
import { baseAPI } from './commonAPI';
import { api } from './axios';
import { AxiosResponse } from 'axios';
import { Bank } from './interfaces/Bank';

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

  describe('getProviderBanks', () => {
    const mockBanks: Bank[] = [
      {
        id: 1,
        name: 'Test Bank 1',
        code: 'TB1',
        transactionTypes: 'DEPOSIT,WITHDRAWAL',
        depositMatchingMode: 'EXACT' as any
      },
      {
        id: 2,
        name: 'Test Bank 2',
        code: 'TB2',
        transactionTypes: 'DEPOSIT',
        depositMatchingMode: 'PARTIAL' as any
      }
    ];

    it('should successfully fetch provider banks with valid providerId', async () => {
      const providerId = 123;
      const mockResponse = {
        data: mockBanks,
        error: null,
        response: {
          data: mockBanks,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
      expect(api.get).toHaveBeenCalledTimes(1);
    });

    it('should handle empty banks array response', async () => {
      const providerId = 456;
      const mockResponse = {
        data: [],
        error: null,
        response: {
          data: [],
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockResponse);
      expect(result.data).toEqual([]);
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle error when fetching provider banks', async () => {
      const providerId = 789;
      const mockError = {
        data: undefined,
        error: { code: 404, message: 'Provider not found' },
        response: undefined
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockError);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockError);
      expect(result.error).toEqual({ code: 404, message: 'Provider not found' });
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle server error when fetching provider banks', async () => {
      const providerId = 999;
      const mockError = {
        data: undefined,
        error: { code: 500, message: 'Internal server error' },
        response: undefined
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockError);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockError);
      expect(result.error?.code).toBe(500);
      expect(result.error?.message).toBe('Internal server error');
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle network error when fetching provider banks', async () => {
      const providerId = 111;
      const mockError = {
        data: undefined,
        error: { code: 1, message: 'Network error' },
        response: undefined
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockError);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockError);
      expect(result.error?.code).toBe(1);
      expect(result.error?.message).toBe('Network error');
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle zero providerId', async () => {
      const providerId = 0;
      const mockResponse = {
        data: mockBanks,
        error: null,
        response: {
          data: mockBanks,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle negative providerId', async () => {
      const providerId = -1;
      const mockError = {
        data: undefined,
        error: { code: 400, message: 'Invalid provider ID' },
        response: undefined
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockError);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockError);
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });

    it('should handle large providerId values', async () => {
      const providerId = 999999999;
      const mockResponse = {
        data: mockBanks,
        error: null,
        response: {
          data: mockBanks,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await baseAPI.getProviderBanks(providerId);
      
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`../Common/GetProviderBanks?providerId=${providerId}`);
    });
  });
}); 