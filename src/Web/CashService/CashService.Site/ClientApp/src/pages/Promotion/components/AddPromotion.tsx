import React, { useState } from 'react';

import { Promotion } from '../../../services/interfaces/Promotion';
import { IconInfo } from '../../../components/Icons';


interface AddPromotionProps {
  onClose: () => void;
  onSave: (promotion: Promotion) => void;
  promotion?: Promotion | null;
}

const PROMOTION_TYPES = [
  { value: 'RebateOnTurnover', label: 'Rebate on Turnover' },
  // Add more types as needed
];

const AddPromotion: React.FC<AddPromotionProps> = ({ onClose, onSave, promotion }) => {
 
  const [isActive, setIsActive] = useState<boolean>(promotion?.status === 'ACTIVE' || true);
  const [promotionType, setPromotionType] = useState(promotion?.promotionType || PROMOTION_TYPES[0].value);
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

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const [fromDate, toDate] = period.split(' - ').map(date => new Date(date));
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
    <form id='add-promotion-form' onSubmit={handleSubmit}>
    <div className="bg-[#ffffff] p-[16px] rounded-[5px] ">
      <h2 className="pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px] ">Current Promotion</h2>
      <p className="pb-[8px] text-[12px] font-semibold">
        Do you want to keep your promotion active? You can only run one promotion at a time.
      </p>
      <div className="">
        <button
          className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer ${isActive ? 'bg-[var(--button-switch-background-active)] text-[var(--button-switch-background-color-active)]' : 'bg-[var(--button-switch-background)] text-[var(--button-switch-background-color)]'}`}
          onClick={() => setIsActive(true)}
        >
          Enable
        </button>
        <button
          className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer ${!isActive ? 'bg-[var(--button-switch-background-active)] text-[var(--button-switch-background-color-active)]' : 'bg-[var(--button-switch-background)] text-[var(--button-switch-background-color)]'}`}
          onClick={() => setIsActive(false)}
        >
          Disable
        </button>
      </div>
      <hr className='my-[16px] border-[#E0E0E0]' />   
      
      <h3 className="pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px] flex">Promotion Settings <span className="" title="Promotion runs on a weekly basis."><IconInfo className='w-[20px] cursor-pointer h-[20px] ml-[4px] fill-[#163f78]' /></span></h3>
      <p className="text-[12px] text-[#333333] font-semibold">Promotion runs on a weekly basis.</p>
      <div className="flex no-wrap gap-[16px]">
        <div className="w-[100%] max-w-[250px]">
          <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">Promotion Type<span className="text-[#E25757]">*</span></label>
          <select
            className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
            value={promotionType}
            onChange={e => setPromotionType(e.target.value)}
          >
            {PROMOTION_TYPES.map(type => (
              <option key={type.value} value={type.value}>{type.label}</option>
            ))}
          </select>
        </div>
        <div className="w-[100%] max-w-[250px]">
          <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">Period<span className="text-[#E25757]">*</span></label>
          <input
            type="text"
            className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
            placeholder="05/07/2025 - 05/07/2025"
            value={period}
            onChange={e => setPeriod(e.target.value)}
          />
        </div>
        <div className="w-[100%] max-w-[250px]">
          <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">Provider<span className="text-[#E25757]">*</span></label>
          <select
            className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
            value={provider}
            onChange={e => setProvider(e.target.value)}
          >
            <option value="">Select Provider</option>
        {[
            { id: 'provider1', name: 'provider1' },
            { key: 'provider2', name: 'provider2' },
            { key: 'provider3', name: 'provider3' },
            { key: 'provider4', name: 'Live provider4' },
        ].map(p => (
              <option key={p.id} value={p.id}>{p.name}</option>
            ))}
          </select>
        </div>
      </div>
            
      {/* Category Fields */}
      {[
        { key: 'sports', label: 'Sports' },
        { key: 'virtualSports', label: 'Virtual Sports' },
        { key: 'games', label: 'Games' },
        { key: 'liveCasino', label: 'Live Casino' },
      ].map(cat => (
        <div key={cat.key} className="flex no-wrap gap-[16px]">
          <div className="w-[100%] max-w-[250px]">
            <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{cat.label} Percentage<span className="text-[#E25757]">*</span></label>
            <div className="flex items-center relative">
              <input
                type="number"
                min="0"
                step="0.01"
                className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] pr-[32px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
                value={categories[cat.key as keyof typeof categories].percentage}
                onChange={e => handleCategoryChange(cat.key, 'percentage', e.target.value)}
              />
              <span className="absolute right-0 top-0 text-[12px] text-[#333333] h-full w-[32px] leading-[32px] text-center font-bold border-l-[1px] border-[var(--border-control)]">%</span>
            </div>
          </div>
          <div className="w-[100%] max-w-[250px]">
            <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">Min Turnover<span className="text-[#E25757]">*</span></label>
            <input
              type="number"
              min="0"
              className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
              value={categories[cat.key as keyof typeof categories].minTurnover}
              onChange={e => handleCategoryChange(cat.key, 'minTurnover', e.target.value)}
            />
          </div>
          <div className="w-[100%] max-w-[250px]">
            <label className="flex justify-between text-[12px] font-semibold text-[#333333] my-[8px]">
              <div>Max Payout<span className="text-[#E25757]">*</span></div>
              <div className="flex items-center text-[12px]">
                <input
                  type="checkbox"
                  className="mr-2"
                  checked={categories[cat.key as keyof typeof categories].unlimited}
                  onChange={e => handleCategoryChange(cat.key, 'unlimited', e.target.checked)}
                  id={`${cat.key}-unlimited`}
                />
                <label htmlFor={`${cat.key}-unlimited`} className="text-[var(--text-color-primary)]">Unlimited</label>
              </div>            
            </label>
            <input
              type="number"
              min="0"
              className="h-[32px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
              value={categories[cat.key as keyof typeof categories].maxPayout}
              onChange={e => handleCategoryChange(cat.key, 'maxPayout', e.target.value)}
              disabled={categories[cat.key as keyof typeof categories].unlimited}
            />
          </div>
          
        </div>
      ))}
      <div className="flex justify-start mt-[16px]">
        <button
          className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text-cancel)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] cursor-pointer "
          onClick={() => {onClose()}}
        >
          Cancel
        </button>
        <button
          className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
        >
          Save
        </button>
      </div>
    </div>
    </form>
  );
};

export default AddPromotion;
