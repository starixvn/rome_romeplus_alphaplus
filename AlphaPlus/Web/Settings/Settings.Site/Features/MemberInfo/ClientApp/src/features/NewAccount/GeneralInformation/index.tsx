import cn from 'classnames';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';

import { UserLevel, UserLevelName } from '@/constants';
import CommissionGroup from '@/features/NewAccount/GeneralInformation/CommissionGroup';
import { Block, BlockItem } from '@/features/Shared/Block';
import Checkbox from '@/features/Shared/Checkbox';
import { IconCorrect, IconWrong } from '@/features/Shared/Icons';
import Require from '@/features/Shared/Require';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';
import { GeneralInfo } from '@/types/new-account';
import { formatNumber } from '@/utils/number-format';

import styles from './GeneralInfo.module.scss';
import { GeneralInfoErrors, validateGeneralInfo } from './generalInfoValidator';
import PasswordRule from './PasswordRule';
import Username from './Username';

function ToNumberFormat(value: string) {
  const RegexAllowedCharsRegex = /,/g;
  return Number(value.replace(RegexAllowedCharsRegex, ''));
}

function getTargetTooltip(error: GeneralInfoErrors) {
  const { userName, password, maxCredit, memberMaxCredit } = error;
  if (userName) return 'userName';
  if (password) return 'password';
  if (maxCredit) return 'maxCredit';
  if (memberMaxCredit) return 'memberMaxCredit';
  return '';
}

