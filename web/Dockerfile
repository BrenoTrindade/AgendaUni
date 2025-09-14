FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
EXPOSE 80
EXPOSE 5000
WORKDIR /src

COPY AgendaUni.Web.csproj .
RUN dotnet restore AgendaUni.Web.csproj

COPY . .
RUN dotnet build AgendaUni.Web.csproj -c Release -o /app/build

FROM build as publish
RUN dotnet publish AgendaUni.Web.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf