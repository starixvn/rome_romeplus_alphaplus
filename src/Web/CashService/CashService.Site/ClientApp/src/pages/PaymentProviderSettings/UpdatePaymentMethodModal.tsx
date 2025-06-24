import { QRCodeCanvas } from 'qrcode.react';
import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../components/ButtonComponents';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../components/MessageComponents';
import {
  LabelTitleComponent,
  MediumTitleComponent,
  NormalTitleComponent,
} from '../../components/TitleComponents';
import { paymentMethodStatusConstant, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { PaymentMethod } from '../../services/interfaces/Payments/PaymentMethod';
import { paymentProviderService } from '../../services/paymentsAPI';
import { createDefaultAddPaymentMethodViewModel } from '../../services/interfaces/Payments/AddPaymentMethodViewModel';
import { createErrorResultModel } from '../../services/interfaces/ResultModel';

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
  existingPaymentMethod,
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
    telegramAssistantUrl: orgTelegramAssistantUrl,
  });

  const hasFormChanged = (): boolean => {
    const fieldsToCompare: (keyof PaymentMethodFormState)[] = [
      'selectedPaymentMethod',
      'selectedTransactionType',
      'apiKey',
      'merchantID',
      'appSecret',
      'agreeToTerms',
      'telegramAssistantUrl',
    ];

    return fieldsToCompare.some((field) => formState[field] !== orgFormState[field]);
  };

  const orgFormState: PaymentMethodFormState = {
    selectedPaymentMethod: existingPaymentMethod.method.toString(),
    selectedTransactionType: existingPaymentMethod.transactionType.toString(),
    apiKey: '',
    merchantID: '',
    appSecret: '',
    agreeToTerms: true,
    telegramAssistantUrl: orgTelegramAssistantUrl,
  };

  const validateFormState = () => {
    const errors: typeof fieldErrors = {
      selectedTransactionType: !parseInt(formState.selectedTransactionType) ? tGlobal('pleaseFillField') : '',
      selectedPaymentMethod: !parseInt(formState.selectedPaymentMethod) ? tGlobal('pleaseFillField') : '',
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

  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      if (!formState.selectedTransactionType) {
        setPaymentMethods([]);
        setAllMethodsInactive(false);    
        setFormState({ ...formState, 'selectedPaymentMethod': '0' });
        return;
      }

      const transactionType = parseInt(formState.selectedTransactionType);
      const response = await paymentProviderService.getAddPaymentMethodInfo(providerId, transactionType);

      if (response.error) {
        setError(response.error.message);
        return;
      }

      const addPaymentMethodInfo = response.data || createDefaultAddPaymentMethodViewModel();
      setPaymentMethods(addPaymentMethodInfo.paymentMethods);
      setTelegramAssistantUrl(addPaymentMethodInfo.channelContact.tele || '');
      setAllMethodsInactive(
        addPaymentMethodInfo.paymentMethods.length > 0 &&
          addPaymentMethodInfo.paymentMethods.every(
            (method: PaymentMethod) => method.status === inactiveStatus
          )
      );
      
      var ms = addPaymentMethodInfo.paymentMethods.filter((p: any) => p.id !== 0 && p.status === paymentMethodStatusConstant.fail);
      setFormState({ ...formState, 'selectedPaymentMethod': ms.length > 0 ? ms[0].id.toString() : '0' });
    };

    loadPaymentMethods();
  }, [formState.selectedTransactionType]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');
    setFieldErrors({
      selectedTransactionType: '',
      selectedPaymentMethod: '',
      apiKey: '',
      merchantID: '',
      appSecret: '',
      agreeToTerms: '',
    });

    if (!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setIsLoading(false);
      return;
    }

    const isValid = validateFormState();
    if (!isValid) {
      setIsLoading(false);
      return;
    }

    const selectedMethod = paymentMethods.find((method) => method.id === parseInt(formState.selectedPaymentMethod));
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
      appSecret: formState.appSecret
    };

    const response = await paymentProviderService.updatePaymentMethod(updatedSetting);
    if (response.error) {
      setError(response.error.message);
      setIsLoading(false);
      return;
    }

    const result = response.data || createErrorResultModel(1, tGlobal('errorOccurred'));
    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(tGlobal('updateSuccess'));
      setTimeout(() => {
        onClose();
        onSuccess();
      }, 3000);
    } else {
      setError(result.message || tGlobal('updateFailed'));
    }

    setIsLoading(false);
  };

  const onInputChange = (field: keyof PaymentMethodFormState, value: string | boolean) => {
    setFormState((prev) => ({ ...prev, [field]: value }));
    setFieldErrors((prev) => ({ ...prev, [field]: '' }));
  };

  return (
    <div className="bg-[#00000080] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[12px] pt-[0] max-h-[750px] overflow-y-auto bg-[var(--modal-background)] w-full max-w-[560px] max-[768px]:max-w-[90%] rounded-[3px] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]" >
          <MediumTitleComponent text={tPaymentProviderSettings('updatePaymentMethod')} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <ErrorMessageComponent className="mt-[8px]" message={error} />
        <SuccessMessageComponent className="mt-[8px]" message={successMessage} />
        <form onSubmit={onSubmit}>
          <div className="modal-body">
            <div>
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep1')} />
              <div className="flex gap-[10px]">
                <div className="w-[48%]">
                  <LabelTitleComponent text={`${tPaymentProviderSettings('transactionType')}`} required={true} />
                  <select
                    className={`control-field ${fieldErrors.selectedTransactionType ? 'error' : ''}`}
                    value={formState.selectedTransactionType}
                    onChange={(e) => {  onInputChange('selectedTransactionType', e.target.value); }}
                  >
                    <option value="">{tGlobal('select')}</option>
                    {TRANSACTION_TYPES.filter((type) => type.id !== 0).map((type) => (
                      <option key={type.id} value={type.id}>
                        {tPaymentProviderSettings(`transaction-${type.id}`)}
                      </option>
                    ))}
                  </select>
                  <ValidateMessageComponent message={fieldErrors.selectedTransactionType} />
                </div>

                <div className="w-[48%]">
                  <LabelTitleComponent text={`${tPaymentProviderSettings('paymentMethod')}`} required={true} />
                  <select
                    className={`control-field ${fieldErrors.selectedPaymentMethod ? 'error' : ''}`}
                    value={formState.selectedPaymentMethod}
                    onChange={(e) => { onInputChange('selectedPaymentMethod', e.target.value); }}
                  >
                    <option value="">{tGlobal('select')}</option>
                    {paymentMethods
                      .filter((p) => p.id !== 0 && p.status === paymentMethodStatusConstant.fail)
                      .map((method) => (
                        <option key={method.id} value={method.id} >
                          {method.name}                          
                        </option>
                      ))}
                  </select>
                  <ValidateMessageComponent message={fieldErrors.selectedPaymentMethod} />
                </div>
              </div>
            </div>
            <div>
              <hr className="mt-[16px] border-[#0000001F]" />
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep2')} />
              <div className="flex justify-between items-start">
                <div className="pr-[20px]">
                  <p className="text-[12px]">{tPaymentProviderSettings('clickLinkOrScanQRCode')}</p>
                  <a
                    href={telegramAssistantUrl}
                    className="text-[var(--text-link-color)] font-semibold"
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
              <hr className="mt-[16px] mb-[16px] border-[#0000001F]" />
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep3')} />
              <ValidateMessageComponent message={tGlobal('somethingWentWrong')} />
              <div className="flex flex-wrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[48%]">
                  <LabelTitleComponent text={`${tPaymentProviderSettings('apiKey')}`} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.apiKey ? 'error' : ''}`}
                    id="apiKey"
                    value={formState.apiKey}
                    onChange={(e) => { onInputChange('apiKey', e.target.value); }}
                  />
                  <ValidateMessageComponent message={fieldErrors.apiKey} />
                </div>
                <div className="w-[48%]">
                  <LabelTitleComponent text={`${tPaymentProviderSettings('merchantID')}`} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.merchantID ? 'error' : ''}`}
                    id="merchantID"
                    value={formState.merchantID}
                    onChange={(e) => { onInputChange('merchantID', e.target.value); }}
                  />
                  <ValidateMessageComponent message={fieldErrors.merchantID} />
                </div>
                <div className="w-[48%]">
                  <LabelTitleComponent text={`${tPaymentProviderSettings('appSecret')}`} required={true} />
                  <input
                    type="password"
                    className={`control-field ${fieldErrors.appSecret ? 'error' : ''}`}
                    id="appSecret"
                    value={formState.appSecret}
                    onChange={(e) => { onInputChange('appSecret', e.target.value); }}
                  />
                  <ValidateMessageComponent message={fieldErrors.appSecret} />
                </div>
              </div>
            </div>
          </div>
          <div className="flex flex-wrap mt-[16px] mb-[16px]">
            <label className="w-full flex items-center justify-center cursor-pointer">
              <input
                type="checkbox"
                className="w-[18px] h-[18px]"
                id="agreeToTerms"
                checked={formState.agreeToTerms}
                disabled={true}
                onChange={(e) => { onInputChange('agreeToTerms', e.target.checked); }}
              />
              <span className="flex-1 pl-[6px]">
                {tPaymentProviderSettings('agreeTermsAndConditions')}
              </span>
            </label>
            <ValidateMessageComponent message={fieldErrors.agreeToTerms} />
          </div>
          <div className="flex justify-center gap-[8px]">
            <ButtonCancelComponent text={tPaymentProviderSettings('setUpLater')} onClick={onClose} />
            <ButtonApplyComponent
              text={isLoading ? tGlobal('loading') : tGlobal('update')}
              type='submit'
              onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)}
              disabled={isLoading || allMethodsInactive || isBlockAction}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdatePaymentMethodModal;
