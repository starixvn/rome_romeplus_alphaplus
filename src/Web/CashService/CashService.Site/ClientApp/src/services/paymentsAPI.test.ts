import { describe, it, expect, vi, beforeEach } from 'vitest';
import { paymentProviderService } from './paymentsAPI';
import { api } from '../config/axios';
import { PaymentProvider, PaymentMethod, Bank, BankAccount, PaymentTransactionLimit, PaymentProviderFeeGroup, AddPaymentMethodViewModel } from './interfaces/Payments';
import { ResultModel } from './interfaces/ResultModel';
import { TransactionType } from './interfaces/TransactionType';

// Mock the axios api
vi.mock('../config/axios', () => ({
  api: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
  },
}));

describe('paymentProviderService', () => {
  beforeEach(() => {
    // Clear all mocks before each test
    vi.clearAllMocks();
  });

  describe('getProviders', () => {
    it('should return providers when API call is successful', async () => {
      // Arrange
      const mockProviders: PaymentProvider[] = [
        { 
          id: 1, 
          name: 'Provider 1',
          fee: 0.5,
          status: 1,
          lastUpdatedStr: '2024-03-20'
        },
        { 
          id: 2, 
          name: 'Provider 2',
          fee: 0.7,
          status: 1,
          lastUpdatedStr: '2024-03-20'
        },
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockProviders });

      // Act
      const result = await paymentProviderService.getProviders();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetProviders', { method: 'GET' });
      expect(result).toEqual(mockProviders);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getProviders();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetProviders', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });

  describe('getDailyWithdrawalCount', () => {
    it('should return withdrawal count when API call is successful', async () => {
      // Arrange
      const mockCount = 5;
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockCount });

      // Act
      const result = await paymentProviderService.getDailyWithdrawalCount();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetDailyWithdrawalCount', { method: 'GET' });
      expect(result).toBe(mockCount);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.getDailyWithdrawalCount()).rejects.toThrow('Network error');
    });
  });

  describe('addPaymentMethod', () => {
    it('should successfully add payment method', async () => {
      // Arrange
      const mockPaymentMethod: PaymentMethod = {
        id: 1,
        name: 'Test Method',
        providerId: 1,
        transactionType: 1,
        method: 1,
        status: 1
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.addPaymentMethod(mockPaymentMethod);

      // Assert
      expect(api.post).toHaveBeenCalledWith('AddPaymentMethod', mockPaymentMethod);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockPaymentMethod: PaymentMethod = {
        id: 1,
        name: 'Test Method',
        providerId: 1,
        transactionType: 1,
        method: 1,
        status: 1
      };
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.addPaymentMethod(mockPaymentMethod)).rejects.toThrow('Network error');
    });
  });

  describe('getBanksByTransactionType', () => {
    it('should return banks when API call is successful', async () => {
      // Arrange
      const mockBanks: Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1',
          code: 'B1',
          transactionTypes: '1,2',
          depositMatchingMode: { 
            sourceType: 'BANK',
            mappingTypes: [1, 2],
            sourceTypeName: 'Bank Account'
          }
        },
        { 
          id: 2, 
          name: 'Bank 2',
          code: 'B2',
          transactionTypes: '1,2',
          depositMatchingMode: { 
            sourceType: 'BANK',
            mappingTypes: [1, 2],
            sourceTypeName: 'Bank Account'
          }
        },
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockBanks });

      // Act
      const result = await paymentProviderService.getBanksByTransactionType(1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetBanksByTransactionType?transactionType=1', { method: 'GET' });
      expect(result).toEqual(mockBanks);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getBanksByTransactionType(1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetBanksByTransactionType?transactionType=1', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });

  describe('getBankAccounts', () => {
    it('should return bank accounts when API call is successful', async () => {
      // Arrange
      const mockAccounts: BankAccount[] = [
        { 
          id: 1, 
          accountNumber: '123456', 
          bankId: 1,
          accountNickname: 'Test Account 1',
          accountHolderName: 'John Doe',
          transactionTypes: '1,2',
          status: 1,
          code: 'ACC1',
          bankName: 'Test Bank',
          lastUpdated: '2024-03-20',
          lastUpdatedStr: '2024-03-20',
          depositMatchingMode: 1,
          promptPay: false,
          mobile: '1234567890',
          countryCode: '+1',
          bank_TransTypeList: '1,2',
          agent_TransTypeList: '1,2',
          agent_DepositMatchingMode: 1,
          bankDepositMatchingMode: { 
            sourceType: 'BANK',
            mappingTypes: [1, 2],
            sourceTypeName: 'Bank Account'
          },
          creator: 'System'
        },
        { 
          id: 2, 
          accountNumber: '789012', 
          bankId: 1,
          accountNickname: 'Test Account 2',
          accountHolderName: 'Jane Doe',
          transactionTypes: '1,2',
          status: 1,
          code: 'ACC2',
          bankName: 'Test Bank',
          lastUpdated: '2024-03-20',
          lastUpdatedStr: '2024-03-20',
          depositMatchingMode: 1,
          promptPay: false,
          mobile: '0987654321',
          countryCode: '+1',
          bank_TransTypeList: '1,2',
          agent_TransTypeList: '1,2',
          agent_DepositMatchingMode: 1,
          bankDepositMatchingMode: { 
            sourceType: 'BANK',
            mappingTypes: [1, 2],
            sourceTypeName: 'Bank Account'
          },
          creator: 'System'
        },
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockAccounts });

      // Act
      const result = await paymentProviderService.getBankAccounts(1, 1, 1, '');

      // Assert
      expect(api.get).toHaveBeenCalledWith(
        'GetBankAccounts?bankId=1&transactionType=1&status=1&searchText=',
        { method: 'GET' }
      );
      expect(result).toEqual(mockAccounts);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getBankAccounts(1, 1, 1, '');

      // Assert
      expect(api.get).toHaveBeenCalledWith(
        'GetBankAccounts?bankId=1&transactionType=1&status=1&searchText=',
        { method: 'GET' }
      );
      expect(result).toEqual([]);
    });
  });

  describe('getTransactionLimit', () => {
    it('should return transaction limit when API call is successful', async () => {
      // Arrange
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 50,
        maxPerWithdrawalTransaction: 500
      };
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockLimit });

      // Act
      const result = await paymentProviderService.getTransactionLimit();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetTransactionLimit');
      expect(result).toEqual(mockLimit);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.getTransactionLimit()).rejects.toThrow('Network error');
    });
  });

  describe('setDailyWithdrawalCount', () => {
    it('should successfully set daily withdrawal count', async () => {
      // Arrange
      const mockCount = 5;
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.setDailyWithdrawalCount(mockCount);

      // Assert
      expect(api.post).toHaveBeenCalledWith('SetDailyWithdrawalCount', { count: mockCount });
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockCount = 5;
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.setDailyWithdrawalCount(mockCount)).rejects.toThrow('Network error');
    });
  });

  describe('getProviderFees', () => {
    it('should return provider fees when API call is successful', async () => {
      // Arrange
      const mockFees: PaymentProviderFeeGroup[] = [
        {
          transactionType: '1',
          fees: [
            {
              transactionType: '1',
              method: '1',
              rate: 0.5,
              rateText: '0.5%'
            }
          ]
        }
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockFees });

      // Act
      const result = await paymentProviderService.getProviderFees(1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetProviderFees?providerId=1', { method: 'GET' });
      expect(result).toEqual(mockFees);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getProviderFees(1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetProviderFees?providerId=1', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });

  describe('getProviderSettings', () => {
    it('should return provider settings when API call is successful', async () => {
      // Arrange
      const mockSettings = { setting1: 'value1', setting2: 'value2' };
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockSettings });

      // Act
      const result = await paymentProviderService.getProviderSettings(1, 1, 1, 1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('ProviderSettings?providerId=1&method=1&status=1&transactionType=1', { method: 'GET' });
      expect(result).toEqual(mockSettings);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.getProviderSettings(1, 1, 1, 1)).rejects.toThrow('Network error');
    });
  });

  describe('getAddPaymentMethodInfo', () => {
    it('should return payment method info when API call is successful', async () => {
      // Arrange
      const mockInfo: AddPaymentMethodViewModel = {
        paymentMethods: [
          {
            id: 1,
            name: 'Test Method',
            providerId: 1,
            transactionType: 1,
            method: 1,
            status: 1
          }
        ],
        channelContact: { tele: '1234567890' }
      };
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockInfo });

      // Act
      const result = await paymentProviderService.getAddPaymentMethodInfo(1, 1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetAddPaymentMethodInfo?providerId=1&transactionType=1', { method: 'GET' });
      expect(result).toEqual(mockInfo);
    });

    it('should return default object when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getAddPaymentMethodInfo(1, 1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetAddPaymentMethodInfo?providerId=1&transactionType=1', { method: 'GET' });
      expect(result).toEqual({ paymentMethods: [], channelContact: { tele: '' } });
    });
  });

  describe('updatePaymentMethod', () => {
    it('should successfully update payment method', async () => {
      // Arrange
      const mockPaymentMethod: PaymentMethod = {
        id: 1,
        name: 'Updated Method',
        providerId: 1,
        transactionType: 1,
        method: 1,
        status: 1
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.updatePaymentMethod(mockPaymentMethod);

      // Assert
      expect(api.post).toHaveBeenCalledWith('UpdatePaymentMethod', mockPaymentMethod);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockPaymentMethod: PaymentMethod = {
        id: 1,
        name: 'Updated Method',
        providerId: 1,
        transactionType: 1,
        method: 1,
        status: 1
      };
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.updatePaymentMethod(mockPaymentMethod)).rejects.toThrow('Network error');
    });
  });

  describe('updateProvider', () => {
    it('should successfully update provider', async () => {
      // Arrange
      const mockProvider = {
        id: 1,
        name: 'Updated Provider',
        fee: 0.5,
        status: 1
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.updateProvider(mockProvider);

      // Assert
      expect(api.post).toHaveBeenCalledWith('UpdateProvider', mockProvider);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockProvider = {
        id: 1,
        name: 'Updated Provider',
        fee: 0.5,
        status: 1
      };
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.updateProvider(mockProvider)).rejects.toThrow('Network error');
    });
  });

  describe('getBanksByCust', () => {
    it('should return banks when API call is successful', async () => {
      // Arrange
      const mockBanks: Bank[] = [
        { 
          id: 1, 
          name: 'Bank 1',
          code: 'B1',
          transactionTypes: '1,2',
          depositMatchingMode: { 
            sourceType: 'BANK',
            mappingTypes: [1, 2],
            sourceTypeName: 'Bank Account'
          }
        }
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockBanks });

      // Act
      const result = await paymentProviderService.getBanksByCust();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetBanksByCust', { method: 'GET' });
      expect(result).toEqual(mockBanks);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getBanksByCust();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetBanksByCust', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });

  describe('getTransactionTypes', () => {
    it('should return transaction types when API call is successful', async () => {
      // Arrange
      const mockTypes: TransactionType[] = [
        { id: 1, name: 'Type 1', bankId: 1, isDeposit: true },
        { id: 2, name: 'Type 2', bankId: 1, isDeposit: false }
      ];
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockTypes });

      // Act
      const result = await paymentProviderService.getTransactionTypes();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetTransactionTypes', { method: 'GET' });
      expect(result).toEqual(mockTypes);
    });

    it('should return empty array when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act
      const result = await paymentProviderService.getTransactionTypes();

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetTransactionTypes', { method: 'GET' });
      expect(result).toEqual([]);
    });
  });

  describe('setTransactionLimit', () => {
    it('should successfully set transaction limit', async () => {
      // Arrange
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 50,
        maxPerWithdrawalTransaction: 500
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.setTransactionLimit(mockLimit);

      // Assert
      expect(api.post).toHaveBeenCalledWith('SetTransactionLimit', mockLimit);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockLimit: PaymentTransactionLimit = {
        dailyWithdrawalCount: 5,
        minPerDepositTransaction: 100,
        maxPerDepositTransaction: 1000,
        minPerWithdrawalTransaction: 50,
        maxPerWithdrawalTransaction: 500
      };
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.setTransactionLimit(mockLimit)).rejects.toThrow('Network error');
    });
  });

  describe('addBankAccount', () => {
    it('should successfully add bank account', async () => {
      // Arrange
      const mockAccount: BankAccount = {
        id: 1,
        accountNumber: '123456',
        bankId: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'ACC1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: '+1',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: { 
          sourceType: 'BANK',
          mappingTypes: [1, 2],
          sourceTypeName: 'Bank Account'
        },
        creator: 'System'
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.addBankAccount(mockAccount);

      // Assert
      expect(api.post).toHaveBeenCalledWith('AddBankAccount', mockAccount);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockAccount: BankAccount = {
        id: 1,
        accountNumber: '123456',
        bankId: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'ACC1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: '+1',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: { 
          sourceType: 'BANK',
          mappingTypes: [1, 2],
          sourceTypeName: 'Bank Account'
        },
        creator: 'System'
      };
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.addBankAccount(mockAccount)).rejects.toThrow('Network error');
    });
  });

  describe('getBankAccountById', () => {
    it('should return bank account when API call is successful', async () => {
      // Arrange
      const mockAccount: BankAccount = {
        id: 1,
        accountNumber: '123456',
        bankId: 1,
        accountNickname: 'Test Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'ACC1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: '+1',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: { 
          sourceType: 'BANK',
          mappingTypes: [1, 2],
          sourceTypeName: 'Bank Account'
        },
        creator: 'System'
      };
      (api.get as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockAccount });

      // Act
      const result = await paymentProviderService.getBankAccountById(1);

      // Assert
      expect(api.get).toHaveBeenCalledWith('GetBankAccountById?bankAccountId=1');
      expect(result).toEqual(mockAccount);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.get as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.getBankAccountById(1)).rejects.toThrow('Network error');
    });
  });

  describe('updateBankAccount', () => {
    it('should successfully update bank account', async () => {
      // Arrange
      const mockAccount: BankAccount = {
        id: 1,
        accountNumber: '123456',
        bankId: 1,
        accountNickname: 'Updated Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'ACC1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: '+1',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: { 
          sourceType: 'BANK',
          mappingTypes: [1, 2],
          sourceTypeName: 'Bank Account'
        },
        creator: 'System'
      };
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.put as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.updateBankAccount(mockAccount);

      // Assert
      expect(api.put).toHaveBeenCalledWith('UpdateBankAccount', mockAccount);
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockAccount: BankAccount = {
        id: 1,
        accountNumber: '123456',
        bankId: 1,
        accountNickname: 'Updated Account',
        accountHolderName: 'John Doe',
        transactionTypes: '1,2',
        status: 1,
        code: 'ACC1',
        bankName: 'Test Bank',
        lastUpdated: '2024-03-20',
        lastUpdatedStr: '2024-03-20',
        depositMatchingMode: 1,
        promptPay: false,
        mobile: '1234567890',
        countryCode: '+1',
        bank_TransTypeList: '1,2',
        agent_TransTypeList: '1,2',
        agent_DepositMatchingMode: 1,
        bankDepositMatchingMode: { 
          sourceType: 'BANK',
          mappingTypes: [1, 2],
          sourceTypeName: 'Bank Account'
        },
        creator: 'System'
      };
      const mockError = new Error('Network error');
      (api.put as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.updateBankAccount(mockAccount)).rejects.toThrow('Network error');
    });
  });

  describe('deleteBankAccount', () => {
    it('should successfully delete bank account', async () => {
      // Arrange
      const mockResult: ResultModel = { 
        isSuccessful: true, 
        message: 'Success',
        code: 200
      };
      (api.post as ReturnType<typeof vi.fn>).mockResolvedValueOnce({ data: mockResult });

      // Act
      const result = await paymentProviderService.deleteBankAccount(1);

      // Assert
      expect(api.post).toHaveBeenCalledWith('DeleteBankAccount', { bankAccountId: 1 });
      expect(result).toEqual(mockResult);
    });

    it('should throw error when API call fails', async () => {
      // Arrange
      const mockError = new Error('Network error');
      (api.post as ReturnType<typeof vi.fn>).mockRejectedValueOnce(mockError);

      // Act & Assert
      await expect(paymentProviderService.deleteBankAccount(1)).rejects.toThrow('Network error');
    });
  });
}); 