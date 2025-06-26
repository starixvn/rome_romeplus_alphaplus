import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import TitleComponent from '../../../components/TitleComponents';
import Filter from './components/Filter.tsx';
import Table from './components/Table.tsx';
import Search from './components/Search.tsx';
import './PendingPromotion.css';
import i18n from '../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { ButtonFullComponent } from '../../../components/ButtonComponents.tsx';
import AddBonusForm from './components/AddBonusForm';

const mockData: any[] = [];

const PendingPromotion: React.FC = () => {
  useCookieLanguage(i18n);
  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([null, null]);
  const [promotionType, setPromotionType] = useState<string>('');
  const [search, setSearch] = useState<string>('');
  const [showAddBonus, setShowAddBonus] = useState(false);

  const filteredData = mockData;

  return (
    <>
      {showAddBonus ? (
        <AddBonusForm onClose={() => setShowAddBonus(false)} />
      ) : (
        <div className="flex h-full flex-col">
          <TitleComponent title="Pending Promotion" />
          <div className="mb-[8px]">
            <ButtonFullComponent
              text={`Add Bonus`}
              onClick={() => setShowAddBonus(true)}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>
          <div className="filters-container mb-[5px]">
            <Filter
              dateRange={dateRange}
              setDateRange={setDateRange}
              promotionType={promotionType}
              setPromotionType={setPromotionType}
            />
          </div>
          <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
            <Search search={search} setSearch={setSearch} onDownload={() => {}} />
          </div>
          <Table data={filteredData} />
        </div>
      )}
    </>
  );
};

export default PendingPromotion;

if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <PendingPromotion />
    </React.StrictMode>
  );
}
