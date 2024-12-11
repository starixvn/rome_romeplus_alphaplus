import UserLevelName from '../../common/UserLevelName';
import { OpenSingleModel } from '../open-api';

export const ColumnContentByLevel = ({
    downlines,
    isAffected,
    downlineRoleId,
    redirectViewListDetail
}: {
    downlines: number;
    isAffected: boolean;
    downlineRoleId: number;
    redirectViewListDetail: (isAffected: boolean, downlineRoleId: number) => void;
}) => (
    <span>
        {downlines === 0 ? (
            downlines
        ) : (
            <a href="#" onClick={() => redirectViewListDetail(isAffected, downlineRoleId)}>
                {downlines}
            </a>
        )}
    </span>
);

export const BodyRowContent = ({
    level,
    redirectViewListDetail
}: {
    level: OpenSingleModel;
    redirectViewListDetail: (isAffected: boolean, downlineRoleId: number) => void;
}) => (
    <tr>
        <td>
            <UserLevelName levelId={level.RoleId} />
        </td>
        {[
            {
                downlines: level.AffectedDownlines,
                isAffected: true,
                downlineRoleId: level.RoleId
            },
            {
                downlines: level.UnchangedDownlines,
                isAffected: false,
                downlineRoleId: level.RoleId
            }
        ].map((column, index) => {
            const { downlines, downlineRoleId, isAffected } = column;
            return (
                <td className="text-center" key={index}>
                    <ColumnContentByLevel {...{ downlines, isAffected, downlineRoleId, redirectViewListDetail }} />
                </td>
            );
        })}
    </tr>
);

export const FooterContent = ({
    affectedDownlinesTotal,
    unchangedDownlinesTotal,
    redirectViewListDetail,
    totalText = 'Total',
    noData = 'There is no data.'
}: {
    affectedDownlinesTotal: number;
    unchangedDownlinesTotal: number;
    redirectViewListDetail: (isAffected: boolean, downlineRoleId: number) => void;
    totalText?: string;
    noData?: string;
}) => (
    <tfoot>
        {affectedDownlinesTotal + unchangedDownlinesTotal === 0 ? (
            <tr>
                <td colSpan={3} className="text-center">
                    {noData}
                </td>
            </tr>
        ) : (
            <tr className="tbl-footer">
                <td>{totalText}</td>
                {[
                    {
                        downlines: affectedDownlinesTotal,
                        isAffected: true
                    },
                    {
                        downlines: unchangedDownlinesTotal,
                        isAffected: false
                    }
                ].map((column, index) => {
                    const { downlines, isAffected } = column;
                    return (
                        <td className="text-center" key={index}>
                            <ColumnContentByLevel
                                {...{ downlines, isAffected, redirectViewListDetail, downlineRoleId: 0 }}
                            />
                        </td>
                    );
                })}
            </tr>
        )}
    </tfoot>
);
