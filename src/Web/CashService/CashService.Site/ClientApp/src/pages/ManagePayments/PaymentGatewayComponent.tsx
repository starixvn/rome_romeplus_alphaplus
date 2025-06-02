import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonFullComponent,
  ButtonOutlineComponent,
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
} from '../../components/ButtonComponents';
import { IconInfo, IconPen } from '../../components/Icons';
import Loading from '../../components/LoadingComponent';
import { ErrorMessageComponent } from '../../components/MessageComponents';
import {
  StatusEnabledComponent,
  StatusDisabledComponent,
  StatusNoneComponent,
} from '../../components/StatusComponents';
import TitleComponent, { MediumTitleComponent } from '../../components/TitleComponents';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';

interface PaymentGatewayComponentProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
  activeTab: string;
}

const PaymentGatewayComponent: React.FC<PaymentGatewayComponentProps> = ({
  setActiveTab,
  activeTab,
}) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [providers, setProviders] = React.useState<PaymentProvider[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [selectedProvider, setSelectedProvider] = React.useState<PaymentProvider | null>(null);
  const [showFeePopup, setShowFeePopup] = React.useState<boolean>(false);
  const [showDailyLimitPopup, setShowDailyLimitPopup] = React.useState<boolean>(false);

  React.useEffect(() => {
    const fetchProviders = async () => {
      setLoading(true);
      const response = await paymentProviderService.getProviders();

      if (response.error) {
        setError(response.error.message);
        setLoading(false);
        return;
      }

      const data = response.data || [];
      setProviders(data);
      setError(null);
      setLoading(false);
    };

    fetchProviders();
  }, []);

  const onFeeClick = (provider: PaymentProvider) => {
    setSelectedProvider(provider);
    setShowFeePopup(true);
  };

  const onCloseFeePopup = () => {
    setShowFeePopup(false);
    setSelectedProvider(null);
  };

  const onSetLimitClick = () => {
    setShowDailyLimitPopup(true);
  };

  const onCloseDailyLimitPopup = () => {
    setShowDailyLimitPopup(false);
  };

  const onSaveDailyLimit = () => {};

  const onEditClick = (provider: PaymentProvider) => {
    localStorage.setItem('cashsite_previousUrl', window.location.href);
    location.href = 'ManageProviderSettings?id=' + provider.id;
  };

  return (
    <div className="py-[12px]">
      <TitleComponent title={tManagePayments('managePayments')} showTippy={true}>
        <div>
          <div>
            <IconInfo className="w-[18px] h-[18px] fill-[#ffffff] inline-block vertical-middle mr-[4px] relative top-[-2px]" />
            {tManagePayments('managePayments')}
          </div>
          <div>• {tManagePayments('managePaymentsTip1')}</div>
          <div>• {tManagePayments('managePaymentsTip2')}</div>
        </div>
      </TitleComponent>

      <div className="card">
        <div className="card-header">
          <ul className="flex">
            <li className="mr-[8px]">
              {activeTab === 'paymentGateway' && (
                <ButtonTabActiveComponent text={tManagePayments('paymentGateway')} />
              )}
            </li>
            <li>
              <ButtonTabInactiveComponent
                text={tManagePayments('bankAccount')}
                onClick={() => setActiveTab('bankAccount')}
              />
            </li>
          </ul>
        </div>
        <div className="card-body">
          <div className="bg-[#ffffff] max-w-[1000px]">
            <div className="px-[14px] py-[8px]">
              <MediumTitleComponent
                className="mb-[8px]"
                text={tManagePayments('setDailyWithdrawalCount')}
              />
              <div className="pb-[8px] text-[12px] max-[768px]:text-[16px] ">
                {tManagePayments('setDailyWithdrawalCountForYourMembers')}
              </div>
              <ButtonOutlineComponent
                text={tManagePayments('setLimit')}
                onClick={() => onSetLimitClick()}
              />
              <span className="block w-[100%] h-[1px] bg-[#e0e0e0] my-[10px]"></span>
              <MediumTitleComponent
                className="my-[8px]"
                text={tManagePayments('managePaymentProviders')}
              />
            </div>
            {loading ? (
              <Loading />
            ) : error ? (
              <ErrorMessageComponent message={error} />
            ) : (
              <div className="table-fixed-header">
                <table className="text-[12px] max-[768px]:text-[16px] w-full border-collapse border border-[#0000001f] border-t-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
                  <thead>
                    <tr>
                      <th scope="col">#</th>
                      <th scope="col">{tGlobal('action')}</th>
                      <th scope="col">{tManagePayments('provider')}</th>
                      <th scope="col">{tManagePayments('fee')}</th>
                      <th scope="col">{tManagePayments('providerStatus')}</th>
                      <th scope="col">{tManagePayments('lastUpdated')}</th>
                    </tr>
                  </thead>
                  <tbody className="font-[500]">
                    {providers.length > 0 ? (
                      providers.map((provider, index) => (
                        <tr key={provider.id}>
                          <td>{index + 1}</td>
                          <td>
                            <div className="flex justify-center items-center">
                              <button
                                className="mx-[4px] cursor-pointer p-[4px] rounded-[4px] hover:bg-[var(--button-background-cancel-hover)] text-[var(--text-color-primary)]"
                                onClick={() => onEditClick(provider)}
                              >
                                <IconPen />
                              </button>
                            </div>
                          </td>
                          <td>{provider.name}</td>
                          <td>
                            <ButtonFullComponent
                              text={tGlobal('view')}
                              onClick={() => onFeeClick(provider)}
                            />
                          </td>
                          <td>
                            {provider.status === 0 ? (
                              <StatusDisabledComponent />
                            ) : provider.status === 1 ? (
                              <StatusEnabledComponent />
                            ) : (
                              <StatusNoneComponent />
                            )}
                          </td>
                          <td>{provider.lastUpdatedStr}</td>
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan={6} className="text-center">
                          {tGlobal('noData')}
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            )}

            {/* Fee Popup Modal */}
            {showFeePopup && selectedProvider && (
              <ProviderFeesPopupModal provider={selectedProvider} onClose={onCloseFeePopup} />
            )}

            {/* Daily Limit Popup Modal */}
            {showDailyLimitPopup && (
              <DailyLimitPopupModal onClose={onCloseDailyLimitPopup} onSave={onSaveDailyLimit} />
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default PaymentGatewayComponent;
