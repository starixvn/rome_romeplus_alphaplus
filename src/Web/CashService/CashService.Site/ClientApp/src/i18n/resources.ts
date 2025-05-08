// Import all translations
const locales = ['en', 'vi', 'zh-CN', 'zh-TW', 'ja', 'ko', 'th'] as const;
const modules = ['global', 
  'managePayments', 'memberSite', 'pendingDeposit', 
  'pendingWithdrawal', 'transactionHistory', 'promotion'] as const;

type Module = typeof modules[number];

interface Resources {
  [key: string]: {
    [key: string]: any;
  };
}

const resources: Resources = locales.reduce((acc, locale) => {
  acc[locale] = modules.reduce((moduleAcc, module) => {
    moduleAcc[module] = require(`./locales/${module}/${locale}.json`);
    return moduleAcc;
  }, {} as Record<Module, any>);
  return acc;
}, {} as Resources);

export { resources }; 