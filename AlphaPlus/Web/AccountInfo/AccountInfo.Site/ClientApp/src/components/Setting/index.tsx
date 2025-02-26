import { useGlobalContext } from '../../context/global-context';
import SettingItem from '../../models/setting-item';
import CustomAnchorTag from '../common/CustomAnchorTag';
import NoInformation from '../common/NoInformation';
import styles from './Settings.module.scss';

type Props = {
    settingItems: SettingItem[] | undefined;
};

export default function Setting({ settingItems }: Props) {
    const { mobileView } = useGlobalContext();

    if (!settingItems) {
        return mobileView ? <NoInformation /> : null;
    }

    return (
        <div className={styles.setting}>
            {settingItems.map((value, index) => {
                return (
                    <div className="col-xs-6" key={index}>
                        <CustomAnchorTag href={value.Url}>{value.Text}</CustomAnchorTag>
                    </div>
                );
            })}
        </div>
    );
}
