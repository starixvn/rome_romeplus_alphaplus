import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
  ],

  build: {
    assetsDir: '',
    rollupOptions: {
      input: {
        memberSites: 'src/pages/MemberSites/index.tsx',
        managePayments: 'src/pages/ManagePayments/index.tsx',
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
