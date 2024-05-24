import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { BlockIds, UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import DownlineListResponse from '../../models/DownlineListReponse';
import SettingsService, { handleError } from '../../services/settings-service';
import AccordionItem from '../common/AccordionItem';
import DownlineListContent from './DownlineListContent';
import TotalDirectDownlines from './TotalDirectDownlines';
import TotalIndirectDownlines from './TotalIndirectDownlines';

export default function DownlineList() {
    const { t } = useTranslation();
    const { expandedBlockId, customer, handleBlockClick } = useGlobalContext();
    const [downlineListData, setDownlineListData] = useState<DownlineListResponse | undefined>(undefined);

    useEffect(() => {
        setDownlineListData(undefined);
    }, [customer?.CustId]);

    const fetchDownlineListFromServer = (pageIndex = 1, reloadByStatus = false, statusId = 1, totalPages = 0) => {
        if (customer?.CustId && (downlineListData === null || pageIndex > 1 || reloadByStatus)) {
            $.blockUI?.();
            SettingsService.GetDownlineList(customer?.CustId, customer?.RoleId, pageIndex, statusId, totalPages)
                .then((data) => {
                    data.PageIndex = pageIndex;
                    data.StatusId = statusId;

                    // data.DownlineList =
                    //     downlineListData === null || reloadByStatus
                    //         ? data.DownlineList
                    //         : [...downlineListData.DownlineList, ...data.DownlineList];

                    // if (pageIndex > 1) {
                    //     data.TotalDownlines = downlineListData.TotalDownlines;
                    // }

                    setDownlineListData(data);
                })
                .catch(handleError)
                .finally(() => $.unblockUI?.());
        }
    };

    const handleClick = () => {
        handleBlockClick?.(BlockIds.DownlineList);
        if (!downlineListData && customer) {
            SettingsService.GetDownlineList(customer?.CustId, customer?.RoleId).then((data) =>
                setDownlineListData(data)
            );
        }
    };

    const totalIndirectDownlines = downlineListData?.TotalDownlines[1];
    const totalDirectDownlines = downlineListData?.TotalDownlines[0];

    return (
        <AccordionItem
            expanded={expandedBlockId === BlockIds.DownlineList}
            iconClass="icon-downline"
            featureText={t('DownlineList')}
            onClick={handleClick}
        >
            <div id="total-downlines">
                {customer?.RoleId === UserLevel.Master && totalIndirectDownlines && (
                    <TotalIndirectDownlines {...totalIndirectDownlines} />
                )}
                {totalDirectDownlines && (
                    <TotalDirectDownlines
                        {...totalDirectDownlines}
                        fetchDownlineListFromServer={fetchDownlineListFromServer}
                    />
                )}
                {downlineListData && (
                    <DownlineListContent
                        downlineList={downlineListData.DownlineList}
                        statusId={downlineListData.StatusId}
                        pageIndex={downlineListData.PageIndex}
                        totalPages={downlineListData.TotalPages}
                        fetchDownlineListFromServer={fetchDownlineListFromServer}
                    />
                )}
            </div>
        </AccordionItem>
    );
}
