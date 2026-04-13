const { defineConfig } = require('cypress');

module.exports = defineConfig({
    e2e: {
        baseUrl: 'http://localhost:3000',
        supportFile: false,
        specPattern: 'e2e/**/*.spec.js',
        video: false,
        screenshotOnRunFailure: true,
    },
    component: {
        devServer: {
            framework: 'react',
            bundler: 'webpack',
        },
        specPattern: 'unit/**/*.test.js',
    },
    viewportWidth: 1280,
    viewportHeight: 720,
});