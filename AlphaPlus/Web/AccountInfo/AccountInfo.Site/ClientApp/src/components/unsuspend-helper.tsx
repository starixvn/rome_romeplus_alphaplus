import { TFunction } from 'i18next';

import { usernameRegExp } from '../helpers';
import { UnsuspendMultipleModel } from '../models/UnsuspenDownlines/UnsuspendMultipleViewModel';

export const viewInformation = (t: TFunction) => {
    const style = `style='padding-left: 15px;'`;
    const messages = `<div>
            <div> ${t('UnsuspendedNote:ViewInformation1')}</div>
            <div> ${t('UnsuspendedNote:ViewInformation2')}</div>
            <div ${style}> ${t('UnsuspendedNote:ViewInformation3')}</div>
            <div ${style}> ${t('UnsuspendedNote:ViewInformation4')}</div>
            <div> ${t('UnsuspendedNote:ViewInformation5')}</div>
            <div> ${t('UnsuspendedNote:ViewInformation6')}</div>
        </div>`;

    window.top?.bootboxAlert(messages);
};

export const getMultipleConfirmMessage = (
    t: TFunction,
    searchAccountRoleId: number,
    viewModel: UnsuspendMultipleModel[],
    originalViewModel: UnsuspendMultipleModel[]
) => {
    if (searchAccountRoleId > 1) {
        return t('UnsuspendedNote:MultipleConfirm');
    }

    const unsuspendUsernames: string[] = [],
        suspendUsernames: string[] = [];

    viewModel.map(function (item, index) {
        if (item.Suspended !== originalViewModel[index].Suspended) {
            if (item.Suspended) {
                suspendUsernames.push(item.Username);
            } else {
                unsuspendUsernames.push(item.Username);
            }
        }
    });

    let message =
        unsuspendUsernames.length > 0
            ? usernameRegExp(t('UnsuspendMultipleConfirmMessage'), unsuspendUsernames.join(', ')) + '<br />'
            : '';

    message +=
        suspendUsernames.length > 0
            ? usernameRegExp(t('SuspendMultipleConfirmMessage'), suspendUsernames.join(', ')) + '<br />'
            : '';

    message += t('UnsuspendedNote:SubmitMultipleConfirm3');

    return message;
};
