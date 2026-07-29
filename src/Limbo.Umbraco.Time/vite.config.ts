import { defineConfig } from 'vite';

// [CHANGE: upgrade to Umbraco 17] Related: package.json, Limbo.Umbraco.Time.csproj
// Builds the TypeScript/Lit backoffice client into wwwroot. @umbraco-cms/* imports are kept
// external (resolved at runtime by the backoffice import map). emptyOutDir is disabled so the
// committed wwwroot/umbraco-package.json manifest is preserved across builds - the "prebuild"
// script in package.json deletes the previously emitted *.js / *.js.map instead, so stale hashed
// chunks from an earlier build are not left behind (and shipped) in the NuGet package.
export default defineConfig({
  base: '/App_Plugins/Limbo.Umbraco.Time/',
  build: {
    lib: {
      entry: 'src/index.ts',
      formats: ['es'],
      fileName: 'limbo-time',
    },
    outDir: 'wwwroot',
    emptyOutDir: false,
    sourcemap: true,
    rollupOptions: {
      external: [/^@umbraco/],
    },
  },
});
