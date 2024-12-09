import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { handleError } from '../../services/axios-helper';
import UnsuspendAPI from '../../services/UnsuspendAPI';
import UserLevelName from '../common/UserLevelName';
import ReactPagination, { PageOptions } from '../ReactPagination';

type Props = {
    isAffected: boolean;
    downlineRoleId: number;
    searchAccountId: number;
    searchAccountName: string;
    redirectMainPage: () => void;
};

export default function UnsuspendViewList({
    isAffected,
    downlineRoleId,
    searchAccountId,
    searchAccountName,
    redirectMainPage
}: Props) {
    const defaultPageLength = 50;
    const { t } = useTranslation();
    const [activePage, setActivePage] = useState(1);
    const [pageLength, setPageLength] = useState(50);
    const [viewModel, setViewModel] = useState<any>(null);

    const totalRecords = viewModel?.Data?.TotalRows,
        showPaging = totalRecords > defaultPageLength;
    let counter = pageLength * (activePage - 1);

    const fetchCustomerListFromServer = ({ page, pageLength }: PageOptions) => {
        $.blockUI?.();
        setActivePage(page);
        setPageLength(pageLength || defaultPageLength);

        UnsuspendAPI.fetchCustomerList(
            searchAccountId,
            searchAccountName,
            pageLength || defaultPageLength,
            page - 1,
            isAffected,
            downlineRoleId
        )
            .then((responeData) => {
                setViewModel(responeData);

                $.unblockUI?.();
            })
            .catch(handleError);
    };

    const Back = (
        <div>
            <a className="btn-cancel" href="#" onClick={redirectMainPage}>
                {t('Back')}
            </a>
        </div>
    );

    if (viewModel === null) {
        return null;
    }

    return (
        <div>
            {showPaging && Back}
            <table className="tblRpt tblRpt-bordered width-100per">
                <thead>
                    <tr>
                        <th>{t('NumberColumn')}</th>
                        <th>{t('Level')}</th>
                        <th>{t('Username')}</th>
                    </tr>
                </thead>
                <tbody>
                    {viewModel.Data.Levels.map((level: any) => {
                        const length = level.DownlineUsernames.length;
                        return level.DownlineUsernames.map((username: string, index: number) => {
                            counter++;
                            return (
                                <tr key={counter}>
                                    <td>{counter}</td>
                                    {index === 0 && (
                                        <td rowSpan={length}>{<UserLevelName levelId={level.RoleId} />}</td>
                                    )}
                                    <td>{username}</td>
                                </tr>
                            );
                        });
                    })}
                </tbody>
            </table>
            {showPaging && (
                <ReactPagination
                    pageResource={t('Page')}
                    ofResource={t('of')}
                    page={activePage}
                    pageLength={pageLength}
                    totalRecords={totalRecords}
                    onPageChange={fetchCustomerListFromServer}
                    prevPageRenderer={() => <span className="icon-chevron-left" />}
                    nextPageRenderer={() => <span className="icon-chevron-right" />}
                    firstPageRenderer={() => <span className="icon-skip-previous" />}
                    lastPageRenderer={() => <span className="icon-skip-next" />}
                    pageRecordsRenderer={({ totalRecords, from, to }) =>
                        `${t('Displaying')} ${from} ${t('to')} ${to} ${t('of')} ${totalRecords}`
                    }
                />
            )}
            {Back}
        </div>
    );
}
