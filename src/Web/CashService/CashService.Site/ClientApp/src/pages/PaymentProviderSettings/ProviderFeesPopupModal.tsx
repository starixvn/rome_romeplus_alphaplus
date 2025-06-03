import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { PaymentProviderFeeGroup } from '../../services/interfaces/Payments/PaymentProviderFeeGroup';
import { paymentProviderService } from '../../services/paymentsAPI';
import Loading from '../../components/LoadingComponent';
import PopupModalComponent from '../../components/PopupModalComponent';

interface ProviderFeesPopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const ProviderFeesPopupModal: React.FC<ProviderFeesPopupModalProps> = ({ provider, onClose }) => {
  const { t: tGlobal } = useTranslation('global');
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
              <table className="table-striped">
                <thead>
                  <tr>
                    <th>{tPaymentProviderSettings('transactionType')}</th>
                    <th>{tPaymentProviderSettings('paymentMethod')}</th>
                    <th>{tPaymentProviderSettings('feeRate')}</th>
                  </tr>
                </thead>
                <tbody className='font-[500]'>
                  {feeData.length === 0 && (
                    <tr>
                      <td colSpan={3} className="text-center">{tGlobal('noData')}</td>
                    </tr>
                  )}
                  {feeData.length > 0 && (
                    feeData.map((feeGroup, groupIndex) => (
                      <React.Fragment key={groupIndex}>
                        {feeGroup.fees.map((fee, feeIndex) => (
                          <tr key={`${groupIndex}-${feeIndex}`} className={feeIndex === 0 ? "table-primary" : ""}>
                            {feeIndex === 0 && (
                              <td rowSpan={feeGroup.fees.length}>
                                {tPaymentProviderSettings(`transaction-${feeGroup.transactionType}`)}
                              </td>
                            )}
                            <td>{fee.method}</td>
                            <td className='text-[var(--text-color-primary)]'>
                              {fee.rateText}
                            </td>
                          </tr>
                        ))}
                      </React.Fragment>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}
    </PopupModalComponent>
  );
};

export default ProviderFeesPopupModal; 