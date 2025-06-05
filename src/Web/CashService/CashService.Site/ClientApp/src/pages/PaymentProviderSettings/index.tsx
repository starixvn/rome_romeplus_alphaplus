import '../../styles/common.css';
import '../../styles/global.scss';

import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';

import Tippy from '@tippyjs/react';

import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonFullComponent,
  ButtonOutlineComponent,
} from '../../components/ButtonComponents';
import { IconInfo } from '../../components/Icons';
import { LoadingFullComponent } from '../../components/LoadingComponent';
import { ResultMessageComponent } from '../../components/MessageComponents';
import {
  StatusCompletedComponent,
  StatusFailedComponent,
  StatusMaintenanceComponent,
  StatusNoneComponent,
  StatusProcessingComponent,
} from '../../components/StatusComponents';
import TitleComponent, {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../components/TitleComponents';
import { paymentMethodStatusConstant, paymentProviderStatusConstant, STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18n-paymentProviderSettings';
import { PaymentMethod } from '../../services/interfaces/Payments';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import { formatInputValue, formatNumber } from '../../utils/numberFormatting';
import { onBackNavigation, reloadPage } from '../../utils/sharedUtils';
import AddPaymentMethodModal from './AddPaymentMethodModal';
import BanksPopupModal from './BanksPopupModal';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import UpdatePaymentMethodModal from './UpdatePaymentMethodModal';

declare global {
  interface Window {
    providerSettingsViewModel: any;
  }
}

interface FormState {
  providerStatus: number;
  providerSettings: any[];
}

interface ValidationError {
  index: number;
  field: string;
  error: string;
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [showFeePopup, setShowFeePopup] = React.useState(false);
  const [showAddPaymentMethod, setShowAddPaymentMethod] = React.useState(false);
  const [showBanksPopup, setShowBanksPopup] = React.useState(false);
  const [showUpdatePaymentMethod, setShowUpdatePaymentMethod] = React.useState(false);
  const [selectedSetting, setSelectedSetting] = React.useState<any>(null);
  const [providerSettings, setProviderSettings] = React.useState<any[]>([]);
  const [originalSettings, setOriginalSettings] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  const [updating, setUpdating] = React.useState(false);
  const [updateMessage, setUpdateMessage] = React.useState<{
    type: 'success' | 'error';
    text: string;
  } | null>(null);
  const [providerStatus, setProviderStatus] = React.useState(
    window.providerSettingsViewModel.Provider.Status
  );
  const [validationErrors, setValidationErrors] = React.useState<{ [key: string]: string }>({});

  // State for dropdowns
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedStatus, setSelectedStatus] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');

  const [formState, setFormState] = useState<FormState>({
    providerStatus: window.providerSettingsViewModel.Provider.Status,
    providerSettings: window.providerSettingsViewModel.Provider.ProviderSettings,
  });
  const [originalFormState] = useState<FormState>({
    providerStatus: window.providerSettingsViewModel.Provider.Status,
    providerSettings: window.providerSettingsViewModel.Provider.ProviderSettings,
  });

  useCookieLanguage(i18n);
  // Fetch provider settings when component mounts or when dropdowns change
  React.useEffect(() => {
    const fetchProviderSettings = async () => {
      setLoading(true);
      const method = selectedPaymentMethod ? parseInt(selectedPaymentMethod) : 0;
      const status = selectedStatus ? parseInt(selectedStatus) : 0;
      const transactionType = selectedTransactionType ? parseInt(selectedTransactionType) : 0;
      const response = await paymentProviderService.getProviderSettings(
        window.providerSettingsViewModel.Provider.Id,
        method,
        status,
        transactionType
      );

      if (response.error) {
        setError(response.error.message);
        setLoading(false);
        return;
      }

      const settings = response.data || [];
      setProviderSettings(settings);
      setOriginalSettings(settings);
      setError(null);
      setLoading(false);
    };

    fetchProviderSettings();
  }, [selectedPaymentMethod, selectedStatus, selectedTransactionType]);

  const onPaymentMethodChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedPaymentMethod(event.target.value);
  };

  const onStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedStatus(event.target.value);
  };

  const onTransactionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedTransactionType(event.target.value);
  };

  const onViewFees = () => {
    setShowFeePopup(true);
  };
  const onCloseFeePopup = () => {
    setShowFeePopup(false);
  };
  const onAddPaymentMethod = () => {
    setShowAddPaymentMethod(true);
  };
  const onCloseAddPaymentMethod = () => {
    setShowAddPaymentMethod(false);
  };

  const onViewBanks = (setting: any) => {
    setSelectedSetting(setting);
    setShowBanksPopup(true);
  };

  const onCloseBanksPopup = () => {
    setShowBanksPopup(false);
    setSelectedSetting(null);
  };

  // Handler for minimum amount change
  const onMinAmountChange = (index: number, value: string) => {
    const formattedValue = formatInputValue(value);

    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      minAmount: formattedValue,
    };
    setProviderSettings(updatedSettings);
    setFormState((prev) => ({
      ...prev,
      providerSettings: updatedSettings,
    }));

    // Clear validation error for this field
    const newValidationErrors = { ...validationErrors };
    delete newValidationErrors[`minAmount-${index}`];
    delete newValidationErrors[`maxAmount-${index}`];
    setValidationErrors(newValidationErrors);
  };

  // Handler for maximum amount change
  const onMaxAmountChange = (index: number, value: string) => {
    const formattedValue = formatInputValue(value);

    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      maxAmount: formattedValue,
    };
    setProviderSettings(updatedSettings);
    setFormState((prev) => ({
      ...prev,
      providerSettings: updatedSettings,
    }));

    // Clear validation error for this field
    const newValidationErrors = { ...validationErrors };
    delete newValidationErrors[`minAmount-${index}`];
    delete newValidationErrors[`maxAmount-${index}`];
    setValidationErrors(newValidationErrors);
  };

  const onSettingToggle = (index: number) => {
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      isEnabled: !updatedSettings[index].isEnabled,
    };
    setProviderSettings(updatedSettings);
    setFormState((prev) => ({
      ...prev,
      providerSettings: updatedSettings,
    }));
  };

  // Handler for reset button
  const onReset = () => {
    setProviderSettings([...originalSettings]);
    setSelectedPaymentMethod('');
    setSelectedStatus('');
    setSelectedTransactionType('');
    setProviderStatus(window.providerSettingsViewModel.Provider.Status);
    setFormState(originalFormState);
    setUpdateMessage(null);
    setValidationErrors({});
  };

  const hasChanged = (setting: any) => {
    const originalSetting = originalSettings.find(
      (s) =>
        s.methodId === setting.methodId &&
        s.status === setting.status &&
        s.transactionType === setting.transactionType
    );

    if (!originalSetting) return true;

    return (
      setting.isEnabled !== originalSetting.isEnabled ||
      setting.minAmount !== originalSetting.minAmount ||
      setting.maxAmount !== originalSetting.maxAmount
    );
  };

  const validateFormState = (): ValidationError[] => {
    const validationErrors: ValidationError[] = [];
    providerSettings.forEach((setting, index) => {
      const minAmount = parseFloat(setting.minAmount);
      const maxAmount = parseFloat(setting.maxAmount);

      if (isNaN(minAmount) || isNaN(maxAmount)) {
        validationErrors.push({
          index,
          field: 'minAmount',
          error: tGlobal('invalidAmountFormat'),
        });
      }

      if (minAmount > maxAmount) {
        validationErrors.push({
          index,
          field: 'minAmount',
          error: tGlobal('minAmountGreaterThanMax'),
        });
      }

      if (minAmount < 0) {
        validationErrors.push({
          index,
          field: 'minAmount',
          error: tGlobal('amountCannotBeNegative'),
        });
      }

      if (maxAmount < 0) {
        validationErrors.push({
          index,
          field: 'maxAmount',
          error: tGlobal('amountCannotBeNegative'),
        });
      }

      if (minAmount < setting.minAmountTrans) {
        validationErrors.push({
          index,
          field: 'minAmount',
          error: tPaymentProviderSettings('minAmountLessThanMinTrans'),
        });
      }

      if (maxAmount > setting.maxAmountTrans) {
        validationErrors.push({
          index,
          field: 'maxAmount',
          error: tPaymentProviderSettings('maxAmountGreaterThanMaxTrans'),
        });
      }
    });

    return validationErrors;
  };

  // Handler for update button
  const onUpdate = async () => {
    if (!hasFormChanged()) {
      setUpdateMessage({
        type: 'error',
        text: tGlobal('thereNoChangeToUpdate'),
      });
      return;
    }

    const validationErrors = validateFormState();
    const errorMap: { [key: string]: string } = {};
    validationErrors.forEach((err) => {
      errorMap[`${err.field}-${err.index}`] = err.error;
    });
    setValidationErrors(errorMap);

    if (validationErrors.length > 0) {
      setUpdateMessage({
        type: 'error',
        text: validationErrors
          .map((err) => `${providerSettings[err.index].methodName}: ${err.error}`)
          .join('<br />'),
      });
      return;
    }

    setValidationErrors({});
    setUpdating(true);
    setUpdateMessage(null);

    var provider = {
      providerId: window.providerSettingsViewModel.Provider.Id,
      name: window.providerSettingsViewModel.Provider.Name,
      status: providerStatus === originalFormState.providerStatus ? -1 : providerStatus ? 1 : 0,
      providerSettings: providerSettings.filter((setting) => hasChanged(setting)),
    };

    const response = await paymentProviderService.updateProvider(provider);
    if (response.error) {
      setUpdateMessage({
        type: 'error',
        text: response.error.message,
      });
      return;
    }

    const result = response.data;
    if (result.isSuccessful) {
      setUpdateMessage({
        type: 'success',
        text: tGlobal('updateSuccess'),
      });

      setTimeout(() => {
        reloadPage();
      }, 3000);
    } else {
      setUpdateMessage({
        type: 'error',
        text: result.message || tGlobal('updateFailed'),
      });
    }

    setUpdating(false);
  };

  // Handler for provider status toggle
  const onProviderStatusToggle = () => {
    const newStatus = !providerStatus;
    setProviderStatus(newStatus);
    setFormState((prev) => ({
      ...prev,
      providerStatus: newStatus ? 1 : 0,
    }));
  };

  // Handler for retrying setup
  const onRetrySetup = async (index: number) => {
    setSelectedSetting(providerSettings[index]);
    setShowUpdatePaymentMethod(true);
  };

  const onCloseUpdatePaymentMethod = () => {
    setShowUpdatePaymentMethod(false);
    setSelectedSetting(null);
  };

  const failStatus = paymentMethodStatusConstant.fail;
  const isEditable = (paymentMethod: PaymentMethod) => {
    return paymentMethod.status === paymentMethodStatusConstant.completed;
  };
  const hasEditableSettings = () => {
    return providerSettings.some((setting) => isEditable(setting));
  };

  const hasFormChanged = () => {
    // Check if provider status has changed
    if (formState.providerStatus !== originalFormState.providerStatus) {
      return true;
    }

    // Check if any provider settings have changed
    if (formState.providerSettings.length !== originalFormState.providerSettings.length) {
      return true;
    }

    // Compare each setting's properties
    return formState.providerSettings.some((setting, index) => {
      const originalSetting = originalFormState.providerSettings[index];
      return (
        setting.isEnabled !== originalSetting.isEnabled ||
        setting.minAmount !== originalSetting.minAmount ||
        setting.maxAmount !== originalSetting.maxAmount
      );
    });
  };

  const renderStatusComponent = (status: number) => {
    switch (status) {
      case paymentMethodStatusConstant.processing:
        return <StatusProcessingComponent />;
      case paymentMethodStatusConstant.completed:
        return <StatusCompletedComponent />;
      case paymentMethodStatusConstant.fail:
        return <StatusFailedComponent />;
      case paymentMethodStatusConstant.maintenance:
        return <StatusMaintenanceComponent />;
      default:
        return <StatusNoneComponent />;
    }
  };

  return (
    <div className="payment-provider-settings h-full">
      {loading ? (<LoadingFullComponent />) : 
        error ? (<ResultMessageComponent type="error" message={error} />) : 
        (
        <>
          {updateMessage && (
            <ResultMessageComponent
              className="my-[16px] max-[768px]:mx-[16px] max-[768px]:my-0"
              type={updateMessage.type}
              message={updateMessage.text}
            />
          )}
          <div className="py-[10px] mb-[8px]">
            <div className="settings-header">
              <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-color)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
                <BackButtonComponent onClick={onBackNavigation} />
                <TitleComponent title={window.providerSettingsViewModel.Provider.Name} />
              </div>
            </div>
            <div className="flex gap-[24px] max-[768px]:flex-col max-[768px]:gap-[8px]">
              <div>
                <LabelTitleComponent text={tPaymentProviderSettings('setUpPaymentMethodsForMembers')} />
                <ButtonFullComponent
                  text={`+ ${tPaymentProviderSettings('addPaymentMethod')}`}
                  onClick={onAddPaymentMethod}
                />
              </div>
              {providerStatus != paymentProviderStatusConstant.notConfigured && (
                <div>
                  <p className="flex items-center">
                    <LabelTitleComponent text={`${tGlobal('enable')}/${tGlobal('disable')}`} />
                    <Tippy
                      content={
                        <div>
                          <div>
                            <IconInfo className="w-[18px] h-[18px] fill-[#ffffff] inline-block vertical-middle mr-[4px] relative top-[-2px]" />
                            {tPaymentProviderSettings('enableDisable')}
                          </div>
                          <div>• {tPaymentProviderSettings('enableDisableTip1')}</div>
                          <div>• {tPaymentProviderSettings('enableDisableTip2')}</div>
                          <div>• {tPaymentProviderSettings('enableDisableTip3')}</div>
                        </div>
                      }
                      placement="top"
                      theme="light"
                      arrow={true}
                    >
                      <span className="cursor-pointer">
                        <IconInfo className="w-[20px] h-[20px] ml-[4px] fill-[#118AF2]" />
                      </span>
                    </Tippy>
                  </p>
                  <label className="inline-block">
                    <input
                      type="checkbox"
                      id="enableToggle"
                      checked={providerStatus}
                      onChange={onProviderStatusToggle}
                      className="opacity-0 absolute hidden peer "
                    />
                    <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]'></span>
                  </label>
                </div>
              )}
            </div>
          </div>
          <div className="py-[10px]">
            <div className="flex flex-wrap mb-[8px]">
              <MediumTitleComponent text={window.providerSettingsViewModel.Provider.Name} />
              <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tPaymentProviderSettings('paymentMethod')} />
                  <select
                    className="control-field"
                    value={selectedPaymentMethod}
                    onChange={onPaymentMethodChange}
                  >
                    {window.providerSettingsViewModel.AddPaymentMethodViewModel.PaymentMethods.map(
                      (method: any) => (
                        <option key={method.Id} value={method.Id}>
                          {method.Id === 0 ? tGlobal('all') : method.Name}
                        </option>
                      )
                    )}
                  </select>
                </div>

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tPaymentProviderSettings('status')} />
                  <select
                    className="control-field"
                    value={selectedStatus}
                    onChange={onStatusChange}
                  >
                    {STATUS_OPTIONS.map((status) => (
                      <option key={status.id} value={status.id}>
                        {tPaymentProviderSettings(`status-${status.id}`)}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tPaymentProviderSettings('transactionType')} />
                  <select
                    className="control-field"
                    value={selectedTransactionType}
                    onChange={onTransactionTypeChange}
                  >
                    {TRANSACTION_TYPES.map((type) => (
                      <option key={type.id} value={type.id}>
                        {type.id === 0
                          ? tGlobal('all')
                          : tPaymentProviderSettings(`transaction-${type.id}`)}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <ButtonOutlineComponent
                className="ml-auto mt-auto"
                text={tPaymentProviderSettings('viewProviderFees', { providerName: window.providerSettingsViewModel.Provider.Name })}
                onClick={onViewFees}
              />
            </div>

            <div className="table-fixed-header table-fixed-body">
              <table className=" w-full">
                <thead>
                  <tr>
                    <th className="col-shadown col-payment-method">
                      <span className="relative z-3">
                        {tPaymentProviderSettings('paymentMethod')}
                      </span>
                    </th>
                    <th>{tPaymentProviderSettings('status')}</th>
                    <th>{tPaymentProviderSettings('transactionType')}</th>
                    <th>{tPaymentProviderSettings('minimum')}</th>
                    <th>{tPaymentProviderSettings('maximum')}</th>
                    <th>
                      <div className="flex items-center justify-center">
                        {tGlobal('enable')}/{tGlobal('disable')}
                        <Tippy
                          content={
                            <div>
                              <div>
                                <IconInfo className="w-[18px] h-[18px] fill-[#ffffff] inline-block vertical-middle mr-[4px] relative top-[-2px]" />
                                {tPaymentProviderSettings('enableDisable')}
                              </div>
                              <div>• {tPaymentProviderSettings('enableDisableTip1')}</div>
                              <div>• {tPaymentProviderSettings('enableDisableTip2')}</div>
                              <div>• {tPaymentProviderSettings('enableDisableTip3')}</div>
                            </div>
                          }
                          placement="top"
                          theme="light"
                          arrow={true}
                        >
                          <span className="cursor-pointer">
                            <IconInfo />
                          </span>
                        </Tippy>
                      </div>
                    </th>
                    <th>{tPaymentProviderSettings('bank')}</th>
                  </tr>
                </thead>
                <tbody>
                  {providerSettings.map((setting, index) => (
                    <tr key={index}>
                      <td className="col-shadown col-payment-method font-semibold">
                        <span className="relative z-3">{setting.methodName}</span>
                      </td>
                      <td>
                        {renderStatusComponent(setting.status)}
                        {setting.status === failStatus && (
                          <>
                            <br />
                            <span
                              className="inline-block mt-[4px] cursor-pointer text-[var(--text-color-primary)] font-semibold underline"
                              onClick={() => onRetrySetup(index)}
                            >
                              {tPaymentProviderSettings('retrySetup')}
                            </span>
                          </>
                        )}
                      </td>
                      <td className="font-semibold">
                        {tPaymentProviderSettings(`transaction-${setting.transactionType}`)}
                      </td>
                      <td>
                        {isEditable(setting) ? (
                          <div className="flex flex-col max-[768px]:w-[150px]">
                            <input
                              type="text"
                              className={`control-field ${validationErrors[`minAmount-${index}`] ? 'error' : ''}`}
                              value={formatNumber(setting.minAmount)}
                              onChange={(e) => onMinAmountChange(index, e.target.value)}
                              placeholder={tPaymentProviderSettings('minimum')}
                              maxLength={15}
                            />
                            <label className="text-left text-[11px] max-[768px]:text-[15px] text-[#828282] font-semibold mt-[4px]">
                              {tPaymentProviderSettings('minAmount')}{' '}
                              {formatNumber(setting.minAmountTrans)}
                            </label>
                          </div>
                        ) : (
                          <div className="flex flex-col">-</div>
                        )}
                      </td>
                      <td>
                        {isEditable(setting) ? (
                          <div className="flex flex-col max-[768px]:w-[150px]">
                            <input
                              type="text"
                              className={`control-field ${validationErrors[`maxAmount-${index}`] ? 'error' : ''}`}
                              value={formatNumber(setting.maxAmount)}
                              onChange={(e) => onMaxAmountChange(index, e.target.value)}
                              placeholder={tPaymentProviderSettings('maximum')}
                              maxLength={15}
                            />
                            <label className="text-left text-[11px] max-[768px]:text-[15px] text-[#666666] font-semibold mt-[4px]">
                              {tPaymentProviderSettings('maxAmount')}{' '}
                              {formatNumber(setting.maxAmountTrans)}
                            </label>
                          </div>
                        ) : (
                          <div className="flex flex-col">-</div>
                        )}
                      </td>
                      <td>
                        {isEditable(setting) ? (
                          <div className="flex items-center justify-center">
                            <label className="cursor-pointer ">
                              <input
                                type="checkbox"
                                id={`toggle-${index}`}
                                checked={setting.isEnabled || false}
                                onChange={() => onSettingToggle(index)}
                                className="opacity-0 absolute hidden peer"
                              />
                              <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]'></span>
                            </label>
                          </div>
                        ) : (
                          <div className="flex flex-col">-</div>
                        )}
                      </td>
                      <td>
                        {!isEditable(setting) ? (<>-</>) : 
                          setting.bankRequired ? 
                            (<ButtonOutlineComponent text={tPaymentProviderSettings('listOfBanks')} onClick={() => onViewBanks(setting)} />) : 
                            (<span className="font-semibold color-[#333333]">{tPaymentProviderSettings('noBankRequired')}{' '}</span>)
                        }
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="flex justify-start gap-[8px] py-[8px] mt-[8px]">
              <ButtonCancelComponent text={tGlobal('reset')} onClick={onReset} />
              <ButtonApplyComponent
                text={updating ? tGlobal('updating') : tGlobal('update')}
                onClick={onUpdate}
                disabled={providerStatus == paymentProviderStatusConstant.notConfigured || updating || !hasEditableSettings()}
              />
            </div>
          </div>
        </>
      )}

      {/* Fee Popup Modal */}
      {showFeePopup && (
        <ProviderFeesPopupModal
          provider={
            {
              id: window.providerSettingsViewModel.Provider.Id,
              name: window.providerSettingsViewModel.Provider.Name,
            } as PaymentProvider
          }
          onClose={onCloseFeePopup}
        />
      )}

      {/* Add Payment Method Modal */}
      {showAddPaymentMethod && (
        <AddPaymentMethodModal
          providerId={window.providerSettingsViewModel.Provider.Id}
          onClose={onCloseAddPaymentMethod}
          onSuccess={reloadPage}
          orgTelegramAssistantUrl={
            window.providerSettingsViewModel.AddPaymentMethodViewModel.ChannelContact.Tele
          }
        />
      )}

      {/* Banks Popup Modal */}
      {showBanksPopup && selectedSetting && (
        <BanksPopupModal onClose={onCloseBanksPopup} transactionType={selectedSetting.transactionType} />
      )}

      {/* Update Payment Method Modal */}
      {showUpdatePaymentMethod && selectedSetting && (
        <UpdatePaymentMethodModal
          providerId={window.providerSettingsViewModel.Provider.Id}
          onClose={onCloseUpdatePaymentMethod}
          onSuccess={reloadPage}
          orgTelegramAssistantUrl={
            window.providerSettingsViewModel.AddPaymentMethodViewModel.ChannelContact.Tele
          }
          existingPaymentMethod={selectedSetting}
        />
      )}
    </div>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PaymentProviderSettings />
  </React.StrictMode>
);
