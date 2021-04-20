Remove-Item ./src/AccManagement/AccManagement.API/transactions.db -ErrorAction Ignore
dotnet run -c Release -p ./src/AccManagement/AccManagement.API/AccManagement.API.csproj