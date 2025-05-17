export const displayAmount = (amount: number): string => {
  if (amount >= 1000) {
    return amount.toLocaleString('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
  }
  return amount.toFixed(2);
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
