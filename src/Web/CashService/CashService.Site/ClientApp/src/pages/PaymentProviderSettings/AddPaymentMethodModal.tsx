import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentMethod } from '../../services/interfaces/PaymentMethod';
import { IconCheck, IconError, IconClose } from '../../components/Icons';
interface AddPaymentMethodModalProps {
  onClose: () => void;
  onSave: () => void;
  providerId: number;
}

const AddPaymentMethodModal: React.FC<AddPaymentMethodModalProps> = ({ onClose, onSave, providerId }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');
  
  // Form state
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');
  const [agreeToTerms, setAgreeToTerms] = React.useState(false);
  const [paymentMethods, setPaymentMethods] = React.useState<PaymentMethod[]>([]);
  const [allMethodsInactive, setAllMethodsInactive] = React.useState<boolean>(false);
  const [apiKey, setApiKey] = React.useState('');
  const [merchantID, setMerchantID] = React.useState('');
  const [appSecret, setAppSecret] = React.useState('');

  const inactiveStatus = 0;
  const activeStatus = 1;
  
  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      try {
        if (selectedTransactionType) {
          const methods = await paymentProviderService.getPaymentMethods(providerId, parseInt(selectedTransactionType));
          setPaymentMethods(methods);
          // Check if all methods are inactive using STATUS_OPTIONS constant
          setAllMethodsInactive(methods.length > 0 && methods.every(method => method.status === inactiveStatus));
        } else {
          setPaymentMethods([]);
          setAllMethodsInactive(false);
        }
      } catch (err) {
        console.error('Failed to load payment methods:', err);
        setError(tGlobal('errorOccurred'));
      }
    };

    loadPaymentMethods();
  }, [selectedTransactionType]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    if (!apiKey || !merchantID || !appSecret) {
      setError(tGlobal('pleaseFillAllFields'));
      setIsLoading(false);
      return;
    }

    try {
      const selectedMethod = paymentMethods.find(method => method.id === parseInt(selectedPaymentMethod));
      const newSetting = {
        id: 0, // New payment method, ID will be assigned by the server
        name: selectedMethod?.name || '',
        providerId,
        method: parseInt(selectedPaymentMethod),
        transactionType: parseInt(selectedTransactionType),
        status: activeStatus,
        custId: 0,
        apiMethodKey: apiKey,
        merchantID: merchantID,
        appSecret: appSecret
      };

      const response = await paymentProviderService.addPaymentMethod(newSetting);
      
      if (response.isSuccessful) {
        setSuccessMessage(tGlobal('updateSuccess'));
        setTimeout(() => {
          onSave();
          onClose();
        }, 3000);
      } else {
        setError(response.message || tGlobal('updateFailed'));
      }
    } catch (err) {
      setError(tGlobal('errorOccurred'));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] bg-[#ffffff] w-full max-w-[560px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h3 className="text-[16px] text-[#163f78] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{tManagePayments('setUpPaymentMethods')}</h3>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            <div>
              <h4 className="text-[14px] pt-[16px] text-[#163f78] font-semibold pb-[4px]">{tManagePayments('addPaymentMethodStep1')}</h4>
              <div className='flex gap-[10px]'>
              <div className="w-[48%]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('transactionType')}<span className='text-[#E25757]'>*</span></label>
                <select 
                  className="h-[35px] w-full border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer" 
                  value={selectedTransactionType}
                  onChange={(e) => setSelectedTransactionType(e.target.value)}
                  required
                >
                  <option value="">{tGlobal('select')}</option>
                  {TRANSACTION_TYPES.filter(type => type.id !== 0).map(type => (
                    <option key={type.id} value={type.id}>{tManagePayments(`transaction-${type.id}`)}</option>
                  ))}
                </select>
              </div>
            
              <div className="w-[48%]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('paymentMethod')}<span className='text-[#E25757]'>*</span></label>
                <select 
                  className="h-[35px] w-full border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer" 
                  value={selectedPaymentMethod}
                  onChange={(e) => setSelectedPaymentMethod(e.target.value)}
                  required
                >
                  <option value="">{tGlobal('select')}</option>
                  {paymentMethods.filter(p => p.id !== 0).map(method => (
                    <option 
                      key={method.id} 
                      value={method.id}
                      disabled={method.status === inactiveStatus}
                    >
                      {tManagePayments(`payment-method-${method.id}`)}{method.status === inactiveStatus ? ` (${tGlobal('underMaintenance')})` : ''}
                    </option>
                  ))}
                </select>
              </div>
              </div>
            </div>
            <div>
              <hr className='mt-[16px] border-[#0000001F]'  />
              <h4 className="text-[14px] pt-[16px] text-[#163f78] font-semibold pb-[4px]">{tManagePayments('addPaymentMethodStep2')}</h4>
              <p className='mt-[8px]'>{tManagePayments('clickLinkOrScanQRCode')}</p>
              <a href={tManagePayments('addPaymentMethodStep2_Link')} target="_blank" rel="noopener noreferrer">{tManagePayments('addPaymentMethodStep2_Link')}</a>
              <div style={{ marginTop: '16px', textAlign: 'center' }}>
                <img src="../assets/qrcode.png" alt="QR Code" style={{ width: '150px', height: '150px', display: 'inline-block' }} />
              </div>
            </div>
            <div>
            <hr className='mt-[16px] mb-[16px] border-[#0000001F]'  />
              <label className="text-[14px] pt-[16px] text-[#163f78] font-semibold pb-[4px]">{tManagePayments('addPaymentMethodStep3')}</label>
              <div className='flex flex-wrap gap-[10px]'>
              <div className="w-[48%]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('apiKey')}<span className='text-[#E25757]'>*</span></label>
                <input
                  type="text"
                  className="h-[35px] w-full border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                  id="apiKey"
                  value={apiKey}
                  onChange={e => setApiKey(e.target.value)}
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('merchantID')}<span className='text-[#E25757]'>*</span></label>
                <input
                  type="text"
                  className="h-[35px] w-full border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                  id="merchantID"
                  value={merchantID}
                  onChange={e => setMerchantID(e.target.value)}
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('appSecret')}<span className='text-[#E25757]'>*</span></label>
                <input
                  type="text"
                  className="h-[35px] w-full border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                  id="appSecret"
                  value={appSecret}
                  onChange={e => setAppSecret(e.target.value)}
                  required
                />
              </div>
              </div>
            </div>
           
            {error && <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#FFF2F2] border-[1px] border-[#FE0000] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconError className='w-[20px] h-[20px] fill-[#fe0000] absolute top-[8px] left-[10px]' />{error}</div>}
            {successMessage && <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#E2FFF2] border-[1px] border-[#5cb774] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconCheck className='w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]' />{successMessage}</div>}
          </div>
          <div className="flex flex-wrap mt-[16px] mb-[16px]">
            <label className="w-full flex items-center justify-center cursor-pointer text-[#333] text-[12px] leading-[14px]">
              <input
                type="checkbox"
                className="w-[18px] h-[18px]"
                id="agreeToTerms"
                checked={agreeToTerms}
                onChange={(e) => setAgreeToTerms(e.target.checked)}
                required
              />
              <span className="flex-1 pl-[6px]">
                {tManagePayments('agreeTermsAndConditions')}
              </span>
            </label>
            </div>
            <div className='flex justify-center'>
            <button type="button" className="w-[140px] h-[30px] rounded-[20px] text-[#163f78] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer " onClick={onClose}>
              {tManagePayments('setUpLater')}
            </button>
            <button 
              type="submit" 
              className="w-[140px] capitalize font-bold h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] disabled:bg-[#375A8A]"
              disabled={isLoading || allMethodsInactive}
            >
              {isLoading ? tGlobal('loading') : tGlobal('save')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPaymentMethodModal; 