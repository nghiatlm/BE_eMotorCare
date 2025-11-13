# Stage 1: Restore and build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and projects
COPY *.sln .
COPY BE_eMotoCare.API/BE_eMotoCare.API.csproj BE_eMotoCare.API/
COPY eMototCare.BLL/eMototCare.BLL.csproj eMototCare.BLL/
COPY eMotoCare.DAL/eMotoCare.DAL.csproj eMotoCare.DAL/
COPY eMotoCare.BO/eMotoCare.BO.csproj eMotoCare.BO/

RUN dotnet restore

# Copy source and build
COPY . .
RUN dotnet publish BE_eMotoCare.API/BE_eMotoCare.API.csproj -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/out .

# Expose the same ports
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "BE_eMotoCare.API.dll"]
