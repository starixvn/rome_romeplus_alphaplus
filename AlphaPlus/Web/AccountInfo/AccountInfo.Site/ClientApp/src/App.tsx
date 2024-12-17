import axios from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import styles from './App.module.scss';
import BalanceInfo from './components/BalanceInfo';
import ContactInfo from './components/ContactInfo';
import CustomerInfo from './components/CustomerInfo';
import AccountInfoDesktop from './components/Desktop';
import DownlineList from './components/DownlineList';
import LoginInfo from './components/LoginInfo';
import AccountInfoMobile from './components/Mobile';
import OnlineList from './components/OnlineList';
import SearchBox from './components/SearchBox';
import Setting from './components/Setting';
import Status from './components/Status';
import { BlockIds, LoadCustomerAllStatus, LoadCustomerInfo, UserLevel } from './constants';
import { GlobalContext } from './context/global-context';
import Customer from './models/customer';
import DownlineListResponse from './models/DownlineListReponse';
import OnlineUser from './models/online-user';
import SettingItem from './models/setting-item';
import StatusItem from './models/status-item';
import { handleError } from './services/axios-helper';
import SettingsService from './services/settings-service';

function App() {
    const { actorName, actorRoleId, mobileView } = params;
    const [searchAccountRoleId, setSearchAccountRoleId] = useState(actorRoleId - 1);
    const [username, setUsername] = useState<string | undefined>('');
    const [searchCounter, setSearchCounter] = useState(0);
    const [customer, setCustomer] = useState<Customer | undefined>(undefined);
    const [requestType, setRequestType] = useState<string>(LoadCustomerInfo.ByClick);
    const [downlineListData, setDownlineListData] = useState<DownlineListResponse | undefined>(undefined);
    const [lastTxn, setLastTxn] = useState('');
    const [onlineUsers, setOnlineUsers] = useState<OnlineUser[] | undefined>(undefined);
    const [statusData, setStatusData] = useState<StatusItem[] | undefined>(undefined);
    const [settingData, setSettingData] = useState<SettingItem[] | undefined>(undefined);
    const CancelToken = axios.CancelToken;
    const source = CancelToken.source();
    const lastSearchUsernameCacheKey = `${actorName}-lastSearchUsername`;
    const { t } = useTranslation();
    const [redirectToBalanceInfoPage, setRedirectToBalanceInfoPage] = useState(false);

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

                if (typeof result !== 'undefined') {
                    setSearchCounter(searchCounter + 1);
                    setLastTxn('');

                    if (result) {
                        setUsername(username);
                        setSearchAccountRoleId(result.RoleId);
                        setCustomer(result);
                        setRedirectToBalanceInfoPage(loadCustomerInfoByClick);
                        setOnlineUsers(undefined);
                        setDownlineListData(undefined);

                        if (resetSetting) {
                            setStatusData(undefined);
                            setSettingData(undefined);
                        }

                        localStorage.setItem(lastSearchUsernameCacheKey, username);
                    } else {
                        setUsername('');
                        setCustomer(undefined);
                        setOnlineUsers(undefined);
                        setDownlineListData(undefined);
                        setStatusData(undefined);
                        setSettingData(undefined);

                        localStorage.removeItem(lastSearchUsernameCacheKey);
                    }
                }
            })
            .catch(handleError)
            .finally(() => {
                $.unblockUI?.();
            });
    };

    const fetchDownlineListFromServer = (pageIndex = 1, reloadByStatus = false, statusId = 1, totalPages = 0) => {
        if (customer?.CustId && (!downlineListData || pageIndex > 1 || reloadByStatus)) {
            $.blockUI?.();
            SettingsService.GetDownlineList(customer?.CustId, customer?.RoleId, pageIndex, statusId, totalPages)
                .then((data) => {
                    data.PageIndex = pageIndex;
                    data.StatusId = statusId;
                    setDownlineListData(data);
                    setRedirectToBalanceInfoPage(false);
                })
                .catch(handleError)
                .finally(() => $.unblockUI?.());
        }
    };

    const fetchLastTxnFromServer = () => {
        if (customer && customer.CustId > 0 && !lastTxn) {
            $.blockUI?.();
            SettingsService.GetLastTxn(customer.UserName)
                .then((lastTxn) => {
                    setLastTxn(lastTxn);
                    setRedirectToBalanceInfoPage(false);
                })
                .catch(handleError)
                .finally(() => $.unblockUI?.());
        }
    };

    const fetchOnlineListFromServer = () => {
        if (customer?.CustId && onlineUsers === undefined) {
            SettingsService.GetOnlineList(customer.CustId).then((data) => {
                setOnlineUsers(data);
                setRedirectToBalanceInfoPage(false);
            });
        }
    };

    const blockUIWrapper = () => {
        if ($('.ui-widget-overlay:visible').length > 0) {
            $.blockUI?.();
        }
    };

    const getCustomerStatus = (requestId: string | undefined) => {
        if ((!statusData || !settingData || requestId === LoadCustomerAllStatus.ByUpdateStatus) && customer) {
            blockUIWrapper();

            const { CustId, UserName, RoleId } = customer;
            let uplineId: number;
            let uplineName: string;

            switch (RoleId) {
                case UserLevel.Master:
                    uplineId = customer.SuperId;
                    uplineName = customer.SuperName;
                    break;
                case UserLevel.Agent:
                    uplineId = customer.MasterId;
                    uplineName = customer.MasterName;
                    break;
                default:
                    uplineId = customer.AgentId;
                    uplineName = customer.AgentName;
                    break;
            }

            SettingsService.GetCustomerAllStatus(CustId, UserName, RoleId, uplineId, uplineName)
                .then((responseData) => {
                    if (requestId === LoadCustomerAllStatus.ByClick) {
                        source.cancel(LoadCustomerAllStatus.ByUpdateStatus);
                    }
                    setStatusData(responseData.Status);
                    setSettingData(responseData.Settings);
                    setRedirectToBalanceInfoPage(false);
                })
                .catch(handleError)
                .finally(() => $.unblockUI?.());
        }
    };

    useEffect(() => {
        const lastSearchUsername = localStorage.getItem(lastSearchUsernameCacheKey);
        if (lastSearchUsername) {
            setUsername(lastSearchUsername);
            getCustomerInfo(lastSearchUsername);
        }
    }, []);

    useEffect(() => {
        setDownlineListData(undefined);
        setLastTxn('');
        setOnlineUsers(undefined);
        setStatusData(undefined);
        setSettingData(undefined);
    }, [customer?.CustId]);

    const handleOpenIPInfo = (ip: string) => {
        const IPInfoPath = `${customer?.ViewLoginIPLink}?ip=${ip}`;
        window.top?.ageWnd.open(IPInfoPath);
    };

    const accordionItems = [
        {
            blockId: BlockIds.ContactInfo,
            featureText: t('ContactInfo'),
            iconClass: 'icon-contact-info',
            pathname: '/contactInfo',
            visible: true,
            children: <ContactInfo />
        },
        {
            blockId: BlockIds.BalanceInfo,
            featureText: t('BalanceInfo'),
            iconClass: 'icon-balance',
            pathname: '/balanceInfo',
            visible: true,
            children: <BalanceInfo />
        },
        {
            blockId: BlockIds.DownlineList,
            featureText: t('DownlineList'),
            iconClass: 'icon-downline',
            pathname: '/downlineList',
            visible: actorRoleId > UserLevel.Agent && searchAccountRoleId > UserLevel.Member,
            fetchDataFromServer: fetchDownlineListFromServer,
            children: (
                <DownlineList
                    downlineListData={downlineListData}
                    fetchDownlineListFromServer={fetchDownlineListFromServer}
                />
            )
        },
        {
            blockId: BlockIds.Status,
            featureText: t('Status'),
            iconClass: 'icon-status',
            pathname: '/status',
            visible: true,
            fetchDataFromServer: getCustomerStatus,
            children: <Status statusItems={statusData} getCustomerStatus={getCustomerStatus} />
        },
        {
            blockId: BlockIds.Setting,
            featureText: t('Setting'),
            iconClass: 'icon-settings',
            pathname: '/setting',
            visible: true,
            fetchDataFromServer: getCustomerStatus,
            children: <Setting settingItems={settingData} />
        },
        {
            blockId: BlockIds.LoginInfo,
            featureText: t('LoginInfo'),
            iconClass: 'icon-login',
            pathname: '/loginInfo',
            visible: params.showLoginInfo,
            fetchDataFromServer: fetchLastTxnFromServer,
            children: <LoginInfo lastTxn={lastTxn} />
        },
        {
            blockId: BlockIds.OnlineList,
            featureText: t('OnlineList'),
            iconClass: 'icon-onlinelist',
            pathname: '/onlineList',
            visible: true,
            fetchDataFromServer: fetchOnlineListFromServer,
            children: <OnlineList onlineUsers={onlineUsers} />
        }
    ];

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
                    handleOpenIPInfo
                }}
            >
                <SearchBox />
                <CustomerInfo searchCounter={searchCounter} />
                {mobileView ? (
                    <AccountInfoMobile
                        customerInfo={customer}
                        accordionItems={accordionItems}
                        redirectToBalanceInfoPage={redirectToBalanceInfoPage}
                    />
                ) : (
                    customer && (
                        <AccountInfoDesktop
                            accordionItems={accordionItems}
                            redirectToBalanceInfoPage={redirectToBalanceInfoPage}
                        />
                    )
                )}
            </GlobalContext.Provider>
        </div>
    );
}

export default App;
