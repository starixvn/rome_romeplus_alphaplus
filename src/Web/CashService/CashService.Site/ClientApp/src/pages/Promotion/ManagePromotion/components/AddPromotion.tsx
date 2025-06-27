import React, { useState } from 'react';
import { Product, PromotionRequest } from '../../../../services/interfaces/Promotion';
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
import {
  TitleComponent,
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../../../components/Title';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import TreeSelect, { TreeNodeOption } from '../../../../components/TreeSelect/TreeSelect';

interface AddPromotionProps {
  onClose: () => void;
}

const PROMOTION_TYPES = [{ value: 'RebateOnTurnover', label: 'Rebate on Turnover' }];

export const PRODUCTS: Product[] = [
  {
    productId: 1,
    productName: 'Sportsbook',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 2,
    productName: 'Number Game',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 3,
    productName: 'Lottery',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 4,
    productName: 'Sports Lottery',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 5,
    productName: 'RNG Keno',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 6,
    productName: 'Table Game',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 7,
    productName: 'Virtual Sports',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 8,
    productName: 'Bitcoin',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 9,
    productName: 'Saba Promotion',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
  {
    productId: 10,
    productName: 'Live Casino',
    isSelected: true,
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
    children: [
      {
        productId: 11,
        productName: 'FGG',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 12,
        productName: 'Allbet',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 13,
        productName: 'BBIN',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 14,
        productName: 'SA Gaming',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 15,
        productName: 'AE Sexy',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 16,
        productName: 'ION',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 17,
        productName: 'WM',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 18,
        productName: 'BG',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 19,
        productName: 'PP Live Casino',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 20,
        productName: 'YeeBet',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 21,
        productName: 'ON Casino',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 22,
        productName: 'Microgaming Live',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
    ],
  },
  {
    productId: 23,
    productName: 'RNG Casino',
    isSelected: true,
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
    children: [
      {
        productId: 24,
        productName: 'Virtual Games',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 25,
        productName: 'Vgaming',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 26,
        productName: 'Macau Games',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 27,
        productName: 'Saba Casino',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 28,
        productName: 'Arcadia Gaming',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 29,
        productName: 'SG',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 30,
        productName: 'PP',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 31,
        productName: 'PG Soft',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 32,
        productName: 'Joker',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 33,
        productName: 'MaxGame',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 34,
        productName: 'Habanero',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 35,
        productName: 'CG',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 36,
        productName: 'AdvantPlay',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 37,
        productName: 'AdvantPlay Mini',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 38,
        productName: 'SEAL Entertainment',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 39,
        productName: 'MG-RNG',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 40,
        productName: 'Nextspin',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 41,
        productName: 'SABA xD',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 42,
        productName: 'GPI',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 43,
        productName: 'PLAYSTAR',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 44,
        productName: 'ASKMESLOT',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
      {
        productId: 45,
        productName: 'Funky Games',
        isSelected: true,
        children: [],
        productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
      },
    ],
  },
  {
    productId: 46,
    productName: 'Cash Service Billing',
    isSelected: true,
    children: [],
    productSettings: { percentage: 0, minTurnover: 0, maxPayout: 0, unlimited: false },
  },
];

function convertProductToTreeNodeOption(products: Product[]): TreeNodeOption[] {
  return products.map((product) => ({
    label: product.productName,
    value: product.productId,
    children:
      product.children && product.children.length > 0
        ? convertProductToTreeNodeOption(product.children)
        : [],
  }));
}

const AddPromotion: React.FC<AddPromotionProps> = ({ onClose }) => {
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [isActive, setIsActive] = useState<boolean>(true);
  const [promotionType, setPromotionType] = useState(PROMOTION_TYPES[0].value);
  const [period, setPeriod] = useState<[Date | null, Date | null]>([null, null]);
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedProductIds, setSelectedProductIds] = useState<number[]>([]);
  const [fieldErrors, setFieldErrors] = useState({
    percentage: '',
    minTurnover: '',
    maxPayout: '',
    unlimited: '',
  });

  const handleProductChange = (values: (string | number)[], highestParents: TreeNodeOption[]) => {
    setSelectedProductIds(values.map((v) => v as number));
    setProducts(PRODUCTS.filter((p) => highestParents.some((h) => h.value === p.productId)));
  };

  const handleProductFieldChange = (productId: number, field: string, value: number | boolean) => {
    setProducts((prev) =>
      prev.map((p) =>
        p.productId === productId
          ? { ...p, productSettings: { ...p.productSettings, [field]: value } }
          : p
      )
    );
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    setFieldErrors({
      percentage: '',
      minTurnover: '',
      maxPayout: '',
      unlimited: '',
    });
    setSuccessMessage('Promotion saved successfully');
    const [fromDate, toDate] = period.map((date) => (date ? new Date(date) : null));
    const promotionData: PromotionRequest = {
      promotionTitle: '',
      promotionType,
      fromDate,
      toDate,
      products: products.filter((p) => selectedProductIds.includes(p.productId)),
      enabled: isActive,
    };
    console.log(promotionData);
  };

  return (
    <>
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <BackButtonComponent onClick={onClose} />
        <TitleComponent title={`Add Promotion`} />
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
            </div>

            <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
              <LabelTitleComponent text="Period" required={true} />
              <DateRangePickerComponent
                value={period}
                onChange={(value) => setPeriod(value as [Date | null, Date | null])}
              />
            </div>

            <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
              <LabelTitleComponent text="Provider" required={true} />
              <TreeSelect
                options={convertProductToTreeNodeOption(PRODUCTS)}
                value={selectedProductIds}
                onChange={handleProductChange}
              />
            </div>
          </div>

          {products.map((product) => (
            <div
              key={product.productId}
              className="flex flex-nowrap gap-[16px] mb-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full"
            >
              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={`${product.productName} Percentage`} required={true} />
                <div className="flex items-center relative">
                  <input
                    type="number"
                    min="0"
                    step="0.01"
                    className={`control-field pr-[32px] ${fieldErrors[`${product.productName} Percentage` as keyof typeof fieldErrors] ? 'error' : ''}`}
                    value={product.productSettings.percentage}
                    onChange={(e) =>
                      handleProductFieldChange(
                        product.productId,
                        'percentage',
                        Number(e.target.value)
                      )
                    }
                    disabled={!product.isSelected}
                  />
                  <span className="absolute right-0 top-0 text-[12px] text-[#333333] h-full w-[32px] leading-[32px] text-center font-bold border-l-[1px] border-[var(--border-control)]">
                    %
                  </span>
                </div>
                <ValidateMessageComponent
                  message={
                    fieldErrors[`${product.productName}Percentage` as keyof typeof fieldErrors]
                  }
                />
              </div>

              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text="Min Turnover" required={true} />
                <input
                  type="number"
                  min="0"
                  className={`control-field ${!product.isSelected ? 'opacity-50' : ''}`}
                  value={product.productSettings.minTurnover}
                  onChange={(e) =>
                    handleProductFieldChange(
                      product.productId,
                      'minTurnover',
                      Number(e.target.value)
                    )
                  }
                  disabled={!product.isSelected}
                />
              </div>

              <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                <div className="flex justify-between items-center">
                  <LabelTitleComponent text="Max Payout" required={true} />
                  <div className="flex items-center text-[12px]">
                    <input
                      type="checkbox"
                      className="mr-2"
                      checked={product.productSettings.unlimited}
                      onChange={(e) =>
                        handleProductFieldChange(product.productId, 'unlimited', e.target.checked)
                      }
                      id={`${product.productId}-unlimited`}
                      disabled={!product.isSelected}
                    />
                    <label
                      htmlFor={`${product.productId}-unlimited`}
                      className="text-[var(--text-color-primary)]"
                    >
                      Unlimited
                    </label>
                  </div>
                </div>
                <input
                  type="number"
                  min="0"
                  className={`control-field ${!product.isSelected ? 'opacity-50' : ''}`}
                  value={product.productSettings.maxPayout}
                  onChange={(e) =>
                    handleProductFieldChange(product.productId, 'maxPayout', Number(e.target.value))
                  }
                  disabled={product.productSettings.unlimited || !product.isSelected}
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
    </>
  );
};

export default AddPromotion;
