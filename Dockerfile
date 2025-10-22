# Stage 1: Restore and build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore
COPY *.sln .
COPY BE_eMotoCare.API/BE_eMotoCare.API.csproj BE_eMotoCare.API/
COPY eMototCare.BLL/eMototCare.BLL.csproj eMototCare.BLL/
COPY eMotoCare.DAL/eMotoCare.DAL.csproj eMotoCare.DAL/
COPY eMotoCare.BO/eMotoCare.BO.csproj eMotoCare.BO/

RUN dotnet restore

# Copy the rest of the source
COPY . .

# Build the WebAPI project
RUN dotnet publish BE_eMotoCare.API/BE_eMotoCare.API.csproj -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Cài các package cho Chromium headless
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    ca-certificates \
    fonts-liberation \
    libasound2 \
    libatk1.0-0 \
    libc6 \
    libcairo2 \
    libcups2 \
    libdbus-1-3 \
    libexpat1 \
    libfontconfig1 \
    libgcc1 \
    libgconf-2-4 \
    libgdk-pixbuf2.0-0 \
    libglib2.0-0 \
    libgtk-3-0 \
    libnspr4 \
    libnss3 \
    libpango-1.0-0 \
    libpangocairo-1.0-0 \
    libstdc++6 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxi6 \
    libxrandr2 \
    libxrender1 \
    libxss1 \
    libxtst6 \
    **libgbm1** \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out .

ENV ASPNETCORE_URLS="http://+:5154;https://+:7314"

# Expose HTTP & HTTPS
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "BE_eMotoCare.API.dll"]
