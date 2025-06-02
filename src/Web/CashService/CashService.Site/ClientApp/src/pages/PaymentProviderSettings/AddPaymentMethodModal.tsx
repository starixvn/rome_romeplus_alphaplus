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
import { TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { PaymentMethod } from '../../services/interfaces/Payments/PaymentMethod';
import { paymentProviderService } from '../../services/paymentsAPI';

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

const AddPaymentMethodModal: React.FC<AddPaymentMethodModalProps> = ({
  onClose,
  onSuccess,
  providerId,
  orgTelegramAssistantUrl,
}) => {
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

  const inactiveStatus = 4;

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

      const response = await paymentProviderService.getAddPaymentMethodInfo(
        providerId,
        parseInt(formState.selectedTransactionType)
      );
      if (response.error) {
        setError(response.error.message);
        return;
      }

      const addPaymentMethodInfo = response.data || {
        paymentMethods: [],
        channelContact: { tele: '' },
      };
      setPaymentMethods(addPaymentMethodInfo.paymentMethods);
      setTelegramAssistantUrl(addPaymentMethodInfo.channelContact.tele || '');
      setAllMethodsInactive(
        addPaymentMethodInfo.paymentMethods.length > 0 &&
          addPaymentMethodInfo.paymentMethods.every(
            (m: PaymentMethod) => m.status === inactiveStatus
          )
      );
    };

    loadPaymentMethods();
  }, [formState.selectedTransactionType]);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    const isValid = validateFormState();
    if (!isValid) {
      setIsLoading(false);
      return;
    }

    const selectedMethod = paymentMethods.find(
      (method) => method.id === parseInt(formState.selectedPaymentMethod)
    ) || { status: 0, name: '' };

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
      appSecret: formState.appSecret,
      isValidFromProvider: false,
    };

    const isValidFromProvider = true; // call provider api to check if the payment method is valid
    newSetting.isValidFromProvider = isValidFromProvider;

    const isUpdate = selectedMethod.status == 3;

    const response = isUpdate
      ? await paymentProviderService.updatePaymentMethod(newSetting)
      : await paymentProviderService.addPaymentMethod(newSetting);

    if (response.error) {
      setError(response.error.message);
      setIsLoading(false);
      return;
    }

    const result = response.data || {
      isSuccessful: false,
      code: 1,
      message: tGlobal('errorOccurred'),
    };

    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(isUpdate ? tGlobal('updateSuccess') : tGlobal('addSuccess'));
      setTimeout(() => {
        onClose();
        onSuccess();
      }, 3000);
    } else {
      setError(result.message || (isUpdate ? tGlobal('updateFailed') : tGlobal('addFailed')));
    }

    setIsLoading(false);
  };

  const onInputChange = (field: keyof PaymentMethodFormState, value: string | boolean) => {
    setFormState((prev) => ({ ...prev, [field]: value }));
    setFieldErrors((prev) => ({ ...prev, [field]: '' }));
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] max-h-[750px] overflow-y-auto bg-[#ffffff] w-full max-w-[560px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <MediumTitleComponent
            className="font-bold!"
            text={tPaymentProviderSettings('setUpPaymentMethods')}
          />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <ErrorMessageComponent className="mt-[8px]" message={error} />
        <SuccessMessageComponent className="mt-[8px]" message={successMessage} />
        <form onSubmit={onSubmit}>
          <div className="modal-body ">
            <div>
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep1')} />
              <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent
                    text={`${tPaymentProviderSettings('transactionType')}`}
                    required={true}
                  />
                  <select
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedTransactionType ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    value={formState.selectedTransactionType}
                    onChange={(e) => {
                      onInputChange('selectedTransactionType', e.target.value);
                    }}
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

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent
                    text={`${tPaymentProviderSettings('paymentMethod')}`}
                    required={true}
                  />
                  <select
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.selectedPaymentMethod ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    value={formState.selectedPaymentMethod}
                    onChange={(e) => {
                      onInputChange('selectedPaymentMethod', e.target.value);
                    }}
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
                          {method.status === inactiveStatus
                            ? ` (${tGlobal('underMaintenance')})`
                            : ''}
                        </option>
                      ))}
                  </select>
                  <ValidateMessageComponent message={fieldErrors.selectedPaymentMethod} />
                </div>
              </div>
            </div>
            <div>
              <hr className="mt-[16px] mb-[8px] border-[#0000001F]" />
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep2')} />
              <p className="mt-[8px] ">{tPaymentProviderSettings('clickLinkOrScanQRCode')}</p>
              <a
                href={telegramAssistantUrl}
                className=" text-[var(--text-color-primary)] font-semibold"
                target="_blank"
                rel="noopener noreferrer"
              >
                {telegramAssistantUrl}
              </a>
              <div className="mt-[16px] flex justify-center">
                {telegramAssistantUrl && <QRCodeCanvas value={telegramAssistantUrl} size={128} />}
              </div>
            </div>
            <div>
              <hr className="mt-[16px] mb-[8px] border-[#0000001F]" />
              <NormalTitleComponent text={tPaymentProviderSettings('addPaymentMethodStep3')} />
              <div className="flex flex-wrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent
                    text={`${tPaymentProviderSettings('apiKey')}`}
                    required={true}
                  />
                  <input
                    type="text"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.apiKey ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="apiKey"
                    value={formState.apiKey}
                    onChange={(e) => {
                      onInputChange('apiKey', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.apiKey} />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent
                    text={`${tPaymentProviderSettings('merchantID')}`}
                    required={true}
                  />
                  <input
                    type="text"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.merchantID ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="merchantID"
                    value={formState.merchantID}
                    onChange={(e) => {
                      onInputChange('merchantID', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.merchantID} />
                </div>
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent
                    text={`${tPaymentProviderSettings('appSecret')}`}
                    required={true}
                  />
                  <input
                    type="password"
                    className={`h-[32px] max-[768px]:h-[40px] max-[768px]:leading-[40px] w-full border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.appSecret ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    id="appSecret"
                    value={formState.appSecret}
                    onChange={(e) => {
                      onInputChange('appSecret', e.target.value);
                    }}
                  />
                  <ValidateMessageComponent message={fieldErrors.appSecret} />
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
                  onInputChange('agreeToTerms', e.target.checked);
                }}
              />
              <span className="flex-1 pl-[6px] max-[768px]:leading-[18px]">
                {tPaymentProviderSettings('agreeTermsAndConditions')}
              </span>
            </label>
            <ValidateMessageComponent message={fieldErrors.agreeToTerms} />
          </div>
          <div className="flex justify-center gap-[8px]">
            <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose} />
            <ButtonApplyComponent
              text={tGlobal('save')}
              onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)}
              disabled={isLoading || allMethodsInactive || isBlockAction}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPaymentMethodModal;
