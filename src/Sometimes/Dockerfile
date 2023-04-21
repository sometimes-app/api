FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /app/build

EXPOSE 5000

ENTRYPOINT ["dotnet", "/app/build/Sometimes.dll"]
