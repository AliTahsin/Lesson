#!/bin/bash
echo "Running Backend Tests..."
dotnet test Backend.Tests/Backend.Tests.csproj

echo "Running Frontend Tests..."
cd Frontend.Tests && npm test && npm run cypress:run

echo "Running Mobile Tests..."
cd ../Mobile.Tests && npm test && detox test

echo "Running Load Tests..."
k6 run Load.Tests/load-test.js

echo "Running Security Tests..."
node Security.Tests/zap-scan.js

echo "All tests completed!"