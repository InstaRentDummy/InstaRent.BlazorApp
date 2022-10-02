#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InstaRent.BlazorApp/InstaRent.BlazorApp.csproj", "InstaRent.BlazorApp/"]
RUN dotnet restore "InstaRent.BlazorApp/InstaRent.BlazorApp.csproj"
COPY . .
WORKDIR "/src/InstaRent.BlazorApp"
RUN dotnet build "InstaRent.BlazorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstaRent.BlazorApp.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["InstaRent.BlazorApp/nginx.conf", "/etc/nginx/nginx.conf"]
COPY ["etc/dev-cert/localhost.crt", "/etc/nginx/localhost.crt"]  
COPY ["etc/dev-cert/localhost.rsa", "/etc/nginx/localhost.rsa"]  