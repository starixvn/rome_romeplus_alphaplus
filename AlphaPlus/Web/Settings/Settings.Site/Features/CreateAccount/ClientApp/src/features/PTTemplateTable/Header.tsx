import { useTranslation } from 'react-i18next';

import usePTTemplateStore from './ptTemplateStore';

export default function Header({ actorLevelId }: { actorLevelId: number }) {
  const isSpecialSite = usePTTemplateStore((state) => state.isSpecialSite);
  const ptSettings = usePTTemplateStore((state) => state.ptSettings);
  const { t } = useTranslation();

  if (isSpecialSite) {
    return (
      <thead>
        <tr>
          <th style={{ width: '55px' }}>#</th>
          <th>{`${t('TemplateName')} `}</th>
          <th>{t(`PT_Level_${actorLevelId}`)}</th>
          <th style={{ width: '110px', whiteSpace: 'normal' }}>{t('SelectByDefault')}</th>
          <th style={{ width: '90px' }}>{t('Action')}</th>
        </tr>
      </thead>
    );
  }

  return (
    <thead>
      <tr>
        <th rowSpan={3} style={{ width: '55px' }}>
          #
        </th>
        <th rowSpan={3} style={{ width: '450px' }}>
          {`${t('TemplateName')} `}
        </th>
        <th colSpan={ptSettings.map((setting) => setting.subGroups).flat().length}>{t(`PT_Level_${actorLevelId}`)}</th>
        <th rowSpan={3} style={{ width: '110px', whiteSpace: 'normal' }}>
          {t('SelectByDefault')}
        </th>
        <th rowSpan={3} style={{ width: '90px' }}>
          {t('Action')}
        </th>
      </tr>
      <tr className="thead-row2">
        {ptSettings.map((group) => (
          <th key={group.groupId} colSpan={group.subGroups.length} rowSpan={group.subGroups.length > 1 ? 1 : 2}>
            {group.groupName}
          </th>
        ))}
      </tr>
      <tr className="thead-row2">
        {ptSettings
          .filter((setting) => setting.subGroups.length > 1)
          .map((setting) => setting.subGroups)
          .flat()
          .map((group) => (
            <th key={group.subGroupId}>{group.subGroupName}</th>
          ))}
      </tr>
    </thead>
  );
}
