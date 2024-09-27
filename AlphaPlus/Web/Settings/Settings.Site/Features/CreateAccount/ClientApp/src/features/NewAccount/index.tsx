import i18next, { changeLanguage } from 'i18next';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Outlet, useNavigate, useParams, useSearchParams } from 'react-router-dom';
import { shallow } from 'zustand/shallow';

import { createAccount, getCustomerSetting } from '@/api/new-account';
import { getPTTemplates } from '@/api/position-taking';
import { createAccountRootUrl } from '@/constants';
import { ProductGroupIds } from '@/constants/product';
import ProductTab from '@/features/NewAccount/ProductGroupTab';
import Layout from '@/features/Shared/Layout';
import scrollToElementByClassName from '@/features/Shared/ScrollHelper';
import useNewAccountStore from '@/store/newAccountStore';
import { SubMessagesResult } from '@/types';
import { encrypt } from '@/utils/encryptor';

import ErrorBlock from '../Shared/ErrorBlock';
import Loader from '../Shared/Loader';
import WrapperTooltip from '../Shared/WrapperTooltip';
import GeneralInformation from './GeneralInformation';
import { validateGeneralInfo } from './GeneralInformation/generalInfoValidator';
import styles from './NewAccount.module.scss';
import NewAccountSuccessPopup from './NewAccountPopup/NewAccountSuccessPopup';
import Popup from './NewAccountPopup/Popup';
import TransferCondition from './TransferCondition';


