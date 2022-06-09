dotnet watch run
dotnet restore
dotnet build

dotnet new classlib -o Core
dotnet sln add Core
dotnet add refrerence Core/


dotnet ef database drop -p Infrastructure -s API
dotnet ef migrations remove -p Infrastructure -s API
dotnet ef migrations add InitialCreate -p Infrastructure/ -s API/ -o Data/Migrations

