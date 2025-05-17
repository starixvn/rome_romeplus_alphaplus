export const displayAmount = (amount: number): string => {
  if (amount >= 1000) {
    return amount.toLocaleString('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
  return amount.toFixed(2);
};

export const TransactionType = {
  Deposit: 1,
  Withdrawal: 2,
};

export const renderBankAccount = (bankAccount: string) => {
  const [bankCode, accountNumber] = bankAccount.split('_');
  return (
    <div className="flex items-center">
      <img
        src={`../assets/bank/${bankCode}.png`}
        alt={bankCode}
        className="w-[16px] h-[16px] mr-[8px] inline-block"
      />
      <span>{accountNumber}</span>
    </div>
  );
};

export const mappingStatus = (status: number) => {
  switch (status) {
    case 0:
      return 'Waiting';
    case 1:
      return 'Cancelled';
    case 2:
      return 'Pending';
    case 3:
      return 'EXPIRED';
    case 4:
      return 'Completed';
    case 5:
      return 'Rejected';
    case 6:
      return 'Provider Rejected';
    default:
      return '';
  }
};
