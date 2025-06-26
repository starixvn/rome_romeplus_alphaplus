import React, { useState } from 'react';
import { Promotion } from '../../../../services/interfaces/Promotion';
import { IconInfo } from '../../../../components/Icons';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
  DisableButtonComponent,
  EnableButtonComponent,
} from '../../../../components/Buttons/index';
import CustomDropdown from '../../../../components/CustomDropdownComponent';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../../../components/Message';
import TitleComponent, {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../../../components/TitleComponents';

interface AddPromotionProps {
  onClose: () => void;
  onSave: (promotion: Promotion) => void;
  promotion?: Promotion | null;
}

const PROMOTION_TYPES = [{ value: 'RebateOnTurnover', label: 'Rebate on Turnover' }];

const PROVIDER_OPTIONS = [
  { value: 'All', label: 'All' },
  { value: 'provider1', label: 'provider1' },
  { value: 'provider2', label: 'provider2' },
  { value: 'provider3', label: 'provider3' },
];

const AddPromotion: React.FC<AddPromotionProps> = ({ onClose, onSave, promotion }) => {
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  const [isActive, setIsActive] = useState<boolean>(promotion?.status === 'ACTIVE' || true);
  const [promotionType, setPromotionType] = useState(
    promotion?.promotionType || PROMOTION_TYPES[0].value
  );
  const [period, setPeriod] = useState(promotion?.period || '');
  const [provider, setProvider] = useState(promotion?.provider || '');
  const [categories, setCategories] = useState({
    sports: {
      percentage: promotion?.sportsPercentage?.toString() || '',
      minTurnover: '',
      maxPayout: promotion?.sportsMaxPayout?.toString() || '',
      unlimited: false,
    },
    virtualSports: {
      percentage: promotion?.virtualSportsPercentage?.toString() || '',
      minTurnover: '',
      maxPayout: promotion?.virtualSportsMaxPayout?.toString() || '',
      unlimited: false,
    },
    games: {
      percentage: promotion?.gamesPercentage?.toString() || '',
      minTurnover: '',
      maxPayout: promotion?.gamesMaxPayout?.toString() || '',
      unlimited: false,
    },
    liveCasino: {
      percentage: promotion?.liveCasinoPercentage?.toString() || '',
      minTurnover: '',
      maxPayout: promotion?.liveCasinoMaxPayout?.toString() || '',
      unlimited: false,
    },
  });

  const [fieldErrors, setFieldErrors] = useState({
    promotionType: '',
    period: '',
    provider: '',
    sportsPercentage: '',
    virtualSportsPercentage: '',
    gamesPercentage: '',
    liveCasinoPercentage: '',
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    // Field-level validation
    const errors = {
      promotionType: !promotionType ? 'Please select promotion type' : '',
      period: !period ? 'Please enter period' : '',
      provider: !provider ? 'Please select provider' : '',
      sportsPercentage: !categories.sports.percentage ? 'Please enter sports percentage' : '',
      virtualSportsPercentage: !categories.virtualSports.percentage
        ? 'Please enter virtual sports percentage'
        : '',
      gamesPercentage: !categories.games.percentage ? 'Please enter games percentage' : '',
      liveCasinoPercentage: !categories.liveCasino.percentage
        ? 'Please enter live casino percentage'
        : '',
    };
    setFieldErrors(errors);
    const hasError = Object.values(errors).some(Boolean);
    if (hasError) {
      setLoading(false);
      return;
    }
    setSuccessMessage('Promotion saved successfully');
    const [fromDate, toDate] = period.split(' - ').map((date) => new Date(date));
    const promotionData: Promotion = {
      promotionId: promotion?.promotionId || 0,
      promotionTitle: '',
      promotionType,
      fromDate,
      toDate,
      sportsPercentage: parseFloat(categories.sports.percentage) || 0,
      virtualSportsPercentage: parseFloat(categories.virtualSports.percentage) || 0,
      gamesPercentage: parseFloat(categories.games.percentage) || 0,
      liveCasinoPercentage: parseFloat(categories.liveCasino.percentage) || 0,
      sportsMaxPayout: parseFloat(categories.sports.maxPayout) || 0,
      virtualSportsMaxPayout: parseFloat(categories.virtualSports.maxPayout) || 0,
      gamesMaxPayout: parseFloat(categories.games.maxPayout) || 0,
      liveCasinoMaxPayout: parseFloat(categories.liveCasino.maxPayout) || 0,
      period,
      provider,
      status: isActive ? 'ACTIVE' : 'INACTIVE',
      lastUpdated: new Date().toISOString(),
    };
    onSave(promotionData);
  };

  const handleCategoryChange = (cat: string, field: string, value: string | boolean) => {
    setCategories((prev) => ({
      ...prev,
      [cat]: {
        ...prev[cat as keyof typeof prev],
        [field]: value,
        ...(field === 'unlimited' && value ? { maxPayout: '' } : {}),
      },
    }));
  };

  return (
    <div tabIndex={-1}>
      <div className="modal-content max-w-[1200px]">
        <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
          <BackButtonComponent onClick={onClose} />
          <TitleComponent title="Manage Promotion" />
        </div>

        <form onSubmit={handleSubmit} className="pb-[16px]">
          <div className="modal-body">
            <ErrorMessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={error} />
            <SuccessMessageComponent
              className="mb-[16px] max-[768px]:mb-[0]"
              message={successMessage}
            />

            <MediumTitleComponent text="Current Promotion" />
            <p className="my-[8px] text-[12px] font-semibold">
              Do you want to keep your promotion active? You can only run one promotion at a time.
            </p>
            <div
              className="mb-[16px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]"
              role="group"
            >
              <EnableButtonComponent isActive={isActive} onClick={() => setIsActive(true)} />
              <DisableButtonComponent isActive={!isActive} onClick={() => setIsActive(false)} />
            </div>

            <MediumTitleComponent text="Promotion Settings" />
            <div className="flex items-center mb-[8px]">
              <p className="text-[12px] text-[#333333] font-semibold">
                Promotion runs on a weekly basis.
              </p>
              <span className="ml-[4px]" title="Promotion runs on a weekly basis.">
                <IconInfo className="w-[20px] cursor-pointer h-[20px] fill-[#163f78]" />
              </span>
            </div>

            <div className="flex flex-nowrap gap-[16px] mb-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text="Promotion Type" required={true} />
                <CustomDropdown
                  options={PROMOTION_TYPES}
                  onSelect={(option) => setPromotionType(option.value.toString())}
                  initialValue={promotionType}
                  showOptionAll={false}
                />
                <ValidateMessageComponent message={fieldErrors.promotionType} />
              </div>

              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text="Period" required={true} />
                <input
                  type="text"
                  className={`control-field ${fieldErrors.period ? 'error' : ''}`}
                  placeholder="05/07/2025 - 05/07/2025"
                  value={period}
                  onChange={(e) => setPeriod(e.target.value)}
                />
                <ValidateMessageComponent message={fieldErrors.period} />
              </div>

              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text="Provider" required={true} />
                <CustomDropdown
                  options={PROVIDER_OPTIONS}
                  onSelect={(option) => setProvider(option.value.toString())}
                  initialValue={provider}
                  showOptionAll={false}
                />
                <ValidateMessageComponent message={fieldErrors.provider} />
              </div>
            </div>

            {/* Category Fields */}
            {[
              { key: 'sports', label: 'Sports' },
              { key: 'virtualSports', label: 'Virtual Sports' },
              { key: 'games', label: 'Games' },
              { key: 'liveCasino', label: 'Live Casino' },
            ].map((cat) => (
              <div
                key={cat.key}
                className="flex flex-nowrap gap-[16px] mb-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full"
              >
                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={`${cat.label} Percentage`} required={true} />
                  <div className="flex items-center relative">
                    <input
                      type="number"
                      min="0"
                      step="0.01"
                      className={`control-field pr-[32px] ${fieldErrors[`${cat.key}Percentage` as keyof typeof fieldErrors] ? 'error' : ''}`}
                      value={categories[cat.key as keyof typeof categories].percentage}
                      onChange={(e) => handleCategoryChange(cat.key, 'percentage', e.target.value)}
                    />
                    <span className="absolute right-0 top-0 text-[12px] text-[#333333] h-full w-[32px] leading-[32px] text-center font-bold border-l-[1px] border-[var(--border-control)]">
                      %
                    </span>
                  </div>
                  <ValidateMessageComponent
                    message={fieldErrors[`${cat.key}Percentage` as keyof typeof fieldErrors]}
                  />
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text="Min Turnover" required={true} />
                  <input
                    type="number"
                    min="0"
                    className="control-field"
                    value={categories[cat.key as keyof typeof categories].minTurnover}
                    onChange={(e) => handleCategoryChange(cat.key, 'minTurnover', e.target.value)}
                  />
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <div className="flex justify-between items-center mb-[4px]">
                    <LabelTitleComponent text="Max Payout" required={true} />
                    <div className="flex items-center text-[12px]">
                      <input
                        type="checkbox"
                        className="mr-2"
                        checked={categories[cat.key as keyof typeof categories].unlimited}
                        onChange={(e) =>
                          handleCategoryChange(cat.key, 'unlimited', e.target.checked)
                        }
                        id={`${cat.key}-unlimited`}
                      />
                      <label
                        htmlFor={`${cat.key}-unlimited`}
                        className="text-[var(--text-color-primary)]"
                      >
                        Unlimited
                      </label>
                    </div>
                  </div>
                  <input
                    type="number"
                    min="0"
                    className="control-field"
                    value={categories[cat.key as keyof typeof categories].maxPayout}
                    onChange={(e) => handleCategoryChange(cat.key, 'maxPayout', e.target.value)}
                    disabled={categories[cat.key as keyof typeof categories].unlimited}
                  />
                </div>
              </div>
            ))}
          </div>

          <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">
            <ButtonCancelComponent text="Cancel" onClick={onClose} />
            <ButtonApplyComponent
              text={loading ? 'Saving...' : 'Save'}
              type="submit"
              disabled={loading}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddPromotion;
