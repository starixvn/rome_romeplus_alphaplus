import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentMethod } from '../../services/interfaces/Payments/PaymentMethod';
import { QRCodeCanvas } from 'qrcode.react';
import { ErrorMessageComponent, SuccessMessageComponent, ValidateMessageComponent } from '../../components/MessageComponents';
import { ButtonApplyComponent, ButtonCancelComponent, ButtonClosePopupComponent } from '../../components/ButtonComponents';
import { LableTitleComponent, MediumTitleComponent, NormalTitleComponent } from '../../components/TitleComponents';

interface UpdatePaymentMethodModalProps {
  onClose: () => void;
  onSuccess: () => void;
  providerId: number;
  orgTelegramAssistantUrl: string;
  existingPaymentMethod: PaymentMethod;
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

const UpdatePaymentMethodModal: React.FC<UpdatePaymentMethodModalProps> = ({ 
  onClose, 
  onSuccess,
  providerId, 
  orgTelegramAssistantUrl,
  existingPaymentMethod 
}) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');
  const [isBlockAction, setIsBlockAction] = React.useState<boolean>(false);

  // Combined form state
  const [formState, setFormState] = React.useState<PaymentMethodFormState>({
    selectedPaymentMethod: existingPaymentMethod.method.toString(),
    selectedTransactionType: existingPaymentMethod.transactionType.toString(),
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: true,
    telegramAssistantUrl: orgTelegramAssistantUrl
  });

  const hasFormChanged = (): boolean => {
    const fieldsToCompare: (keyof PaymentMethodFormState)[] = [
      'selectedPaymentMethod',
      'selectedTransactionType',
      'apiKey',
      'merchantID',
      'appSecret',
      'agreeToTerms',
      'telegramAssistantUrl'
    ];

    return fieldsToCompare.some(field => formState[field] !== orgFormState[field]);
  };

  const orgFormState: PaymentMethodFormState = {
    selectedPaymentMethod: existingPaymentMethod.method.toString(),
    selectedTransactionType: existingPaymentMethod.transactionType.toString(),
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: true,
    telegramAssistantUrl: orgTelegramAssistantUrl
  };

  const validateFormState = () => {
    const errors: typeof fieldErrors = {
      selectedTransactionType: !formState.selectedTransactionType ? tGlobal('pleaseFillField') : '',
      selectedPaymentMethod: !formState.selectedPaymentMethod ? tGlobal('pleaseFillField') : '',
      apiKey: !formState.apiKey ? tGlobal('pleaseFillField') : '',
      merchantID: !formState.merchantID ? tGlobal('pleaseFillField') : '',
      appSecret: !formState.appSecret ? tGlobal('pleaseFillField') : '',
      agreeToTerms: !formState.agreeToTerms ? tGlobal('pleaseAgreeTerms') : '',
    };

    const hasError = Object.values(errors).some(Boolean);
    setFieldErrors(errors);
    return !hasError;
  };

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

  const inactiveStatus = 0;
  
  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      if (!formState.selectedTransactionType) {
        setPaymentMethods([]);
        setAllMethodsInactive(false);
        return;
      }

      const response = await paymentProviderService.getAddPaymentMethodInfo(providerId, parseInt(formState.selectedTransactionType));
      
      if(response.error) {
        setError(response.error.message);
        return;
      }

