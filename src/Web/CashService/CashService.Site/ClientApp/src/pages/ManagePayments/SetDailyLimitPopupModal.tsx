import * as React from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../components/ButtonComponents';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../components/MessageComponents';
import { LabelTitleComponent, MediumTitleComponent } from '../../components/TitleComponents';
import { paymentProviderService } from '../../services/paymentsAPI';
import { ensureIntegerInput } from '../../utils/inputHandlers';
import { createErrorResultModel } from '../../services/interfaces/ResultModel';

interface SetDailyLimitPopupModalProps {
  onClose: () => void;
  onSave: (dailyLimit: number) => void;
}

const SetDailyLimitPopupModal: React.FC<SetDailyLimitPopupModalProps> = ({ onClose, onSave }) => {
  const { t } = useTranslation(['managePayments', 'global']);
  const [dailyLimit, setDailyLimit] = React.useState<number>(1);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');
  const [orgDailyLimit, setOrgDailyLimit] = React.useState<number>(1);
  const [isBlockAction, setIsBlockAction] = React.useState<boolean>(false);
  const [fieldErrors, setFieldErrors] = React.useState({ dailyLimit: '' });

  const onDailyLimitChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = Number(ensureIntegerInput(e).target.value);
    setDailyLimit(value);
  };

  React.useEffect(() => {
    const loadDailyWithdrawalCount = async () => {
      const response = await paymentProviderService.getDailyWithdrawalCount();

      if (response.error) {
        setError(response.error.message);
        return;
      }

      const count = response.data ?? 0;
      setDailyLimit(count);
      setOrgDailyLimit(count);
    };

    loadDailyWithdrawalCount();
  }, [t]);

  React.useEffect(() => {
    validateState();
  }, [dailyLimit]);

  const validateState = () => {
    setFieldErrors({ dailyLimit: '' });

    if (dailyLimit < 1 || dailyLimit > 32767) {
      setFieldErrors({
        ...fieldErrors,
        dailyLimit: t('managePayments:dailyWithdrawalCountOutOfRange'),
      });

      return false;
    }

    return true;
  };

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setSuccessMessage('');
    setError('');

    if (orgDailyLimit === dailyLimit) {
      setError(t('global:thereNoChangeToUpdate'));
      setIsLoading(false);
      return;
    }

    const isValid = validateState();
    if (!isValid) {
      setIsLoading(false);
      return;
    }

    const response = await paymentProviderService.setDailyWithdrawalCount(dailyLimit);
    if (response.error) {
      setError(response.error.message);
      setIsLoading(false);
      return;
    }

    const result = response.data ?? createErrorResultModel(1, t('global:errorOccurred'));

    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(t('managePayments:setLimitSuccess'));
      onSave(dailyLimit);
      setTimeout(() => {
        onClose();
      }, 3000);
    } else {
      setError(result.message);
    }

    setIsLoading(false);
  };

  return (
    <div className="bg-[#00000080] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-w-[400px] max-[768px]:max-w-[90%] rounded-[3px]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]" >
          <MediumTitleComponent text={t('managePayments:setDailyWithdrawalCount')} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <div className="my-[8px]">
          <ErrorMessageComponent message={error} />
          <SuccessMessageComponent message={successMessage} />
        </div>
        <form onSubmit={onSubmit}>
          <div className="mb-[16px]">
            <div className=" ">
              <div className="pb-[8px] mb-[8px] block">
                {t('managePayments:setMaxDailyWithdrawalCountForYourMembers')}{' '}
              </div>
              <LabelTitleComponent text={`${t('managePayments:dailyWithdrawalCount')}`} />
              <input
                type="text"
                maxLength={5}
                className="control-field"
                id="dailyLimit"
                value={dailyLimit}
                onKeyDown={(e) => {
                  if (e.key === "Enter") { 
                    e.preventDefault(); 
                  }
                }}
                onChange={onDailyLimitChange}
                min="0"
              />
              <ValidateMessageComponent message={fieldErrors.dailyLimit} />
            </div>
          </div>
          <div className="flex justify-center gap-[8px]">
            <ButtonCancelComponent text={t('global:cancel')} onClick={onClose} />
            <ButtonApplyComponent 
              text={isLoading ? t('global:loading') : t('global:save')} 
              type='submit'
              disabled={isLoading || isBlockAction}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetDailyLimitPopupModal;
