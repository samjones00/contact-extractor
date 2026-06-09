import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/Contacts': {
        target: 'http://localhost:5005',
        changeOrigin: true,
        secure: false
      },
      '/Settings/locations': {
        target: 'http://localhost:5005',
        changeOrigin: true,
        secure: false
      }
    }
  }
});
