import { validateMemberPasswordRule } from './validators';

describe('ValidatorMemberPasswordRule', () => {
  test('password length less than 6', () => {
    // arrange
    const newPassword = '1234';
    // act
    const isValid = validateMemberPasswordRule(newPassword);
    // assert
    expect(isValid).toEqual(false);
  });

  test('password include white space', () => {
    // arrange
    const newPassword = '1234 aaAA';
    // act
    const isValid = validateMemberPasswordRule(newPassword);
    // assert
    expect(isValid).toEqual(false);
  });

  test('password is valid', () => {
    // arrange
    const newPassword = '1234aaAA';
    // act
    const isValid = validateMemberPasswordRule(newPassword);
    // assert
    expect(isValid).toEqual(true);
  });
});
