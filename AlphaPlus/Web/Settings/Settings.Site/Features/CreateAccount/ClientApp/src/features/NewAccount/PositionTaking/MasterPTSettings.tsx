import cn from 'classnames';
import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block, BlockColumn, BlockRow } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { MasterPTSetting, MasterPTSubGroupConfigAndPTSetting, SubGroup } from '@/types/new-account';

import {
    getMasterPTByChangeM2, getMasterPTByChangeUM2, maxA2, maxA2Auto, maxM2, maxN2, maxUM2, minA2, minA2Auto, minM2,
    minN2, minUM2
} from './masterPTRule';
import styles from './PositionTaking.module.scss';
import PositionTalkingNote from './PositionTalkingNote';
import PTSelect from './PTSelect';
import PTTemplateSelect from './PTTemplateSelect';
import { PTProps, PTRowProps } from './type';

interface PTColumnProps {
  value: MasterPTSubGroupConfigAndPTSetting;
  is3DecimalSite: boolean;
  handleChangeValue: (newValue: Partial<MasterPTSetting>) => void;
}

function UM2({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minUM2(value.maxN3SubtractA4)}
      max={maxUM2(value.minM3)}
      value={value.uM2}
      onSelectChange={(e) => {
        handleChangeValue({
          ...getMasterPTByChangeUM2(value.minA4, value.maxN3, value.minM3, value.a2, value.n2, Number(e.target.value)),
        });
      }}
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function M2({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minM2(value.maxN3SubtractA4)}
      max={maxM2(value.minM3)}
      value={value.m2}
      onSelectChange={(e) => {
        handleChangeValue({
          ...getMasterPTByChangeM2(value.minA4, value.maxN3, value.minM3, value.a2, value.n2, Number(e.target.value)),
        });
      }}
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function A2Auto({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minA2Auto(value.maxN3, value.uM2)}
      max={maxA2Auto(value.minM3, value.minA4)}
      value={value.a2}
      onSelectChange={(e) =>
        handleChangeValue({
          a2: Number(e.target.value),
          n2: Number(e.target.value) > value.n2 ? value.n2 : Number(e.target.value),
        })
      }
      is3DecimalSite={is3DecimalSite}
    />
  );
}

function A2({ value, is3DecimalSite, handleChangeValue }: PTColumnProps) {
  return (
    <PTSelect
      min={minA2(value.maxN3, value.m2)}
      max={maxA2(value.minM3, value.m2, value.minA4)}
      value={value.a2}
      onSelectChange={(e) =>
        handleChangeValue({
          a2: Number(e.target.value),
          n2: Number(e.target.value) > value.n2 ? value.n2 : Number(e.target.value),
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
  value: MasterPTSubGroupConfigAndPTSetting;
  subGroupId: number;
  handleChangeValue: (newValue: Partial<MasterPTSetting>) => void;
}) {
  return (
    <label htmlFor={`auto-${subGroupId}`} className={styles.copy}>
      <input
        id={`auto-${subGroupId}`}
        type="checkbox"
        checked={value.iM2}
        onChange={(e) => {
          const newIM2 = e.target.checked;

          if (newIM2) {
            handleChangeValue({
              iM2: newIM2,
              ...getMasterPTByChangeUM2(value.minA4, value.maxN3, value.minM3, value.a2, value.n2, value.uM2),
            });
            return;
          }

          handleChangeValue({
            iM2: newIM2,
            ...getMasterPTByChangeM2(value.minA4, value.maxN3, value.minM3, value.a2, value.n2, value.m2),
          });
        }}
      />
    </label>
  );
}

function MasterPTRow({ is3DecimalSite, productGroupId, subGroupId }: PTRowProps) {
  const value = useNewAccountStore(
    (state) =>
      (state.ptSetting[productGroupId] as SubGroup[]).find(
        (item) => item.subGroupId === subGroupId,
      ) as MasterPTSubGroupConfigAndPTSetting,
  );
  const [disableAutoMPT, isSelectingTemplate] = useNewAccountStore(
    (state) => [state.disableAutoMPT, state.isSelectingPTTemplate],
    shallow,
  );
  const updatePTSetting = useNewAccountStore((state) => state.updatePTSetting);

  const handleChangeValue = (newValue: Partial<MasterPTSetting>) => {
    updatePTSetting(productGroupId, subGroupId, { ...newValue });
  };

  return (
    <div className={cn({ [styles['is-selecting-template']]: isSelectingTemplate })}>
      <BlockRow label={value.subGroupName}>
        <BlockColumn>
          {value.iM2 ? (
            <UM2 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          ) : (
            <M2 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          )}
        </BlockColumn>
        {!disableAutoMPT && (
          <BlockColumn>
            <IsAuto value={value} subGroupId={subGroupId} handleChangeValue={handleChangeValue} />
          </BlockColumn>
        )}

        <BlockColumn>
          {value.iM2 ? (
            <A2Auto value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          ) : (
            <A2 value={value} is3DecimalSite={is3DecimalSite} handleChangeValue={handleChangeValue} />
          )}
        </BlockColumn>
        <BlockColumn>
          <PTSelect
            min={minN2()}
            max={maxN2(value.a2)}
            value={value.n2}
            onSelectChange={(e) => handleChangeValue({ n2: Number(e.target.value) })}
            is3DecimalSite={is3DecimalSite}
          />
        </BlockColumn>
      </BlockRow>
    </div>
  );
}

export default function MasterPTSettings({ is3DecimalSite, productGroupId }: PTProps) {
  const { t } = useTranslation();
  const disableAutoMPT = useNewAccountStore((state) => state.disableAutoMPT);
  const groupItems = useNewAccountStore(
    (state) => state.ptConfig[productGroupId],
  ) as MasterPTSubGroupConfigAndPTSetting[];

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
        <BlockColumn>{t('MasterPT')}</BlockColumn>
        {!disableAutoMPT && <BlockColumn>{t('IsAuto')}</BlockColumn>}
        <BlockColumn>{t('AgentMaxPT')}</BlockColumn>
        <BlockColumn>{t('AgentMinPT')}</BlockColumn>
      </BlockRow>
      {groupItems.map((value) => (
        <MasterPTRow
          is3DecimalSite={is3DecimalSite}
          key={value.subGroupId}
          productGroupId={productGroupId}
          subGroupId={value.subGroupId}
        />
      ))}
    </Block>
  );
}
