import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Promotion } from '../../../services/interfaces/Promotion';


interface AddPromotionProps {
  onClose: () => void;
  onSave: (promotion: Promotion) => void;
}

const PROMOTION_TYPES = [
  { value: 'RebateOnTurnover', label: 'Rebate on Turnover' },
  // Add more types as needed
];

const INITIAL_CATEGORY = {
  percentage: '',
  minTurnover: '',
  maxPayout: '',
  unlimited: false,
};

const AddPromotion: React.FC<AddPromotionProps> = ({ onClose, onSave }) => {
  const { t } = useTranslation(['promotion', 'global']);
  const [isActive, setIsActive] = useState(true);
  const [promotionType, setPromotionType] = useState(PROMOTION_TYPES[0].value);
  const [period, setPeriod] = useState('');
  const [provider, setProvider] = useState('');
  const [categories, setCategories] = useState({
    sports: { ...INITIAL_CATEGORY },
    virtualSports: { ...INITIAL_CATEGORY },
    games: { ...INITIAL_CATEGORY },
    liveCasino: { ...INITIAL_CATEGORY },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const promotion: Promotion = {
        promotionTitle: '',
        promotionType: '',
        fromDate: new Date(),
        toDate: new Date(),
        sportsPercentage: 1,
        virtualSportsPercentage: 1,
        gamesPercentage: 1,
        liveCasinoPercentage: 1,
        sportsMaxPayout: 1,
        virtualSportsMaxPayout: 1,
        gamesMaxPayout: 1,
        liveCasinoMaxPayout: 1,
        period: '',
        provider: '',
        status: '',
        lastUpdated: '',
    };
    onSave(promotion);
    console.log('Form submitted');
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
    <div className="bg-white rounded-lg shadow-sm p-6 max-w-3xl mx-auto">
      <h2 className="text-lg font-semibold mb-2">Current Promotion</h2>
      <p className="mb-4 text-gray-600 text-sm">
        Do you want to keep your promotion active? You can only run one promotion at a time.
      </p>
      <div className="flex gap-2 mb-6">
        <button
          className={`px-4 py-2 rounded ${isActive ? 'bg-blue-800 text-white' : 'bg-gray-200 text-gray-700'}`}
          onClick={() => setIsActive(true)}
        >
          Enable
        </button>
        <button
          className={`px-4 py-2 rounded ${!isActive ? 'bg-blue-800 text-white' : 'bg-gray-200 text-gray-700'}`}
          onClick={() => setIsActive(false)}
        >
          Disable
        </button>
      </div>

      <h3 className="text-md font-semibold mb-2">Promotion Settings <span className="ml-1 text-blue-500 cursor-pointer" title="Promotion runs on a weekly basis.">i</span></h3>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
        <div>
          <label className="block text-sm font-medium mb-1">Promotion Type*</label>
          <select
            className="w-full border rounded px-2 py-1"
            value={promotionType}
            onChange={e => setPromotionType(e.target.value)}
          >
            {PROMOTION_TYPES.map(type => (
              <option key={type.value} value={type.value}>{type.label}</option>
            ))}
          </select>
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Period*</label>
          <input
            type="text"
            className="w-full border rounded px-2 py-1"
            placeholder="05/07/2025 - 05/07/2025"
            value={period}
            onChange={e => setPeriod(e.target.value)}
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Provider*</label>
          <select
            className="w-full border rounded px-2 py-1"
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
        <div key={cat.key} className="grid grid-cols-1 md:grid-cols-4 gap-4 items-end mb-2">
          <div>
            <label className="block text-sm font-medium mb-1">{cat.label} Percentage*</label>
            <div className="flex items-center gap-1">
              <input
                type="number"
                min="0"
                step="0.01"
                className="w-full border rounded px-2 py-1"
                value={categories[cat.key as keyof typeof categories].percentage}
                onChange={e => handleCategoryChange(cat.key, 'percentage', e.target.value)}
              />
              <span className="text-gray-500">%</span>
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Min Turnover*</label>
            <input
              type="number"
              min="0"
              className="w-full border rounded px-2 py-1"
              value={categories[cat.key as keyof typeof categories].minTurnover}
              onChange={e => handleCategoryChange(cat.key, 'minTurnover', e.target.value)}
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Max Payout*</label>
            <input
              type="number"
              min="0"
              className="w-full border rounded px-2 py-1"
              value={categories[cat.key as keyof typeof categories].maxPayout}
              onChange={e => handleCategoryChange(cat.key, 'maxPayout', e.target.value)}
              disabled={categories[cat.key as keyof typeof categories].unlimited}
            />
          </div>
          <div className="flex items-center mt-6">
            <input
              type="checkbox"
              className="mr-2"
              checked={categories[cat.key as keyof typeof categories].unlimited}
              onChange={e => handleCategoryChange(cat.key, 'unlimited', e.target.checked)}
              id={`${cat.key}-unlimited`}
            />
            <label htmlFor={`${cat.key}-unlimited`} className="text-sm">Unlimited</label>
          </div>
        </div>
      ))}
      <div className="flex justify-end">
        <button
          className="px-4 py-2 mr-2 text-gray-700 bg-gray-200 rounded hover:bg-gray-300 transition-colors"
          onClick={() => {onClose()}}
        >
          Cancel
        </button>
        <button
          className="px-4 py-2 text-white bg-blue-800 rounded hover:bg-blue-900 transition-colors"
        >
          Save
        </button>
      </div>
    </div>
    </form>
  );
};

export default AddPromotion;
