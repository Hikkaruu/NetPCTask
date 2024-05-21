@echo off
cd Back\NetPCTask
start cmd /k dotnet run

cd..\..\Front\netpctask
start http://localhost:5288/swagger/index.html
start /wait cmd /c "npm install && npm start"
