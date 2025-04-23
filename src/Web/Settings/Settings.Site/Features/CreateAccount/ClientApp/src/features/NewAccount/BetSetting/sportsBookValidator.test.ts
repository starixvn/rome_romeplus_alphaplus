import SportsBookValidator from '@/features/NewAccount/BetSetting/sportsBookValidator';
import { SportsBookBetSettingItem } from '@/types/new-account';

const item: SportsBookBetSettingItem = {
  $type: 2,
  minBet: 10,
  maxBet: 100,
  maxPerMatch: 1000,
  maxPayoutPerMatch: 10000,
  upline: 'null',
  sportTypeId: 1,
  sportTypeName: 'Soccer',
  betTypeId: 1,
  betTypeName: 'Handicap',
  displayName: 'Soccer',
  subGroupId: 0,
  subGroupName: 'null',
  isLive: false,
};

describe('SportsBookValidator', () => {
  test('unlimited bet setting', () => {
    // arrange
    const newBetSetting = { ...item, minBet: 0, maxBet: 0, maxPerMatch: 0, maxPayoutPerMatch: 0 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('minBet');
    // assert
    expect(error).toEqual('');
  });

  test('unlimited bet setting special i7 sport type', () => {
    // arrange
    const is789y = true;
    const config = { ...item, sportTypeId: 43, betTypeId: 1 };
    const newBetSetting = { ...item, minBet: 0, maxBet: 0, maxPerMatch: 0, maxPayoutPerMatch: 0 };
    // act
    const error = new SportsBookValidator(is789y, config, newBetSetting).validate('minBet');
    // assert
    expect(error).toEqual('minBet-mustBeGreaterThanOrEqualTo-10');
  });

  test('prevent bet setting', () => {
    // arrange
    const newBetSetting = { ...item, minBet: 0, maxBet: 0.1, maxPerMatch: 0.1, maxPayoutPerMatch: 0.1 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('minBet');
    // assert
    expect(error).toEqual('');
  });

  test("min bet less than upline's min bet", () => {
    // arrange
    const newBetSetting = { ...item, minBet: 9 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('minBet');
    // assert
    expect(error).toEqual('minBet-mustBeGreaterThanOrEqualTo-10');
  });

  test('min bet greater than max bet', () => {
    // arrange
    const newBetSetting = { ...item, minBet: 20, maxBet: 19 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('minBet');
    // assert
    expect(error).toEqual('minBet-mustBeLessThanOrEqualTo-maxBet');
  });

  test("max bet greater than upline's max bet", () => {
    // arrange
    const newBetSetting = { ...item, maxBet: 101 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxBet');
    // assert
    expect(error).toEqual('maxBet-mustBeLessThanOrEqualTo-100');
  });

  test('max bet greater than max per match', () => {
    // arrange
    const newBetSetting = { ...item, maxBet: 90, maxPerMatch: 89, minBet: 91 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxBet');
    // assert
    expect(error).toEqual('maxBet-mustBeLessThanOrEqualTo-maxPerMatch');
  });

  test('max bet less than min bet', () => {
    // arrange
    const newBetSetting = { ...item, minBet: 91, maxBet: 90 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxBet');
    // assert
    expect(error).toEqual('maxBet-mustBeGreaterThanOrEqualTo-minBet');
  });

  test("max per match greater than upline's max per match", () => {
    // arrange
    const newBetSetting = { ...item, maxPerMatch: 1000.01 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxPerMatch');
    // assert
    expect(error).toEqual('maxPerMatch-mustBeLessThanOrEqualTo-1000');
  });

  test('max per match greater than max payout per match', () => {
    // arrange
    const newBetSetting = { ...item, maxPerMatch: 999.09, maxPayoutPerMatch: 999.08 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxPerMatch');
    // assert
    expect(error).toEqual('maxPerMatch-mustBeLessThanOrEqualTo-maxPayoutPerMatch');
  });

  test('max per match less than max bet', () => {
    // arrange
    const newBetSetting = { ...item, maxBet: 99, maxPerMatch: 89.99 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxPerMatch');
    // assert
    expect(error).toEqual('maxPerMatch-mustBeGreaterThanOrEqualTo-maxBet');
  });

  test("max payout per match greater than upline's max payout per match", () => {
    // arrange
    const newBetSetting = { ...item, maxPayoutPerMatch: 10000.01 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxPayoutPerMatch');
    // assert
    expect(error).toEqual('maxPayoutPerMatch-mustBeLessThanOrEqualTo-10000');
  });

  test('max payout per match less than max per match', () => {
    // arrange
    const newBetSetting = { ...item, maxPerMatch: 1000, maxPayoutPerMatch: 999 };
    // act
    const error = new SportsBookValidator(false, item, newBetSetting).validate('maxPayoutPerMatch');
    // assert
    expect(error).toEqual('maxPayoutPerMatch-mustBeGreaterThanOrEqualTo-maxPerMatch');
  });
});
