/// <reference types="vitest" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'


// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
  ],

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
        'src/services/interfaces/**',
      ],
      include: [
        'src/**/*.{ts,tsx}'    // Only include TypeScript and TSX files in src
      ]
    },
  },

  build: {
    assetsDir: '',
    rollupOptions: {
      input: {
        memberSites: 'src/pages/MemberSites/index.tsx',
        managePayments: 'src/pages/ManagePayments/index.tsx',
        pendingDeposit: 'src/pages/Transactions/PendingDeposit/index.tsx',
        transactionsHistory: 'src/pages/Transactions/History/index.tsx',
        paymentProviderSettings: 'src/pages/PaymentProviderSettings/index.tsx'
      }
    }
  },
 
  server: {
    port: 5173,
    strictPort: true,
    proxy: {
      '/api': {
        target: 'http://localhost:5100',
        changeOrigin: true
      }
    }
  }
})