export default function NewAccount() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { productId } = useParams();
  const [searchParams] = useSearchParams();

  const initNewAccountState = useNewAccountStore((state) => state.intiNewAccountState);
  const validateParlayMaxPayout = useNewAccountStore((state) => state.validateParlayMaxPayout);
  const validateSportsBookBetSetting = useNewAccountStore((state) => state.validateSportsBookBetSetting);
  const validateRNGBetSetting = useNewAccountStore((state) => state.validateRNGGamesBetSetting);
  const validateOtherGamesBetSetting = useNewAccountStore((state) => state.validateOtherGamesBetSetting);
  const validateTransferCondition = useNewAccountStore((state) => state.validateTransferCondition);

  const [theme, setTheme] = useNewAccountStore((state) => [state.theme, state.setTheme], shallow);
  const userLevel = useNewAccountStore((state) => state.userLevel);
  const showPopup = useNewAccountStore((state) => state.showPopup);
  const getSettings = useNewAccountStore((state) => state.getProductGroupAndUISettings);
  const displayTransferCondition = useNewAccountStore((state) => state.displayTransferCondition);
  const increaseSubmitCount = useNewAccountStore((state) => state.increaseSubmitCount);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<SubMessagesResult | ''>();
  const [violatePT, setViolatePT] = useState(false);
  const [blockSubmit, setBlockSubmit] = useState(false);
  useEffect(() => {
    const timer = setTimeout(() => window.top?.indexManager.initFeedbackFunction(6, undefined, 500), 1000);
    return () => clearTimeout(timer);
  }, []);

  useEffect(() => {
    if (searchParams.has('isDarkMode')) {
      const isDarkMode = searchParams.get('isDarkMode') === 'true';
      setTheme(isDarkMode ? 'dark' : 'light');
    }

    if (searchParams.has('lng')) {
      const lng = searchParams.get('lng') ?? 'en-US';
      const { languages } = i18next;
      if (!languages.includes(lng)) {
        changeLanguage('en-US');
      }
    }
  }, [searchParams, setTheme, changeLanguage]);

  useEffect(() => {
    getCustomerSetting()
      .then(async (response) => {
        const customerSettings = response?.data;
        if (!customerSettings) setLoading(true);
        const { data: ptTemplates } = await getPTTemplates();
        const ptError = initNewAccountState(customerSettings.generalInfo, customerSettings.productGroups, ptTemplates);
        if (ptError.length > 0) {
          setViolatePT(true);
          setError({
            code: 0,
            message: t('violateMinMaxPT'),
            isSuccessful: false,
            subMessages: [
              ...ptError.map((err) => {
                if (err.message.includes('MinPT')) {
                  return {
                    isLive: err.isLive,
                    message: err.message.replace('MinPT', t('minPT'))};
                }

                if (err.message.includes('MaxPT')) {
                  return {
                    isLive: err.isLive,
                    message: err.message.replace('MaxPT', t('maxPT'))};
                }

                return err;
              }),
            ],
          });
        }
        setLoading(false);

      })
      .catch((e) => {
        // console.log(e);
        setLoading(true);
      });

  }, [initNewAccountState, t]);

  const validateAllProduct = () => {
    const productError = [];
    if (!validateSportsBookBetSetting()) {
      productError.push(ProductGroupIds.SportsBook);
    }
    if (!validateParlayMaxPayout(userLevel)) {
      productError.push(ProductGroupIds.SportsBook);
    }
    if (!validateRNGBetSetting()) {
      productError.push(ProductGroupIds.RNGGames);
    }
    if (!validateOtherGamesBetSetting()) {
      productError.push(ProductGroupIds.OthersGames);
    }

    return productError;
  };

  const handleCreateNewAccount = async () => {
    setError('');
    const { uiBehavior, generalInfo, productGroups, templateName } = getSettings();
    const generalError = await validateGeneralInfo(generalInfo, t);
    let validateSuccess = true;
    const { userName, password, maxCredit, memberMaxCredit } = generalError;
    if (userName || password || maxCredit || memberMaxCredit) validateSuccess = false;

    const productErrs = validateAllProduct();
    if (productErrs.length > 0) {
      const currentProductId = productId ?? 1;
      if (!productErrs.includes(Number(currentProductId))) {
        navigate(`/products/${Math.min(...productErrs)}`);
      }
      validateSuccess = false;
    }

    if (!validateTransferCondition()) validateSuccess = false;

    if (!validateSuccess) {
      increaseSubmitCount();
      setBlockSubmit(false);
      setTimeout(() => scrollToElementByClassName('errTooltipOpen'), 250);
      return;
    }

    var secretKey = (document.getElementById('SecretKey') as HTMLInputElement).value;
    var encryptedPassword = encrypt(generalInfo.password, secretKey);
    setBlockSubmit(true);
    const { data: result } = await createAccount(
      uiBehavior,
      { ...generalInfo, password: encryptedPassword, templateName },
      productGroups,
    );
    setBlockSubmit(false);
    if (!result.isSuccessful) {
      window.scrollTo(0, 0);
      setError(result);
    } else {
      showPopup(
        <NewAccountSuccessPopup
          username={generalInfo.userName}
          domain={result.availableAlphaDomain?.slice(0,3).join(', ') || ''}
          credit={generalInfo.maxCredit?.toLocaleString("en-US", {
            style: "decimal",
            maximumFractionDigits: 2
          })  || "0"}
          handleOk={() => {
            const { language } = i18next;
            window.location.replace(
              `${window.location.origin}${createAccountRootUrl}?lng=${language}&${
                theme === 'dark' ? 'isDarkMode=true' : ''
              }`,
            );
          }}
        />
      );
    }
  };

  return (
    <Layout theme={theme}>
      <Popup />
      {(loading || blockSubmit) && <Loader />}
      {!loading ? (
        <>
          <div className={styles.btn}>
            <button type="button" onClick={handleCreateNewAccount} disabled={violatePT || blockSubmit}>
              {t('CreateAccount')}
            </button>
          </div>
          {error && <ErrorBlock error={error} />}
          <GeneralInformation />
          <div className={styles.product}>
            <WrapperTooltip id="global-tooltip" />
            <ProductTab />
            <div className={styles['content-product']}>
              <Outlet />
            </div>
          </div>
          {displayTransferCondition && <TransferCondition />}
          <div className={styles.btn}>
            <button type="button" onClick={handleCreateNewAccount} disabled={violatePT || blockSubmit}>
              {t('CreateAccount')}
            </button>
          </div>
        </>
      ) : (
        ''
      )}
    </Layout>
  );
}
