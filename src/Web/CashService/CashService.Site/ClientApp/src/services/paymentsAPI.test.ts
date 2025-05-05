import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { paymentProviderService } from './paymentsAPI';
import { api } from '../config/axios';
import type { Mock } from 'vitest';

vi.mock('../config/axios', () => ({
  api: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  },
}));

describe('paymentProviderService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('getProviders returns providers on success', async () => {
    const mockProviders = [
      { id: 1, name: 'A', fee: 1.2, status: 1, lastUpdatedStr: '2023-01-01' },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockProviders });
    const result = await paymentProviderService.getProviders();
    expect(result).toEqual(mockProviders);
    expect(api.get).toHaveBeenCalledWith('GetProviders', { method: 'GET' });
  });

  it('getProviders returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getProviders();
    expect(result).toEqual([]);
  });

  it('getDailyWithdrawalCount returns number on success', async () => {
    (api.get as Mock).mockResolvedValueOnce({ data: 5 });
    const result = await paymentProviderService.getDailyWithdrawalCount();
    expect(result).toBe(5);
    expect(api.get).toHaveBeenCalledWith('GetDailyWithdrawalCount', { method: 'GET' });
  });

  it('getDailyWithdrawalCount throws on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.getDailyWithdrawalCount()).rejects.toThrow('fail');
  });

  it('setDailyWithdrawalCount returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    (api.post as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.setDailyWithdrawalCount(3);
    expect(result).toEqual(mockResult);
    expect(api.post).toHaveBeenCalledWith('SetDailyWithdrawalCount', { count: 3 });
  });

  it('setDailyWithdrawalCount throws on error', async () => {
    (api.post as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.setDailyWithdrawalCount(3)).rejects.toThrow('fail');
  });

  it('getProviderFees returns fees on success', async () => {
    const mockFees = [
      { transactionType: 'Deposit', fees: [{ transactionType: 'Deposit', method: 'Bank', rate: 1.5 }] },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockFees });
    const result = await paymentProviderService.getProviderFees(1);
    expect(result).toEqual(mockFees);
    expect(api.get).toHaveBeenCalledWith('GetProviderFees?providerId=1', { method: 'GET' });
  });

  it('getProviderFees returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getProviderFees(1);
    expect(result).toEqual([]);
  });

  it('getProviderSettings returns settings on success', async () => {
    (api.get as Mock).mockResolvedValueOnce({ data: { foo: 'bar' } });
    const result = await paymentProviderService.getProviderSettings(1, 2, 3, 4);
    expect(result).toEqual({ foo: 'bar' });
    expect(api.get).toHaveBeenCalledWith(
      expect.stringContaining('ProviderSettings?'),
      { method: 'GET' }
    );
  });

  it('getProviderSettings throws on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.getProviderSettings(1, 2, 3, 4)).rejects.toThrow('fail');
  });

  it('getPaymentMethods returns methods on success', async () => {
    const mockMethods = [
      { id: 1, name: 'Bank', providerId: 1, transactionType: 2, method: 1, status: 1 },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockMethods });
    const result = await paymentProviderService.getPaymentMethods(1, 2);
    expect(result).toEqual(mockMethods);
    expect(api.get).toHaveBeenCalledWith('GetPaymentMethods?providerId=1&transactionType=2', { method: 'GET' });
  });

  it('getPaymentMethods returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getPaymentMethods(1, 2);
    expect(result).toEqual([]);
  });

  it('addPaymentMethod returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    const mockMethod = { id: 1, name: 'Bank', providerId: 1, transactionType: 2, method: 1, status: 1 };
    (api.post as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.addPaymentMethod(mockMethod);
    expect(result).toEqual(mockResult);
    expect(api.post).toHaveBeenCalledWith('AddPaymentMethod', mockMethod);
  });

  it('addPaymentMethod throws on error', async () => {
    (api.post as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.addPaymentMethod({ id: 1, name: 'Bank', providerId: 1, transactionType: 2, method: 1, status: 1 })).rejects.toThrow('fail');
  });

  it('getBanksByTransactionType returns banks on success', async () => {
    const mockBanks = [
      { id: 1, name: 'BankA', transactionTypes: 'Deposit', depositMatchingMode: 1 },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockBanks });
    const result = await paymentProviderService.getBanksByTransactionType(2);
    expect(result).toEqual(mockBanks);
    expect(api.get).toHaveBeenCalledWith('GetBanksByTransactionType?transactionType=2', { method: 'GET' });
  });

  it('getBanksByTransactionType returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getBanksByTransactionType(2);
    expect(result).toEqual([]);
  });

  it('updateProvider returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    (api.post as Mock).mockResolvedValueOnce({ data: mockResult });
    const provider = { id: 1, name: 'A' };
    const result = await paymentProviderService.updateProvider(provider);
    expect(result).toEqual(mockResult);
    expect(api.post).toHaveBeenCalledWith('UpdateProvider', provider);
  });

  it('updateProvider throws on error', async () => {
    (api.post as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.updateProvider({ id: 1, name: 'A' })).rejects.toThrow('fail');
  });

  it('getBanksByCust returns banks on success', async () => {
    const mockBanks = [
      { id: 1, name: 'BankA', transactionTypes: 'Deposit', depositMatchingMode: 1 },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockBanks });
    const result = await paymentProviderService.getBanksByCust();
    expect(result).toEqual(mockBanks);
    expect(api.get).toHaveBeenCalledWith('GetBanksByCust', { method: 'GET' });
  });

  it('getBanksByCust returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getBanksByCust();
    expect(result).toEqual([]);
  });

  it('getTransactionTypes returns types on success', async () => {
    const mockTypes = [
      { id: 1, name: 'Deposit', bankId: 1, isDeposit: true },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockTypes });
    const result = await paymentProviderService.getTransactionTypes();
    expect(result).toEqual(mockTypes);
    expect(api.get).toHaveBeenCalledWith('GetTransactionTypes', { method: 'GET' });
  });

  it('getTransactionTypes returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getTransactionTypes();
    expect(result).toEqual([]);
  });

  it('getBankAccounts returns accounts on success', async () => {
    const mockAccounts = [
      { id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockAccounts });
    const result = await paymentProviderService.getBankAccounts(1, 2, 3, 'search');
    expect(result).toEqual(mockAccounts);
    expect(api.get).toHaveBeenCalledWith(
      expect.stringContaining('GetBankAccounts?'),
      { method: 'GET' }
    );
  });

  it('getBankAccounts returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getBankAccounts(1, 2, 3, 'search');
    expect(result).toEqual([]);
  });

  it('getTransactionLimit returns limit on success', async () => {
    const mockLimit = { dailyWithdrawalCount: 1, minPerDepositTransaction: 2, maxPerDepositTransaction: 3, minPerWithdrawalTransaction: 4, maxPerWithdrawalTransaction: 5 };
    (api.get as Mock).mockResolvedValueOnce({ data: mockLimit });
    const result = await paymentProviderService.getTransactionLimit();
    expect(result).toEqual(mockLimit);
    expect(api.get).toHaveBeenCalledWith('GetTransactionLimit');
  });

  it('getTransactionLimit throws on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.getTransactionLimit()).rejects.toThrow('fail');
  });

  it('setTransactionLimit returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    const mockLimit = { dailyWithdrawalCount: 1, minPerDepositTransaction: 2, maxPerDepositTransaction: 3, minPerWithdrawalTransaction: 4, maxPerWithdrawalTransaction: 5 };
    (api.post as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.setTransactionLimit(mockLimit);
    expect(result).toEqual(mockResult);
    expect(api.post).toHaveBeenCalledWith('SetTransactionLimit', mockLimit);
  });

  it('setTransactionLimit throws on error', async () => {
    (api.post as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.setTransactionLimit({ dailyWithdrawalCount: 1, minPerDepositTransaction: 2, maxPerDepositTransaction: 3, minPerWithdrawalTransaction: 4, maxPerWithdrawalTransaction: 5 })).rejects.toThrow('fail');
  });

  it('addBankAccount returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    const mockAccount = { id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' };
    (api.post as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.addBankAccount(mockAccount);
    expect(result).toEqual(mockResult);
    expect(api.post).toHaveBeenCalledWith('AddBankAccount', mockAccount);
  });

  it('addBankAccount throws on error', async () => {
    (api.post as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.addBankAccount({ id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' })).rejects.toThrow('fail');
  });

  it('getBankAccountById returns account on success', async () => {
    const mockAccount = { id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' };
    (api.get as Mock).mockResolvedValueOnce({ data: mockAccount });
    const result = await paymentProviderService.getBankAccountById(1);
    expect(result).toEqual(mockAccount);
    expect(api.get).toHaveBeenCalledWith('GetBankAccountById?bankAccountId=1');
  });

  it('getBankAccountById throws on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.getBankAccountById(1)).rejects.toThrow('fail');
  });

  it('updateBankAccount returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    const mockAccount = { id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' };
    (api.put as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.updateBankAccount(mockAccount);
    expect(result).toEqual(mockResult);
    expect(api.put).toHaveBeenCalledWith('UpdateBankAccount', mockAccount);
  });

  it('updateBankAccount throws on error', async () => {
    (api.put as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.updateBankAccount({ id: 1, accountNumber: '123', accountNickname: 'A', accountHolderName: 'B', transactionTypes: 'Deposit', status: 1, bankId: 1, bankName: 'BankA', lastUpdated: '', lastUpdatedStr: '', depositMatchingMode: 1, promptPay: false, mobile: '', mobilePrefix: '' })).rejects.toThrow('fail');
  });

  it('deleteBankAccount returns result on success', async () => {
    const mockResult = { isSuccessful: true, message: 'ok', code: 0 };
    (api.delete as Mock).mockResolvedValueOnce({ data: mockResult });
    const result = await paymentProviderService.deleteBankAccount(1);
    expect(result).toEqual(mockResult);
    expect(api.delete).toHaveBeenCalledWith('DeleteBankAccount?bankAccountId=1');
  });

  it('deleteBankAccount throws on error', async () => {
    (api.delete as Mock).mockRejectedValueOnce(new Error('fail'));
    await expect(paymentProviderService.deleteBankAccount(1)).rejects.toThrow('fail');
  });

  it('getMobilePrefixes returns prefixes on success', async () => {
    const mockPrefixes = [
      { id: 1, prefix: '+66', country: 'TH', status: 1 },
    ];
    (api.get as Mock).mockResolvedValueOnce({ data: mockPrefixes });
    const result = await paymentProviderService.getMobilePrefixes();
    expect(result).toEqual(mockPrefixes);
    expect(api.get).toHaveBeenCalledWith('GetMobilePrefixes', { method: 'GET' });
  });

  it('getMobilePrefixes returns [] on error', async () => {
    (api.get as Mock).mockRejectedValueOnce(new Error('fail'));
    const result = await paymentProviderService.getMobilePrefixes();
    expect(result).toEqual([]);
  });
}); 