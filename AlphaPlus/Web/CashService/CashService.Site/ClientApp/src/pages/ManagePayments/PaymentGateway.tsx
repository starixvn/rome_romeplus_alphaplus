import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';
import styles from './PaymentGateway.module.scss';

interface PaymentGatewayProps {
  // Add props here as needed
}

const PaymentGateway: React.FC<PaymentGatewayProps> = () => {
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
      try {
        setLoading(true);
        const data = await paymentProviderService.getProviders();
        setProviders(data);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch payment providers:', err);
        setError('Failed to load payment providers. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchProviders();
  }, []);

  const getStatusText = (status: number) => {
    return status === 1 ? tManagePayments('active') : tManagePayments('inactive');
  };

  const handleFeeClick = (provider: PaymentProvider) => {
    setSelectedProvider(provider);
    setShowFeePopup(true);
  };

  const handleClosePopup = () => {
    setShowFeePopup(false);
    setSelectedProvider(null);
  };

  const handleSetLimitClick = () => {
    setShowDailyLimitPopup(true);
  };

  const handleCloseDailyLimitPopup = () => {
    setShowDailyLimitPopup(false);
  };

  const handleSaveDailyLimit = () => {};

  const handleEditClick = (provider: PaymentProvider) => {
    location.href = 'ManageProviderSettings?id=' + provider.id;
  };

  return (
    <div className="bg-[#ffffff] max-w-[1000px]">
      <div className="px-[14px] pb-[8px]">
        <div className="pb-[8px] mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px] ">
          {tManagePayments('setDailyWithdrawalCount')}
        </div>
        <div className="pb-[8px] text-[12px]">
          {tManagePayments('setDailyWithdrawalCountForYourMembers')}
        </div>
        <div className="mt-[8px] mb-[8px]">
          <button
            type="button"
            className="bg-[#ffffff] text-[#163f78] border-[1px] border-[#163f78] rounded-[5px] px-[16px] py-[4px] text-[12px] font-bold leading-[20.58px] tracking-[0.12px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
            onClick={() => handleSetLimitClick()} // Default values, should be updated with actual values
          >
            {tManagePayments('setLimit')}
          </button>
        </div>
        <span className='block w-[100%] h-[1px] bg-[#e0e0e0] my-[10px]' ></span>
        <div className="pb-[8px] mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px]">
          {tManagePayments('managePaymentProviders')}
        </div>
      </div>
      {loading ? (
        <div className="text-center p-4">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">{tGlobal('loading')}</span>
          </div>
          <p className="mt-2">{tManagePayments('loading')}</p>
        </div>
      ) : error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : (
        <div className={styles['table-payment']}>
          <table className="text-[12px] w-full border-collapse border border-[#0000001f] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
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
            <tbody>
              {providers.length > 0 ? (
                providers.map((provider, index) => (
                  <tr key={provider.id}>
                    <td>{index + 1}</td>
                    <td>
                      <button
                        className="btn btn-sm btn-outline-primary me-1"
                        onClick={() => handleEditClick(provider)}
                      >
                        {tGlobal('edit')}
                      </button>
                    </td>
                    <td>{provider.name}</td>
                    <td>
                      <button
                        className="btn btn-sm btn-outline-primary me-1"
                        onClick={() => handleFeeClick(provider)}
                      >
                        {tManagePayments('fee')}
                      </button>
                    </td>
                    <td>
                      <span
                        className={`badge ${provider.status === 1 ? 'bg-success' : 'bg-danger'}`}
                      >
                        {getStatusText(provider.status)}
                      </span>
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
        <ProviderFeesPopupModal provider={selectedProvider} onClose={handleClosePopup} />
      )}

      {/* Daily Limit Popup Modal */}
      {showDailyLimitPopup && (
        <DailyLimitPopupModal onClose={handleCloseDailyLimitPopup} onSave={handleSaveDailyLimit} />
      )}
    </div>
  );
};

export default PaymentGateway;
