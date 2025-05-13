import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';

import { STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import AddPaymentMethodModal from './AddPaymentMethodModal';
import BanksPopupModal from './BanksPopupModal';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import { formatNumber, formatInputValue } from '../../utils/numberFormatting';
import { IconBack, IconInfo } from '../../components/Icons';
import Tippy from '@tippyjs/react';
import i18n from '../../i18n/i18n-paymentProviderSettings';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import { ResultMessage } from '../../components/ResultMessage';
import '../../styles/global.scss';
import './PaymentProviderSettings.css';

import Loading from '../../components/ui/Loading';
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
  const [selectedSetting, setSelectedSetting] = React.useState<any>(null);
  const [providerSettings, setProviderSettings] = React.useState<any[]>([]);
  const [originalSettings, setOriginalSettings] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  const [updating, setUpdating] = React.useState(false);
  const [updateMessage, setUpdateMessage] = React.useState<{type: 'success' | 'error', text: string} | null>(null);
  const [providerStatus, setProviderStatus] = React.useState(window.providerSettingsViewModel.Provider.Status);
  
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
      setUpdating(true);
      setUpdateMessage(null);
      
      var provider = {
        id: window.providerSettingsViewModel.Provider.Id,
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

  return (
    <div className="payment-provider-settings h-full">       
      {loading ? (
        <div className='flex justify-center items-center h-full'>
          <Loading />
        </div>
      ) : error ? (
        <div className="error-container">
          <p className="error-message">{error}</p>
        </div>
      ) : (
        <>
        <div className='text-[var(--primary-color)] px-[8px] py-[4px] border-b-[2px] border-[#cfcfcf] border-dotted mb-[4px]'>
          <span className='cursor-pointer inline-flex items-center py-[8px]' onClick={() => {
              document.location.href = document.referrer;
            }
          }>
            <IconBack className='w-[18px] h-[18px] fill-[var(--primary-color)] mr-[8px]'  />
            <span className='text-[12px] text-[var(--primary-color)] font-semibold'>{tGlobal('back')}</span>
          </span>
        </div>
        <div className='my-[16px]'>
          {updateMessage && (
            <ResultMessage
              type={updateMessage.type}
              message={updateMessage.text}
            />           
          )}
        </div>
        <div className='bg-[#ffffff] px-[16px] py-[10px] mb-[8px]'>
          <div className="settings-header">
            <h2 className='pb-[8px] mt-[8px] text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px] '>{window.providerSettingsViewModel.Provider.Name}</h2>
          </div>
          <div className="flex gap-[24px]">
            <div>      
            <p className="block text-[12px] font-semibold text-[#333333] my-[8px]">
              {tPaymentProviderSettings('setUpPaymentMethodsForMembers')}
            </p>
            <button className="w-[183px] h-[30px] rounded-[5px] text-[#ffffff] px-[16px] py-[4px] text-[12px] font-bold bg-[var(--primary-color)] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" onClick={handleAddPaymentMethod}>+{tPaymentProviderSettings('addPaymentMethod')}</button>
            </div>
            <div className="d-flex align-items-center">
              <label className="flex text-[12px] font-semibold text-[#333333] my-[8px]" htmlFor='enableToggle'>
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
              </label>
              <label className='inline-block'>
                <input
                  type="checkbox"
                  id="enableToggle"
                  checked={providerStatus}
                  onChange={handleProviderStatusToggle}
                  className="opacity-0 absolute hidden peer " 
                />
                <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[#c2c2c2] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[#ffffff] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--primary-color)]'>

                </span>
              </label>
            </div>            
          </div>
        </div>
        <div className='bg-[#ffffff] px-[16px] py-[10px]'>
          <div className='flex flex-wrap mb-[8px]'>
          <h2 className='w-[100%] mt-[8px] text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px] '>{window.providerSettingsViewModel.Provider.Name}</h2>
            <div className="w-[230px] pr-[14px]">
              <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('paymentMethod')}</label>
              <select 
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer" 
                value={selectedPaymentMethod}
                onChange={handlePaymentMethodChange}
              >
                {window.providerSettingsViewModel.PaymentMethods.map((method: any) => (
                  <option key={method.Id} value={method.Id}>{method.Id === 0 ? tGlobal('all') : method.Name}</option>
                ))}
              </select>
            </div>

            <div className="w-[230px] pr-[14px]">
              <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('status')}</label>
              <select 
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer" 
                value={selectedStatus}
                onChange={handleStatusChange}
              >
                {STATUS_OPTIONS.map(status => (
                  <option key={status.id} value={status.id}>{tPaymentProviderSettings(`status-${status.id}`)}</option>
                ))}
              </select>
            </div>

            <div className="w-[230px] pr-[14px]">
              <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tPaymentProviderSettings('transactionType')}</label>
              <select 
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer" 
                value={selectedTransactionType}
                onChange={handleTransactionTypeChange}
              >
                {TRANSACTION_TYPES.map(type => (
                  <option key={type.id} value={type.id}>{type.id === 0 ? tGlobal('all') : tPaymentProviderSettings(`transaction-${type.id}`)}</option>
                ))}
              </select>
            </div>
            <button className="rounded-[5px] text-[var(--primary-color)] p-[8px] text-[12px] font-bold border-[1px] border-[var(--primary-color)] ml-auto mt-auto bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer leading-[14px]" onClick={handleViewFees}>{tPaymentProviderSettings('viewProviderFees', {providerName: window.providerSettingsViewModel.Provider.Name})}</button>
          </div>
          
          <div className="table-fixed-header table-fixed-body">
            <table className="text-[12px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
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
                    <td><span className='rounded-[20px] font-semibold px-[10px] py-[3px]  bg-[#AFE9D7]'>{tPaymentProviderSettings(`status-${setting.status}`)}</span></td>
                    <td className='font-semibold'>{tPaymentProviderSettings(`transaction-${setting.transactionType}`)}</td>
                    <td>
                      <div className='flex flex-col'>
                        <input 
                          type="text" 
                          className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer mb-2px]" 
                          value={formatNumber(setting.minAmount)} 
                          onChange={(e) => handleMinAmountChange(index, e.target.value)}
                          placeholder={tPaymentProviderSettings('minimum')}
                        />
                        <label className="text-left text-[11px] text-[#828282] font-semibold">{tPaymentProviderSettings('minAmount')} {formatNumber(setting.minAmountTrans)}</label>
                      </div>
                    </td>
                    <td>
                      <div className='flex flex-col'>
                      <input 
                        type="text" 
                        className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer mb-2px]m" 
                        value={formatNumber(setting.maxAmount)} 
                        onChange={(e) => handleMaxAmountChange(index, e.target.value)}
                        placeholder={tPaymentProviderSettings('maximum')}
                      />
                      <label className="text-left text-[11px] text-[#828282] font-semibold">{tPaymentProviderSettings('maxAmount')} {formatNumber(setting.maxAmountTrans)}</label>
                      </div>
                    </td>
                    <td>
                      <div className="flex items-center justify-center">
                        <label className='cursor-pointer '>
                          <input
                            type="checkbox"
                            id={`toggle-${index}`}
                            checked={setting.isEnabled || false}
                            onChange={() => handleSettingToggle(index)}
                            className="opacity-0 absolute hidden peer"
                          />
                          <span className='block cursor-pointer w-[60px] h-[30px] rounded-[20px] bg-[#c2c2c2] relative after:content-[""] after:absolute after:top-[3px] after:left-[3px] after:w-[24px] after:h-[24px] after:rounded-[50%] after:bg-[#ffffff] after:transition-all after:duration-300 after:ease-in-out after:shadow-[0_2px_4px_0_rgba(0,0,0,0.25)] peer-checked:after:left-[32px] peer-checked:bg-[var(--primary-color)]'></span>
                        </label>                        
                      </div>
                    </td>
                    <td>
                      {setting.bankRequired ? (
                        <button 
                          className="rounded-[5px] text-[var(--primary-color)] p-[8px] text-[12px] font-bold border-[1px] border-[var(--primary-color)] ml-auto mt-auto bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer leading-[14px]"
                          onClick={() => handleViewBanks(setting)}
                        >
                          {tPaymentProviderSettings('listOfBanks')}
                        </button>
                      ) : ( <span className='font-semibold color-[#333333]'>{tPaymentProviderSettings('noBankRequired')} </span>) }

                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="flex justify-end py-[8px] mt-[8px]">
            <button className="w-[140px] h-[30px] rounded-[20px] text-[var(--primary-color)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--primary-color)]  bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer " onClick={handleReset}>{tGlobal('reset')}</button>
            <button 
              className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--primary-color)] ml-[8px] bg-[var(--primary-color)] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" 
              onClick={handleUpdate}
              disabled={updating}
            >
              {updating ? tGlobal('updating') : tGlobal('update')}
            </button>
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
        />
      )}

      {/* Banks Popup Modal */}
      {showBanksPopup && selectedSetting && (
        <BanksPopupModal
          onClose={handleCloseBanksPopup} 
          transactionType={selectedSetting.transactionType}
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