import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import DownlineListResponse from '../../models/DownlineListReponse';
import DownlineListContent from './DownlineListContent';
import TotalDirectDownlines from './TotalDirectDownlines';
import TotalIndirectDownlines from './TotalIndirectDownlines';

type Props = {
    downlineListData: DownlineListResponse | undefined;
    fetchDownlineListFromServer: (
        pageIndex?: number,
        reloadByStatus?: boolean,
        statusId?: number,
        totalPages?: number
    ) => void;
};

export default function DownlineList({ downlineListData, fetchDownlineListFromServer }: Props) {
    const { customer } = useGlobalContext();

    const totalIndirectDownlines = downlineListData?.TotalDownlines[1];
    const totalDirectDownlines = downlineListData?.TotalDownlines[0];

    return (
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
    );
}
