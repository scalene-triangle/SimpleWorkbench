import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

const apiTarget =
  process.env.services__api__https__0 ??
  process.env.services__api__http__0 ??
  process.env.SERVICES__API__HTTPS__0 ??
  process.env.SERVICES__API__HTTP__0;

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: apiTarget
      ? {
          "/api": {
            target: apiTarget,
            changeOrigin: true,
            secure: false
          }
        }
      : undefined
  }
});
