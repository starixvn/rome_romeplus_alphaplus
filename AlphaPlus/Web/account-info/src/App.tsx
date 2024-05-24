import axios from 'axios';
import { useState } from 'react';

import styles from './App.module.scss';
import BalanceInfo from './components/BalanceInfo';
import ContactInfo from './components/ContactInfo';
import CustomerInfo from './components/CustomerInfo';
import DownlineList from './components/DownlineList';
import LoginInfo from './components/LoginInfo';
import OnlineList from './components/OnlineList';
import SearchBox from './components/SearchBox';
import StatusAndSetting from './components/StatusAndSetting';
import { BlockIds, LoadCustomerAllStatus, LoadCustomerInfo, UserLevel } from './constants';
import { GlobalContext } from './context/global-context';
import Customer from './models/customer';
import SettingsService, { handleError } from './services/settings-service';

function App() {
    const { actorRoleId, mobileView } = params;
    const [searchAccountRoleId, setSearchAccountRoleId] = useState(actorRoleId - 1);
    const [username, setUsername] = useState<string | undefined>('');
    const [searchCounter, setSearchCounter] = useState(0);
    const [customer, setCustomer] = useState<Customer | undefined>(undefined);
    const [expandedBlockId, setExpandedBlockId] = useState<BlockIds | undefined>(BlockIds.BalanceInfo);
    const [requestType, setRequestType] = useState<string>(LoadCustomerInfo.ByClick);
    const CancelToken = axios.CancelToken;
    const source = CancelToken.source();

    const getCustomerInfo = (username: string, requestId: string = LoadCustomerInfo.ByClick) => {
        setRequestType(requestId);
        const loadCustomerInfoByClick = requestId === LoadCustomerInfo.ByClick;
        const resetSetting = requestId !== LoadCustomerInfo.ByUpdateStatus;

        $.blockUI?.();
        SettingsService.GetCustomerInfo(username)
            .then((result) => {
                if (loadCustomerInfoByClick) {
                    source.cancel(LoadCustomerInfo.ByUpdateGivenCredit);
                    source.cancel(LoadCustomerInfo.ByUpdateStatus);
                    source.cancel(LoadCustomerAllStatus.ByClick);
                    source.cancel(LoadCustomerAllStatus.ByUpdateStatus);
                }

                setSearchCounter(searchCounter + 1);
                setCustomer(result);
                if (result) {
                    setSearchAccountRoleId(result.RoleId - 1);
                } else {
                    if (resetSetting) {
                        setCustomer(undefined);
                    }
                }
            })
            .catch(handleError)
            .finally(() => {
                $.unblockUI?.();
            });
    };

    const handleBlockClick = (blockId: BlockIds) => {
        if (blockId === expandedBlockId) {
            setExpandedBlockId?.(undefined);
        } else {
            setExpandedBlockId?.(blockId);
        }
    };

    const handleOpenIPInfo = (ip: string) => {
        const IPInfoPath = `${customer?.ViewLoginIPLink}?ip=${ip}`;
        window.top?.ageWnd.open(IPInfoPath);
    };

    return (
        <div className={styles.container}>
            <GlobalContext.Provider
                value={{
                    requestType,
                    mobileView,
                    username,
                    setUsername,
                    getCustomerInfo,
                    customer,
                    expandedBlockId,
                    handleBlockClick,
                    handleOpenIPInfo
                }}
            >
                <SearchBox />
                <CustomerInfo searchCounter={searchCounter} />
                {customer && (
                    <div className={styles['accordions']}>
                        <ContactInfo />
                        <BalanceInfo />
                        {actorRoleId > UserLevel.Agent && searchAccountRoleId > UserLevel.Member && <DownlineList />}
                        <StatusAndSetting />
                        <LoginInfo />
                        <OnlineList />
                    </div>
                )}
            </GlobalContext.Provider>
        </div>
    );
}

export default App;
