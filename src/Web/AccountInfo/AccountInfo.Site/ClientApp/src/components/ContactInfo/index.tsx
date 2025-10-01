import { useTranslation } from 'react-i18next';
import { useGlobalContext } from '../../context/global-context';
import NoInformation from '../common/NoInformation';
import { RemarkInfo as RemarkInfoRow } from './RemarkInfoRow';
import { ContactInfoRow } from './ContactInfoRow';
import { Tooltip } from 'react-tippy';

const FormatValueWithTooltip = (value: string): React.ReactNode | string => {
    const maxLength = 15;
    const isTruncated = value?.length > maxLength;

    if (isTruncated) {
        return (
            <Tooltip {...{ title: value, position: 'bottom', trigger: 'mouseenter', arrow: true }}>
                {`${value.substring(0, maxLength)}...`}
            </Tooltip>
        )
    }

    return value;
};

export default function ContactInfo() {
    const { t } = useTranslation();
    const { customer } = useGlobalContext();

    if (!customer) {
        return <NoInformation />;
    }

    const contactInfoPropsArray = [
        {
            title: t('Nickname'),
            key: 'Nickname',
            value: customer.NickName,
            visible: customer.ShowNickName,
            component: ContactInfoRow
        },
        {
            title: t('Name'),
            key: 'Name',
            value: customer.FullName,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: customer.IsCashMember ? t('Mobile') : t('Phone'),
            key: 'Phone',
            value: customer.IsCashMember ? customer.MobilePhone : customer.Phone,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: t('Fax'),
            key: 'Fax',
            value: customer.Fax,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: t('Remark'),
            key: 'Remark',
            value: customer.Remark,
            visible: true,
            component: RemarkInfoRow
        }
    ];

    return (
        <>
            {contactInfoPropsArray.map(
                ({ title, value, visible, component: Component, key }) =>
                    visible && <Component key={key} title={title} value={FormatValueWithTooltip(value)} />
            )}
        </>
    );
}
