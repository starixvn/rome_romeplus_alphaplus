/// <reference types="vitest" />
import { defineConfig } from 'vite';
import fs from 'fs';
import path from 'path';
import tailwindcss from '@tailwindcss/vite';
import react from '@vitejs/plugin-react';

const copyWithoutHash = () => {
  return {
    name: 'copy-without-hash',
    writeBundle() {
      const distDir = 'dist';
      const files = fs.readdirSync(distDir);
      
      files.forEach(file => {
        if (file.includes('updateInfoCashMember') && file.includes('.js')) {
          const sourcePath = path.join(distDir, file);
          const targetPath = path.join(distDir, 'updateInfoCashMember.js');
          fs.copyFileSync(sourcePath, targetPath);
        }
      });
    }
  };
};
// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss(), copyWithoutHash()],

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
    assetsDir: '',
    cssCodeSplit: false,
    rollupOptions: {
      input: {
        memberSites: 'src/pages/MemberSites/index.tsx',
        managePayments: 'src/pages/ManagePayments/index.tsx',
        pendingDeposit: 'src/pages/Transactions/PendingDeposit/index.tsx',
        pendingWithdrawal: 'src/pages/Transactions/PendingWithdrawal/index.tsx',
        transactionsHistory: 'src/pages/Transactions/History/index.tsx',
        paymentProviderSettings: 'src/pages/PaymentProviderSettings/index.tsx',
        updateInfoCashMember: 'src/pages/CustomerSites/pages/UpdateInfoCashMember.tsx',
        pendingPromotion: 'src/pages/Promotion/PendingPromotion/index.tsx',
      },
      output: {
        assetFileNames: 'index-[hash][extname]',
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
});
