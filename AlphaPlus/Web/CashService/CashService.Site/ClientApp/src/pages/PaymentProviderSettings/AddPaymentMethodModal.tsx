import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { TRANSACTION_TYPES, STATUS_OPTIONS } from '../../constants/paymentConstants';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentMethod } from '../../services/interfaces/PaymentMethod';

interface AddPaymentMethodModalProps {
  onClose: () => void;
  onSave: () => void;
  providerId: number;
}

const AddPaymentMethodModal: React.FC<AddPaymentMethodModalProps> = ({ onClose, onSave, providerId }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');
  
  // Form state
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');
  const [agreeToTerms, setAgreeToTerms] = React.useState(false);
  const [paymentMethods, setPaymentMethods] = React.useState<PaymentMethod[]>([]);
  const [allMethodsInactive, setAllMethodsInactive] = React.useState<boolean>(false);

  const inactiveStatus = STATUS_OPTIONS.find(option => option.name === 'Inactive')?.id;
  
  // Load payment methods when component mounts
  React.useEffect(() => {
    const loadPaymentMethods = async () => {
      try {
        const methods = await paymentProviderService.getAllPaymentMethods();
        setPaymentMethods(methods);
        // Check if all methods are inactive using STATUS_OPTIONS constant
        setAllMethodsInactive(methods.length > 0 && methods.every(method => method.status === inactiveStatus));
      } catch (err) {
        console.error('Failed to load payment methods:', err);
        setError(tGlobal('errorOccurred'));
      }
    };

    loadPaymentMethods();
  }, [tGlobal]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    try {
      const selectedMethod = paymentMethods.find(method => method.id === parseInt(selectedPaymentMethod));
      const newSetting = {
        id: 0, // New payment method, ID will be assigned by the server
        name: selectedMethod?.name || '',
        providerId,
        method: parseInt(selectedPaymentMethod),
        transactionType: parseInt(selectedTransactionType),
        status: 1, // Active status
        creator: 1 // Default creator ID
      };

      const response = await paymentProviderService.addPaymentMethod(newSetting);
      
      if (response.isSuccessful) {
        setSuccessMessage(tGlobal('updateSuccess'));
        setTimeout(() => {
          onSave();
          onClose();
        }, 3000);
      } else {
        setError(response.message || tGlobal('updateFailed'));
      }
    } catch (err) {
      setError(tGlobal('errorOccurred'));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h5 className="modal-title">{tManagePayments('setUpPaymentMethods')}</h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="modal-body">

            <div className="form-group mb-3">
              <label className="form-label">{tManagePayments('transactionType')}</label>
              <select 
                className="form-select" 
                value={selectedTransactionType}
                onChange={(e) => setSelectedTransactionType(e.target.value)}
                required
              >
                <option value="">{tGlobal('select')}</option>
                {TRANSACTION_TYPES.filter(type => type.id !== 0).map(type => (
                  <option key={type.id} value={type.id}>{type.name}</option>
                ))}
              </select>
            </div>
            
            <div className="form-group mb-3">
              <label className="form-label">{tManagePayments('paymentMethod')}</label>
              <select 
                className="form-select" 
                value={selectedPaymentMethod}
                onChange={(e) => setSelectedPaymentMethod(e.target.value)}
                required
              >
                <option value="">{tGlobal('select')}</option>
                {paymentMethods.map(method => (
                  <option 
                    key={method.id} 
                    value={method.id}
                    disabled={method.status === inactiveStatus}
                  >
                    {method.name}{method.status === inactiveStatus ? ` (${tGlobal('underMaintenance')})` : ''}
                  </option>
                ))}
              </select>
            </div>

            {error && <div className="text-danger mt-2">{error}</div>}
            {successMessage && <div className="text-success mt-2">{successMessage}</div>}
          </div>
          <div className="modal-footer">
            <div className="form-check mb-3 w-100">
              <input
                type="checkbox"
                className="form-check-input"
                id="agreeToTerms"
                checked={agreeToTerms}
                onChange={(e) => setAgreeToTerms(e.target.checked)}
                required
              />
              <label className="form-check-label" htmlFor="agreeToTerms">
                {tManagePayments('agreeTermsAndConditions')}
              </label>
            </div>
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              {tManagePayments('setUpLater')}
            </button>
            <button 
              type="submit" 
              className="btn btn-primary" 
              disabled={isLoading || allMethodsInactive}
            >
              {isLoading ? tGlobal('loading') : tGlobal('save')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPaymentMethodModal; 