export default function GeneralInformation() {
  const { t } = useTranslation();
  const submitCount = useNewAccountStore((state) => state.submitCount);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);
  const updateGeneralInfo = useNewAccountStore((state) => state.updateGeneralInfo);
  const updateSportsBookCommissions = useNewAccountStore((state) => state.updateSportsBookCommissions);
  const { uplineUserName, newUserName, uplineRoleId } = generalInfo;

  const [showPassword, setShowPassword] = useState(false);
  const [openTooltip, setOpenTooltip] = useState(false);
  const [targetTooltip, setTargetTooltip] = useState<string>('');
  const [errors, setErrors] = useState<GeneralInfoErrors>({
    userName: '',
    password: '',
    maxCredit: '',
    memberMaxCredit: '',
  });

  useEffect(() => {
    if (submitCount > 0) {
      validateGeneralInfo(generalInfo, t).then((error) => {
        const { userName, password, maxCredit, memberMaxCredit } = error;
        if (userName || password || maxCredit || memberMaxCredit) {
          setOpenTooltip(true);
          setTargetTooltip(getTargetTooltip(error));
          setErrors(error);
        }
      });
    }
  }, [submitCount]);

  const renderAllowPT = () => {
    const isCreateMaster = generalInfo.uplineRoleId === UserLevel.Super;
    if (!isCreateMaster) {
      return null;
    }
    return (
      <div className={styles['col-allowpt']}>
        <BlockItem label={'\u00A0'}>
          <Checkbox
            label={t('allowAutoPt') ?? ''}
            checked={!generalInfo.disableAutoMPT}
            onChange={() => updateGeneralInfo({ disableAutoMPT: !generalInfo.disableAutoMPT })}
            marginLeft
          />
        </BlockItem>
      </div>
    );
  };

  const validate = async (value: Partial<GeneralInfo>, field: keyof GeneralInfoErrors) => {
    const result = await validateGeneralInfo({ ...generalInfo, ...value }, t, field);
    const { userName, password, maxCredit, memberMaxCredit } = result;
    if (field === 'userName') setErrors((errors) => ({ ...errors, userName }));
    if (field === 'password') setErrors((errors) => ({ ...errors, password }));
    if (field === 'maxCredit') setErrors((errors) => ({ ...errors, maxCredit, memberMaxCredit }));
    if (field === 'memberMaxCredit') setErrors((errors) => ({ ...errors, memberMaxCredit }));
  };

  return (
    <Block
      title={t('general-information')}
      isCollapse={{ collapse: !Object.values(errors).some((item) => item !== ''), resetCollapse: submitCount }}
    >
      <div className={`${styles.container} ${styles['general-info']}`}>
        <div className={styles['row-info']}>
          <BlockItem
            label={
              <>
                {`${t('username')} `}
                <Require />
              </>
            }
          >
            <Username
              error={errors.userName}
              newUserName={newUserName}
              uplineUserName={uplineUserName}
              onChangeUserName={(value) => {
                setTargetTooltip('userName');
                setOpenTooltip(true);
                updateGeneralInfo({
                  userName: value.toUpperCase(),
                  newUserName: value.substring(value.length - newUserName.length),
                });
                validate({ userName: value }, 'userName');
              }}
              onBlur={() => {
                setOpenTooltip(false);
              }}
            />
          </BlockItem>
          <BlockItem
            label={
              <>
                {`${t('password')} `}
                <Require />
                <PasswordRule />
              </>
            }
          >
            <div className={styles.password}>
              <input
                maxLength={uplineRoleId === UserLevel.Agent ? 10 : 16}
                id="password"
                type={showPassword ? 'text' : 'password'}
                name="password"
                value={generalInfo.password ?? ''}
                className={cn({ error: errors.password })}
                onChange={(e) => {
                  updateGeneralInfo({ password: e.target.value });
                  validate({ password: e.target.value }, 'password');
                }}
                onBlur={() => {
                  setOpenTooltip(false);
                }}
                onFocus={() => {
                  setTargetTooltip('password');
                  setOpenTooltip(true);
                }}
              />
              <span
                className={cn('icon', !showPassword ? 'icon-eye-close' : 'icon-eye')}
                aria-hidden="true"
                onClick={() => setShowPassword(!showPassword)}
              ></span>
            </div>
            {errors.password && <span>{errors.password ? IconWrong : IconCorrect}</span>}
          </BlockItem>
          <BlockItem label={t('firstName')}>
            <input
              type="text"
              value={generalInfo.firstName ?? ''}
              maxLength={50}
              onChange={(e) => updateGeneralInfo({ firstName: e.target.value })}
            />
          </BlockItem>
          <BlockItem label={t('lastName')}>
            <input
              type="text"
              value={generalInfo.lastName ?? ''}
              maxLength={50}
              onChange={(e) => updateGeneralInfo({ lastName: e.target.value })}
            />
          </BlockItem>
        </div>
        <div className={styles['row-info']}>
          <div className={styles['col-max-credit']}>
            <BlockItem
              label={
                <>
                  {`${t(`${UserLevelName[uplineRoleId - 1].toLowerCase()}MaxCredit`)} `}
                  <Require />
                </>
              }
            >
              <NumericFormat
                id="maxCredit"
                name="maxCredit"
                maxLength={14}
                value={generalInfo.maxCredit}
                className={cn({ error: errors.maxCredit })}
                onChange={(e) => {
                  const value = e.target.value ? ToNumberFormat(e.target.value) : undefined;
                  updateGeneralInfo({
                    maxCredit: value,
                    uplineMemberMaxCredit: value === undefined || isNaN(value) ? 0 : value,
                  });
                  validate({ maxCredit: value }, 'maxCredit');
                  validate(
                    { uplineMemberMaxCredit: value === undefined || isNaN(value) ? 0 : value },
                    'memberMaxCredit',
                  );
                }}
                onBlur={() => {
                  setTargetTooltip('');
                  setOpenTooltip(false);
                }}
                onFocus={() => {
                  setTargetTooltip('maxCredit');
                  setOpenTooltip(true);
                }}
                allowNegative={false}
                decimalScale={2}
                thousandSeparator=","
                valueIsNumericString
              />
              <span>{errors.maxCredit ? IconWrong : IconCorrect}</span>
              <span className={styles['max-credit']}>
                {`${t(`${UserLevelName[uplineRoleId]}_AvailableGivenCredit`)}: `}
                <b>{formatNumber(generalInfo.uplineMaxCredit)}</b>
              </span>
            </BlockItem>

            {uplineRoleId === UserLevel.Master && (
              <BlockItem
                label={
                  <>
                    {`${t(`memberMaxCredit`)} `}
                    <Require />
                  </>
                }
              >
                <NumericFormat
                  id="memberMaxCredit"
                  name="memberMaxCredit"
                  maxLength={14}
                  value={generalInfo.memberMaxCredit}
                  className={cn({ error: errors.memberMaxCredit })}
                  onChange={(e) => {
                    const value = e.target.value ? ToNumberFormat(e.target.value) : undefined;
                    updateGeneralInfo({ memberMaxCredit: value });
                    validate({ memberMaxCredit: value }, 'memberMaxCredit');
                  }}
                  onBlur={() => {
                    setTargetTooltip('');
                    setOpenTooltip(false);
                  }}
                  onFocus={() => {
                    setTargetTooltip('memberMaxCredit');
                    setOpenTooltip(true);
                  }}
                  allowNegative={false}
                  decimalScale={2}
                  thousandSeparator=","
                  valueIsNumericString
                />
                <span>{errors.memberMaxCredit ? IconWrong : IconCorrect}</span>
                <span className={styles['max-credit']}>
                  {`${t('Member_AvailableGivenCredit')}: `}
                  <b>{formatNumber(generalInfo.uplineMemberMaxCredit)}</b>
                </span>
              </BlockItem>
            )}
          </div>

          {renderAllowPT()}
          <BlockItem label={t('phone')}>
            <input
              type="text"
              value={generalInfo.phone ?? ''}
              maxLength={50}
              onChange={(e) => updateGeneralInfo({ phone: e.target.value })}
            />
          </BlockItem>
          <BlockItem label={t('mobilePhone')}>
            <input
              type="text"
              value={generalInfo.mobilePhone ?? ''}
              maxLength={50}
              onChange={(e) => updateGeneralInfo({ mobilePhone: e.target.value })}
            />
          </BlockItem>
          <BlockItem label={t('fax')}>
            <input
              type="text"
              value={generalInfo.fax ?? ''}
              maxLength={50}
              onChange={(e) => updateGeneralInfo({ fax: e.target.value })}
            />
          </BlockItem>
          {uplineRoleId === UserLevel.Agent && (
            <BlockItem label={t('Group')}>
              <CommissionGroup
                value={generalInfo.group}
                onChange={(e) => {
                  const newCommissionGroup = e.target.value;
                  updateGeneralInfo({ group: newCommissionGroup });
                  const lastChar = newCommissionGroup[newCommissionGroup.length - 1];
                  const newCommission =
                    generalInfo.commissions.find((e) => e.name === newCommissionGroup || e.name === lastChar)
                      ?.discount ?? 0;
                  updateSportsBookCommissions(1, 1, newCommission);
                }}
              />
            </BlockItem>
          )}

          {errors[targetTooltip as keyof GeneralInfoErrors] && (
            <WrapperTooltip
              content={errors[targetTooltip as keyof GeneralInfoErrors]}
              anchorSelect={`#${targetTooltip}`}
              type="error"
              isOpen={openTooltip}
            />
          )}
        </div>
      </div>
    </Block>
  );
}
