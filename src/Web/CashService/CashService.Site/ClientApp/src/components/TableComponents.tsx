import React from 'react';
import { useTranslation } from 'react-i18next';

export interface TableRowProps {
  key: string;
  className: string;
  columns: React.ReactNode[];
}

interface TableComponentProps {
  numOfColumns: number;
  resourcesTranslation: any;
  headerRows: string[][];
  bodyRows: TableRowProps[];
}

export const TableComponent: React.FC<TableComponentProps> = ({ 
  numOfColumns,
  resourcesTranslation,
  headerRows,
  bodyRows,
  }) => {
  const { t } = useTranslation('global');

  return (
    <table className="table-striped">
      <thead>
        {headerRows.map((headerRow) => (
          <tr>
            {headerRow.map((header) => (<th>{resourcesTranslation(header)}</th>))}
          </tr>
        ))}
      </thead>
      <tbody className='font-[500]'>
        {bodyRows.length === 0 && (
            <tr>
              <td colSpan={numOfColumns} className="text-center">{t('noData')}</td>
            </tr>
        )}

        {bodyRows.length > 0 && bodyRows.map((bodyRow, rowIndex) => (
          <tr key={bodyRow.key || rowIndex} className={bodyRow.className || ''}>
            {bodyRow.columns}
          </tr>
        ))}
      </tbody>
    </table>
  );
};