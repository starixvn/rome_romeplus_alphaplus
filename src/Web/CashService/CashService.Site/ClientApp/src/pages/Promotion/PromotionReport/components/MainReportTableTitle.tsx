import { useState } from 'react';
import Tippy from '@tippyjs/react';
import { useTranslation } from 'react-i18next';
import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import { copyHtmlToClipboard } from '../../../../utils/sharedUtils';

export default function ReportTitle({
  tableRef,
  search,
  setSearch,
  onExportExcel,
}: {
  tableRef: React.RefObject<HTMLTableElement | null>;
  search: string;
  setSearch: (val: string) => void;
  onExportExcel: () => void;
}) {
  const { t } = useTranslation(['promotion', 'global']);
  const [textCopy, setTextCopy] = useState(t('global:copy'));

  async function handleCopy() {
    await copyHtmlToClipboard(tableRef.current, () => {
      setTextCopy(t('global:copied'));
      setTimeout(() => setTextCopy(t('global:copy')), 3000);
    });
  }

  return (
    <div className="search-section flex items-center w-full justify-between border border-[var(--table-border-color)] border-b-0 p-[6px]">
      <div></div>
      <div className="flex items-center gap-[8px]">
        <Tippy
          content={t('global:exportToExcel')}
          placement="top"
          theme="light"
          arrow={true}
          hideOnClick={false}
        >
          <button
            onClick={onExportExcel}
            className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer"
          >
            <IconExcel />
          </button>
        </Tippy>
        <Tippy content={textCopy} placement="top" theme="light" arrow={true} hideOnClick={false}>
          <button onClick={handleCopy} className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer">
            <IconCopy />
          </button>
        </Tippy>
        <div className="flex">
          <div className="w-[250px] ml-auto relative">
            <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
              <IconSearch />
            </span>
            <input
              type="text"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="control-field pl-[30px]!"
              placeholder={t('promotion:table.promotionTitle')}
              autoComplete="off"
            />
          </div>
        </div>
      </div>
    </div>
  );
}
