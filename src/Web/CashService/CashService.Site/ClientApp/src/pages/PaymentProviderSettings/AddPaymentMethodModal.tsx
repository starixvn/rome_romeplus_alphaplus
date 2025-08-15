import { QRCodeCanvas } from 'qrcode.react';
import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonComponent,
} from '../../components/Buttons/index';
import { MessageComponent, ResultType } from '../../components/Message';
import {
  TitleComponent
} from '../../components/Title';
import { TRANSACTION_TYPES, paymentMethodStatusConstant } from '../../constants/paymentConstants';
import { PaymentMethod } from '../../services/interfaces/Payments/PaymentMethod';
import { paymentProviderService } from '../../services/paymentsAPI';
import { createDefaultAddPaymentMethodViewModel } from '../../services/interfaces/Payments/AddPaymentMethodViewModel';
import { createErrorResultModel } from '../../services/interfaces/ResultModel';
import { IconArrowDown } from '../../components/Icons';
import { ButtonIconComponent } from '../../components/Buttons/ButtonIconComponent';

interface AddPaymentMethodModalProps {
  onClose: () => void;
  onSuccess: () => void;
  providerId: number;
  orgTelegramAssistantUrl: string;
  isCashServiceEnabled: boolean;
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

const AddPaymentMethodModal: React.FC<AddPaymentMethodModalProps> = ({
  onClose,
  onSuccess,
  providerId,
  orgTelegramAssistantUrl,
  isCashServiceEnabled,
}) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [isBlockAction, setIsBlockAction] = React.useState<boolean>(false);
  
  const [resultType, setResultType] = React.useState<ResultType | undefined>(undefined);
  const [resultMessage, setResultMessage] = React.useState<string>('');
  const [isShowAppSecret, setIsShowAppSecret] = React.useState<boolean>(false);

  // Combined form state
  const [formState, setFormState] = React.useState<PaymentMethodFormState>({
    selectedPaymentMethod: '',
    selectedTransactionType: '',
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: false,
    telegramAssistantUrl: orgTelegramAssistantUrl,
  });

  const [paymentMethods, setPaymentMethods] = React.useState<PaymentMethod[]>([]);
  const [allMethodsInactive, setAllMethodsInactive] = React.useState<boolean>(false);
  const [telegramAssistantUrl, setTelegramAssistantUrl] =
    React.useState<string>(orgTelegramAssistantUrl);
  const [fieldErrors, setFieldErrors] = React.useState({
    selectedTransactionType: '',
    selectedPaymentMethod: '',
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: '',
  });

  const inactiveStatus = paymentMethodStatusConstant.maintenance;

  const validateFormState = () => {
    const errors: typeof fieldErrors = {
      selectedTransactionType: !formState.selectedTransactionType ? tGlobal('pleaseFillField') : '',
      selectedPaymentMethod: !formState.selectedPaymentMethod ? tGlobal('pleaseFillField') : '',
      apiKey: !formState.apiKey ? tGlobal('pleaseFillField') : '',
      merchantID: !formState.merchantID ? tGlobal('pleaseFillField') : '',
      appSecret: !formState.appSecret ? tGlobal('pleaseFillField') : '',
      agreeToTerms: !formState.agreeToTerms ? tGlobal('pleaseAgreeTerms') : '',
    };

    const isValid = !Object.values(errors).some(Boolean);
    setFieldErrors(errors);
    return isValid;
  };

  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      if (!formState.selectedTransactionType) {
        setPaymentMethods([]);
        setAllMethodsInactive(false);
        return;
      }

      const transactionType = parseInt(formState.selectedTransactionType);
      const response = await paymentProviderService.getAddPaymentMethodInfo(providerId, transactionType);
      if (response.error) {
        setResultType('error');
        setResultMessage(response.error.message);
        return;
      }

      const addPaymentMethodInfo = response.data || createDefaultAddPaymentMethodViewModel();
      setPaymentMethods(addPaymentMethodInfo.paymentMethods.filter((p: any) => p.id !== paymentMethodStatusConstant.completed));
      setTelegramAssistantUrl(addPaymentMethodInfo.channelContact.tele || '');

