@echo off

SET root=%cd%

cd "%cd%\src"

dotnet build

cd %root%

echo Press any button..
pause > nul