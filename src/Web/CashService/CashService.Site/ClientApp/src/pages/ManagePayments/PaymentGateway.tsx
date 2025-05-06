import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import ProviderFeesPopupModal from './ProviderFeesPopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';
import '../../styles/global.scss';
import TitleCommon from '../../components/ui/TitleCommon';
import { IconPen } from '../../components/Icons';

interface PaymentGatewayProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
  activeTab: string;
}

const PaymentGateway: React.FC<PaymentGatewayProps> = ({ setActiveTab, activeTab }) => {
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
      console.log("fetchProviders");
      
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
    return status === 1 ? tManagePayments('enabled') : tManagePayments('disabled');
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
    <div className="p-[12px]">
      <TitleCommon title={tManagePayments('managePayments')} />

      <div className="card">
        <div className="card-header">
          <ul className="flex">
            <li className="mr-[8px]">
              <button
                  className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'paymentGateway' ? ' bg-[#163f78] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[#163f78] cursor-pointer'}`}
                  onClick={() => setActiveTab('paymentGateway')}
                >
                {tManagePayments('paymentGateway')}
              </button>
            </li>
            <li>
              <button
                  className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'bankAccount' ? ' bg-[#163f78] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[#163f78] cursor-pointer'}`}
                  onClick={() => setActiveTab('bankAccount')}
                >
                {tManagePayments('bankAccount')}
              </button>
            </li>
          </ul>
        </div>
        <div className="card-body">
          <div className="bg-[#ffffff] max-w-[1000px]">
            <div className="px-[14px] py-[8px]">
              <div className="pb-[8px] mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px] ">
                {tManagePayments('setDailyWithdrawalCount')}
              </div>
              <div className="pb-[8px] text-[12px]">
                {tManagePayments('setDailyWithdrawalCountForYourMembers')}
              </div>
              <div className="mt-[8px] mb-[8px]">
                <button
                  type="button"
                  className=" bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer  text-[#163f78] border-[1px] border-[#163f78] rounded-[5px] px-[16px] py-[4px] text-[12px] font-bold cursor-pointer leading-[20.58px] tracking-[0.12px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                  onClick={() => handleSetLimitClick()} // Default values, should be updated with actual values
                >
                  {tManagePayments('setLimit')}
                </button>
              </div>
              <span className="block w-[100%] h-[1px] bg-[#e0e0e0] my-[10px]"></span>
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
              <div className="table-fixed-header">
                <table className="text-[12px] w-full border-collapse border border-[#0000001f] border-t-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
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
                            <div className='flex justify-center items-center'>
                              <button
                                  className="mx-[4px] cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[#163f78]"
                                  onClick={() => handleEditClick(provider)}
                                >
                                  <IconPen />
                              </button>
                            </div>
                          </td>
                          <td>{provider.name}</td>
                          <td>
                            <button
                              className="font-bold text-[12px] tracking-[0.12px] leading-[30px] px-[16px] rounded-[4px] bg-[#163f78] hover:bg-[#375A8A] text-[#ffffff] cursor-pointer"
                              onClick={() => handleFeeClick(provider)}
                            >
                              {tManagePayments('fee')}
                            </button>
                          </td>
                          <td>
                            <span
                              className={`rounded-[20px] px-[10px] py-[3px] ${provider.status === 1 ? ' bg-[#D4E7D9] ' : ' bg-[#FBD8D8]'}`}
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
        </div>
      </div>
    </div>
  );
};

export default PaymentGateway;
