import SettingItem from './setting-item';

export default interface StatusItem extends SettingItem {
    Disabled: boolean;
    Checked: boolean;
    ShowInCustomerList: boolean;
    ConfirmMessage: string;
    IsEditOnProductLevel: boolean;
    DisplayOrder: string;
    IsPopup: boolean;
    JsonParams: string;
    PopupHeaderText: string;
    ReloadCustomerInfo: boolean;
}
