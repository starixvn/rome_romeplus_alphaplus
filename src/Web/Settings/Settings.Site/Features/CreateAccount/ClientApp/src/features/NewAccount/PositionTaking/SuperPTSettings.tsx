import cn from 'classnames';
import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import useNewAccountStore from '@/store/newAccountStore';
import { SubGroup, SuperPTSetting, SuperPTSubGroupConfigAndPTSetting } from '@/types/new-account';

import { Block, BlockColumn, BlockRow } from '../../Shared/Block';
import styles from './PositionTaking.module.scss';
import PositionTalkingNote from './PositionTalkingNote';
import PTSelect from './PTSelect';
import PTTemplateSelect from './PTTemplateSelect';
import {
    getPTSettingByChangeS3, getPTSettingByChangeUS3, maxM3, maxM3Auto, maxN3, maxS3, maxUS3, minM3, minM3Auto, minN3,
    minS3, minUS3
} from './superPTRule';
import { PTProps, PTRowProps } from './type';

interface PTColumnProps {
  value: SuperPTSubGroupConfigAndPTSetting;
  is3DecimalSite: boolean;
  handleChangeValue: (newValue: Partial<SuperPTSetting>) => void;
}

function US3({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minUS3(value.maxN4SubtractM4)}
      max={maxUS3(value.minS4)}
      value={value.uS3}
      onSelectChange={(e) => {
        handleChangeValue({
          ...getPTSettingByChangeUS3(value.maxN4, value.minM4, value.minS4, value.m3, value.n3, Number(e.target.value)),
        });
      }}
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function S3({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minS3(value.maxN4SubtractM4)}
      max={maxS3(value.minS4)}
      value={value.s3}
      onSelectChange={(e) => {
        handleChangeValue({
          ...getPTSettingByChangeS3(value.maxN4, value.minM4, value.minS4, value.m3, value.n3, Number(e.target.value)),
        });
      }}
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function M3Auto({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minM3Auto(value.maxN4, value.uS3)}
      max={maxM3Auto(value.minS4, value.minM4)}
      value={value.m3}
      onSelectChange={(e) =>
        handleChangeValue({
          m3: Number(e.target.value),
          n3: Number(e.target.value) > value.n3 ? value.n3 : Number(e.target.value),
        })
      }
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function M3({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minM3(value.maxN4, value.s3)}
      max={maxM3(value.minS4, value.s3, value.minM4)}
      value={value.m3}
      onSelectChange={(e) =>
        handleChangeValue({
          m3: Number(e.target.value),
          n3: Number(e.target.value) > value.n3 ? value.n3 : Number(e.target.value),
        })
      }
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function IsAuto({
  value,
  subGroupId,
  handleChangeValue,
}: {
  value: SuperPTSubGroupConfigAndPTSetting;
  subGroupId: number;
  handleChangeValue: (newValue: Partial<SuperPTSetting>) => void;
}) {
  return (
    <label htmlFor={`auto-${subGroupId}`} className={styles.copy}>
      <input
        id={`auto-${subGroupId}`}
        type="checkbox"
        checked={value.iS3}
        onChange={(e) => {
          const newIS3 = e.target.checked;
          if (newIS3) {
            handleChangeValue({
              iS3: newIS3,
              ...getPTSettingByChangeUS3(value.maxN4, value.minM4, value.minS4, value.m3, value.n3, value.uS3),
            });
            return;
          }

          handleChangeValue({
            iS3: newIS3,
            ...getPTSettingByChangeS3(value.maxN4, value.minM4, value.minS4, value.m3, value.n3, value.s3),
          });
        }}
      />
    </label>
  );
}

function SuperPTRow({ is3DecimalSite, productGroupId, subGroupId }: PTRowProps) {
  const value = useNewAccountStore(
    (state) =>
      (state.ptSetting[productGroupId] as SubGroup[]).find(
        (item) => item.subGroupId === subGroupId,
      ) as SuperPTSubGroupConfigAndPTSetting,
  );
  const [isSelectingTemplate, updatePTSetting] = useNewAccountStore(
    (state) => [state.isSelectingPTTemplate, state.updatePTSetting],
    shallow,
  );

  const handleChangeValue = (newValue: Partial<SuperPTSetting>) => {
    updatePTSetting(productGroupId, subGroupId, { ...newValue });
  };

  return (
    <div className={cn({ [styles['is-selecting-template']]: isSelectingTemplate })}>
      <BlockRow 
        label={
          <>
            {value.subGroupName}
            {value.tooltipMessage && value.tooltipMessage.trim() && (
              <span 
                className="icon-information-outline" 
                data-tooltip-id="global-tooltip" 
                data-tooltip-content={value.tooltipMessage}
                style={{ marginLeft: '8px', cursor: 'pointer' }}
              />
            )}
          </>
        }
      >
        <BlockColumn>
          {value.iS3 ? (
            <US3 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          ) : (
            <S3 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          )}
        </BlockColumn>
        <BlockColumn>
          <IsAuto value={value} subGroupId={subGroupId} handleChangeValue={handleChangeValue} />
        </BlockColumn>
        <BlockColumn>
          {value.iS3 ? (
            <M3Auto value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          ) : (
            <M3 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          )}
        </BlockColumn>
        <BlockColumn>
          <PTSelect
            min={minN3()}
            max={maxN3(value.m3)}
            value={value.n3}
            onSelectChange={(e) => handleChangeValue({ n3: Number(e.target.value) })}
            is3DecimalSite={is3DecimalSite}
          />
        </BlockColumn>
      </BlockRow>
    </div>
  );
}

export default function SuperPTSettings({ is3DecimalSite, productGroupId }: PTProps) {
  const { t } = useTranslation();
  const groupItems = useNewAccountStore(
    (state) => state.ptConfig[productGroupId],
  ) as SuperPTSubGroupConfigAndPTSetting[];

  return (
    <Block
      title={
        <>
          {t('PositionTaking')}
          <PositionTalkingNote />
        </>
      }
      headerChildren={<PTTemplateSelect />}
    >
      <BlockRow isHeaderRow>
        <BlockColumn>{t('SuperPT')}</BlockColumn>
        <BlockColumn>{t('IsAuto')}</BlockColumn>
        <BlockColumn>{t('MasterMaxPT')}</BlockColumn>
        <BlockColumn>{t('MasterMinPT')}</BlockColumn>
      </BlockRow>
      {groupItems.map((value) => (
        <SuperPTRow
          key={value.subGroupId}
          is3DecimalSite={is3DecimalSite}
          productGroupId={productGroupId}
          subGroupId={value.subGroupId}
        />
      ))}
    </Block>
  );
}
