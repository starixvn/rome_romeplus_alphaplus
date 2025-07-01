import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import { ProductSetting } from '../../../../../services/interfaces/Promotion';

interface ProductSettingsProps {
  productSettings: ProductSetting[];
  handleProductFieldChange: (productId: number, field: string, value: number | boolean) => void;
}

const ProductSettings: React.FC<ProductSettingsProps> = ({
  productSettings,
  handleProductFieldChange,
}) => (
  <>
    {productSettings.map((product) => (
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
              className={`control-field pr-[32px]`}
              value={product.percentage}
              onChange={(e) =>
                handleProductFieldChange(product.productId, 'percentage', Number(e.target.value))
              }
            />
            <span className="absolute right-0 top-0 text-[12px] text-[#333333] h-full w-[32px] leading-[32px] text-center font-bold border-l-[1px] border-[var(--border-control)]">
              %
            </span>
          </div>
        </div>

        <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
          <LabelTitleComponent text="Min Turnover" required={true} />
          <input
            type="number"
            min="0"
            className={`control-field`}
            value={product.minTurnover}
            onChange={(e) =>
              handleProductFieldChange(product.productId, 'minTurnover', Number(e.target.value))
            }
          />
        </div>

        <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
          <div className="flex justify-between items-center">
            <LabelTitleComponent text="Max Payout" required={true} />
            <div className="flex items-center text-[12px]">
              <input
                type="checkbox"
                className="mr-2"
                checked={product.unlimited}
                onChange={(e) =>
                  handleProductFieldChange(product.productId, 'unlimited', e.target.checked)
                }
                id={`${product.productId}-unlimited`}
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
            className={`control-field ${product.unlimited ? 'opacity-50' : ''}`}
            value={product.maxPayout}
            onChange={(e) =>
              handleProductFieldChange(product.productId, 'maxPayout', Number(e.target.value))
            }
            disabled={product.unlimited}
          />
        </div>
      </div>
    ))}
  </>
);

export default ProductSettings;
