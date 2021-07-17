@echo off

SET root=%cd%

cd "%cd%\src\bin\Debug\net5.0"

call server

cd %root%