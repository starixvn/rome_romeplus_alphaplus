import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import { PaymentProviderFeeGroup } from '../../services/interfaces/PaymentProviderFeeGroup';
import { paymentProviderService } from '../../services/paymentsAPI';

interface FeePopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const FeePopupModal: React.FC<FeePopupModalProps> = ({ provider, onClose }) => {
  const { t: tGlobal } = useTranslation();
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState(true);
  const [feeData, setFeeData] = React.useState<PaymentProviderFeeGroup[]>([]);

  React.useEffect(() => {
    const loadFeeData = async () => {
      try {
        setIsLoading(true);
        const fees = await paymentProviderService.getProviderFees(provider.id);
        setFeeData(fees);
      } catch (error) {
        console.error('Error loading fee data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    loadFeeData();
  }, [provider.id]);

  return (
    <>
      <div className="modal fade show" style={{ display: 'block' }} tabIndex={-1}>
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{tManagePayments('feeDetails')} - {provider.name}</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>
            <div className="modal-body">
              {isLoading ? (
                <div className="text-center">
                  <div className="spinner-border" role="status">
                    <span className="visually-hidden">{tGlobal('loading')}</span>
                  </div>
                </div>
              ) : (
                <>
                  <div>{provider.name} {tManagePayments('fees')}</div>
                  <b>{tManagePayments('howItWorks')}</b>
                  <p>{tManagePayments('feesInfo1')}</p>
                  <p>{tManagePayments('feesFormula')}</p>
                  <p>{tManagePayments('feesInfo2')}</p>
                  
                  <table className="table table-striped mt-3">
                    <thead>
                      <tr>
                        <th>{tManagePayments('transactionType')}</th>
                        <th>{tManagePayments('paymentMethod')}</th>
                        <th>{tManagePayments('feeRate')}</th>
                      </tr>
                    </thead>
                    <tbody>
                      {feeData.length > 0 ? (
                        feeData.map((feeGroup, groupIndex) => (
                          <React.Fragment key={groupIndex}>
                            {feeGroup.fees.map((fee, feeIndex) => (
                              <tr key={`${groupIndex}-${feeIndex}`} className={feeIndex === 0 ? "table-primary" : ""}>
                                {feeIndex === 0 && (
                                  <td rowSpan={feeGroup.fees.length}>
                                    {feeGroup.transactionType}
                                  </td>
                                )}
                                <td>{fee.method}</td>
                                <td>{(fee.rate * 100).toFixed(2)}%</td>
                              </tr>
                            ))}
                          </React.Fragment>
                        ))
                      ) : (
                        <tr>
                          <td colSpan={3} className="text-center">{tGlobal('noData')}</td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </>
              )}
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                {tGlobal('close')}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="modal-backdrop fade show"></div>
    </>
  );
};

export default FeePopupModal; 