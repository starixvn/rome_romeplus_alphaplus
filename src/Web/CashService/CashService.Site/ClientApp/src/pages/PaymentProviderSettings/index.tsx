import '../../styles/common.css';
import '../../styles/global.scss';

import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';

import Tippy from '@tippyjs/react';

import {
    BackButtonComponent, ButtonApplyComponent, ButtonCancelComponent, ButtonFullComponent,
    DisableButtonComponent, EnableButtonComponent
} from '../../components/Buttons/index';
import { IconArrowDown, IconInfo, IconReload } from '../../components/Icons';
import { LoadingFullComponent } from '../../components/LoadingComponent';
import { ResultMessageComponent } from '../../components/Message';
import {
    StatusCompletedComponent, StatusFailedComponent, StatusMaintenanceComponent,
    StatusNoneComponent, StatusProcessingComponent
} from '../../components/StatusComponents';
import {
    TitleComponent,
    LabelTitleComponent, MediumTitleComponent
} from '../../components/Title';
import {
  paymentMethodStatusConstant,
  paymentProviderStatusConstant,
  STATUS_OPTIONS,
  TRANSACTION_TYPES,
} from '../../constants/paymentConstants';
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
import NoRecordFound from '../../components/NoRecordFoundComponent';

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
  const [isCashServiceEnabled, setIsCashServiceEnabled] = React.useState(false);

  const [formState, setFormState] = useState<FormState>({
    providerStatus: window.providerSettingsViewModel.Provider.Status,
    providerSettings: window.providerSettingsViewModel.Provider.ProviderSettings,
  });
  const [originalFormState] = useState<FormState>({
    providerStatus: window.providerSettingsViewModel.Provider.Status,
    providerSettings: window.providerSettingsViewModel.Provider.ProviderSettings,
  });

  React.useEffect(() => {
    const isCashServiceEnabled = document.getElementById('IsCashServiceEnabled')?.getAttribute('value');
    setIsCashServiceEnabled(isCashServiceEnabled === '1');
  }, []);

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
    <div className="payment-provider-settings">
      {loading ? (
        <LoadingFullComponent />
      ) : error ? (
        <ResultMessageComponent type="error" message={error} />
      ) : (
        <>
          {updateMessage && (
            <ResultMessageComponent
              className="mt-[10px] max-[768px]:mx-[16px] max-[768px]:my-0"
              type={updateMessage.type}
              message={updateMessage.text}
            />
          )}
         
          <div className="settings-header">
            <div className="flex items-center justify-flex-start mb-[12px] py-[8px] border-[var(--border-color)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
              <BackButtonComponent onClick={onBackNavigation} />
              <TitleComponent title={window.providerSettingsViewModel.Provider.Name} />                
            </div>
          </div>
        
          <div className="pb-[10px]">
            
              <MediumTitleComponent text={window.providerSettingsViewModel.Provider.Name} />
              <p className="my-[8px]">
                {tPaymentProviderSettings('RequirePaymentMethod')}
              </p>
              {providerStatus != paymentProviderStatusConstant.notConfigured && (
                  <div
                    className="mb-[10px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]"
                    role="group"
                  >
                    <EnableButtonComponent
                      onClick={onProviderStatusToggle}
                      isActive={providerStatus}
                    />
                    <DisableButtonComponent
                      onClick={onProviderStatusToggle}
                      isActive={!providerStatus}
                    />
                  </div>
                )}
              <div className="flex flex-wrap w-full">
                <div className="w-[160px] mr-[12px] mb-[12px]">
                  <LabelTitleComponent text={tPaymentProviderSettings('paymentMethod')} />
                  <div className="relative">
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
                    <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                  </div>
                </div>

                <div className="w-[160px] mr-[12px] mb-[12px]">
                  <LabelTitleComponent text={tPaymentProviderSettings('status')} />
                  <div className="relative">
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
                    <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                  </div>
                </div>

                <div className="w-[160px] mr-[12px] mb-[12px]">
                  <LabelTitleComponent text={tPaymentProviderSettings('transactionType')} />
                  <div className="relative">
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
                    <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                  </div>
                </div>
              </div>
             
            <div className="w-full border border-[var(--table-border-color)] border-b-0 p-[6px] flex">
              <ButtonFullComponent
                text={`${tPaymentProviderSettings('addPaymentMethod')}`}
                onClick={onAddPaymentMethod}
              />
              <ButtonFullComponent
                className="ml-auto"
                text={tPaymentProviderSettings('viewProviderFees', {
                  providerName: window.providerSettingsViewModel.Provider.Name,
                })}
                onClick={onViewFees}
              />
            </div>
            <div className="table table-fixed-body">
              <table className="w-full">
                <thead>
                  <tr>
                    <th className="col-shadown bg-[var(--table-header-background)] left-0 min-w-[113px] sticky z-11">
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
                      <td className="col-shadown bg-[var(--table-background)] left-0 min-w-[113px] sticky z-11 font-semibold">
                        <span className="relative z-3">{setting.methodName}</span>
                      </td>
                      <td>
                        <div className="inline-block relative px-[22px]">
                          {renderStatusComponent(setting.status)}
                          {setting.status === failStatus && (
                            <>
                              <Tippy
                                content={<div>{tPaymentProviderSettings('retrySetup')}</div>}
                                placement="top"
                                theme="light"
                                arrow={true}
                              >
                                <span
                                  className="cursor-pointer absolute right-[0px] top-[2px]"
                                  onClick={() => onRetrySetup(index)}
                                >
                                  <IconReload className="fill-[var(--icon-color-error)] cursor-pointer" />
                                </span>
                              </Tippy>
                            </>
                          )}
                        </div>
                      </td>
                      <td className="font-semibold">
                        {tPaymentProviderSettings(`transaction-${setting.transactionType}`)}
                      </td>
                      <td className="w-[240px]">
                        {isEditable(setting) ? (
                          <div className="flex items-center justify-start w-[220px] text-[12px]">
                            <input
                              type="text"
                              className={`control-field  w-[50%]! ${validationErrors[`minAmount-${index}`] ? 'error' : ''}`}
                              value={formatNumber(setting.minAmount)}
                              onChange={(e) => onMinAmountChange(index, e.target.value)}
                              placeholder={tPaymentProviderSettings('minimum')}
                              maxLength={15}
                            />
                            <label className="text-left text-[var(--text-color)]">
                              <span className="text-[14px] px-[4px]">&gt;=</span>
                              {formatNumber(setting.minAmountTrans)}
                            </label>
                          </div>
                        ) : (
                          <div className="flex flex-col">-</div>
                        )}
                      </td>
                      <td className="w-[240px]">
                        {isEditable(setting) ? (
                          <div className="flex items-center justify-start w-[220px] text-[12px]">
                            <input
                              type="text"
                              className={`control-field w-[50%]! ${validationErrors[`maxAmount-${index}`] ? 'error' : ''}`}
                              value={formatNumber(setting.maxAmount)}
                              onChange={(e) => onMaxAmountChange(index, e.target.value)}
                              placeholder={tPaymentProviderSettings('maximum')}
                              maxLength={15}
                            />
                            <label className="text-left text-[var(--text-color)]">
                              <span className="text-[14px] px-[4px]">&lt;=</span>
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
                              <span className='block cursor-pointer w-[60px] h-[28px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[22px] after:h-[22px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[34px] peer-checked:bg-[var(--button-switch-background-active)]'></span>
                            </label>
                          </div>
                        ) : (
                          <div className="flex flex-col">-</div>
                        )}
                      </td>
                      <td>
                        {!isEditable(setting) ? (
                          <>-</>
                        ) : setting.bankRequired ? (
                          <ButtonFullComponent
                            text={tPaymentProviderSettings('listOfBanks')}
                            onClick={() => onViewBanks(setting)}
                          />
                        ) : (
                          <span className="font-semibold">
                            {tPaymentProviderSettings('noBankRequired')}{' '}
                          </span>
                        )}
                      </td>
                    </tr>
                  ))}
                  {
                    providerSettings.length === 0 && (
                      <NoRecordFound colSpan={7} />
                    )
                  }
                </tbody>
              </table>
            </div>
            <div className="flex justify-start gap-[8px] py-[8px] mt-[8px]">
              <ButtonCancelComponent text={tGlobal('reset')} onClick={onReset} />
              <ButtonApplyComponent
                text={updating ? tGlobal('updating') : tGlobal('update')}
                type="submit"
                onClick={onUpdate}
                disabled={
                  !isCashServiceEnabled ||
                  providerStatus == paymentProviderStatusConstant.notConfigured ||
                  updating ||
                  !hasEditableSettings()
                }
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
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
          isCashServiceEnabled={isCashServiceEnabled}
          orgTelegramAssistantUrl={
            window.providerSettingsViewModel.AddPaymentMethodViewModel.ChannelContact.Tele
          }
        />
      )}

      {/* Banks Popup Modal */}
      {showBanksPopup && selectedSetting && (
        <BanksPopupModal
          onClose={onCloseBanksPopup}
          transactionType={selectedSetting.transactionType}
        />
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
          isCashServiceEnabled={isCashServiceEnabled}
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
