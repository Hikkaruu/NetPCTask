@echo off
cd Back\NetPCTask
start cmd /k dotnet run

cd..\..\Front\netpctask
start cmd /k npm start
start http://localhost:5288/swagger/index.html
