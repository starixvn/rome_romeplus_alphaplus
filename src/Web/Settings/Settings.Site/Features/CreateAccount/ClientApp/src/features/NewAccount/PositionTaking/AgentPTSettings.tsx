import cn from 'classnames';
import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block, BlockColumn, BlockRow } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { AgentPTSetting, AgentPTSubGroupConfigAndPTSetting, SubGroup } from '@/types/new-account';

import styles from './PositionTaking.module.scss';
import PositionTalkingNote from './PositionTalkingNote';
import PTSelect from './PTSelect';
import PTTemplateSelect from './PTTemplateSelect';
import { PTProps, PTRowProps } from './type';

function AgentPTRow({ is3DecimalSite, productGroupId, subGroupId }: PTRowProps) {
  const value = useNewAccountStore(
    (state) =>
      (state.ptSetting[productGroupId] as SubGroup[]).find(
        (item) => item.subGroupId === subGroupId,
      ) as AgentPTSubGroupConfigAndPTSetting,
  );
  const [isSelectingTemplate, updatePTSetting] = useNewAccountStore(
    (state) => [state.isSelectingPTTemplate, state.updatePTSetting],
    shallow,
  );

  const handleChangeValue = (newValue: Partial<AgentPTSetting>) => {
    updatePTSetting(productGroupId, subGroupId, { ...newValue });
  };

  return (
    <div className={cn({ [styles['is-selecting-template']]: isSelectingTemplate })}>
      <BlockRow label={value.subGroupName}>
        <BlockColumn>
          <PTSelect
            min={value.maxN2}
            max={value.minA2}
            value={value.a1}
            onSelectChange={(e) => handleChangeValue({ a1: Number(e.target.value) })}
            is3DecimalSite={is3DecimalSite}
          />
        </BlockColumn>
      </BlockRow>
    </div>
  );
}

export default function AgentPTSettings({ is3DecimalSite, productGroupId }: PTProps) {
  const { t } = useTranslation();
  const groupItems = useNewAccountStore(
    (state) => state.ptConfig[productGroupId],
  ) as AgentPTSubGroupConfigAndPTSetting[];

  return (
    <div className={styles.shortContent}>
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
          <BlockColumn>{t('agentPT')}</BlockColumn>
        </BlockRow>
        {groupItems?.map((value) => (
          <AgentPTRow
            is3DecimalSite={is3DecimalSite}
            key={value.subGroupId}
            productGroupId={productGroupId}
            subGroupId={value.subGroupId}
          />
        ))}
      </Block>
    </div>
  );
}
