import path from 'path';
import { defineConfig } from 'vite';

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      '@': `${path.resolve(__dirname, 'src')}`,
    },
  },
  build: {
    // generate manifest.json in outDir
    manifest: true,
    rollupOptions: {
      // overwrite default .html entry
      input: {
        createAccount: 'src/pages/createAccount.tsx',
        ptTemplate: 'src/pages/ptTemplate.tsx',
        productSettingInfo: 'src/pages/productSettingInfo.tsx',
      },
    },
  },
});
