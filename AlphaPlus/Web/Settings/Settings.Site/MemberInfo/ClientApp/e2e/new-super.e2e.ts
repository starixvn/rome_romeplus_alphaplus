import { test } from '@playwright/test';

test('CreateNewSuper', async ({ page }) => {
  await page.goto('http://localhost:8081/ex-main/_Authorization/SignIn.aspx?ReturnUrl=%2fsite-main%2f');
  await page.locator('#txtUserName').fill('s789y1Name');
  await page.locator('#txtUserName').press('Tab');
  await page.locator('#txtPassWord').fill('1234ggGG');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Get Started' }).click();
  await page.frameLocator('iframe[name="main"]').getByLabel('Close').click();
  await page.frameLocator('iframe[name="main"]').locator('#password').click();
  await page.frameLocator('iframe[name="main"]').locator('#password').fill('1234aaAA');
  await page.frameLocator('iframe[name="main"]').getByRole('button', { name: 'Create Account' }).first().click();
  await page.frameLocator('iframe[name="main"]').getByRole('button', { name: 'Ok' }).click();
});
