import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const promotionTypeOptions: DropdownOption[] = [
  { value: 1, label: 'Rebate on Turnover' },
  { value: 2, label: 'Rebate on Deposit' },
  { value: 3, label: 'Rebate on Loss' },
  { value: 4, label: 'Rebate on Profit' },
  { value: 5, label: 'Rebate on Net Revenue' },
  { value: 6, label: 'Rebate on Net Gaming Revenue' },
  { value: 7, label: 'Rebate on Net Profit' },
  { value: 8, label: 'Bonus' },
  { value: 9, label: 'Freebet' },
];

const statusOptions: DropdownOption[] = [
  { value: 1, label: 'Enabled' },
  { value: 2, label: 'Disabled' },
];

// export default function MainReportFilter({ onFilterChange }: any) {
//   const today = new Date();
//   const todayInGMT4 = convertToGMT4(today);
//   todayInGMT4.setHours(23, 59, 59, 0);

//   const sevenDaysAgo = new Date();
//   const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
//   sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
//   sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);

//   const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([
//     sevenDaysAgoInGMT4,
//     todayInGMT4,
//   ]);
//   const [promotionType, setPromotionType] = useState<string>('');
//   const [status, setStatus] = useState<string>('');
//   const currentYear = new Date().getFullYear();

//   function handleFilterChange() {
//     const queryString = `fromDate=${dateRange[0]?.toISOString()}&toDate=${dateRange[1]?.toISOString()}&promotionType=${promotionType}&status=${status}`;
//     onFilterChange(queryString);
//   }

//   return (
//     <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
//       <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
//         <LabelTitleComponent text="Date Range" />
//         <DateRangePickerComponent
//           value={dateRange}
//           onChange={(value) => {
//             setDateRange(value);
//             handleFilterChange();
//           }}
//           minYear={currentYear - 3}
//           maxRangeDays={92}
//         />
//       </div>
//       <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
//         <LabelTitleComponent text="Promotion Type" />
//         <CustomDropdownComponent
//           options={promotionTypeOptions}
//           onSelect={(opt) => {
//             setPromotionType(opt.value as string);
//             handleFilterChange();
//           }}
//           initialValue={promotionType}
//           showOptionAll
//           searchable
//         />
//       </div>
//       <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
//         <LabelTitleComponent text="Status" />
//         <CustomDropdownComponent
//           options={statusOptions}
//           onSelect={(opt) => {
//             setStatus(opt.value as string);
//             handleFilterChange();
//           }}
//           initialValue={status}
//           showOptionAll
//         />
//       </div>
//     </div>
//   );
// }

interface FilterProps {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  promotionType: string;
  setPromotionType: (type: string) => void;
  status: string;
  setStatus: (status: string) => void;
}

export default function MainReportFilter({
  dateRange,
  setDateRange,
  promotionType,
  setPromotionType,
  status,
  setStatus,
}: FilterProps) {
  const currentYear = new Date().getFullYear();
  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Date Range" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minYear={currentYear - 3}
          maxRangeDays={92}
        />
      </div>
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Promotion Type" />
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          showOptionAll
          searchable
        />
      </div>
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Status" />
        <CustomDropdownComponent
          options={statusOptions}
          onSelect={(opt) => setStatus(opt.value as string)}
          initialValue={status}
          showOptionAll
        />
      </div>
    </div>
  );
}
