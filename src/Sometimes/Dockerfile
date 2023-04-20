FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet build -c Release -o /app/build

EXPOSE 5228

ENTRYPOINT ["dotnet", "run"]
