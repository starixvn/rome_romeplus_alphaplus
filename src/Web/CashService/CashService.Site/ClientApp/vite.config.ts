/// <reference types="vitest" />
import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite';
import react from '@vitejs/plugin-react';

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const useHash = mode !== 'fixed';
  
  return {
    plugins: [react(), tailwindcss()],
    test: {
      globals: true,
      environment: 'jsdom',
      ui: false,
      setupFiles: './setuptest.ts',
      coverage: {
        provider: 'v8',
        reporter: ['text', 'json'],
        exclude: [
          '**/*.test.{ts,tsx}',
          'src/i18n/**',
          'src/services/interfaces/**',
          'src/constants/**',
        ],
        include: [
          'src/**/*.{ts,tsx}', // Only include TypeScript and TSX files in src
        ],
      },
    },

    build: {
      outDir: useHash ? 'dist' : 'dist/fixed',
      assetsDir: '',
      cssCodeSplit: useHash ? false : true,
      rollupOptions: {
        input: {
          memberSites: 'src/pages/MemberSites/index.tsx',
          managePayments: 'src/pages/ManagePayments/index.tsx',
          pendingDeposit: 'src/pages/Transactions/PendingDeposit/index.tsx',
          pendingWithdrawal: 'src/pages/Transactions/PendingWithdrawal/index.tsx',
          transactionsHistory: 'src/pages/Transactions/History/index.tsx',
          paymentProviderSettings: 'src/pages/PaymentProviderSettings/index.tsx',
          managePromotion: 'src/pages/Promotion/ManagePromotion/index.tsx',
          pendingPromotion: 'src/pages/Promotion/PendingPromotion/index.tsx',
          promotionReport: 'src/pages/Promotion/PromotionReport/index.tsx',
          topBanner: 'src/pages/Advertisement/TopBanner.tsx',
          mainBanner: 'src/pages/Advertisement/MainBanner.tsx',
        },
        output: {
          assetFileNames: useHash ? 'index-[hash][extname]' : '[name][extname]',
          entryFileNames: useHash ? '[name]-[hash].js' : '[name].js',
          chunkFileNames: useHash ? '[name]-[hash].js' : '[name].js',
        },
      },
    },

    server: {
      port: 5173,
      strictPort: true,
      proxy: {
        '/api': {
          target: 'http://localhost:5100',
          changeOrigin: true,
        },
      },
    },
  };
});
