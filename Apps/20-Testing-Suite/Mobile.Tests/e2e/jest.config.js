module.exports = {
  preset: 'ts-jest',
  testEnvironment: 'node',
  testMatch: ['**/*.e2e.js'],
  setupFilesAfterEnv: ['./setup.js'],
  verbose: true,
};