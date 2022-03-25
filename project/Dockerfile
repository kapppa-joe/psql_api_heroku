
# build the app as production and output to /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /source
COPY . /source/
RUN dotnet publish -c release -o /app

# run the production app with another image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

# seems that $PORT has to be in CMD for it to get the $PORT env var at runtime :3
CMD ASPNETCORE_URLS=http://*:$PORT dotnet project.dll
