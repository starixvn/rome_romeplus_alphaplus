import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { BlockIds } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import DownlineListResponse from '../../models/DownlineListReponse';
import SettingsService from '../../services/settings-service';
import AccordionItem from '../common/AccordionItem';
import { BlockProps } from '../common/type';
import TotalDownlines from './TotalDownlines';

export default function DownlineList({ blockId, onClick }: BlockProps) {
    const { t } = useTranslation();
    const { expandedBlockId, customer } = useGlobalContext();
    const [downlineListData, setDownlineListData] = useState<DownlineListResponse | undefined>(undefined);

    useEffect(() => {
        setDownlineListData(undefined);
    }, [customer?.CustId]);

    const handleClick = () => {
        onClick(blockId);

        if (!downlineListData && customer) {
            SettingsService.GetDownlineList(customer?.CustId, customer?.RoleId).then((data) =>
                setDownlineListData(data)
            );
        }
    };

    return (
        <AccordionItem
            expanded={expandedBlockId === BlockIds.DownlineList}
            iconClass="icon-downline"
            featureText={t('DownlineList')}
            onClick={handleClick}
        >
            {downlineListData && <TotalDownlines totalDownlines={downlineListData.TotalDownlines} />}
        </AccordionItem>
    );
}
