import fixReactVirtualized from 'esbuild-plugin-react-virtualized';
import { defineConfig } from 'vite';

import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    optimizeDeps: {
        esbuildOptions: {
            plugins: [fixReactVirtualized]
        }
    },
    plugins: [react()],
    build: {
        rollupOptions: {
            input: {
                main: '/src/main.tsx',
                givenCredit: '/src/given-credit.tsx',
                profilePicture: '/src/profile-picture.tsx'
            },
            output: {
                entryFileNames: '[name].js',
                chunkFileNames: '[name].js',
                assetFileNames: '[name].[ext]'
            }
        }
    }
});
