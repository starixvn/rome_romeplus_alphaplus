import { describe, it, expect, vi, beforeEach } from 'vitest';
import { paymentProviderService } from './paymentsAPI';
import { api } from '../config/axios';
import { PaymentProvider } from './interfaces/PaymentProvider';
import { PaymentProviderFeeGroup } from './interfaces/PaymentProviderFeeGroup';
import { PaymentMethod } from './interfaces/PaymentMethod';
import { Bank } from './interfaces/Bank';
import { BankAccount } from './interfaces/BankAccount';
import { PaymentTransactionLimit } from './interfaces/PaymentTransactionLimit';
import { TransactionType } from './interfaces/TransactionType';
import { ResultModel } from './interfaces/ResultModel';

// Mock the axios instance
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

  describe('getProviders', () => {
    it('should fetch payment providers successfully', async () => {
      const mockProviders: PaymentProvider[] = [
        { id: 1, name: 'Provider 1', status: 1, fee: 0.5, lastUpdatedStr: '2024-03-20' },
        { id: 2, name: 'Provider 2', status: 1, fee: 1.0, lastUpdatedStr: '2024-03-20' },
      ];

      (api.get as any).mockResolvedValueOnce({ data: mockProviders });

      const result = await paymentProviderService.getProviders();
      expect(result).toEqual(mockProviders);
      expect(api.get).toHaveBeenCalledWith('GetProviders', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getProviders();
      expect(result).toEqual([]);
    });
  });

  describe('getProviderFees', () => {
    it('should fetch provider fees successfully', async () => {
      const mockFees: PaymentProviderFeeGroup[] = [
        { 
          transactionType: 'DEPOSIT',
          fees: [{ 
            transactionType: 'DEPOSIT',
            method: 'BANK_TRANSFER',
            rate: 0.5,
            rateText: '0.5%'
          }]
        },
        { 
          transactionType: 'WITHDRAWAL',
          fees: [{ 
            transactionType: 'WITHDRAWAL',
            method: 'BANK_TRANSFER',
            rate: 1.0,
            rateText: '1.0%'
          }]
        },
      ];

      (api.get as any).mockResolvedValueOnce({ data: mockFees });

      const result = await paymentProviderService.getProviderFees(1);
      expect(result).toEqual(mockFees);
      expect(api.get).toHaveBeenCalledWith('GetProviderFees?providerId=1', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getProviderFees(1);
      expect(result).toEqual([]);
    });
  });

  describe('getPaymentMethods', () => {
    it('should fetch payment methods successfully', async () => {
      const mockMethods: PaymentMethod[] = [
        { 
          id: 1, 
          providerId: 1, 
          name: 'Method 1', 
          status: 1,
          transactionType: 1,
          method: 1
        },
        { 
          id: 2, 
          providerId: 1, 
          name: 'Method 2', 
          status: 1,
          transactionType: 1,
          method: 1
        },
      ];

      (api.get as any).mockResolvedValueOnce({ data: mockMethods });

      const result = await paymentProviderService.getPaymentMethods(1, 1);
      expect(result).toEqual(mockMethods);
      expect(api.get).toHaveBeenCalledWith('GetPaymentMethods?providerId=1&transactionType=1', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getPaymentMethods(1, 1);
      expect(result).toEqual([]);
    });
  });

  describe('getBanksByTransactionType', () => {
    it('should fetch banks successfully', async () => {
      const mockBanks: Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1', 
          transactionTypes: 'DEPOSIT',
          depositMatchingMode: {
            sourceType: 'BANK',
            mappingTypes: [1],
            sourceTypeName: 'Bank Transfer'
          }
        },
        { 
          id: 2, 
          name: 'Bank 2', 
          transactionTypes: 'WITHDRAWAL',
          depositMatchingMode: {
            sourceType: 'BANK',
            mappingTypes: [1],
            sourceTypeName: 'Bank Transfer'
          }
        },
      ];

      (api.get as any).mockResolvedValueOnce({ data: mockBanks });

      const result = await paymentProviderService.getBanksByTransactionType(1);
      expect(result).toEqual(mockBanks);
      expect(api.get).toHaveBeenCalledWith('GetBanksByTransactionType?transactionType=1', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getBanksByTransactionType(1);
      expect(result).toEqual([]);
    });
  });

  describe('getBankAccounts', () => {
    it('should fetch bank accounts successfully', async () => {
      const mockAccounts: BankAccount[] = [
        {
          id: 1,
          bankId: 1,
          accountNumber: '123',
          status: 1,
          accountNickname: 'Test Account 1',
          accountHolderName: 'John Doe',
          transactionTypes: 'DEPOSIT',
          code: 'CODE1',
          bankName: 'Test Bank',
          lastUpdated: '2024-03-20',
          lastUpdatedStr: '2024-03-20',
          depositMatchingMode: 1,
          promptPay: false,
          mobile: '1234567890',
          countryCode: 'TH',
          bank_TransTypeList: 'DEPOSIT',
          agent_TransTypeList: 'DEPOSIT',
          agent_DepositMatchingMode: 1,
          bankDepositMatchingMode: {
            sourceType: 'BANK',
            mappingTypes: [1],
            sourceTypeName: 'Bank Transfer'
          },
          creator: 'admin'
        },
        {
          id: 2,
          bankId: 1,
          accountNumber: '456',
          status: 1,
          accountNickname: 'Test Account 2',
          accountHolderName: 'Jane Doe',
          transactionTypes: 'WITHDRAWAL',
          code: 'CODE2',
          bankName: 'Test Bank',
          lastUpdated: '2024-03-20',
          lastUpdatedStr: '2024-03-20',
          depositMatchingMode: 1,
          promptPay: false,
          mobile: '0987654321',
          countryCode: 'TH',
          bank_TransTypeList: 'WITHDRAWAL',
          agent_TransTypeList: 'WITHDRAWAL',
          agent_DepositMatchingMode: 1,
          bankDepositMatchingMode: {
            sourceType: 'BANK',
            mappingTypes: [1],
            sourceTypeName: 'Bank Transfer'
          },
          creator: 'admin'
        }
      ];

      (api.get as any).mockResolvedValueOnce({ data: mockAccounts });

      const result = await paymentProviderService.getBankAccounts(1, 1, 1, '');
      expect(result).toEqual(mockAccounts);
      expect(api.get).toHaveBeenCalledWith(
        'GetBankAccounts?bankId=1&transactionType=1&status=1&searchText=',
        { method: 'GET' }
      );
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getBankAccounts(1, 1, 1, '');
      expect(result).toEqual([]);
    });
  });

  describe('addBankAccount', () => {
    it('should add bank account successfully', async () => {
      const mockAccount: BankAccount = {
        id: 1,
        bankId: 1,
        accountNumber: '123',
        status: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: 'DEPOSIT',
        code: 'CODE1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'TH',
        bank_TransTypeList: 'DEPOSIT',
        agent_TransTypeList: 'DEPOSIT',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'BANK',
          mappingTypes: [1],
          sourceTypeName: 'Bank Transfer'
        },
        creator: 'admin'
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };

      (api.post as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.addBankAccount(mockAccount);
      expect(result).toEqual(mockResult);
      expect(api.post).toHaveBeenCalledWith('AddBankAccount', mockAccount);
    });

    it('should throw error on failure', async () => {
      const mockAccount: BankAccount = {
        id: 1,
        bankId: 1,
        accountNumber: '123',
        status: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: 'DEPOSIT',
        code: 'CODE1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'TH',
        bank_TransTypeList: 'DEPOSIT',
        agent_TransTypeList: 'DEPOSIT',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'BANK',
          mappingTypes: [1],
          sourceTypeName: 'Bank Transfer'
        },
        creator: 'admin'
      };

      (api.post as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.addBankAccount(mockAccount)).rejects.toThrow('Network error');
    });
  });

  describe('getTransactionLimit', () => {
    it('should fetch transaction limit successfully', async () => {
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 200,
        maxPerWithdrawalTransaction: 2000
      };

      (api.get as any).mockResolvedValueOnce({ data: mockLimit });

      const result = await paymentProviderService.getTransactionLimit();
      expect(result).toEqual(mockLimit);
      expect(api.get).toHaveBeenCalledWith('GetTransactionLimit');
    });

    it('should throw error on failure', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.getTransactionLimit()).rejects.toThrow('Network error');
    });
  });

  describe('getDailyWithdrawalCount', () => {
    it('should fetch daily withdrawal count successfully', async () => {
      const mockCount = 5;
      (api.get as any).mockResolvedValueOnce({ data: mockCount });

      const result = await paymentProviderService.getDailyWithdrawalCount();
      expect(result).toEqual(mockCount);
      expect(api.get).toHaveBeenCalledWith('GetDailyWithdrawalCount', { method: 'GET' });
    });

    it('should throw error on failure', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.getDailyWithdrawalCount()).rejects.toThrow('Network error');
    });
  });

  describe('setDailyWithdrawalCount', () => {
    it('should set daily withdrawal count successfully', async () => {
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.setDailyWithdrawalCount(5);
      expect(result).toEqual(mockResult);
      expect(api.post).toHaveBeenCalledWith('SetDailyWithdrawalCount', { count: 5 });
    });

    it('should throw error on failure', async () => {
      (api.post as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.setDailyWithdrawalCount(5)).rejects.toThrow('Network error');
    });
  });

  describe('getProviderSettings', () => {
    it('should fetch provider settings successfully', async () => {
      const mockSettings = {
        providerId: 1,
        method: 1,
        status: 1,
        transactionType: 1
      };
      (api.get as any).mockResolvedValueOnce({ data: mockSettings });

      const result = await paymentProviderService.getProviderSettings(1, 1, 1, 1);
      expect(result).toEqual(mockSettings);
      expect(api.get).toHaveBeenCalledWith('ProviderSettings?providerId=1&method=1&status=1&transactionType=1', { method: 'GET' });
    });

    it('should throw error on failure', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.getProviderSettings(1, 1, 1, 1)).rejects.toThrow('Network error');
    });
  });

  describe('addPaymentMethod', () => {
    it('should add payment method successfully', async () => {
      const mockMethod: PaymentMethod = {
        id: 1,
        providerId: 1,
        name: 'Test Method',
        status: 1,
        transactionType: 1,
        method: 1
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.addPaymentMethod(mockMethod);
      expect(result).toEqual(mockResult);
      expect(api.post).toHaveBeenCalledWith('AddPaymentMethod', mockMethod);
    });

    it('should throw error on failure', async () => {
      const mockMethod: PaymentMethod = {
        id: 1,
        providerId: 1,
        name: 'Test Method',
        status: 1,
        transactionType: 1,
        method: 1
      };
      (api.post as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.addPaymentMethod(mockMethod)).rejects.toThrow('Network error');
    });
  });

  describe('updateProvider', () => {
    it('should update provider successfully', async () => {
      const mockProvider = {
        id: 1,
        name: 'Updated Provider',
        status: 1
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.updateProvider(mockProvider);
      expect(result).toEqual(mockResult);
      expect(api.post).toHaveBeenCalledWith('UpdateProvider', mockProvider);
    });

    it('should throw error on failure', async () => {
      const mockProvider = {
        id: 1,
        name: 'Updated Provider',
        status: 1
      };
      (api.post as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.updateProvider(mockProvider)).rejects.toThrow('Network error');
    });
  });

  describe('getBanksByCust', () => {
    it('should fetch banks by customer successfully', async () => {
      const mockBanks: Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1', 
          transactionTypes: 'DEPOSIT',
          depositMatchingMode: {
            sourceType: 'BANK',
            mappingTypes: [1],
            sourceTypeName: 'Bank Transfer'
          }
        }
      ];
      (api.get as any).mockResolvedValueOnce({ data: mockBanks });

      const result = await paymentProviderService.getBanksByCust();
      expect(result).toEqual(mockBanks);
      expect(api.get).toHaveBeenCalledWith('GetBanksByCust', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getBanksByCust();
      expect(result).toEqual([]);
    });
  });

  describe('getTransactionTypes', () => {
    it('should fetch transaction types successfully', async () => {
      const mockTypes: TransactionType[] = [
        { id: 1, name: 'DEPOSIT', bankId: 1, isDeposit: true },
        { id: 2, name: 'WITHDRAWAL', bankId: 1, isDeposit: false }
      ];
      (api.get as any).mockResolvedValueOnce({ data: mockTypes });

      const result = await paymentProviderService.getTransactionTypes();
      expect(result).toEqual(mockTypes);
      expect(api.get).toHaveBeenCalledWith('GetTransactionTypes', { method: 'GET' });
    });

    it('should return empty array on error', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      const result = await paymentProviderService.getTransactionTypes();
      expect(result).toEqual([]);
    });
  });

  describe('setTransactionLimit', () => {
    it('should set transaction limit successfully', async () => {
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 200,
        maxPerWithdrawalTransaction: 2000
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.setTransactionLimit(mockLimit);
      expect(result).toEqual(mockResult);
      expect(api.post).toHaveBeenCalledWith('SetTransactionLimit', mockLimit);
    });

    it('should throw error on failure', async () => {
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 200,
        maxPerWithdrawalTransaction: 2000
      };
      (api.post as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.setTransactionLimit(mockLimit)).rejects.toThrow('Network error');
    });
  });

  describe('getBankAccountById', () => {
    it('should fetch bank account by id successfully', async () => {
      const mockAccount: BankAccount = {
        id: 1,
        bankId: 1,
        accountNumber: '123',
        status: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: 'DEPOSIT',
        code: 'CODE1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'TH',
        bank_TransTypeList: 'DEPOSIT',
        agent_TransTypeList: 'DEPOSIT',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'BANK',
          mappingTypes: [1],
          sourceTypeName: 'Bank Transfer'
        },
        creator: 'admin'
      };
      (api.get as any).mockResolvedValueOnce({ data: mockAccount });

      const result = await paymentProviderService.getBankAccountById(1);
      expect(result).toEqual(mockAccount);
      expect(api.get).toHaveBeenCalledWith('GetBankAccountById?bankAccountId=1');
    });

    it('should throw error on failure', async () => {
      (api.get as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.getBankAccountById(1)).rejects.toThrow('Network error');
    });
  });

  describe('updateBankAccount', () => {
    it('should update bank account successfully', async () => {
      const mockAccount: BankAccount = {
        id: 1,
        bankId: 1,
        accountNumber: '123',
        status: 1,
        accountNickname: 'Updated Account',
        accountHolderName: 'John Doe',
        transactionTypes: 'DEPOSIT',
        code: 'CODE1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'TH',
        bank_TransTypeList: 'DEPOSIT',
        agent_TransTypeList: 'DEPOSIT',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'BANK',
          mappingTypes: [1],
          sourceTypeName: 'Bank Transfer'
        },
        creator: 'admin'
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.put as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.updateBankAccount(mockAccount);
      expect(result).toEqual(mockResult);
      expect(api.put).toHaveBeenCalledWith('UpdateBankAccount', mockAccount);
    });

    it('should throw error on failure', async () => {
      const mockAccount: BankAccount = {
        id: 1,
        bankId: 1,
        accountNumber: '123',
        status: 1,
        accountNickname: 'Updated Account',
        accountHolderName: 'John Doe',
        transactionTypes: 'DEPOSIT',
        code: 'CODE1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'TH',
        bank_TransTypeList: 'DEPOSIT',
        agent_TransTypeList: 'DEPOSIT',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'BANK',
          mappingTypes: [1],
          sourceTypeName: 'Bank Transfer'
        },
        creator: 'admin'
      };
      (api.put as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.updateBankAccount(mockAccount)).rejects.toThrow('Network error');
    });
  });

  describe('deleteBankAccount', () => {
    it('should delete bank account successfully', async () => {
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.delete as any).mockResolvedValueOnce({ data: mockResult });

      const result = await paymentProviderService.deleteBankAccount(1);
      expect(result).toEqual(mockResult);
      expect(api.delete).toHaveBeenCalledWith('DeleteBankAccount?bankAccountId=1');
    });

    it('should throw error on failure', async () => {
      (api.delete as any).mockRejectedValueOnce(new Error('Network error'));

      await expect(paymentProviderService.deleteBankAccount(1)).rejects.toThrow('Network error');
    });
  });
}); 