      const addPaymentMethodInfo = response.data || {paymentMethods: [], channelContact: {tele: ''}};
      setPaymentMethods(addPaymentMethodInfo.paymentMethods);
      setTelegramAssistantUrl(addPaymentMethodInfo.channelContact.tele || '');
      setAllMethodsInactive(addPaymentMethodInfo.paymentMethods.length > 0 && addPaymentMethodInfo.paymentMethods.every((method: PaymentMethod) => method.status === inactiveStatus));
    };

    loadPaymentMethods();
  }, [formState.selectedTransactionType]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    if(!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setIsLoading(false);
      return;
    }

    const isValid = validateFormState();
    if (!isValid) {
      setIsLoading(false);
      return;
    }

    const selectedMethod = paymentMethods.find(method => method.id === parseInt(formState.selectedPaymentMethod));
    const updatedSetting = {
      id: existingPaymentMethod.id,
      name: selectedMethod?.name || '',
      providerId,
      method: parseInt(formState.selectedPaymentMethod),
      transactionType: parseInt(formState.selectedTransactionType),
      status: existingPaymentMethod.status,
      custId: existingPaymentMethod.custId || 0,
      apiMethodKey: formState.apiKey,
      merchantID: formState.merchantID,
        appSecret: formState.appSecret,
        isValidFromProvider: false
    };
    
      const isValidFromProvider = true; // call provider api to check if the payment method is valid
      updatedSetting.isValidFromProvider = isValidFromProvider;
    const response = await paymentProviderService.updatePaymentMethod(updatedSetting);
    if(response.error) {
      setError(response.error.message);
      setIsLoading(false);
      return;
    }

    const result = response.data || {isSuccessful: false, code: 1, message: tGlobal('errorOccurred')};
    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(tGlobal('updateSuccess'));
      setTimeout(() => { onClose(); onSuccess(); }, 3000);
    } else {
      setError(result.message || tGlobal('updateFailed'));
    }
    
    setIsLoading(false);
  };

  const onInputChange = (field: keyof PaymentMethodFormState, value: string | boolean) => {
    setFormState(prev => ({ ...prev, [field]: value }));
    setFieldErrors(prev => ({ ...prev, [field]: '' }));
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] max-h-[750px] overflow-y-auto bg-[#ffffff] w-full max-w-[560px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative"> 
          <MediumTitleComponent className='font-bold!' text={tPaymentProviderSettings('updatePaymentMethod')} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <ErrorMessageComponent className='mt-[8px]' message={error} />
        <SuccessMessageComponent className='mt-[8px]' message={successMessage} />
        <form onSubmit={onSubmit}>
          <div className="modal-body"> 
            <div>
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep1')} />             
              <div className='flex gap-[10px]'>
              <div className="w-[48%]">
                <LableTitleComponent text={`${tPaymentProviderSettings('transactionType')}`} required={true} />
                <select 
                  className={`h-[32px] max-[768px]:h-[40px] w-full border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedTransactionType ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  value={formState.selectedTransactionType}
                  onChange={(e) => {
                    onInputChange('selectedTransactionType', e.target.value);
                  }}
                >
                  <option value="">{tGlobal('select')}</option>
                  {TRANSACTION_TYPES.filter(type => type.id !== 0).map(type => (
                    <option key={type.id} value={type.id}>{tPaymentProviderSettings(`transaction-${type.id}`)}</option>
                  ))}
                </select>
                <ValidateMessageComponent message={fieldErrors.selectedTransactionType}  />
              </div>
            
              <div className="w-[48%]">
                <LableTitleComponent text={`${tPaymentProviderSettings('paymentMethod')}`} required={true} />
                <select 
                  className={`h-[32px] max-[768px]:h-[40px] w-full border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedPaymentMethod ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  value={formState.selectedPaymentMethod}
                  onChange={(e) => {
                    onInputChange('selectedPaymentMethod', e.target.value);
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
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep2')} />              
              <p className='text-[12px]'>{tPaymentProviderSettings('clickLinkOrScanQRCode')}</p>
              <a href={telegramAssistantUrl} className='text-[12px] text-[var(--text-color-primary)] font-semibold' target="_blank" rel="noopener noreferrer">{telegramAssistantUrl}</a>
              <div className="mt-[16px] flex justify-center">
                {telegramAssistantUrl && <QRCodeCanvas value={telegramAssistantUrl} size={128} />}
              </div>
            </div>
            <div>
            <hr className='mt-[16px] mb-[16px] border-[#0000001F]'  />
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep3')} />  123
              <ValidateMessageComponent message={error} />
              <div className='flex flex-wrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
              <div className="w-[48%]">
                <LableTitleComponent text={`${tPaymentProviderSettings('apiKey')}`} required={true} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-full border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.apiKey ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  id="apiKey"
                  value={formState.apiKey}
                  onChange={(e) => {
                    onInputChange('apiKey', e.target.value);
                  }}
                />
                <ValidateMessageComponent message={fieldErrors.apiKey}  />
              </div>
              <div className="w-[48%]">
                <LableTitleComponent text={`${tPaymentProviderSettings('merchantID')}`} required={true} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-full border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.merchantID ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  id="merchantID"
                  value={formState.merchantID}
                  onChange={(e) => {
                    onInputChange('merchantID', e.target.value);
                  }}
                />
                <ValidateMessageComponent message={fieldErrors.merchantID}  />
              </div>
              <div className="w-[48%]">
                <LableTitleComponent text={`${tPaymentProviderSettings('appSecret')}`} required={true} />
                <input
                  type="password"
                  className={`h-[32px] max-[768px]:h-[40px] w-full border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.appSecret ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                  id="appSecret"
                  value={formState.appSecret}
                  onChange={(e) => {
                    onInputChange('appSecret', e.target.value);
                  }}
                />
                <ValidateMessageComponent message={fieldErrors.appSecret}  />
              </div>
              </div>
            </div>
            
          </div>
          <div className="flex flex-wrap mt-[16px] mb-[16px]">
            <label className="w-full flex items-center justify-center cursor-pointer text-[#333] text-[12px] leading-[14px]">
              <input
                type="checkbox"
                className="w-[18px] h-[18px]"
                id="agreeToTerms"
                checked={formState.agreeToTerms}
                disabled={true}
                onChange={(e) => {
                  onInputChange('agreeToTerms', e.target.checked);
                }}
              />
              <span className="flex-1 pl-[6px]">
                {tPaymentProviderSettings('agreeTermsAndConditions')}
              </span>
            </label>
            <ValidateMessageComponent message={fieldErrors.agreeToTerms}  />
            </div>
            <div className='flex justify-center gap-[8px]'>
            <ButtonCancelComponent text={tPaymentProviderSettings('setUpLater')} onClick={onClose} />
            <ButtonApplyComponent text={isLoading ? tGlobal('loading') : tGlobal('update')} onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)} disabled={isLoading || allMethodsInactive || isBlockAction} />
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdatePaymentMethodModal; 