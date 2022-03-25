FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY . /app/
RUN dotnet dev-certs https

#run a build at this stage to detect issues earlier.
RUN dotnet build

ENV ASPNETCORE_URLS=https://*:5002
CMD dotnet watch run

#use this if ASPNETCORE_URLS was ignored
#CMD dotnet run --urls https://0.0.0.0:5002
