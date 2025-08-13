import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonComponent,
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
} from '../../components/Buttons/index';

import Loading from '../../components/LoadingComponent';
import { MessageComponent } from '../../components/Message';
import { StatusComponent } from '../../components/Status';
import { TitleComponent } from '../../components/Title';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';
import NoRecordFound from '../../components/NoRecordFoundComponent';
import { ButtonIconComponent } from '../../components/Buttons/ButtonIconComponent';
import UpcomingComponent from '../../components/UpcomingComponent';

interface PaymentGatewayComponentProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
  isCashServiceEnabled: boolean;
}

const PaymentGatewayComponent: React.FC<PaymentGatewayComponentProps> = ({
  setActiveTab,
  isCashServiceEnabled,
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

  const renderProviderStatus = (status: number) => {
    switch (status) {
      case 0:
        return <StatusComponent type="disabled" />;
      case 1:
        return <StatusComponent type="completed" text={tGlobal('enabled')} />;
      default:
        return <StatusComponent type="failed" text="-" />;
    }
  };

  const onEditClick = (provider: PaymentProvider) => {
    localStorage.setItem('cashsite_previousUrl', window.location.href);
    location.href = 'ManageProviderSettings?id=' + provider.id;
  };

  React.useEffect(() => {
    top?.indexManager.hideFloatingFeedbackIcon();
  }, []);

  return (
    <>
      <TitleComponent type="main" text={tManagePayments('managePayments')} showTippy={true}>
        <div>
          <div>• {tManagePayments('managePaymentsTip1')}</div>
          <div>• {tManagePayments('managePaymentsTip2')}</div>
        </div>
      </TitleComponent>
      <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
        <li className="mr-[8px]">
          <ButtonTabActiveComponent text={tManagePayments('paymentGateway')} />
        </li>
        <li>
          <ButtonTabInactiveComponent
            text={tManagePayments('bankAccount')}
            onClick={() => setActiveTab('bankAccount')}
          />
        </li>
      </ul>

      <div className="max-w-[1000px]">
        <UpcomingComponent hasTab={true} />
        {loading ? (
          <Loading />
        ) : error ? (
          <MessageComponent message={error} type="error" className="mb-[10px]" />
        ) : (
          <>
            <div className="w-full border border-[var(--table-border-color)] border-b-0 p-[6px] flex">
              <ButtonComponent
                variant="full"
                text={tManagePayments('setLimit')}
                onClick={() => onSetLimitClick()}
              />
            </div>
            <div className="table">
              <table className=" w-full border-collapse">
                <thead>
                  <tr>
                    <th scope="col" className="border-l border-[var(--border-color)]">
                      #
                    </th>
                    <th scope="col">{tGlobal('action')}</th>
                    <th scope="col">{tManagePayments('provider')}</th>
                    <th scope="col">{tManagePayments('fee')}</th>
                    <th scope="col">{tManagePayments('providerStatus')}</th>
                  </tr>
                </thead>
                <tbody className="font-[500]">
                  {providers.length > 0 &&
                    providers.map((provider, index) => (
                      <tr key={provider.id}>
                        <td>{index + 1}</td>
                        <td>
                          <div className="flex justify-center items-center">
                            <ButtonIconComponent
                              style="Edit"
                              onClick={() => onEditClick(provider)}
                            />
                          </div>
                        </td>
                        <td>{provider.name}</td>
                        <td>
                          <ButtonComponent
                            variant="full"
                            text={tGlobal('view')}
                            onClick={() => onFeeClick(provider)}
                          />
                        </td>
                        <td>{renderProviderStatus(provider.status)}</td>
                      </tr>
                    ))}
                  {providers.length === 0 && <NoRecordFound colSpan={5} />}
                </tbody>
              </table>
            </div>
          </>
        )}

        {/* Fee Popup Modal */}
        {showFeePopup && selectedProvider && (
          <ProviderFeesPopupModal provider={selectedProvider} onClose={onCloseFeePopup} />
        )}

        {/* Daily Limit Popup Modal */}
        {showDailyLimitPopup && (
          <DailyLimitPopupModal
            onClose={onCloseDailyLimitPopup}
            onSave={onSaveDailyLimit}
            isCashServiceEnabled={isCashServiceEnabled}
          />
        )}
      </div>
    </>
  );
};

export default PaymentGatewayComponent;
