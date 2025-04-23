import SettingItem from './setting-item';
import StatusItem from './status-item';

export default interface CustomerStatus {
    Status: StatusItem[];
    Settings: SettingItem[];
}
