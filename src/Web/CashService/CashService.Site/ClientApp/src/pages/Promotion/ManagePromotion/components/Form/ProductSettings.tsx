import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import { ProductSetting } from '../../../../../services/interfaces/Promotion';

interface ProductSettingsProps {
  productCategories: ProductSetting[];
  handleProductFieldChange: (productId: number, field: string, value: number | boolean) => void;
  selectedProductCategories: number[];
  readOnlyMode: boolean;
}

const ProductSettings: React.FC<ProductSettingsProps> = ({
  productCategories,
  handleProductFieldChange,
  selectedProductCategories,
  readOnlyMode
}) => (
  <>
    {productCategories.map((product) => (
      <div
        key={product.productCategoryId}
        className="flex flex-wrap w-full">
        <div className="w-[190px] mr-[12px] mb-[12px]">
          <LabelTitleComponent text={`${product.productCategoryName} Percentage`} required />
          <div className="flex items-center relative">
            <input
              type="number"
              min="0"
              step="0.01"
              className={`control-field pr-[41px]! ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
              value={product.percentage}
              disabled={readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)}
              onChange={(e) =>
                handleProductFieldChange(
                  product.productCategoryId,
                  'percentage',
                  Number(e.target.value)
                )
              }
            />
            <span className={`absolute right-0 top-0 h-full w-[32px] leading-[28px] text-center border-l-[1px] border-[var(--border-control)] ${!selectedProductCategories.includes(product.productCategoryId) ? 'opacity-50' : ''}`}>
              %
            </span>
          </div>
        </div>

        <div className="w-[190px] mr-[12px] mb-[12px]">
          <LabelTitleComponent text="Min Turnover" required={true} />
          <input
            type="number"
            min="0"
            className={`control-field ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
            value={product.minTurnover}
            disabled={readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)}
            onChange={(e) =>
              handleProductFieldChange(
                product.productCategoryId,
                'minTurnover',
                Number(e.target.value)
              )
            }
          />
        </div>

        <div className="w-[190px] mr-[12px] mb-[12px]">
          <div className="flex justify-between items-center">
            <LabelTitleComponent text="Max Payout" required={true} />
            <div className="flex items-center text-[12px]">
              <input
                type="checkbox"
                className={`mr-[5px] ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
                checked={product.unlimited}
                disabled={readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)}
                onChange={(e) =>
                  handleProductFieldChange(product.productCategoryId, 'unlimited', e.target.checked)
                }
                id={`${product.productCategoryId}-unlimited`}
              />
              <label
                htmlFor={`${product.productCategoryId}-unlimited`}
                className="text-[var(--text-link-color)]"
              >
                Unlimited
              </label>
            </div>
          </div>
          <input
            type="text"
            min="0"
            className={`control-field ${product.unlimited ? 'opacity-50' : ''} ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
            value={product.maxPayout}
            disabled={readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) || product.unlimited}
            onChange={(e) =>
              handleProductFieldChange(
                product.productCategoryId,
                'maxPayout',
                Number(e.target.value)
              )
            }
          />
        </div>
      </div>
    ))}
  </>
);

export default ProductSettings;
