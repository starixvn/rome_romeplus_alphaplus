import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import DownlineListResponse from '../../models/DownlineListReponse';
import TotalDownline from '../../models/total-downline';
import NoInformation from '../common/NoInformation';
import DownlineListContent from './DownlineListContent';
import TotalDirectDownlines from './TotalDirectDownlines';
import TotalIndirectDownlines from './TotalIndirectDownlines';

type Props = {
    totalDownlines: TotalDownline[];
    downlineListData: DownlineListResponse | undefined;
    directMembersData: DownlineListResponse | undefined;
    fetchDownlineListFromServer: (
        pageIndex?: number,
        reloadByStatus?: boolean,
        statusId?: number,
        totalPages?: number
    ) => void;
    fetchDirectMembersFromServer: (
        pageIndex?: number,
        reloadByStatus?: boolean,
        statusId?: number,
        totalPages?: number
    ) => void;
};

export default function DownlineList({
    totalDownlines,
    downlineListData,
    directMembersData,
    fetchDownlineListFromServer,
    fetchDirectMembersFromServer
}: Props) {
    const { customer, mobileView } = useGlobalContext();

    if (!mobileView && downlineListData === null) {
        return null;
    }

    const totalDirectDownlines = totalDownlines.find(
        (x) => x.RoleId === (customer?.RoleId ?? 0) - 1 && !x.IsDirectMember
    );
    const totalDirectMembers = totalDownlines.find((x) => x.IsDirectMember);

    if (totalDownlines.length === 0) {
        return <NoInformation />;
    }

    const totalIndirectDownlines = totalDownlines.find((x) => x.RoleId === UserLevel.Member && !x.IsDirectMember);

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
            {downlineListData?.DownlineList && downlineListData.DownlineList.length > 0 && (
                <DownlineListContent
                    downlineList={downlineListData.DownlineList}
                    statusId={downlineListData.StatusId}
                    pageIndex={downlineListData.PageIndex}
                    totalPages={downlineListData.TotalPages}
                    fetchDownlineListFromServer={fetchDownlineListFromServer}
                />
            )}
            {totalDirectMembers && (
                <TotalDirectDownlines
                    {...totalDirectMembers}
                    fetchDownlineListFromServer={fetchDirectMembersFromServer}
                    isDirectMember={true}
                />
            )}
            {directMembersData && directMembersData.DownlineList && directMembersData.DownlineList.length > 0 && (
                <DownlineListContent
                    {...{
                        fetchDownlineListFromServer: fetchDirectMembersFromServer,
                        downlineList: directMembersData.DownlineList,
                        pageIndex: directMembersData.PageIndex,
                        totalPages: directMembersData.TotalPages,
                        statusId: directMembersData.StatusId
                    }}
                />
            )}
        </div>
    );
}
