import type { ContactInfoRowProps } from './ContactInfoType';

export const ContactInfoRow = ({ title, value, key }: ContactInfoRowProps) => {
    return (
        <div className="col-xs-12 row-info" key={key}>
            <div className="col-xs-5">{title}</div>
            <div className="col-xs-7 col-right">{value}</div>
        </div>
    );
};
