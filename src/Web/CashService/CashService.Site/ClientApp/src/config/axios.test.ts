import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { api } from './axios';
import axios from 'axios';

// Mock localStorage
const localStorageMock = {
  getItem: vi.fn(),
  setItem: vi.fn(),
  removeItem: vi.fn(),
  clear: vi.fn(),
};
Object.defineProperty(window, 'localStorage', { value: localStorageMock });

describe('Axios Configuration', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('should create axios instance with correct default configuration', () => {
    expect(api.defaults.baseURL).toBe('');
    expect(api.defaults.headers['Content-Type']).toBe('application/json');
    expect(api.defaults.headers['Accept']).toBe('application/json');
    expect(api.defaults.withCredentials).toBe(true);
    expect(api.defaults.timeout).toBe(120000);
  });

  it('should add authorization header when token exists in localStorage', async () => {
    const token = 'test-token';
    localStorageMock.getItem.mockReturnValue(token);

    const config = {
      headers: {},
    };

    const result = await api.interceptors.request.handlers[0].fulfilled(config);
    expect(result.headers.Authorization).toBe(`Bearer ${token}`);
  });

  it('should not add authorization header when token does not exist', async () => {
    localStorageMock.getItem.mockReturnValue(null);

    const config = {
      headers: {},
    };

    const result = await api.interceptors.request.handlers[0].fulfilled(config);
    expect(result.headers.Authorization).toBeUndefined();
  });

  it('should pass through successful responses', async () => {
    const response = { data: 'test data' };
    const result = await api.interceptors.response.handlers[0].fulfilled(response);
    expect(result).toEqual(response);
  });

}); 