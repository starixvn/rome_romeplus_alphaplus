import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { PaymentProviderFeeGroup } from '../../services/interfaces/Payments/PaymentProviderFeeGroup';
import { paymentProviderService } from '../../services/paymentsAPI';
import Loading from '../../components/LoadingComponent';
import PopupModalComponent from '../../components/PopupModalComponent';
import { TableComponent, TableRowProps } from '../../components/TableComponents';

interface ProviderFeesPopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const ProviderFeesPopupModal: React.FC<ProviderFeesPopupModalProps> = ({ provider, onClose }) => {
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
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
    <PopupModalComponent title={tPaymentProviderSettings('feeDetails')} onClose={onClose}>
        {isLoading ? (<Loading />) : (
          <>
            <div className="mb-[16px]">           
              <p className="font-semibold">{provider.name} {tPaymentProviderSettings('fees')}</p>          
              <p>{tPaymentProviderSettings('howItWorks')}</p>           
              <p>{tPaymentProviderSettings('feesInfo1')}</p>          
              <ul className='list-disc pl-[24px] mb-[16px]'>
                <li>{tPaymentProviderSettings('feesFormula')}</li>
              </ul>
              <p>{tPaymentProviderSettings('feesInfo2')}</p>
            </div>            
            <div className="mb-[16px]">
              <TableComponent
                className='table-striped'
                numOfColumns={3}
                resourcesTranslation={tPaymentProviderSettings}
                headerRows={[['transactionType', 'paymentMethod', 'feeRate']]}
                bodyRows={feeData.flatMap((feeGroup, groupIndex) => {
                  var rows: TableRowProps[] = [];

                  feeGroup.fees.forEach((fee, feeIndex) => {
                    if(feeIndex === 0) {
                      rows.push({
                        key: `${groupIndex}-${feeIndex}`,
                        className: "table-primary",
                        columns: [
                          (<td rowSpan={feeGroup.fees.length}>{tPaymentProviderSettings(`transaction-${feeGroup.transactionType}`)}</td>),
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