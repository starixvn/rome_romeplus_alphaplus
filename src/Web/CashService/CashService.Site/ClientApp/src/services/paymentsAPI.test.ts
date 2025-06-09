import { describe, it, expect, vi, beforeEach } from 'vitest';
import { paymentProviderService } from './paymentsAPI';
import { api } from './axios';
import { AxiosResponse } from 'axios';
import { TransactionType } from './interfaces/TransactionType';
import * as PaymentsInterfaces from './interfaces/Payments';

// Mock the axios api
vi.mock('./axios', () => ({
  api: {
    get: vi.fn(),
    post: vi.fn()
  }
}));

describe('paymentProviderService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getProviders', () => {
    it('should successfully fetch payment providers', async () => {
      const mockProviders = [
        { id: 1, name: 'Provider 1' },
        { id: 2, name: 'Provider 2' }
      ];

      const mockResponse = {
        data: mockProviders,
        error: null,
        response: {
          data: mockProviders,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getProviders();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('GetProviders');
    });
  });

  describe('getDailyWithdrawalCount', () => {
    it('should successfully fetch daily withdrawal count', async () => {
      const mockCount = 5;
      const mockResponse = {
        data: mockCount,
        error: null,
        response: {
          data: mockCount,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getDailyWithdrawalCount();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('GetDailyWithdrawalCount');
    });
  });

  describe('setDailyWithdrawalCount', () => {
    it('should successfully set daily withdrawal count', async () => {
      const count = 10;
      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.setDailyWithdrawalCount(count);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('SetDailyWithdrawalCount', { count });
    });
  });

  describe('getProviderFees', () => {
    it('should successfully fetch provider fees', async () => {
      const providerId = 1;
      const mockFees = [
        { id: 1, fee: 0.5 },
        { id: 2, fee: 1.0 }
      ];

      const mockResponse = {
        data: mockFees,
        error: null,
        response: {
          data: mockFees,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getProviderFees(providerId);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`GetProviderFees?providerId=${providerId}`);
    });
  });

  describe('addPaymentMethod', () => {
    it('should successfully add a payment method', async () => {
      const paymentMethod: PaymentsInterfaces.PaymentMethod = {
        id: 1,
        providerId: 1,
        name: 'Test Method',
        transactionType: 1,
        method: 1,
        status: 1
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.addPaymentMethod(paymentMethod);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('AddPaymentMethod', paymentMethod);
    });
  });

  describe('getTransactionTypes', () => {
    it('should successfully fetch transaction types', async () => {
      const mockTypes: TransactionType[] = [
        { id: 1, name: 'Deposit', bankId: 1, isDeposit: true },
        { id: 2, name: 'Withdrawal', bankId: 1, isDeposit: false }
      ];

      const mockResponse = {
        data: mockTypes,
        error: null,
        response: {
          data: mockTypes,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getTransactionTypes();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('GetTransactionTypes');
    });
  });

  describe('getBankAccounts', () => {
    it('should successfully fetch bank accounts with filters', async () => {
      const bankId = 1;
      const transactionType = 1;
      const status = 1;
      const searchText = 'test';

      const mockAccounts: PaymentsInterfaces.BankAccount[] = [
        {
          id: 1,
          accountNumber: '123456',
          accountNickname: 'Test Account 1',
          accountHolderName: 'John Doe',
          transactionTypes: '1,2',
          status: 1,
          code: 'CODE1',
          bankId: 1,
          bankName: 'Test Bank',
          lastUpdated: '2024-03-20',
          lastUpdatedStr: '2024-03-20',
          depositMatchingMode: 1,
          promptPay: false,
          mobile: '1234567890',
          countryCode: 'US',
          bank_TransTypeList: '1,2',
          agent_TransTypeList: '1,2',
          agent_DepositMatchingMode: 1,
          bankDepositMatchingMode: {
            sourceType: 'test',
            mappingTypes: [],
            sourceTypeName: 'Test'
          },
          creator: 'test'
        }
      ];

      const mockResponse = {
        data: mockAccounts,
        error: null,
        response: {
          data: mockAccounts,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getBankAccounts(bankId, transactionType, status, searchText);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(
        `GetBankAccounts?bankId=${bankId}&transactionType=${transactionType}&status=${status}&searchText=${searchText}`
      );
    });
  });

  describe('getProviderSettings', () => {
    it('should successfully fetch provider settings', async () => {
      const providerId = 1;
      const method = 1;
      const status = 1;
      const transactionType = 1;

      const mockSettings = {
        id: 1,
        providerId: 1,
        method: 1,
        status: 1,
        transactionType: 1
      };

      const mockResponse = {
        data: mockSettings,
        error: null,
        response: {
          data: mockSettings,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getProviderSettings(providerId, method, status, transactionType);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(
        `ProviderSettings?providerId=${providerId}&method=${method}&status=${status}&transactionType=${transactionType}`
      );
    });
  });

  describe('getAddPaymentMethodInfo', () => {
    it('should successfully fetch add payment method info', async () => {
      const providerId = 1;
      const transactionType = 1;

      const mockInfo: PaymentsInterfaces.AddPaymentMethodViewModel = {
        paymentMethods: [],
        channelContact: {
          tele: '1234567890'
        }
      };

      const mockResponse = {
        data: mockInfo,
        error: null,
        response: {
          data: mockInfo,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getAddPaymentMethodInfo(providerId, transactionType);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(
        `GetAddPaymentMethodInfo?providerId=${providerId}&transactionType=${transactionType}`
      );
    });
  });

  describe('updatePaymentMethod', () => {
    it('should successfully update a payment method', async () => {
      const paymentMethod: PaymentsInterfaces.PaymentMethod = {
        id: 1,
        providerId: 1,
        name: 'Updated Method',
        transactionType: 1,
        method: 1,
        status: 1
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.updatePaymentMethod(paymentMethod);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('UpdatePaymentMethod', paymentMethod);
    });
  });

  describe('getBanksByTransactionType', () => {
    it('should successfully fetch banks by transaction type', async () => {
      const transactionType = 1;
      const mockBanks: PaymentsInterfaces.Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1',
          code: 'BANK1',
          transactionTypes: '1,2',
          depositMatchingMode: {
            sourceType: 'test',
            mappingTypes: [1, 2],
            sourceTypeName: 'Test Mode'
          }
        },
        { 
          id: 2, 
          name: 'Bank 2',
          code: 'BANK2',
          transactionTypes: '1,2',
          depositMatchingMode: {
            sourceType: 'test',
            mappingTypes: [1, 2],
            sourceTypeName: 'Test Mode'
          }
        }
      ];

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

      const result = await paymentProviderService.getBanksByTransactionType(transactionType);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`GetBanksByTransactionType?transactionType=${transactionType}`);
    });
  });

  describe('updateProvider', () => {
    it('should successfully update a provider', async () => {
      const provider = {
        id: 1,
        name: 'Updated Provider',
        status: 1
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.updateProvider(provider);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('UpdateProvider', provider);
    });
  });

  describe('getBanksByCust', () => {
    it('should successfully fetch banks by customer', async () => {
      const mockBanks: PaymentsInterfaces.Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1',
          code: 'BANK1',
          transactionTypes: '1,2',
          depositMatchingMode: {
            sourceType: 'test',
            mappingTypes: [1, 2],
            sourceTypeName: 'Test Mode'
          }
        },
        { 
          id: 2, 
          name: 'Bank 2',
          code: 'BANK2',
          transactionTypes: '1,2',
          depositMatchingMode: {
            sourceType: 'test',
            mappingTypes: [1, 2],
            sourceTypeName: 'Test Mode'
          }
        }
      ];

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

      const result = await paymentProviderService.getBanksByCust();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('GetBanksByCust');
    });
  });

  describe('getTransactionLimit', () => {
    it('should successfully fetch transaction limit', async () => {
      const mockLimit: PaymentsInterfaces.PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 100,
        maxPerWithdrawalTransaction: 1000
      };

      const mockResponse = {
        data: mockLimit,
        error: null,
        response: {
          data: mockLimit,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getTransactionLimit();
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith('GetTransactionLimit');
    });
  });

  describe('setTransactionLimit', () => {
    it('should successfully set transaction limit', async () => {
      const limit: PaymentsInterfaces.PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 100,
        maxPerWithdrawalTransaction: 1000
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.setTransactionLimit(limit);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('SetTransactionLimit', limit);
    });
  });

  describe('addBankAccount', () => {
    it('should successfully add a bank account', async () => {
      const bankAccount: PaymentsInterfaces.BankAccount = {
        id: 1,
        accountNumber: '123456',
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'CODE1',
        bankId: 1,
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'US',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'test',
          mappingTypes: [],
          sourceTypeName: 'Test'
        },
        creator: 'test'
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.addBankAccount(bankAccount);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('AddBankAccount', bankAccount);
    });
  });

  describe('getBankAccountById', () => {
    it('should successfully fetch bank account by id', async () => {
      const bankAccountId = 1;
      const mockBankAccount: PaymentsInterfaces.BankAccount = {
        id: 1,
        accountNumber: '123456',
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'CODE1',
        bankId: 1,
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'US',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'test',
          mappingTypes: [],
          sourceTypeName: 'Test'
        },
        creator: 'test'
      };

      const mockResponse = {
        data: mockBankAccount,
        error: null,
        response: {
          data: mockBankAccount,
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.get).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.getBankAccountById(bankAccountId);
      expect(result).toEqual(mockResponse);
      expect(api.get).toHaveBeenCalledWith(`GetBankAccountById?bankAccountId=${bankAccountId}`);
    });
  });

  describe('updateBankAccount', () => {
    it('should successfully update a bank account', async () => {
      const bankAccount: PaymentsInterfaces.BankAccount = {
        id: 1,
        accountNumber: '123456',
        accountNickname: 'Updated Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'CODE1',
        bankId: 1,
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: 'US',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: {
          sourceType: 'test',
          mappingTypes: [],
          sourceTypeName: 'Test'
        },
        creator: 'test'
      };

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.updateBankAccount(bankAccount);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('UpdateBankAccount', bankAccount);
    });
  });

  describe('deleteBankAccount', () => {
    it('should successfully delete a bank account', async () => {
      const bankAccountId = 1;

      const mockResponse = {
        data: { success: true },
        error: null,
        response: {
          data: { success: true },
          status: 200,
          statusText: 'OK',
          headers: {},
          config: {} as any
        } as AxiosResponse
      };

      vi.mocked(api.post).mockResolvedValueOnce(mockResponse);

      const result = await paymentProviderService.deleteBankAccount(bankAccountId);
      expect(result).toEqual(mockResponse);
      expect(api.post).toHaveBeenCalledWith('DeleteBankAccount', { bankAccountId });
    });
  });
}); 