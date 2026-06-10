import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

const apiTarget = process.env.API_URL || 'http://localhost:5005';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/Solicitors': {
        target: apiTarget,
        changeOrigin: true,
        secure: false
      }
    }
  }
});
