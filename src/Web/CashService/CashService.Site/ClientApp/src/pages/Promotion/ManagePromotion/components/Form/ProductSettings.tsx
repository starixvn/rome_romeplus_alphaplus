import React from 'react';
import { useTranslation } from 'react-i18next';
import { TitleComponent } from '../../../../../components/Title';
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
  readOnlyMode,
}) => {
  const { t } = useTranslation('promotion');
  return (
    <>
      {productCategories.map((product) => (
        <div
          key={product.productCategoryId}
          className="w-[618px] border-[1px] border-[var(--border-control)] rounded-[4px] px-[10px] pt-[5px] bg-[#f8f8f8] dark:bg-[#2b2b2b] mb-[8px] "
        >
          <div className="">
            <div className="inline-block mr-[10px] [&>*]:pt-[1px]! [&>*]:pb-[3px]!">
              <TitleComponent
                text={`${product.productCategoryName}`}
                type="normal"
              ></TitleComponent>
            </div>
          </div>
          <div className="flex flex-wrap w-full">
            <div className="w-[190px] mr-[10px] mb-[10px]">
              <TitleComponent text={`${t('addPromotionForm.percentage')}`} required type="label" />
              <div className="flex items-center relative">
                <input
                  type="number"
                  min="1"
                  step="0.01"
                  className={`control-field pr-[41px]! ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
                  value={product.percentage}
                  disabled={
                    readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)
                  }
                  onChange={(e) =>
                    handleProductFieldChange(
                      product.productCategoryId,
                      'percentage',
                      Number(e.target.value)
                    )
                  }
                />
                <span
                  className={`absolute right-0 top-0 h-full w-[32px] leading-[28px] text-center border-l-[1px] border-[var(--border-control)] ${!selectedProductCategories.includes(product.productCategoryId) ? 'opacity-50' : ''}`}
                >
                  %
                </span>
              </div>
            </div>

            <div className="w-[190px] mr-[12px] mb-[12px]">
              <TitleComponent
                text={t('addPromotionForm.minTurnover')}
                required={true}
                type="label"
              />
              <input
                type="number"
                min="1"
                className={`control-field ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
                value={product.minTurnover}
                disabled={
                  readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)
                }
                onChange={(e) =>
                  handleProductFieldChange(
                    product.productCategoryId,
                    'minTurnover',
                    Number(e.target.value)
                  )
                }
              />
            </div>

            <div className="w-[190px] mb-[12px]">
              <div className="flex justify-between items-center">
                <TitleComponent
                  text={t('addPromotionForm.maxPayout')}
                  required={true}
                  type="label"
                />
                <div className="flex items-center text-[12px]">
                  <input
                    type="checkbox"
                    className={`mr-[5px] ${readOnlyMode || !selectedProductCategories.includes(product.productCategoryId) ? 'disabled' : ''}`}
                    checked={product.unlimited}
                    disabled={
                      readOnlyMode || !selectedProductCategories.includes(product.productCategoryId)
                    }
                    onChange={(e) =>
                      handleProductFieldChange(
                        product.productCategoryId,
                        'unlimited',
                        e.target.checked
                      )
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
                disabled={
                  readOnlyMode ||
                  !selectedProductCategories.includes(product.productCategoryId) ||
                  product.unlimited
                }
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
        </div>
      ))}
    </>
  );
};

export default ProductSettings;
