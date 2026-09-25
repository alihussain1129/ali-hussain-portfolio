# Ali Hussain Portfolio

Full-stack portfolio and learning-resource website built with React, ASP.NET Core, PostgreSQL, and ASP.NET Identity.

## Local development

### Backend

```powershell
cd backend-api
dotnet run --project ".\AliHussainPortfolio.Api\AliHussainPortfolio.Api.csproj" --urls http://localhost:5055
```

The API applies EF Core migrations on startup and uses the `AliHussainPortfolioDb` PostgreSQL database from `appsettings.json`.

### Frontend

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173`. The Vite proxy forwards `/api` requests to `http://localhost:5055`.

## Admin

Open `http://localhost:5173/admin`.

The development seed account is configured by the API defaults for local use. Set your own `Admin__Email` and `Admin__Password` values before deployment; do not publish production credentials.

The admin area supports course creation, PDF uploads, dashboard counts, and contact-message review/deletion.

## Production configuration

Set these environment variables on the API host:

```text
ConnectionStrings__DefaultConnection=Host=<host>;Port=5432;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
JwtSettings__SecretKey=<long random secret>
JwtSettings__Issuer=<your API issuer>
JwtSettings__Audience=<your frontend audience>
Admin__Email=<admin email>
Admin__Password=<strong admin password>
ASPNETCORE_ENVIRONMENT=Production
```

Configure the frontend `/api` proxy or deploy the API and frontend behind the same HTTPS domain. Use HTTPS, restrict CORS to the deployed frontend origin, and use persistent object storage for uploaded PDFs instead of local disk for multi-instance hosting.

## Azure API + Vercel frontend

The recommended deployment uses Neon for PostgreSQL, Azure App Service for the ASP.NET Core API, and Vercel for the Vite frontend.

### Azure App Service API

Create a Linux Web App in the `ali-hussain-portfolio-rg` resource group. Deploy the repository with the Dockerfile at `backend-api/Dockerfile`, using the repository root as the Docker build context. The image listens on port `10000`.

Configure these App Service application settings:

```text
WEBSITES_PORT=10000
ASPNETCORE_URLS=http://0.0.0.0:10000
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=<host>;Port=5432;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
JwtSettings__SecretKey=<long random secret>
JwtSettings__Issuer=https://<your-app-service>.azurewebsites.net
JwtSettings__Audience=https://<your-vercel-app>.vercel.app
Admin__Email=<admin email>
Admin__Password=<strong admin password>
Cors__Origins__0=https://<your-vercel-app>.vercel.app
```

Use the .NET/Npgsql connection string from Neon. Keep all secrets in App Service configuration; never commit them to the repository. After deployment, verify `https://<your-app-service>.azurewebsites.net/health`.

### Vercel frontend

1. Import this repository into Vercel.
2. Set the root directory to `frontend`.
3. Use `npm run build` as the build command and `dist` as the output directory.
4. Add `VITE_API_URL=https://<your-app-service>.azurewebsites.net`.
5. After the Vercel URL is known, set `Cors__Origins__0` in App Service to that exact HTTPS origin and restart the API.

## Alternative deployment preparation

The repository includes [`render.yaml`](./render.yaml), [`backend-api/Dockerfile`](./backend-api/Dockerfile), and a Cloudflare Pages SPA fallback at [`frontend/public/_redirects`](./frontend/public/_redirects).

### Render API

1. Create a Render Web Service from this repository and use the included Blueprint, or configure the Dockerfile path as `./backend-api/Dockerfile`.
2. Add the environment variables listed in `render.yaml`.
3. Use your hosted PostgreSQL connection string for `ConnectionStrings__DefaultConnection` (for example, `Host=...;Port=5432;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true`).
4. After deployment, verify `https://YOUR-API.onrender.com/health`.

The API applies EF Core migrations during startup. The hosted PostgreSQL user must have permission to create/update the application schema.

### Cloudflare Pages

1. Create a Pages project from this repository.
2. Set the root directory to `frontend`.
3. Set the build command to `npm run build`.
4. Set the output directory to `dist`.
5. Add `VITE_API_URL=https://YOUR-API.onrender.com` as a production environment variable.
6. Add the final Cloudflare Pages URL as `Cors__Origins__0` in Render and redeploy the API.

Never commit connection strings, JWT secrets, admin passwords, or provider API keys. Local development continues to use the Vite proxy when `VITE_API_URL` is not set.

## Validation

```powershell
cd frontend
npm run lint
npm run build

cd ..\backend-api
dotnet build
```
