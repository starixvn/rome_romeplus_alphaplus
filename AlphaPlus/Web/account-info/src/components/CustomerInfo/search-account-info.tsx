import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import { useGlobalContext } from '../../context/global-context';
import Customer from '../../models/customer';
import CustomAnchorTag from '../common/CustomAnchorTag';
import styles from './CustomerInfo.module.scss';

const ClosedReason = {
    ByAdmin: 1,
    ByUpline: 2,
    ByArchive: 4
};

const LockedAccess = {
    FalseOTPSC: 1,
    FalsePassword: 2
};

export default function SearchAccountInfo() {
    const { t } = useTranslation();
    const [css, setCSS] = useState('');
    const [tooltip, setTooltip] = useState('');
    const [isTruncate, setIsTruncate] = useState(false);

    const { customer } = useGlobalContext();
    const username = customer?.UserName ?? '';
    const maxLength = 15;

    useEffect(() => {
        if (username.length > maxLength) {
            setIsTruncate(true);
            setTooltip(username);
        }
    }, [username]);

    useEffect(() => {
        const getStatusInfo = (customerInfo: Customer) => {
            const statusInfo = {
                Closed: [
                    {
                        Id: ClosedReason.ByAdmin,
                        Css: 'closed-byAdmin',
                        Tootltip: t('ClosedByAdmin')
                    },
                    {
                        Id: ClosedReason.ByUpline,
                        Css: 'closed-byUpline',
                        Tootltip: t('ClosedByUpline')
                    },
                    {
                        Id: ClosedReason.ByArchive,
                        Css: 'closed-byArchive',
                        Tootltip: t('ClosedByArchive')
                    }
                ],
                Suspended: {
                    Css: 'suspended',
                    Tootltip: t('ThisAccountWasSuspended')
                },
                Disabled: {
                    Css: 'disabled',
                    Tootltip: t('ThisAccountWasDisabled')
                },
                Enabled: {
                    Css: 'enabled',
                    Tootltip: ''
                },
                Locked: [
                    {
                        Id: LockedAccess.FalseOTPSC,
                        Css: 'locked-falseOTPSC',
                        Tootltip: t('LockedByFalseOTPSC')
                    },
                    {
                        Id: LockedAccess.FalsePassword,
                        Css: 'locked-falsePassword',
                        Tootltip: t('LockedByFalsePassword')
                    }
                ]
            };

            if (!customerInfo.Enabled) {
                return statusInfo.Disabled;
            } else if (customerInfo.Closed) {
                return statusInfo.Closed.find((closedReason) => customerInfo.ClosedReason === closedReason.Id);
            } else if (customerInfo.Suspended) {
                return statusInfo.Suspended;
            } else if (customerInfo.Locked) {
                return statusInfo.Locked.find((locked) => customerInfo.Locked === locked.Id);
            }
            return statusInfo.Enabled;
        };
        if (customer) {
            const statusInfo = getStatusInfo(customer);
            setCSS(statusInfo?.Css ?? '');
            setTooltip(statusInfo?.Tootltip ?? '');
        }
    }, [customer, t]);

    const renderSpanContent = () => {
        return (
            <span className={styles[css]} id="search-account-id" data-search-account-username={username}>
                {isTruncate ? `${username.substring(0, maxLength)}...` : username}
            </span>
        );
    };

    const renderContent = () => {
        if (customer?.CanViewLog) {
            return (
                <CustomAnchorTag classNames="pointer" title={t('viewLog')} href={customer.ViewLogLink}>
                    {renderSpanContent()}
                    <span className="icon-viewlog" />
                </CustomAnchorTag>
            );
        }

        return renderSpanContent();
    };

    const renderContentWithTooltip = () => (
        <Tooltip {...{ title: tooltip, position: 'bottom', trigger: 'mouseenter', arrow: true }}>
            {renderContent()}
        </Tooltip>
    );

    return (
        <div>
            {tooltip === '' ? renderContent() : renderContentWithTooltip()}
            {customer?.IsShowCreateMemberByQRCode && (
                <CustomAnchorTag
                    classNames="pointer"
                    href={customer.CreateMemberByQRCodeUrl}
                    title={t('createMemberWithQRCode')}
                >
                    <span className={styles['icon-clone-account']} />
                </CustomAnchorTag>
            )}
        </div>
    );
}
