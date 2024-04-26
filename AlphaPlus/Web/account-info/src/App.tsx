import { useState } from 'react';

import styles from './App.module.scss';
import BalanceInfo from './components/BalanceInfo';
import ContactInfo from './components/ContactInfo';
import CustomerInfo from './components/CustomerInfo';
import SearchBox from './components/SearchBox';
import { GlobalContext } from './context/global-context';
import Customer from './models/customer';
import SettingsService from './services/settings-service';

export const LoadCustomerInfo = {
    ByUpdateStatus: 'reload_customer_info_by_status',
    ByUpdateGivenCredit: 'reload_customer_info_by_given_credit',
    ByClick: 'load_customer_info_by_click'
};

function App() {
    const [mobileView, setMobileView] = useState(false);
    const [username, setUsername] = useState<string | undefined>('');
    const [searchCounter, setSearchCounter] = useState(0);
    const [customer, setCustomer] = useState<Customer | undefined>(undefined);

    const getCustomerInfo = (username: string, requestId: string = LoadCustomerInfo.ByClick) => {
        SettingsService.GetCustomerInfo(username).then((result) => {
            setSearchCounter(searchCounter + 1);
            setCustomer(result);
        });
    };

    return (
        <div className={styles.container}>
            <GlobalContext.Provider value={{ mobileView, username, setUsername, getCustomerInfo, customer }}>
                <SearchBox />
                <CustomerInfo searchCounter={searchCounter} />
                <div className={styles['accordions']}>
                    <ContactInfo />
                    <BalanceInfo />
                </div>
            </GlobalContext.Provider>
        </div>
    );
}

export default App;
