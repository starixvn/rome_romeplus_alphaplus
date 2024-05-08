import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { BlockIds, UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import SettingItem from '../../models/setting-item';
import StatusItem from '../../models/status-item';
import SettingsService from '../../services/settings-service';
import AccordionItem from '../common/AccordionItem';
import NoInformation from '../common/NoInformation';
import Settings from './Settings';
import Status from './Status';

export default function StatusAndSetting() {
    const { t } = useTranslation();
    const { mobileView, expandedBlockId, handleBlockClick, customer } = useGlobalContext();
    const [statusData, setStatusData] = useState<StatusItem[] | undefined>(undefined);
    const [settingData, setSettingData] = useState<SettingItem[] | undefined>(undefined);

    useEffect(() => {
        setStatusData(undefined);
        setSettingData(undefined);
    }, [customer?.CustId]);

    const renderStatus = () => {
        if (!statusData) {
            return mobileView ? <NoInformation /> : null;
        }

        return <Status statusItems={statusData} />;
    };

    const renderSetting = () => {
        if (!settingData) {
            return mobileView ? <NoInformation /> : null;
        }

        return <Settings settingItems={settingData} />;
    };

    const handleClick = (blockId: BlockIds) => {
        handleBlockClick?.(blockId);

        if ((!statusData || !settingData) && customer) {
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

            SettingsService.GetCustomerAllStatus(CustId, UserName, RoleId, uplineId, uplineName).then(
                (responseData) => {
                    setStatusData(responseData.Status);
                    setSettingData(responseData.Settings);
                }
            );
        }
    };

    return (
        <>
            <AccordionItem
                expanded={expandedBlockId === BlockIds.Status}
                iconClass="icon-status"
                featureText={t('Status')}
                onClick={() => handleClick(BlockIds.Status)}
            >
                {renderStatus()}
            </AccordionItem>
            <AccordionItem
                expanded={expandedBlockId === BlockIds.Setting}
                iconClass="icon-setting"
                featureText={t('Setting')}
                onClick={() => handleClick(BlockIds.Setting)}
            >
                {renderSetting()}
            </AccordionItem>
        </>
    );
}
