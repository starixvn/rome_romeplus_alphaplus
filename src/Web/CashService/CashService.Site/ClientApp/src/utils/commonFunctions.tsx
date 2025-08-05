import { StatusComponent } from '../components/Status';
import Tippy from '@tippyjs/react';

export const displayAmount = (amount: number): string => {
  if (amount >= 1000) {
    const roundedAmount = Math.floor(amount * 100) / 100;
    return roundedAmount.toLocaleString('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
  return (Math.floor(amount * 100) / 100).toFixed(2);
};

export const TransactionType = {
  Deposit: 1,
  Withdrawal: 2,
  Promotion: 11,
};

export const renderBankOptionValue = (bankCode: string, bankAccount: string) => {
  if (!bankAccount) return '';
  return `${bankCode}-${bankAccount}`;
};

export const renderBankAccount = (bankAccount: string, tGlobal?: any) => {
  if (!bankAccount) return <div className="flex items-center max-w-[200px]">-</div>;
  const [bankCode, accountNumber] = bankAccount.split('-');
  if (bankCode && !accountNumber) return <div className="flex items-center">{bankCode}</div>;
  if (!accountNumber && !bankCode) return <div className="inline">-</div>;
  return (
    <>
      <Tippy content={bankCode ? <p>{tGlobal(bankCode)}</p> : <p>{accountNumber}</p>}>
        <div className="inline-flex items-center max-w-[200px]">
          {bankCode && (
            <img
              src={`../assets/bank/${bankCode}.png`}
              alt={bankCode}
              className="w-[16px] h-[16px] mr-[8px] inline-block"
            />
          )}
          <span className="overflow-hidden text-ellipsis whitespace-nowrap">{accountNumber}</span>
        </div>
      </Tippy>
    </>
  );
};

export const mappingStatus = (status: number, textContent?: string) => {
  switch (status) {
    case 0:
      return <StatusComponent type="processing" text={textContent} />;
    case 1:
      return <StatusComponent type="failed" text={textContent} />;
    case 2:
      return <StatusComponent type="maintenance" text={textContent} />;
    case 3:
      return <StatusComponent type="failed" text={textContent} />;
    case 4:
      return <StatusComponent type="completed" text={textContent} />;
    case 5:
      return <StatusComponent type="disabled" text={textContent} />;
    case 6:
      return <StatusComponent type="disabled" text={textContent} />;
    default:
      return <StatusComponent type="failed" text="-" />;
  }
};

export const formatDateTime = (dateTimeStr: string) => {
  if (!dateTimeStr) return '-';

  const date = new Date(dateTimeStr);
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const year = date.getFullYear();
  let hours = date.getHours();
  const minutes = date.getMinutes().toString().padStart(2, '0');
  const seconds = date.getSeconds().toString().padStart(2, '0');
  const ampm = hours >= 12 ? 'PM' : 'AM';
  hours = hours % 12;
  hours = hours ? hours : 12;
  return `${month}/${day}/${year} ${hours}:${minutes}:${seconds} ${ampm}`;
};

export const calculateDateRange = (recentTransactions: string): [string, string] => {
  let today = new Date();
  let startDate = new Date();

  today = convertToGMT4(today);
  startDate = convertToGMT4(startDate);

  switch (recentTransactions) {
    case 'Last30Mins':
      startDate.setMinutes(today.getMinutes() - 30);
      break;
    case 'Last60Mins':
      startDate.setMinutes(today.getMinutes() - 60);
      break;
    default:
      return ['', ''];
  }
  return [startDate.toLocaleString('en-US'), today.toLocaleString('en-US')];
};

export const formatDate = (date: string) => {
  if (!date) return '';
  const dateObj = new Date(date);
  const month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
  const day = dateObj.getDate().toString().padStart(2, '0');
  const year = dateObj.getFullYear();
  return `${month}/${day}/${year}`;
};

export const convertToGMT4 = (date: Date): Date => {
  const newDate = new Date(date);
  const gmtOffset = -4;
  const clientOffset = date.getTimezoneOffset() / 60;
  const offsetDiff = gmtOffset - -clientOffset;

  newDate.setHours(newDate.getHours() + offsetDiff);
  return newDate;
};
