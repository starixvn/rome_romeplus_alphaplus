import axios from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import ProductSettingInfoLayout from '@/features/ProductSettingInfo/ProductSettingInfoLayout';

import styles from './ProductSettingInfo.module.scss';
import ProductSettingInformationTable from './ProductSettingInformationTable';
import SettingRemarksMarketBuilder from './SettingRemarksMarket';
import { ProductSetting } from './type';

type ProductSettingInfoResponse = {
  ProductSettingInfo: ProductSetting[];
  RoleId: number;
  Is789y: boolean;
  ShowBBin: boolean;
  ShowRNGKenoNumberGame: boolean;
};

export default function ProductSettingInfo() {
  const [data, setData] = useState<ProductSettingInfoResponse>();
  const { t } = useTranslation();
  useEffect(() => {
    const productSettingInfoUrl = (document.getElementById('productSettingInfoUrl') as HTMLInputElement).value;
    const getProductsInformationSetting = async () =>
      axios.get<ProductSettingInfoResponse>(`${productSettingInfoUrl}/GetProductSettingInfo`);
    getProductsInformationSetting()
      .then(async (response) => {
        const productsInformationData = response.data;
        setData(productsInformationData);
      })
      .catch(() => {
        setData(undefined);
      });
  }, []);

  if (!data) {
    return <div />;
  }

  return (
    <ProductSettingInfoLayout>
      <div className={styles.container}>
        <div className={styles.session}>
          <div className={styles.title}>{t('SettingRemarks')}</div>
          <SettingRemarksMarketBuilder
            userRoleId={data.RoleId}
            Is789y={data.Is789y}
            showNoteBBin={data.ShowBBin}
            showNoteRNGKenoNumberGame={data.ShowRNGKenoNumberGame}
          />
        </div>
        <div>
          <div className={styles.title}>{t('ProductSettingInformation')}</div>
          <div>{t('ProductGroupTableIntro')}</div>
          <ProductSettingInformationTable ProductSettingInfo={data.ProductSettingInfo} />
        </div>
      </div>
    </ProductSettingInfoLayout>
  );
}
