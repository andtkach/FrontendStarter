echo Build UI
start /B /WAIT cmd /c "npm run build"

echo Build API
start /B /WAIT cmd /c "dotnet build --configuration Release FrontendStarter.sln"

echo DONE