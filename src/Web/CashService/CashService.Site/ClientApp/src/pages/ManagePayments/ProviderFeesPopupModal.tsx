import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import { PaymentProviderFeeGroup } from '../../services/interfaces/PaymentProviderFeeGroup';
import { paymentProviderService } from '../../services/paymentsAPI';
import styles from './PaymentGateway.module.scss';
import { IconClose } from '../../components/Icons';

interface ProviderFeesPopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const ProviderFeesPopupModal: React.FC<ProviderFeesPopupModalProps> = ({ provider, onClose }) => {
  const { t: tGlobal } = useTranslation();
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState(true);
  const [feeData, setFeeData] = React.useState<PaymentProviderFeeGroup[]>([]);

  React.useEffect(() => {
    const loadFeeData = async () => {
      try {
        setIsLoading(true);
        const fees = await paymentProviderService.getProviderFees();
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
    <div className="bg-[#00000066] fixed top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] bg-[#ffffff] max-w-[600px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-center text-[#163f78] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{tManagePayments('feeDetails')} - {provider.name}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <div className="modal-body">
          {isLoading ? (
            <div className="text-center">
              <div className="spinner-border" role="status">
                <span className="visually-hidden">{tGlobal('loading')}</span>
              </div>
            </div>
          ) : (
            <>
              <div className="text-[12px] leading-[14px]">
                <div>
                  <label className="font-semibold">{provider.name} {tManagePayments('fees')}</label>
                </div>
                <div>
                  <label className="form-label">{tManagePayments('howItWorks')}</label>
                </div>
                <p>{tManagePayments('feesInfo1')}</p>
          
                <ul className='list-disc pl-[24px]'>
                  <li>{tManagePayments('feesFormula')}</li>
                </ul>
                <p className='mb-[16px]'>{tManagePayments('feesInfo2')}</p>
              </div>
              
              <div className="form-group mb-3">
                <table className={styles['table-striped']}>
                  <thead>
                    <tr>
                      <th>{tManagePayments('transactionType')}</th>
                      <th>{tManagePayments('paymentMethod')}</th>
                      <th>{tManagePayments('feeRate')}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {feeData.length > 0 ? (
                      feeData.map((feeGroup, groupIndex) => (
                        <React.Fragment key={groupIndex}>
                          {feeGroup.fees.map((fee, feeIndex) => (
                            <tr key={`${groupIndex}-${feeIndex}`} className={feeIndex === 0 ? "table-primary" : ""}>
                              {feeIndex === 0 && (
                                <td rowSpan={feeGroup.fees.length}>
                                  {feeGroup.transactionType}
                                </td>
                              )}
                              <td>{fee.method}</td>
                              <td>{fee.rate.toFixed(2)}%</td>
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
          <button type="button" className="w-[140px] capitalize font-bold h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" onClick={onClose}>
            {tGlobal('close')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProviderFeesPopupModal; 