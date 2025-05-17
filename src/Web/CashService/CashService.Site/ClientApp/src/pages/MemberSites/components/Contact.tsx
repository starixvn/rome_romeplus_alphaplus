import { useTranslation } from "react-i18next";
import { Contact as ContactType } from "../../../services/interfaces/MemberSites";

interface ContactProps {
  contact: ContactType;
  handleContactChange: (contact: ContactType) => void;
}

export const Contact = (props: ContactProps) => {
  const { t } = useTranslation('memberSite');
  const { contact, handleContactChange } = props;
  return (<div key={contact.type} className='w-[230px] pt-[4px] max-[768px]:w-[48%]'>
    <label htmlFor={contact.type} className="block pt-[4px] mb-[8px] font-semibold max-[768px]:mb-[8px]">
      {t(`message.${contact.type}`)}
      {contact.isRequired && <span className='text-[#E25757]'>*</span>}
    </label>
    <input
      id={contact.type}
      type="text"
      value={contact.value}
      onChange={(e) => {
        handleContactChange({ type: contact.type, value: e.target.value, isRequired: contact.isRequired });
      }}
      className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--border-control-focus)] max-[768px]:text-[16px] max-[768px]:leading-[16px]"
    />
  </div>)

}
