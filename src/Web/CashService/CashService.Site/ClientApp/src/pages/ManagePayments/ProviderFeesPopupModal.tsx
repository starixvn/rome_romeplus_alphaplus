import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider, PaymentProviderFeeGroup } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import Loading from '../../components/LoadingComponent';
import PopupModalComponent from '../../components/PopupModalComponent';
import { TableRowProps } from '../../components/TableComponents';
import { TableComponent } from '../../components/TableComponents';

interface ProviderFeesPopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const ProviderFeesPopupModal: React.FC<ProviderFeesPopupModalProps> = ({  provider, onClose }) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState(true);
  const [feeData, setFeeData] = React.useState<PaymentProviderFeeGroup[]>([]);

  React.useEffect(() => {
    const loadFeeData = async () => {
      setIsLoading(true);
      const response = await paymentProviderService.getProviderFees(provider.id);
      if(response.error) {
        setIsLoading(false);
        return;
      }

      const fees = response.data || [];
      setFeeData(fees);
      setIsLoading(false);
    };

    loadFeeData();
  }, [provider.id]);

  return (
    <PopupModalComponent title={`${tManagePayments('feeDetails')} - ${provider.name}`} onClose={onClose}>
      {isLoading ? (<Loading />) : (
          <>                         
            <p className="font-semibold text-[var(--text-label-color)]">{provider.name} {tManagePayments('fees')}</p>                
            <p>{tManagePayments('howItWorks')}</p>              
            <p>{tManagePayments('feesInfo1')}</p>        
            <ul className='list-disc pl-[24px] mb-[16px]'>
              <li>{tManagePayments('feesFormula')}</li>
            </ul>
            <p className='mb-[16px]'>{tManagePayments('feesInfo2')}</p>
                       
            <div className="form-group mb-3">
              <TableComponent
                className='table-striped'
                numOfColumns={3}
                resourcesTranslation={tManagePayments}
                headerRows={[['transactionType', 'paymentMethod', 'feeRate']]}
                bodyRows={feeData.flatMap((feeGroup, groupIndex) => {
                  var rows: TableRowProps[] = [];

                  feeGroup.fees.forEach((fee, feeIndex) => {
                    if(feeIndex === 0) {
                      rows.push({
                        key: `${groupIndex}-${feeIndex}`,
                        className: "table-primary",
                        columns: [
                          (<td rowSpan={feeGroup.fees.length}>{tManagePayments(`transaction-${feeGroup.transactionType}`)}</td>),
                          (<td>{fee.method}</td>),
                          (<td className='text-[#094980] dark:text-[#459be5]'>{fee.rateText}</td>)
                        ]
                      });
                      
                      return;
                    }

                    rows.push({
                      key: `${groupIndex}-${feeIndex}`,
                      className: "",
                      columns: [
                        (<td>{fee.method}</td>),
                        (<td className='text-[#094980] dark:text-[#459be5]'>{fee.rateText}</td>)
                      ]
                    });
                  });

                  return rows;
                })}
              />
            </div>
          </>
        )}
    </PopupModalComponent>
  );
};

export default ProviderFeesPopupModal; 