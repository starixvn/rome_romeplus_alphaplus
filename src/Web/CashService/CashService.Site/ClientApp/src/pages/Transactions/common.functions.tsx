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