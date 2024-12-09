import { TFunction } from 'i18next';

import { usernameRegExp } from '../helpers';
import { UnsuspendMultipleModel } from '../models/UnsuspenDownlines/UnsuspendMultipleViewModel';

export const viewInformation = (t: TFunction) => {
    const style = `style='padding-left: 15px;'`;
    const messages = `<div>
            <div> ${t('ViewInformation1')}</div>
            <div> ${t('ViewInformation2')}</div>
            <div ${style}> ${t('ViewInformation3')}</div>
            <div ${style}> ${t('ViewInformation4')}</div>
            <div> ${t('ViewInformation5')}</div>
            <div> ${t('ViewInformation6')}</div>
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
        return t('MultipleConfirm');
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

    message += t('SubmitMultipleConfirm3');

    return message;
};
