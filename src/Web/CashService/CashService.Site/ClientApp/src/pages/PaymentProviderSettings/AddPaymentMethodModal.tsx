import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentMethod } from '../../services/interfaces/Payments/PaymentMethod';
import { QRCodeCanvas } from 'qrcode.react';
import { ErrorMessageComponent, SuccessMessageComponent, ValidateMessageComponent } from '../../components/MessageComponents';
import { IconClose } from '../../components/Icons';
import { ButtonApply, ButtonCancel } from '../../components/ButtonComponents';

interface AddPaymentMethodModalProps {
  onClose: () => void;
  onSuccess: () => void;
  providerId: number;
  orgTelegramAssistantUrl: string;
}

interface PaymentMethodFormState {
  selectedPaymentMethod: string;
  selectedTransactionType: string;
  apiKey: string;
  merchantID: string;
  appSecret: string;
  agreeToTerms: boolean;
  telegramAssistantUrl: string;
}

const AddPaymentMethodModal: React.FC<AddPaymentMethodModalProps> = ({ onClose, onSuccess, providerId, orgTelegramAssistantUrl }) => {
 
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');
  const [isBlockAction, setIsBlockAction] = React.useState<boolean>(false);

  // Combined form state
  const [formState, setFormState] = React.useState<PaymentMethodFormState>({
    selectedPaymentMethod: '',
    selectedTransactionType: '',
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: false,
    telegramAssistantUrl: orgTelegramAssistantUrl
  });

  const [paymentMethods, setPaymentMethods] = React.useState<PaymentMethod[]>([]);
  const [allMethodsInactive, setAllMethodsInactive] = React.useState<boolean>(false);
  const [telegramAssistantUrl, setTelegramAssistantUrl] = React.useState<string>(orgTelegramAssistantUrl);
  const [fieldErrors, setFieldErrors] = React.useState({
    selectedTransactionType: '',
    selectedPaymentMethod: '',
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: '',
  });

  const inactiveStatus = 4;
  
  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      try {
        if (formState.selectedTransactionType) {
          const response = await paymentProviderService.getAddPaymentMethodInfo(providerId, parseInt(formState.selectedTransactionType));
          
          setPaymentMethods(response.paymentMethods);
          setTelegramAssistantUrl(response.channelContact.tele || '');

          setAllMethodsInactive(response.paymentMethods.length > 0 && response.paymentMethods.every(method => method.status === inactiveStatus));
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
  }, [formState.selectedTransactionType]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    // Field-level validation
    const errors: typeof fieldErrors = {
      selectedTransactionType: !formState.selectedTransactionType ? tGlobal('pleaseFillField') : '',
      selectedPaymentMethod: !formState.selectedPaymentMethod ? tGlobal('pleaseFillField') : '',
      apiKey: !formState.apiKey ? tGlobal('pleaseFillField') : '',
      merchantID: !formState.merchantID ? tGlobal('pleaseFillField') : '',
      appSecret: !formState.appSecret ? tGlobal('pleaseFillField') : '',
      agreeToTerms: !formState.agreeToTerms ? tGlobal('pleaseAgreeTerms') : '',
    };
    setFieldErrors(errors);
    const hasError = Object.values(errors).some(Boolean);
    if (hasError) {
      setIsLoading(false);
      return;
    }

    try {
      const selectedMethod = paymentMethods.find(method => method.id === parseInt(formState.selectedPaymentMethod)) 
        || {status: 0, name: ''};

      const newSetting = {
        id: 0,
        name: selectedMethod.name || '',
        providerId,
        method: parseInt(formState.selectedPaymentMethod),
        transactionType: parseInt(formState.selectedTransactionType),
        status: 1,
        custId: 0,
        apiMethodKey: formState.apiKey,
        merchantID: formState.merchantID,
        appSecret: formState.appSecret
      };

      const isUpdate = selectedMethod.status == 3;
      const response = isUpdate 
        ? await paymentProviderService.updatePaymentMethod(newSetting) 
        : await paymentProviderService.addPaymentMethod(newSetting);
      
      if (response.isSuccessful) { 
        setIsBlockAction(true);
        setSuccessMessage(isUpdate ? tGlobal('updateSuccess') : tGlobal('addSuccess'));
        setTimeout(() => { onClose(); onSuccess(); }, 3000);
      } else {
        setError(response.message || (isUpdate ? tGlobal('updateFailed') : tGlobal('addFailed')));
      }
    } catch (err) {
      setError(tGlobal('errorOccurred'));
    } finally {
      setIsLoading(false);
    }
  };

  const handleInputChange = (field: keyof PaymentMethodFormState, value: string | boolean) => {
    setFormState(prev => ({ ...prev, [field]: value }));
    setFieldErrors(prev => ({ ...prev, [field]: '' }));
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] max-h-[750px] overflow-y-auto bg-[#ffffff] w-full max-w-[560px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h3 className="text-[16px] max-[768px]:text-[20px] text-[var(--text-color-primary)] font-bold pb-[4px]">{tPaymentProviderSettings('setUpPaymentMethods')}</h3>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <ErrorMessageComponent className='mt-[8px]' message={error} />
        <SuccessMessageComponent className='mt-[8px]' message={successMessage} />
        <form onSubmit={handleSubmit}>
          <div className="modal-body ">
            <div>
              <h4 className="text-[14px] max-[768px]:text-[18px] pt-[8px] text-[var(--text-color-primary)] font-semibold pb-[4px]">{tPaymentProviderSettings('addPaymentMethodStep1')}</h4>
              <div className='flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block  font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('transactionType')}<span className='text-[#E25757]'>*</span></label>
                <select 
                  className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedTransactionType ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  value={formState.selectedTransactionType}
                  onChange={(e) => {
                    handleInputChange('selectedTransactionType', e.target.value);
                  }}
                >
                  <option value="">{tGlobal('select')}</option>
                  {TRANSACTION_TYPES.filter(type => type.id !== 0).map(type => (
                    <option key={type.id} value={type.id}>{tPaymentProviderSettings(`transaction-${type.id}`)}</option>
                  ))}
                </select>
                <ValidateMessageComponent message={fieldErrors.selectedTransactionType}  />
              </div>
            
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('paymentMethod')}<span className='text-[#E25757]'>*</span></label>
                <select 
                  className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedPaymentMethod ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  value={formState.selectedPaymentMethod}
                  onChange={(e) => {
                    handleInputChange('selectedPaymentMethod', e.target.value);
                  }}
                >
                  <option value="">{tGlobal('select')}</option>
                  {paymentMethods.filter(p => p.id !== 0).map(method => (
                    <option 
                      key={method.id} 
                      value={method.id}
                      disabled={method.status === inactiveStatus}
                    >
                      {method.name}{method.status === inactiveStatus ? ` (${tGlobal('underMaintenance')})` : ''}
                    </option>
                  ))}
                </select>
                <ValidateMessageComponent message={fieldErrors.selectedPaymentMethod}  />
              </div>
              </div>
            </div>
            <div>
              <hr className='mt-[16px] border-[#0000001F]'  />
              <h4 className="text-[14px] max-[768px]:text-[18px] pt-[16px] text-[var(--text-color-primary)] font-semibold pb-[4px]">{tPaymentProviderSettings('addPaymentMethodStep2')}</h4>
              <p className='mt-[8px] '>{tPaymentProviderSettings('clickLinkOrScanQRCode')}</p>
              <a href={telegramAssistantUrl} className=' text-[var(--text-color-primary)] font-semibold' target="_blank" rel="noopener noreferrer">{telegramAssistantUrl}</a>
              <div className='mt-[16px] flex justify-center'>
                {telegramAssistantUrl && <QRCodeCanvas value={telegramAssistantUrl} size={128} />}
              </div>
            </div>
            <div>
            <hr className='mt-[16px] mb-[16px] border-[#0000001F]'  />
              <label className="text-[14px] max-[768px]:text-[18px] pt-[16px] text-[var(--text-color-primary)] font-semibold pb-[4px]">{tPaymentProviderSettings('addPaymentMethodStep3')}</label>
              <div className='flex flex-wrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block  font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('apiKey')}<span className='text-[#E25757]'>*</span></label>
                  <input
                    type="text"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.apiKey ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="apiKey"
                    value={formState.apiKey}
                    onChange={(e) => {
                      handleInputChange('apiKey', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.apiKey}  />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block  font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('merchantID')}<span className='text-[#E25757]'>*</span></label>
                  <input
                    type="text"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.merchantID ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="merchantID"
                    value={formState.merchantID}
                    onChange={(e) => {
                      handleInputChange('merchantID', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.merchantID}  />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('appSecret')}<span className='text-[#E25757]'>*</span></label>
                  <input
                    type="password"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.appSecret ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="appSecret"
                    value={formState.appSecret}
                    onChange={(e) => {
                      handleInputChange('appSecret', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.appSecret}  />
                </div>
              </div>
            </div>          
            
          </div>
          <div className="flex flex-wrap mt-[16px] mb-[16px]">
            <label className="w-full flex items-center justify-center cursor-pointer text-[#333]">
              <input
                type="checkbox"
                className="w-[18px] h-[18px] cursor-pointer"
                id="agreeToTerms"
                checked={formState.agreeToTerms}
                onChange={(e) => {
                  handleInputChange('agreeToTerms', e.target.checked);
                }}
              />
              <span className="flex-1 pl-[6px] max-[768px]:leading-[18px]">
                {tPaymentProviderSettings('agreeTermsAndConditions')}
              </span>
            </label>
            <ValidateMessageComponent message={fieldErrors.agreeToTerms}  />
            </div>
          <div className='flex justify-center gap-[8px]'>
            <ButtonCancel text={tGlobal('cancel')} onClick={onClose} />
            <ButtonApply text={tGlobal('save')} onClick={() => handleSubmit({ preventDefault: () => {} } as React.FormEvent)} disabled={isLoading || allMethodsInactive || isBlockAction} />
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPaymentMethodModal; 