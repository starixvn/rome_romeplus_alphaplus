import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import { useGlobalContext } from '../../context/global-context';
import NoInformation from '../common/NoInformation';
import { RemarkInfo as RemarkInfoRow } from './RemarkInfoRow';

const ContactInfoRow = ({ title, value }: { title: string; value: string }) => {
    const maxLength = 15;
    const isTruncated = value?.length > maxLength;
    const content = () => <div>{isTruncated ? `${value.substring(0, maxLength)}...` : value}</div>;

    const contentWithTooltip = () => (
        <Tooltip {...{ title: value, position: 'bottom', trigger: 'mouseenter', arrow: true }}>{content()}</Tooltip>
    );

    return (
        <div className="col-xs-12 row-info">
            <div className="col-xs-5">{title}</div>
            <div className="col-xs-7 col-right">{isTruncated ? contentWithTooltip() : content()}</div>
        </div>
    );
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
            value: customer.NickName,
            visible: customer.ShowNickName,
            component: ContactInfoRow
        },
        {
            title: t('Name'),
            value: customer.FullName,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: customer.IsCashMember ? t('Mobile') : t('Phone'),
            value: customer.IsCashMember ? customer.MobilePhone : customer.Phone,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: t('Fax'),
            value: customer.Fax,
            visible: true,
            component: ContactInfoRow
        },
        {
            title: t('Remark'),
            value: customer.Remark,
            visible: true,
            component: RemarkInfoRow
        }
    ];

    return (
        <>
            {contactInfoPropsArray.map(
                ({ title, value, visible, component: Component }, index) =>
                    visible && <Component key={index} title={title} value={value} />
            )}
        </>
    );
}
