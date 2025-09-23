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

    const filteredSettingItems = settingItems.filter((value) => !value.Url.includes('RemarkInfo'));

    return (
        <div className={styles.setting}>
            {filteredSettingItems.map((value, index) => {
                return (
                    <div className="col-xs-6" key={index}>
                        <CustomAnchorTag href={value.Url}>{value.Text}</CustomAnchorTag>
                    </div>
                );
            })}
        </div>
    );
}
