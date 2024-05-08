import SettingItem from '../../models/setting-item';
import CustomAnchorTag from '../common/CustomAnchorTag';
import styles from './Settings.module.scss';

type Props = {
    settingItems: SettingItem[];
};
export default function Settings({ settingItems }: Props) {
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
