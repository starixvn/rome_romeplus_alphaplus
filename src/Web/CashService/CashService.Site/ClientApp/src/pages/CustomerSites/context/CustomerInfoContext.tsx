import React, { createContext, useContext, useState, useEffect, ReactNode, useMemo } from 'react';
import { GetCustomerInfo } from '../services/CustomerInfoServices';
import { CustomerInfoData } from '../types/UpdateCustomerType';

interface CustomerInfoContextType {
  customerInfo: CustomerInfoData;
  originalCustomerInfo: CustomerInfoData;
  loading: boolean;
  error: string | null;
  refreshCustomerInfo: () => void;
}

const CustomerInfoContext = createContext<CustomerInfoContextType | undefined>(undefined);

interface CustomerInfoProviderProps {
  children: ReactNode;
  custId: number;
  isCashMember: boolean;
}
const CustomerInfoDataDefault: CustomerInfoData = {
  fistName: '',
  lastName: '',
  mobilePhone: '',
  mobileCode: '',
  mobileNumber: '',
  phone: '',
  email: '',
  fax: ''
}
export const CustomerInfoProvider: React.FC<CustomerInfoProviderProps> = ({ children, custId, isCashMember }) => {
  const [customerInfo, setCustomerInfo] = useState<CustomerInfoData>(CustomerInfoDataDefault);
  const [originalCustomerInfo, setOriginalCustomerInfo] = useState<CustomerInfoData>(CustomerInfoDataDefault);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchCustomerInfo = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await GetCustomerInfo<CustomerInfoData>({
        custId: custId
      });

      if (isCashMember) {
        const mobile = data.mobilePhone.split('-');
        data.mobileCode = mobile[0];
        data.mobileNumber = mobile[1];
      }

      setOriginalCustomerInfo(data);
      setCustomerInfo(data);

    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to fetch customer info');
      console.error('Error fetching customer info:', err);
    } finally {
      setLoading(false);
    }
  };

  const refreshCustomerInfo = () => {
    fetchCustomerInfo();
  };

  useEffect(() => {
    fetchCustomerInfo();
  }, [custId]);

  const value: CustomerInfoContextType = useMemo(() => ({
    originalCustomerInfo,
    customerInfo,
    loading,
    error,
    refreshCustomerInfo
  }), [customerInfo, loading, error, refreshCustomerInfo, originalCustomerInfo]);

  return (
    <CustomerInfoContext.Provider value={value}>
      {children}
    </CustomerInfoContext.Provider>
  );
};

export const useCustomerInfo = (): CustomerInfoContextType => {
  const context = useContext(CustomerInfoContext);
  if (context === undefined) {
    throw new Error('useCustomerInfo must be used within a CustomerInfoProvider');
  }
  return context;
}; 