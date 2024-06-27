import { TFunction } from 'i18next';

import { checkExistedCustomer } from '@/api/new-account';
import { UserLevel, UserLevelName } from '@/constants';
import { GeneralInfo } from '@/types/new-account';
import { formatThousandNumber } from '@/utils/number-format';

import { validateMemberPasswordRule, validatePasswordRule } from './validators';

export type GeneralInfoErrors = {
  userName: string;
  password: string;
  maxCredit: string;
  memberMaxCredit: string;
};

export default class GeneralInfoValidator {
  private generalInfo: GeneralInfo;
  private errors: GeneralInfoErrors = {
    userName: '',
    password: '',
    maxCredit: '',
    memberMaxCredit: '',
  };

  constructor(generalInfo: GeneralInfo) {
    this.generalInfo = generalInfo;
  }

  private setError(value: Partial<GeneralInfoErrors>) {
    this.errors = {
      ...this.errors,
      ...value,
    };

    return this;
  }

  private getError() {
    return this.errors;
  }

  private async validateUserName(field?: keyof GeneralInfoErrors) {
    if (field && field !== 'userName') return this;
    const isExisted = (await checkExistedCustomer(this.generalInfo.userName)).data;
    if (isExisted) return this.setError({ userName: 'usernameIsInvalid' });

    return this;
  }

  private validatePassword(field?: keyof GeneralInfoErrors) {
    if (field && field !== 'password') return this;
    const { password, uplineRoleId } = this.generalInfo;
    if (!password) return this.setError({ password: 'RequiredField' });
    if (uplineRoleId !== UserLevel.Agent && !validatePasswordRule(password))
      return this.setError({ password: 'PasswordInCorrectFormat' });
    if (uplineRoleId == UserLevel.Agent && !validateMemberPasswordRule(password))
      return this.setError({ password: 'PasswordInCorrectFormat' });

    return this;
  }

  private validateMaxCredit(field?: keyof GeneralInfoErrors) {
    if (field && field !== 'maxCredit') return this;
    const { maxCredit, uplineMaxCredit, uplineRoleId } = this.generalInfo;
    if (maxCredit === undefined) return this.setError({ maxCredit: 'RequiredField' });
    if (maxCredit < 0 || maxCredit > uplineMaxCredit)
      return this.setError({
        maxCredit: `${UserLevelName[uplineRoleId - 1].toLowerCase()}MaxCreditInvalid-0-${uplineMaxCredit}`,
      });

    return this;
  }

  private validateMemberMaxCredit(field?: keyof GeneralInfoErrors) {
    if ((field && field !== 'memberMaxCredit') || this.generalInfo.uplineRoleId !== UserLevel.Master) return this;
    const { memberMaxCredit, uplineMemberMaxCredit } = this.generalInfo;
    if (memberMaxCredit === undefined) return this.setError({ memberMaxCredit: 'RequiredField' });
    if (memberMaxCredit < 0 || memberMaxCredit > uplineMemberMaxCredit)
      return this.setError({
        memberMaxCredit: `${UserLevelName[UserLevel.Member].toLowerCase()}MaxCreditInvalid-0-${uplineMemberMaxCredit}`,
      });

    return this;
  }

  async validate(field?: keyof GeneralInfoErrors) {
    return (await this.validateUserName(field))
      .validatePassword(field)
      .validateMaxCredit(field)
      .validateMemberMaxCredit(field)
      .getError();
  }
}

function getCreditTranslate(t: TFunction, messageKey: string) {
  if (messageKey === '') return messageKey;
  if (messageKey === 'RequiredField') return t(messageKey);

  if (messageKey.includes('MaxCreditInvalid')) {
    const [key, min, max] = messageKey.split('-');
    if (key && min !== undefined && max !== undefined) {
      return t(key, { min: formatThousandNumber(min), max: formatThousandNumber(max) });
    }
  }

  return messageKey;
}

export async function validateGeneralInfo(generalInfo: GeneralInfo, t: TFunction, field?: keyof GeneralInfoErrors) {
  const validator = new GeneralInfoValidator(generalInfo);
  const result = await validator.validate(field);
  const { userName, password, maxCredit, memberMaxCredit } = result;

  const errors = {
    userName: userName ? t(userName) : '',
    password: password ? t(password) : '',
    maxCredit: getCreditTranslate(t, maxCredit),
    memberMaxCredit: getCreditTranslate(t, memberMaxCredit),
  };

  return errors;
}
