import { GeneralInfo } from '@/types/new-account';

import GeneralInfoValidator from './generalInfoValidator';

const generalInfo: GeneralInfo = {
  userName: 'S789Y6000F',
  password: '1234aaAA',
  uplineRoleId: 3,
  uplineUserName: 'S789Y600',
  newUserName: '0F',
  firstName: 'null',
  lastName: 'null',
  phone: 'null',
  mobilePhone: 'null',
  maxCredit: 0,
  memberMaxCredit: 0,
  fax: 'null',
  disableAutoMPT: false,
  group: 'a',
  transferOption: '00000000',
  isAutomateTransfer: true,
  site: 'y789',
  siteLabel: '789y',
  currency: 95,
  templateName: 'null',
  commissions: [],
  uplineMaxCredit: 10000,
  uplineMemberMaxCredit: 0,
};

jest.mock('../../../api/new-account.ts', () => ({
  ...jest.requireActual('../../../api/new-account.ts'),
  checkExistedCustomer: jest.fn().mockResolvedValue({ data: true }),
}));

describe('GeneralInfoValidator', () => {
  test('username is available', async () => {
    // act
    const validator = new GeneralInfoValidator(generalInfo);
    const result = await validator.validate('userName');
    // assert
    expect(result).toEqual({
      userName: 'usernameIsInvalid',
      password: '',
      maxCredit: '',
      memberMaxCredit: '',
    });
  });
});
