import { useTranslation } from 'react-i18next';

import { UserLevel } from '../../constants';

export default function UserLevelName({ levelId }: { levelId: number }) {
    const { t } = useTranslation();
    switch (levelId) {
        case UserLevel.Super:
            return t('Super');
        case UserLevel.Master:
            return t('Master');
        case UserLevel.Agent:
            return t('Agent');
        default:
            return t('Member');
    }
}
