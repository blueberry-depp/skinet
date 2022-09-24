dotnet dev-certs https --trust
dotnet tool install --global dotnet-ef --version 6.0.5


dotnet watch run
dotnet restore
dotnet build

dotnet new classlib -o Core
dotnet sln add Core
dotnet add refrerence Core/


dotnet ef database drop -p Infrastructure -s API
dotnet ef migrations remove -p Infrastructure -s API
dotnet ef migrations add InitialCreate -p Infrastructure/ -s API/ -o Data/Migrations

dotnet ef database update



ng g m shared --flat

install AutoMapper.Extensions.Microsoft.DependencyInjection

json to ts website

git add .
git commit -m "End of section 4"
git push -u origin master

docker-compose down --rmi all



