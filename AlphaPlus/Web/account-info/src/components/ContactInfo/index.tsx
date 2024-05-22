import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import { BlockIds } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import AccordionItem from '../common/AccordionItem';
import NoInformation from '../common/NoInformation';

const ContactInfoRow = ({ title, value }: { title: string; value: string }) => {
    const maxLength = 15;
    const isTruncated = value.length > maxLength;
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
    const { customer, expandedBlockId, handleBlockClick } = useGlobalContext();

    if (!customer) {
        return <NoInformation />;
    }

    const contactInfoPropsArray = [
        {
            title: t('Nickname'),
            value: customer.NickName,
            visible: customer.ShowNickName
        },
        {
            title: t('Name'),
            value: customer.FullName,
            visible: true
        },
        {
            title: t('Phone'),
            value: customer.Phone,
            visible: true
        },
        {
            title: t('Fax'),
            value: customer.Fax,
            visible: true
        }
    ];

    return (
        <AccordionItem
            expanded={expandedBlockId === BlockIds.ContactInfo}
            iconClass="icon-contact-info"
            featureText={t('ContactInfo')}
            onClick={() => handleBlockClick?.(BlockIds.ContactInfo)}
        >
            {contactInfoPropsArray.map(
                ({ title, value, visible }, index) =>
                    visible && <ContactInfoRow key={index} title={title} value={value} />
            )}
        </AccordionItem>
    );
}
