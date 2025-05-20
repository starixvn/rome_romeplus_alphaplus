import React from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { displayAmount, renderBankAccount } from '../../../../common/commonFunctions';
import { ButtonFullComponent } from '../../../../components/ButtonComponents';
import NoRecordFound from '../../../../components/Table/NoRecordFound';

interface DataGridProps {
  deposits: TransactionItem[];
  onProcessClick: (deposit: TransactionItem) => void;
}

const DataGrid: React.FC<DataGridProps> = ({ deposits, onProcessClick }) => {
  const { t } = useTranslation('pendingDeposit');

  const handleProcessClick = (deposit: TransactionItem) => {
    onProcessClick(deposit);
  };

  return (
    <>
      <div className="table-fixed-header table-fixed-body bg-[#ffffff] h-full">
        <table className="max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          <thead className="z-11!">
            <tr>
              <th scope="col" className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-11">
                #
              </th>
              <th
                scope="col"
                className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-11 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
              >
                {t('dataGrid.username')}
              </th>
              <th scope="col">
                <div>{t('dataGrid.firstName')}</div> <div>{t('dataGrid.lastName')}</div>
              </th>
              <th scope="col">{t('dataGrid.amount')}</th>
              <th scope="col">{t('dataGrid.currency')}</th>
              <th scope="col">{t('dataGrid.from')}</th>
              <th scope="col">{t('dataGrid.to')}</th>
              <th scope="col">{t('dataGrid.referenceNo')}</th>
              <th scope="col">{t('dataGrid.dateAndTime')}</th>
              <th scope="col">{t('dataGrid.actions')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {deposits.length > 0 ? (
              deposits.map((deposit, index) => (
                <tr key={deposit.transId}>
                  <td className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-10">
                    {index + 1}
                  </td>
                  <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                    {deposit.username}
                  </td>
                  <td>
                    <div>{deposit.firstName}</div>
                    <div>{deposit.lastName}</div>
                  </td>
                  <td>{displayAmount(deposit.amount)}</td>
                  <td>{deposit.currency}</td>
                  <td>{renderBankAccount(deposit.paymentFrom)}</td>
                  <td>{renderBankAccount(deposit.paymentTo)}</td>
                  <td>{deposit.transId}</td>
                  <td>{deposit.createdTime}</td>
                  <td>
                   
                    <ButtonFullComponent
                      text={t('dataGrid.process')}
                      onClick={() => handleProcessClick(deposit)}
                      className=""
                    />
                  </td>
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={11} message={t('datagrid.noDepositsFound')} />
            )}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default DataGrid;
