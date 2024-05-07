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
        manifest: true,
        rollupOptions: {
            input: '/src/main.tsx'
        }
    }
});
