import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider, PaymentProviderFeeGroup } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';
import Loading from '../../components/LoadingComponent';
import { ButtonApply } from '../../components/ButtonComponents';

interface ProviderFeesPopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const ProviderFeesPopupModal: React.FC<ProviderFeesPopupModalProps> = ({ provider, onClose }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState(true);
  const [feeData, setFeeData] = React.useState<PaymentProviderFeeGroup[]>([]);

  React.useEffect(() => {
    const loadFeeData = async () => {
      try {
        setIsLoading(true);
        const fees = await paymentProviderService.getProviderFees(provider.id);
        setFeeData(fees);
      } catch (error) {
        console.error('Error loading fee data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    loadFeeData();
  }, [provider.id]);

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[600px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f] text-[12px] max-[768px]:text-[16px]">
        <div className="relative">
          <h5 className="text-[16px] text-center text-[var(--text-color-primary)] font-bold leading-[21.6px] pb-[4px]">{tManagePayments('feeDetails')} - {provider.name}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <div className="mb-[12px]">
          {isLoading ? (<Loading />) : (
            <>
              <div className="">
                <div>
                  <label className="font-semibold">{provider.name} {tManagePayments('fees')}</label>
                </div>
                <div>
                  <label className="form-label">{tManagePayments('howItWorks')}</label>
                </div>
                <p>{tManagePayments('feesInfo1')}</p>
          
                <ul className='list-disc pl-[24px] mb-[16px]'>
                  <li>{tManagePayments('feesFormula')}</li>
                </ul>
                <p className='mb-[16px]'>{tManagePayments('feesInfo2')}</p>
              </div>
              
              <div className="form-group mb-3">
                <table className="table-striped">
                  <thead>
                    <tr>
                      <th>{tManagePayments('transactionType')}</th>
                      <th>{tManagePayments('paymentMethod')}</th>
                      <th>{tManagePayments('feeRate')}</th>
                    </tr>
                  </thead>
                  <tbody className='font-[500]'>
                    {feeData.length > 0 ? (
                      feeData.map((feeGroup, groupIndex) => (
                        <React.Fragment key={groupIndex}>
                          {feeGroup.fees.map((fee, feeIndex) => (
                            <tr key={`${groupIndex}-${feeIndex}`} className={feeIndex === 0 ? "table-primary" : ""}>
                              {feeIndex === 0 && (
                                <td rowSpan={feeGroup.fees.length}>
                                  {tManagePayments(`transaction-${feeGroup.transactionType}`)}
                                </td>
                              )}
                              <td>{fee.method}</td>
                              <td className='text-[var(--text-color-primary)]'>{fee.rateText}</td>
                            </tr>
                          ))}
                        </React.Fragment>
                      ))
                    ) : (
                      <tr>
                        <td colSpan={3} className="text-center">{tGlobal('noData')}</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </>
          )}
        </div>
        <div className="flex justify-center">    
          <ButtonApply text={tGlobal('close')} onClick={onClose} />       
        </div>
      </div>
    </div>
  );
};

export default ProviderFeesPopupModal; 