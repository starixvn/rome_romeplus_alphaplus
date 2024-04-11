import { createContext, useContext } from 'react';

import Customer from '../components/models/customer';

export type GlobalContextType = {
    mobileView: boolean;
    username: string;
    setUsername: (value: string) => void;
    customer: Customer;
    setCustomer: (customer: Customer) => void;
};

export const GlobalContext = createContext<Partial<GlobalContextType>>({});

export const useGlobalContext = () => useContext(GlobalContext);
