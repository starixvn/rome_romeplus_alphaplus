import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';

import { STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import AddPaymentMethodModal from './AddPaymentMethodModal';
import BanksPopupModal from './BanksPopupModal';
import UpdatePaymentMethodModal from './UpdatePaymentMethodModal';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import { formatNumber, formatInputValue } from '../../utils/numberFormatting';
import { IconInfo } from '../../components/Icons';
import Tippy from '@tippyjs/react';
import i18n from '../../i18n/i18n-paymentProviderSettings';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import { ResultMessageComponent } from '../../components/MessageComponents';
import '../../styles/common.css';
import '../../styles/global.scss';

import Loading from '../../components/LoadingComponent';
import { ButtonApply, ButtonCancel, ButtonFull, ButtonOutline } from '../../components/ButtonComponents';
import { PaymentMethod } from '../../services/interfaces/Payments';
import BackComponent from '../../components/BackComponent';
declare global {
  interface Window {
    providerSettingsViewModel: any;
  }
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  useCookieLanguage(i18n);
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
  const [updateMessage, setUpdateMessage] = React.useState<{type: 'success' | 'error', text: string} | null>(null);
  const [providerStatus, setProviderStatus] = React.useState(window.providerSettingsViewModel.Provider.Status);
  const [validationErrors, setValidationErrors] = React.useState<{[key: string]: string}>({});
  
  // State for dropdowns
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedStatus, setSelectedStatus] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');

  // Fetch provider settings when component mounts or when dropdowns change
  React.useEffect(() => {
    const fetchProviderSettings = async () => {
      try {
        setLoading(true);
        const method = selectedPaymentMethod ? parseInt(selectedPaymentMethod) : 0;
        const status = selectedStatus ? parseInt(selectedStatus) : 0;
        const transactionType = selectedTransactionType ? parseInt(selectedTransactionType) : 0;
        
        const settings = await paymentProviderService.getProviderSettings(
          window.providerSettingsViewModel.Provider.Id,
          method,
          status,
          transactionType
        );
        setProviderSettings(settings);
        setOriginalSettings(settings);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch provider settings:', err);
        setError('Failed to load provider settings. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchProviderSettings();
  }, [selectedPaymentMethod, selectedStatus, selectedTransactionType]);

  const handlePaymentMethodChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedPaymentMethod(event.target.value);
  };

  const handleStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedStatus(event.target.value);
  };

  const handleTransactionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedTransactionType(event.target.value);
  };

  const handleViewFees = () => {
    setShowFeePopup(true);
  };

  const handleCloseFeePopup = () => {
    setShowFeePopup(false);
  };

  const handleAddPaymentMethod = () => {
    setShowAddPaymentMethod(true);
  };

  const handleCloseAddPaymentMethod = () => {
    setShowAddPaymentMethod(false);
  };

  // Handler for viewing banks
  const handleViewBanks = (setting: any) => {
    setSelectedSetting(setting);
    setShowBanksPopup(true);
  };

  const handleCloseBanksPopup = () => {
    setShowBanksPopup(false);
    setSelectedSetting(null);
  };

  // Handler for minimum amount change
  const handleMinAmountChange = (index: number, value: string) => {
    const formattedValue = formatInputValue(value);
    
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      minAmount: formattedValue
    };
    setProviderSettings(updatedSettings);
    
    // Clear validation error for this field
    const newValidationErrors = {...validationErrors};
    delete newValidationErrors[`minAmount-${index}`];
    delete newValidationErrors[`maxAmount-${index}`];
    setValidationErrors(newValidationErrors);
  };

  // Handler for maximum amount change
  const handleMaxAmountChange = (index: number, value: string) => {
    const formattedValue = formatInputValue(value);
    
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      maxAmount: formattedValue
    };
    setProviderSettings(updatedSettings);
    
    // Clear validation error for this field
    const newValidationErrors = {...validationErrors};
    delete newValidationErrors[`minAmount-${index}`];
    delete newValidationErrors[`maxAmount-${index}`];
    setValidationErrors(newValidationErrors);
  };

  // Handler for toggle change
  const handleSettingToggle = (index: number) => {
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      isEnabled: !updatedSettings[index].isEnabled
    };
    setProviderSettings(updatedSettings);
  };

  // Handler for reset button
  const handleReset = () => {
    setProviderSettings([...originalSettings]);
    setSelectedPaymentMethod('');
    setSelectedStatus('');
    setSelectedTransactionType('');
    setProviderStatus(window.providerSettingsViewModel.Provider.Status);
    setUpdateMessage(null);
  };

  // Handler for update button
  const handleUpdate = async () => {
    try {
      // Validate min and max amounts
      interface ValidationError {
        index: number;
        field: string;
        error: string;
      }

      const validationErrors: ValidationError[] = [];
      providerSettings.forEach((setting, index) => {
        const minAmount = parseFloat(setting.minAmount);
        const maxAmount = parseFloat(setting.maxAmount);
        
        if (isNaN(minAmount) || isNaN(maxAmount)) {
          validationErrors.push({
            index,
            field: 'minAmount',
            error: tGlobal('invalidAmountFormat')
          } as ValidationError);
        }
        
        if (minAmount > maxAmount) {
          validationErrors.push({
            index,
            field: 'minAmount',
            error: tGlobal('minAmountGreaterThanMax')
          } as ValidationError);
        }

        if (minAmount < 0 ) {
          validationErrors.push({
            index,
            field: 'minAmount',
            error: tGlobal('amountCannotBeNegative')
          } as ValidationError);
        }

        if ( maxAmount < 0) {
          validationErrors.push({
            index,
            field: 'maxAmount',
            error: tGlobal('amountCannotBeNegative')
          } as ValidationError);
        }

        if (minAmount < setting.minAmountTrans) {
          validationErrors.push({
            index,
            field: 'minAmount',
            error: tPaymentProviderSettings('minAmountLessThanMinTrans')
          } as ValidationError);
        }

        if (maxAmount > setting.maxAmountTrans) {
          validationErrors.push({
            index,  
            field: 'maxAmount',
            error: tPaymentProviderSettings('maxAmountGreaterThanMaxTrans')
          } as ValidationError);
        }
      });

      // Create error map for input fields
      const errorMap: {[key: string]: string} = {};
      validationErrors.forEach(err => {
        errorMap[`${err.field}-${err.index}`] = err.error;
      });
      setValidationErrors(errorMap);

      if (validationErrors.length > 0) {
        setUpdateMessage({
          type: 'error',
          text: validationErrors.map(err => 
            `${tGlobal('row')} ${err.index + 1}: ${err.error}`
          ).join('<br />')
        });
        return;
      }

      setValidationErrors({});
      setUpdating(true);
      setUpdateMessage(null);
      
      var provider = {
        providerId: window.providerSettingsViewModel.Provider.Id,
        name: window.providerSettingsViewModel.Provider.Name,
        status: providerStatus ? 1 : 0,
        providerSettings: providerSettings
      };

      // Call the API to update provider
      const result = await paymentProviderService.updateProvider(provider);
      
      if (result.isSuccessful) {
        setUpdateMessage({
          type: 'success',
          text: tGlobal('updateSuccess')
        });
        // Update original settings to match current settings after successful update
        setOriginalSettings([...providerSettings]);
      } else {
        setUpdateMessage({
          type: 'error',
          text: result.message || tGlobal('updateFailed')
        });
      }
    } catch (err) {
      console.error('Failed to update provider:', err);
      setUpdateMessage({
        type: 'error',
        text: tGlobal('updateFailed')
      });
    } finally {
      setUpdating(false);
    }
  };

  // Handler for provider status toggle
  const handleProviderStatusToggle = () => {
    setProviderStatus(!providerStatus);
    // You might want to add API call here to update the provider status on the server
  };

  // Handler for retrying setup
  const handleRetrySetup = async (index: number) => {
    setSelectedSetting(providerSettings[index]);
    setShowUpdatePaymentMethod(true);
  };

  const handleCloseUpdatePaymentMethod = () => {
    setShowUpdatePaymentMethod(false);
    setSelectedSetting(null);
  };

  const failStatus = 3;
  const isEditable = (paymentMethod: PaymentMethod) => {
    return paymentMethod.status !== 3 && paymentMethod.status !== 4;
  }

  const hasEditableSettings = () => {
    return providerSettings.some(setting => isEditable(setting));
  };

  const handleBackNavigation = () => {
    const previousUrl = localStorage.getItem('cashsite_previousUrl');
    if (previousUrl) {
      localStorage.removeItem('cashsite_previousUrl');
      window.location.href = previousUrl;
    } else {
      window.history.back();
    }
  };

  return (
    <div className="payment-provider-settings h-full">       
      {loading ? (
        <div className='flex justify-center items-center h-full'>
          <Loading />
        </div>
      ) : error ? (
        <ResultMessageComponent type='error' message={error} />
      ) : (
        <>        
        <BackComponent onClose={handleBackNavigation} />
        {updateMessage && (<ResultMessageComponent className='my-[16px] max-[768px]:mx-[16px] max-[768px]:my-0' type={updateMessage.type} message={updateMessage.text} /> )} 
        <div className='bg-[#ffffff] px-[16px] py-[10px] mb-[8px]'>
          <div className="settings-header">
            <h2 className='pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold max-[768px]:text-[20px] '>{window.providerSettingsViewModel.Provider.Name}</h2>
          </div>
          <div className="flex gap-[24px] max-[768px]:flex-col max-[768px]:gap-[8px]">
            <div>      
            <p className="block font-semibold text-[#333333] my-[8px]">
              {tPaymentProviderSettings('setUpPaymentMethodsForMembers')}
            </p>   
            <ButtonFull text={`+ ${tPaymentProviderSettings('addPaymentMethod')}`} onClick={handleAddPaymentMethod} />            
            </div>
            <div>
              <p className="flex items-center font-semibold text-[#333333] my-[8px]">
                {tGlobal('enable')}/{tGlobal('disable')}
                <Tippy
                  content={
                    <div>
                      <div>
                        <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i> 
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
                  <span className='cursor-pointer'><IconInfo className="w-[20px] h-[20px] ml-[4px] fill-[#163f78]" /></span>
                </Tippy>
              </p>
              <label className='inline-block'>
                <input
                  type="checkbox"
                  id="enableToggle"
                  checked={providerStatus}
                  onChange={handleProviderStatusToggle}
                  className="opacity-0 absolute hidden peer " 
                />
                <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]'>

                </span>
              </label>
            </div>            
          </div>
        </div>
        <div className='bg-[#ffffff] px-[16px] py-[10px]'>
          <div className='flex flex-wrap mb-[8px]'>
            <h2 className='w-[100%] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold max-[768px]:text-[20px] '>{window.providerSettingsViewModel.Provider.Name}</h2>
            <div className='flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
            <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
              <label className="block font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('paymentMethod')}</label>
              <select 
                className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer" 
                value={selectedPaymentMethod}
                onChange={handlePaymentMethodChange}
              >
                {window.providerSettingsViewModel.AddPaymentMethodViewModel.PaymentMethods.map((method: any) => (
                  <option key={method.Id} value={method.Id}>{method.Id === 0 ? tGlobal('all') : method.Name}</option>
                ))}
              </select>
            </div>

            <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
              <label className="block font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('status')}</label>
              <select 
                className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer" 
                value={selectedStatus}
                onChange={handleStatusChange}
              >
                {STATUS_OPTIONS.map(status => (
                  <option key={status.id} value={status.id}>{tPaymentProviderSettings(`status-${status.id}`)}</option>
                ))}
              </select>
            </div>

            <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
              <label className="block  font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('transactionType')}</label>
              <select 
                className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer" 
                value={selectedTransactionType}
                onChange={handleTransactionTypeChange}
              >
                {TRANSACTION_TYPES.map(type => (
                  <option key={type.id} value={type.id}>{type.id === 0 ? tGlobal('all') : tPaymentProviderSettings(`transaction-${type.id}`)}</option>
                ))}
              </select>
            </div> 
            </div>   
        
            <ButtonOutline className="ml-auto mt-auto" text={tPaymentProviderSettings('viewProviderFees', {providerName: window.providerSettingsViewModel.Provider.Name})} onClick={handleViewFees} />
          </div>
          
          <div className="table-fixed-header table-fixed-body">
            <table className=" w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
              <thead>
                <tr>
                  <th className='col-shadown col-payment-method'><span className='relative z-3'>{tPaymentProviderSettings('paymentMethod')}</span></th>
                  <th>{tPaymentProviderSettings('status')}</th>
                  <th>{tPaymentProviderSettings('transactionType')}</th>
                  <th>{tPaymentProviderSettings('minimum')}</th>
                  <th>{tPaymentProviderSettings('maximum')}</th>
                  <th>
                    <div className='flex items-center justify-center'>
                      {tGlobal('enable')}/{tGlobal('disable')}
                      <Tippy
                        content={
                          <div>
                            <div>
                              <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i> 
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
                        <span className='cursor-pointer'><IconInfo className="w-[20px] h-[20px] ml-[4px] fill-[#163f78]" /></span>
                      </Tippy>
                    </div>
                </th>
                  <th>{tPaymentProviderSettings('bank')}</th>
                </tr>
              </thead>
              <tbody>
                {providerSettings.map((setting, index) => (
                  <tr key={index}>
                    <td className='col-shadown col-payment-method font-semibold'><span className='relative z-3'>{setting.methodName}</span></td>
                    <td>
                      <span className={`inline-block min-w-[90px] text-center rounded-[20px] font-semibold px-[10px] py-[3px] ${
                        setting.status === 1 ? 'bg-[#90d3ff]' :
                        setting.status === 2 ? 'bg-[#D4E7D9]' :
                        setting.status === 3 ? 'bg-[#FBD8D8]' :
                        setting.status === 4 ? 'bg-[#fcae7b]' :
                        'bg-[#E0E0E0]'
                      }`}>{tPaymentProviderSettings(`status-${setting.status}`)}</span>
                      {setting.status === failStatus && <><br /><span className='inline-block mt-[4px] cursor-pointer text-[var(--text-color-primary)] font-semibold underline' onClick={() => handleRetrySetup(index)}>{tPaymentProviderSettings('retrySetup')}</span></>}
                    </td>
                    <td className='font-semibold'>{tPaymentProviderSettings(`transaction-${setting.transactionType}`)}</td>
                    <td>
                      {isEditable(setting) ? (
                      <div className='flex flex-col max-[768px]:w-[150px]'>
                        <input 
                          type="text" 
                          className={`h-[35px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer mb-2px] ${validationErrors[`minAmount-${index}`] ? 'border-[#dc3545]' : 'border-[var(--border-control)]'}`}
                          value={formatNumber(setting.minAmount)} 
                          onChange={(e) => handleMinAmountChange(index, e.target.value)}
                          placeholder={tPaymentProviderSettings('minimum')}
                        />
                        <label className="text-left text-[11px] max-[768px]:text-[15px] text-[#828282] font-semibold mt-[4px]">{tPaymentProviderSettings('minAmount')} {formatNumber(setting.minAmountTrans)}</label>
                      </div>
                      ) : (
                        <div className='flex flex-col'>-</div>
                      )}
                    </td>
                    <td>
                      {isEditable(setting) ? (
                      <div className='flex flex-col max-[768px]:w-[150px]'>
                      <input 
                        type="text" 
                        className={`h-[35px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer mb-2px] ${validationErrors[`maxAmount-${index}`] ? 'border-[#dc3545]' : 'border-[var(--border-control)]'}`}
                        value={formatNumber(setting.maxAmount)} 
                        onChange={(e) => handleMaxAmountChange(index, e.target.value)}
                        placeholder={tPaymentProviderSettings('maximum')}
                        />
                        <label className="text-left text-[11px] max-[768px]:text-[15px] text-[#828282] font-semibold mt-[4px]">{tPaymentProviderSettings('maxAmount')} {formatNumber(setting.maxAmountTrans)}</label>
                      </div>
                      ) : (
                        <div className='flex flex-col'>-</div>
                      )}
                    </td>
                    <td>   
                      {isEditable(setting) ? (
                      <div className="flex items-center justify-center">
                        <label className='cursor-pointer '>
                          <input
                            type="checkbox"
                            id={`toggle-${index}`}
                            checked={setting.isEnabled || false}
                            onChange={() => handleSettingToggle(index)}
                            className="opacity-0 absolute hidden peer"
                          />
                          <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[var(--button-switch-background)] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[var(--button-switch-dot-background)] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--button-switch-background-active)]'></span>
                        </label>                        
                      </div>
                      ) : (
                        <div className='flex flex-col'>-</div>
                      )}
                    </td>
                    <td>     
                      {!isEditable(setting) ? <>-</> : setting.bankRequired ? (                        
                        <ButtonOutline text={tPaymentProviderSettings('listOfBanks')} onClick={() => handleViewBanks(setting)} />                   
                      ) : ( <span className='font-semibold color-[#333333]'>{tPaymentProviderSettings('noBankRequired')} </span>) }
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="flex justify-start gap-[8px] py-[8px] mt-[8px]">
            <ButtonCancel text={tGlobal('reset')} onClick={handleReset} />
            <ButtonApply text={updating ? tGlobal('updating') : tGlobal('update')} onClick={handleUpdate} disabled={updating || !hasEditableSettings()} />
          </div>          
         </div>
        </>
      )}

      {/* Fee Popup Modal */}
      {showFeePopup && (
        <ProviderFeesPopupModal
          provider={{id: window.providerSettingsViewModel.Provider.Id, name: window.providerSettingsViewModel.Provider.Name} as PaymentProvider}
          onClose={handleCloseFeePopup}
        />
      )}

      {/* Add Payment Method Modal */}
      {showAddPaymentMethod && (
        <AddPaymentMethodModal
          providerId={window.providerSettingsViewModel.Provider.Id}
          onClose={handleCloseAddPaymentMethod}   
          orgTelegramAssistantUrl={window.providerSettingsViewModel.AddPaymentMethodViewModel.ChannelContact.Tele}
        />
      )}

      {/* Banks Popup Modal */}
      {showBanksPopup && selectedSetting && (
        <BanksPopupModal
          onClose={handleCloseBanksPopup} 
          transactionType={selectedSetting.transactionType}
        />
      )}

      {/* Update Payment Method Modal */}
      {showUpdatePaymentMethod && selectedSetting && (
        <UpdatePaymentMethodModal
          providerId={window.providerSettingsViewModel.Provider.Id}
          onClose={handleCloseUpdatePaymentMethod}
          orgTelegramAssistantUrl={window.providerSettingsViewModel.AddPaymentMethodViewModel.ChannelContact.Tele}
          existingPaymentMethod={selectedSetting}
        />
      )}
    </div>
  );
};

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PaymentProviderSettings />
  </React.StrictMode>
); 