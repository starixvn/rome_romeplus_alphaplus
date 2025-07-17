import React from 'react';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';

interface TableProps {
  data: any[];
}

const Table: React.FC<TableProps> = ({ data }) => {
  return (
    <div className="table-fixed-header table-fixed-body min-h-0">
      <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
        <thead className="z-11!">
          <tr>
            <th className="w-[32px] min-w-[32px] text-center">#</th>
            <th className="w-[40px] min-w-[40px] text-center">
              <input type="checkbox" />
            </th>
            <th className="w-[40px] min-w-[40px] text-center">Edit</th>
            <th className="w-[150px] min-w-[150px] text-center">Username</th>
            <th className="w-[150px] min-w-[150px] text-center">
              Last Name
              <br />
              First Name
            </th>
            <th className="w-[200px] min-w-[200px] text-center">Promotion Title</th>
            <th className="w-[120px] min-w-[120px] text-center">Promotion Type</th>
            <th className="w-[120px] min-w-[120px] text-right">Bonus Amount</th>
            <th className="w-[200px] min-w-[200px] text-center">Date Range</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {data.length === 0 ? (
            <NoRecordFound colSpan={9} />
          ) : (
            data.map((item, idx) => (
              <tr key={item.id || idx}>
                <td className="w-[32px] min-w-[32px] sticky left-0 bg-white z-10">{idx + 1}</td>
                <td className="text-center">
                  <input type="checkbox" />
                </td>
                <td className="text-center">
                  <button className="text-blue-600 underline">Edit</button>
                </td>
                <td className="text-center">{item.username}</td>
                <td className="text-center">
                  <div>{item.lastName}</div>
                  <div>{item.firstName}</div>
                </td>
                <td className="text-center">{item.promotionTitle}</td>
                <td className="text-center">{item.promotionType}</td>
                <td className="text-right">{item.bonusAmount}</td>
                <td className="text-center">{item.dateRange}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;
