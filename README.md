# Ali Hussain Portfolio

Full-stack portfolio and learning-resource website built with React, ASP.NET Core, SQL Server, and ASP.NET Identity.

## Local development

### Backend

```powershell
cd backend-api
dotnet run --project ".\AliHussainPortfolio.Api\AliHussainPortfolio.Api.csproj" --urls http://localhost:5055
```

The API applies EF Core migrations on startup and uses the `AliHussainPortfolioDb` SQL Server database from `appsettings.json`.

### Frontend

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173`. The Vite proxy forwards `/api` requests to `http://localhost:5055`.

## Admin

Open `http://localhost:5173/admin`.

The development seed account is:

- Email: `admin@alihussain.dev`
- Password: `AliHussain@2025!`

Change these values before deployment.

The admin area supports course creation, PDF uploads, dashboard counts, and contact-message review/deletion.

## Production configuration

Set these environment variables on the API host:

```text
ConnectionStrings__DefaultConnection=<hosted SQL Server connection string>
JwtSettings__SecretKey=<long random secret>
JwtSettings__Issuer=<your API issuer>
JwtSettings__Audience=<your frontend audience>
Admin__Email=<admin email>
Admin__Password=<strong admin password>
ASPNETCORE_ENVIRONMENT=Production
```

Configure the frontend `/api` proxy or deploy the API and frontend behind the same HTTPS domain. Use HTTPS, restrict CORS to the deployed frontend origin, and use persistent object storage for uploaded PDFs instead of local disk for multi-instance hosting.

## Free deployment preparation

The repository includes [`render.yaml`](./render.yaml), [`backend-api/Dockerfile`](./backend-api/Dockerfile), and a Cloudflare Pages SPA fallback at [`frontend/public/_redirects`](./frontend/public/_redirects).

### Render API

1. Create a Render Web Service from this repository and use the included Blueprint, or configure the Dockerfile path as `./backend-api/Dockerfile`.
2. Add the environment variables listed in `render.yaml`.
3. Use your hosted SQL Server connection string for `ConnectionStrings__DefaultConnection`.
4. After deployment, verify `https://YOUR-API.onrender.com/health`.

The API applies EF Core migrations during startup. The hosted SQL Server user must have permission to create/update the application schema.

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
