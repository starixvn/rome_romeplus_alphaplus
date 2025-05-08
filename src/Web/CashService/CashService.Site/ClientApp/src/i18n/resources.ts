// Import all translations
const locales = ['en', 'vi', 'zh-CN', 'zh-TW', 'ja', 'ko', 'th'] as const;
const modules = ['global', 
  'managePayments', 'memberSite', 'pendingDeposit', 
  'pendingWithdrawal', 'transactionHistory', 'promotion'] as const;

interface Resources {
  [key: string]: {
    [key: string]: any;
  };
}

const resources: Resources = {};

// Initialize resources asynchronously
const initResources = async () => {
  for (const locale of locales) {
    resources[locale] = {};
    for (const module of modules) {
      const moduleData = await import(`./locales/${module}/${locale}.json`);
      resources[locale][module] = moduleData.default;
    }
  }
};

// Start loading resources
initResources().catch(console.error);

export { resources }; 