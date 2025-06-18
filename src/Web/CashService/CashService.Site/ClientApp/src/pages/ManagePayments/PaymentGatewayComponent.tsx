import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonFullComponent,
  ButtonPenComponent,
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
} from '../../components/ButtonComponents';

import Loading from '../../components/LoadingComponent';
import { ErrorMessageComponent } from '../../components/MessageComponents';
import {
  StatusEnabledComponent,
  StatusDisabledComponent,
  StatusNoneComponent,
} from '../../components/StatusComponents';
import TitleComponent from '../../components/TitleComponents';
import { PaymentProvider } from '../../services/interfaces/Payments/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';
import NoRecordFound from '../../components/NoRecordFoundComponent';

interface PaymentGatewayComponentProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
}

const PaymentGatewayComponent: React.FC<PaymentGatewayComponentProps> = ({
  setActiveTab
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
        return <StatusDisabledComponent />;
      case 1:
        return <StatusEnabledComponent />;
      default:
        return <StatusNoneComponent />;
    }
  };


  const onEditClick = (provider: PaymentProvider) => {
    localStorage.setItem('cashsite_previousUrl', window.location.href);
    location.href = 'ManageProviderSettings?id=' + provider.id;
  };

  return (
    <>
      <TitleComponent title={tManagePayments('managePayments')} showTippy={true}>
        <div>
          <div>• {tManagePayments('managePaymentsTip1')}</div>
          <div>• {tManagePayments('managePaymentsTip2')}</div>
        </div>
      </TitleComponent>
      <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
        <li className="mr-[8px]">
          <ButtonTabInactiveComponent text={tManagePayments('paymentGateway')} />
        </li>
        <li>
          <ButtonTabActiveComponent text={tManagePayments('bankAccount')} onClick={() => setActiveTab('bankAccount')} />
        </li>
      </ul>
       
      <div className="max-w-[1000px]">
         
        {loading ? (<Loading />) : 
          error ? (<ErrorMessageComponent message={error} />) : (
          <>
          <div className="w-full border border-[var(--table-border-color)] border-b-0 p-[6px] flex">
            <ButtonFullComponent text={tManagePayments('setLimit')} onClick={() => onSetLimitClick()} />
          </div>
          <div className="table">
            <table className=" w-full border-collapse">
              <thead>
                <tr>
                  <th scope="col" className="border-l border-[var(--border-color)]">#</th>
                  <th scope="col">{tGlobal('action')}</th>
                  <th scope="col">{tManagePayments('provider')}</th>
                  <th scope="col">{tManagePayments('fee')}</th>
                  <th scope="col">{tManagePayments('providerStatus')}</th>
                </tr>
              </thead>
              <tbody className="font-[500]">                  
                {providers.length > 0 && (
                  providers.map((provider, index) => (
                    <tr key={provider.id}>
                      <td>{index + 1}</td>
                      <td>
                        <div className="flex justify-center items-center">                          
                          <ButtonPenComponent onClick={() => onEditClick(provider)} />
                        </div>
                      </td>
                      <td>{provider.name}</td>
                      <td>
                        <ButtonFullComponent text={tGlobal('view')} onClick={() => onFeeClick(provider)} />
                      </td>
                      <td>{renderProviderStatus(provider.status)}</td>
                    </tr>
                  ))
                )}
                {providers.length === 0 && (
                  <NoRecordFound colSpan={5} />
                )}
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
          <DailyLimitPopupModal onClose={onCloseDailyLimitPopup} onSave={onSaveDailyLimit} />
        )}
      </div>    
     
    </>
  );
};

export default PaymentGatewayComponent;
