dotnet dev-certs https --trust
dotnet tool install --global dotnet-ef --version 6.0.5


dotnet watch run
dotnet restore
dotnet build

dotnet new classlib -o Core
dotnet sln add Core
dotnet add refrerence Core/


dotnet ef database drop -p Infrastructure -s API -c StoreContext 
dotnet ef migrations remove -p Infrastructure -s API
dotnet ef migrations add InitialCreate -p Infrastructure/ -s API/ -c StoreContext -o Data/Migrations

dotnet ef database update



ng g m shared --flat

install AutoMapper.Extensions.Microsoft.DependencyInjection

json to ts website

git add .
git commit -m "End of section 4"
git push -u origin master

docker-compose down --rmi all

git rm dump.rdb

// Migrations become slightly more complicated now because we have two separate contexts. We're going to have two databases, one for store and one for identity,
// we need to specify the infrastructure projects, because that's where our DB context is located. We still need to specify the starter projects, 
which is the API, and we also need to specify the context as well. And we can use a switch called -c. 
dotnet ef migrations add IdentityInitial -p Infrastructure/ -s API/ -c AppIdentityDbContext -o Identity/Migrations

ng g m account
ng g m account-routing --flat
ng g s account --flat --skip-tests
ng g c login --skip-tests
ng g c register --skip-tests
ng g g auth --skip-tests

-p: project
-s: startup project
-c: context
dotnet ef migrations add OrderEntityAdded -p Infrastructure/ -s API/ -c StoreContext 

