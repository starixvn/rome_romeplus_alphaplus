import { TFunction } from 'i18next';

import { usernameRegExp } from '../../helpers';
import { OpenMultipleModel } from './open-api';

export const viewInformation = (t: TFunction) => {
    const style = `style='padding-left: 15px;'`;
    const messages = `<div>
            <div> ${t('OpenDownlines.ViewInformation1')}</div>
            <div> ${t('OpenDownlines.ViewInformation2')}</div>
            <div ${style}> ${t('OpenDownlines.ViewInformation3')}</div>
            <div ${style}> ${t('OpenDownlines.ViewInformation4')}</div>
            <div> ${t('OpenDownlines.ViewInformation5')}</div>
            <div> ${t('OpenDownlines.ViewInformation6')}</div>
        </div>`;

    window.top?.bootboxAlert(messages);
};

export const getMultipleConfirmMessage = (
    t: TFunction,
    searchAccountRoleId: number,
    viewModel: OpenMultipleModel[],
    originalViewModel: OpenMultipleModel[]
) => {
    if (searchAccountRoleId > 1) {
        return t('OpenDownlines.MultipleConfirm');
    }

    const openingUsernames: string[] = [],
        closingUsernames: string[] = [];

    viewModel.forEach((item, index: number) => {
        if (item.IsClosed !== originalViewModel[index].IsClosed) {
            if (item.IsClosed) {
                closingUsernames.push(item.Username);
            } else {
                openingUsernames.push(item.Username);
            }
        }
    });

    let message =
        openingUsernames.length > 0
            ? usernameRegExp(t('OpenMultipleMemberConfirmMessage'), openingUsernames.join(', ')) + '<br />'
            : '';

    message +=
        closingUsernames.length > 0
            ? usernameRegExp(t('CloseMultipleMemberConfirmMessage'), closingUsernames.join(', ')) + '<br />'
            : '';

    message += t('OpenDownlines.SubmitMultipleConfirm3');

    return message;
};
