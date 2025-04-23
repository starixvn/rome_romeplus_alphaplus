import classNames from 'classnames';
import React, { useEffect, useState } from 'react';

import AutosizeInput from '../AutosizeInput';
import styles from './ReactPagination.module.scss';

const limit = (value: number, min: number, max: number) => Math.max(min, Math.min(max, value));

export type PageOptions = {
    page: number;
    pageLength?: number;
};

type Props = {
    className?: string;
    pageResource: string;
    ofResource: string;
    page: number;
    pageLength: number;
    pageLengthMenu: number[];
    totalRecords: number;
    onPageChange: (options: PageOptions) => void;
    prevPageRenderer: () => React.ReactNode;
    nextPageRenderer: () => React.ReactNode;
    pageRecordsRenderer: ({ totalRecords, from, to }: { totalRecords: number; from: number; to: number }) => string;
    firstPageRenderer: () => React.ReactNode;
    lastPageRenderer: () => React.ReactNode;
};

export const PaginationButton = ({
    disabled,
    page,
    pageRender,
    changePage,
    setCurrentPage
}: {
    disabled: boolean;
    page: number;
    pageRender: () => React.ReactNode;
    changePage: ({ page }: { page: number }) => void;
    setCurrentPage: (page: number) => void;
}) => {
    return (
        <div
            className={classNames({
                ['disabled']: disabled
            })}
        >
            <div
                className="page-button"
                onClick={() => {
                    setCurrentPage(page);
                    changePage({ page: page });
                }}
            >
                {pageRender()}
            </div>
        </div>
    );
};

export default function ReactPagination(props: Props) {
    const [currentPage, setPage] = useState(props.page);

    useEffect(() => {
        if (props.page !== currentPage) {
            setPage(props.page);
        }
    }, [props.page, currentPage]);

    const changePage = (options: PageOptions) => {
        const totalRecords = props.totalRecords;
        const { page = props.page, pageLength = props.pageLength } = { ...options };

        const pageMin = 1;
        const pageMax = Math.max(Math.ceil(totalRecords / pageLength), 1);

        props.onPageChange({
            page: limit(page, pageMin, pageMax),
            pageLength
        });
    };

    const setCurrentPage = (nextPage: number) => {
        if (nextPage !== currentPage) {
            setPage(nextPage);
        }
    };

    const handleKeypress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key !== 'Enter') {
            return;
        }

        const nextPage = limit(currentPage, 1, totalPages);
        setCurrentPage(nextPage);
        changePage({ page: nextPage });
    };

    const {
        totalRecords = 0,
        firstPageRenderer,
        prevPageRenderer,
        pageResource,
        ofResource,
        nextPageRenderer,
        pageRecordsRenderer,
        lastPageRenderer,
        pageLengthMenu,
        className
    } = props;
    const pageLength = props.pageLength || 50;
    const totalPages = totalRecords > 0 ? Math.ceil(totalRecords / pageLength) : 1;
    const page = limit(props.page, 1, totalPages);
    const from = limit((page - 1) * pageLength + 1, 1, totalRecords);
    const to = limit((page - 1) * pageLength + pageLength, 1, totalRecords);
    const prevPageDisabled = page <= 1;
    const nextPageDisabled = page >= totalPages;

    return (
        <div {...props} className={classNames(className, styles['table-pagination'])}>
            <div className={styles['table-pagination-block']}>
                <div>
                    <select
                        className={styles['pagination-page-list']}
                        value={pageLength}
                        onChange={(event) => {
                            const pageSize = parseFloat(event.currentTarget.value);
                            changePage({
                                page: pageSize !== props.pageLength ? 1 : page,
                                pageLength: pageSize
                            });
                        }}
                    >
                        {pageLengthMenu.map((val) => (
                            <option
                                key={val}
                                value={val}
                                className={classNames({ ['selected']: props.pageLength === val })}
                            >
                                {val}
                            </option>
                        ))}
                    </select>
                </div>
                <div className={styles['pagination']}>
                    <PaginationButton
                        disabled={prevPageDisabled}
                        page={1}
                        pageRender={firstPageRenderer}
                        changePage={changePage}
                        setCurrentPage={setCurrentPage}
                    />
                    <PaginationButton
                        disabled={prevPageDisabled}
                        page={page > 1 ? page - 1 : page}
                        pageRender={prevPageRenderer}
                        changePage={changePage}
                        setCurrentPage={setCurrentPage}
                    />

                    <div className={styles['pagination-input']}>
                        {`${pageResource} `}
                        <AutosizeInput
                            value={currentPage}
                            onChange={(event) => {
                                const nextPage = Number(event.target.value);
                                if (isNaN(nextPage)) {
                                    return;
                                }
                                setPage(limit(nextPage, 1, props.totalRecords));
                                setPage(limit(nextPage, 1, totalPages));
                            }}
                            onKeyPress={handleKeypress}
                        />
                        {` ${ofResource} `}
                        {totalPages}
                    </div>
                    <PaginationButton
                        disabled={nextPageDisabled}
                        page={page < totalPages ? page + 1 : page}
                        pageRender={nextPageRenderer}
                        changePage={changePage}
                        setCurrentPage={setCurrentPage}
                    />
                    <PaginationButton
                        disabled={nextPageDisabled}
                        page={totalPages}
                        pageRender={lastPageRenderer}
                        changePage={changePage}
                        setCurrentPage={setCurrentPage}
                    />
                </div>
                <div className={styles['pagination-records']}>{pageRecordsRenderer({ totalRecords, from, to })}</div>
            </div>
        </div>
    );
}

ReactPagination.defaultProps = {
    pageResource: 'Page',
    ofResource: 'of',
    page: 1,
    pageLength: 50,
    pageLengthMenu: [50, 100, 200, 500],
    totalRecords: 0,
    onPageChange: (options: PageOptions) => {
        // default onChange
    },
    prevPageRenderer: () => '‹',
    nextPageRenderer: () => '›',
    firstPageRenderer: () => '‹‹',
    lastPageRenderer: () => '››',
    pageRecordsRenderer: ({ totalRecords, from, to }: { totalRecords: number; from: number; to: number }) =>
        `Displaying ${from} to ${to} of ${totalRecords}`
};