      const isAllMethodsInactive =
        addPaymentMethodInfo.paymentMethods.length > 0 &&
        addPaymentMethodInfo.paymentMethods.every((m: PaymentMethod) => m.status === inactiveStatus);
      setAllMethodsInactive(isAllMethodsInactive);
    };

    loadPaymentMethods();
  }, [formState.selectedTransactionType]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setResultType(undefined);

    const isValid = validateFormState();
    if (!isValid) {
      setIsLoading(false);
      return;
    }

    const selectedMethod = paymentMethods.find((method) => method.id === parseInt(formState.selectedPaymentMethod)) 
      || { status: 0, name: '' };

    const newSetting = {
      id: 0,
      name: selectedMethod.name || '',
      providerId,
      method: parseInt(formState.selectedPaymentMethod),
      transactionType: parseInt(formState.selectedTransactionType),
      status: paymentMethodStatusConstant.processing,
      custId: 0,
      apiMethodKey: formState.apiKey,
      merchantID: formState.merchantID,
      appSecret: formState.appSecret
    };

    const isUpdate = selectedMethod.status == paymentMethodStatusConstant.fail;
    const response = isUpdate
      ? await paymentProviderService.updatePaymentMethod(newSetting)
      : await paymentProviderService.addPaymentMethod(newSetting);

    if (response.error) {
      setResultType('error');
      setResultMessage(response.error.message);
      setIsLoading(false);
      return;
    }

    const result = response.data || createErrorResultModel(1, tGlobal('errorOccurred'));

    if (result.isSuccessful) {
      setIsBlockAction(true);
      setResultType('success');
      setResultMessage(isUpdate ? tGlobal('updateSuccess') : tGlobal('addSuccess'));
      setTimeout(() => {
        onClose();
        onSuccess();
      }, 3000);
    } else if(result.code === 200){
      setResultType('warning');
      setResultMessage(tGlobal(`error-${result.code}`));
      setTimeout(() => {
        onClose();
        onSuccess();
      }, 3000);
    } else {
      setResultType('error');
      setResultMessage(result.message || (isUpdate ? tGlobal('updateFailed') : tGlobal('addFailed')));
    }

    setIsLoading(false);
  };

  const onInputChange = (field: keyof PaymentMethodFormState, value: string | boolean) => {
    setFormState((prev) => ({ ...prev, [field]: value }));
    setFieldErrors((prev) => ({ ...prev, [field]: '' }));
  };

  return (
    <div className="bg-[#00000080] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[12px] pt-[0] overflow-y-auto bg-[var(--modal-background)] w-full max-w-[560px] max-[768px]:max-w-[90%] rounded-[3px] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] max-h-[90%] overflow-auto">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]" >
          <TitleComponent type='medium' text={tPaymentProviderSettings('setUpPaymentMethods')} />
          <ButtonIconComponent style='Close' onClick={onClose} />
        </div>
        <MessageComponent type={resultType} message={resultMessage} className="mt-[8px]" />
        <form onSubmit={onSubmit}>
          <div className="modal-body ">
            <div>
              <TitleComponent type='normal' text={tPaymentProviderSettings('addPaymentMethodStep1')} />
              <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <TitleComponent type='label' text={`${tPaymentProviderSettings('transactionType')}`} required={true} />
                  <div className="relative">
                  <select
                    className={`control-field ${fieldErrors.selectedTransactionType ? 'error' : ''}`}
                    value={formState.selectedTransactionType}
                    onChange={(e) => { onInputChange('selectedTransactionType', e.target.value); }}
                  >
                    <option value="">{tGlobal('select')}</option>
                    {TRANSACTION_TYPES.filter((type) => type.id !== 0).map((type) => (
                      <option key={type.id} value={type.id}>
                        {tPaymentProviderSettings(`transaction-${type.id}`)}
                      </option>
                    ))}
                  </select>
                  <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                  </div>
                  <MessageComponent message={fieldErrors.selectedTransactionType} type='validate' />
                </div>

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <TitleComponent type='label' text={`${tPaymentProviderSettings('paymentMethod')}`} required={true} />
                  <div className="relative">
                    <select
                      className={`control-field ${fieldErrors.selectedPaymentMethod ? 'error' : ''}`}
                      value={formState.selectedPaymentMethod}
                      onChange={(e) => { onInputChange('selectedPaymentMethod', e.target.value); }}
                    >
                      <option value="">{tGlobal('select')}</option>
                      {paymentMethods
                        .filter((p) => p.id !== 0)
                        .map((method) => (
                          <option
                            key={method.id}
                            value={method.id}
                            disabled={method.status === inactiveStatus}
                          >
                            {method.name}
                            {method.status === inactiveStatus ? ` (${tGlobal('underMaintenance')})` : ''}
                          </option>
                        ))}
                    </select>
                    <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                  </div>
                  <MessageComponent message={fieldErrors.selectedPaymentMethod} type='validate' />
                </div>
              </div>
            </div>
            <div>
            <hr className="mt-[16px] mb-[8px] border-[#0000001F]" />
            <TitleComponent type='normal' text={tPaymentProviderSettings('addPaymentMethodStep2')} />
            <div className="flex justify-between items-start">
              <div className="pr-[20px]">
                <p className="mt-[8px] ">{tPaymentProviderSettings('clickLinkOrScanQRCode')}</p>
                <a
                  href={telegramAssistantUrl}
                  className=" text-[var(--text-link-color)] font-semibold"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  {telegramAssistantUrl}
                </a>
              </div>
              <div className="flex-1 flex justify-center ">
                {telegramAssistantUrl && <QRCodeCanvas value={telegramAssistantUrl} size={128} />}
              </div>
            </div>
            
            
            </div>
            <div>
              <hr className="mt-[16px] mb-[8px] border-[#0000001F]" />
              <TitleComponent type='normal' text={tPaymentProviderSettings('addPaymentMethodStep3')} />
              <div className="flex flex-wrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <TitleComponent type='label' text={`${tPaymentProviderSettings('apiKey')}`} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.apiKey ? 'error' : ''}`}
                    id="apiKey"
                    value={formState.apiKey}
                    onChange={(e) => { onInputChange('apiKey', e.target.value); }}
                  />
                  <MessageComponent message={fieldErrors.apiKey} type='validate' />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <TitleComponent type='label' text={`${tPaymentProviderSettings('merchantID')}`} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.merchantID ? 'error' : ''}`}
                    id="merchantID"
                    
                    value={formState.merchantID}
                    onChange={(e) => { onInputChange('merchantID', e.target.value); }}
                  />
                  <MessageComponent message={fieldErrors.merchantID} type='validate' />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <TitleComponent type='label' text={`${tPaymentProviderSettings('appSecret')}`} required={true} />
                  <div className="relative">
                  <input
                    type={isShowAppSecret ? 'text' : 'password'}
                    className={`control-field ${fieldErrors.appSecret ? 'error' : ''}`}
                    id="appSecret"
                    value={formState.appSecret}
                    onChange={(e) => { onInputChange('appSecret', e.target.value); }}
                    name="password "
                    autoComplete="new-password "
                  />
                  {
                    isShowAppSecret ? <ButtonIconComponent className='absolute right-[-3px] top-[0] hover:bg-transparent!' style='EyeOff' onClick={() => { setIsShowAppSecret(!isShowAppSecret); }} />
                    : <ButtonIconComponent className='absolute right-[-3px] top-[0] hover:bg-transparent!' style='Eye' onClick={() => { setIsShowAppSecret(!isShowAppSecret); }} />
                  }                  
                  </div>
                  <MessageComponent message={fieldErrors.appSecret} type='validate' />
                </div>
              </div>
            </div>
          </div>
          <div className="flex flex-wrap mt-[16px] mb-[16px]">
            <label className="w-full flex items-center justify-center cursor-pointer">
              <input
                type="checkbox"
                className="w-[18px] h-[18px] cursor-pointer"
                id="agreeToTerms"
                checked={formState.agreeToTerms}
                onChange={(e) => { onInputChange('agreeToTerms', e.target.checked); }}
              />
              <span className="flex-1 pl-[6px]">
                {tPaymentProviderSettings('agreeTermsAndConditions')}
              </span>
            </label>
            <MessageComponent message={fieldErrors.agreeToTerms} type='validate' />
          </div>
          <div className="flex justify-center gap-[8px]">
            <ButtonComponent variant="cancel" text={tGlobal('cancel')} onClick={onClose} type='button' />
            <ButtonComponent
              variant="apply"
              text={tGlobal('save')}
              type='button'
              onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)}
              disabled={isLoading || allMethodsInactive || isBlockAction || !isCashServiceEnabled}
              title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPaymentMethodModal;
