# Ali Hussain Portfolio

Full-stack portfolio and learning-resource website built with React, ASP.NET Core, PostgreSQL, and ASP.NET Identity.

## Local development

### Backend

```powershell
cd backend-api
dotnet run --project ".\AliHussainPortfolio.Api\AliHussainPortfolio.Api.csproj" --urls http://localhost:5055
```

The API uses PostgreSQL from `appsettings.json`. Start a local PostgreSQL instance first, or override `ConnectionStrings__DefaultConnection` with a development database connection string.

### Frontend

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173`. The Vite proxy forwards `/api` requests to `http://localhost:5055`.

## Admin

Open `http://localhost:5173/admin`.

The development seed account is configured by the API defaults for local use. Set your own `Admin__Email` and `Admin__Password` values before deployment; production startup requires them and does not use the development defaults.

The admin area supports course creation, PDF uploads, dashboard counts, and contact-message review/deletion.

## Deployment: Azure App Service API + Vercel website

The current deployment target is the existing Azure for Students subscription for the API, Vercel for the React website, and Neon PostgreSQL for data.

### Azure subscription policy and cost

The subscription policy currently allows these Azure regions: Central India, Poland Central, Malaysia West, UAE North, and Austria East. The Azure portal's Web App wizard accepted a Linux, Code, .NET 10 LTS, Free F1 configuration in UAE North at an estimated $0/month. Select an allowed region; do not use the wizard's default region unless it is on the current policy list.

The Free F1 tier has 1 GB memory and a 60 CPU-minute daily quota, has no SLA, and Microsoft says it is intended for trials, experimentation, and learning rather than production workloads. In the same UAE North wizard, Basic B1 was estimated at $16.06/month. Check the portal's current estimate and your student credit balance before selecting any paid plan.

### Create the API Web App

1. In Azure Portal, open **App Services > Create > Web App**.
2. Select the `Azure for Students` subscription and the existing `ali-hussain-portfolio-rg` resource group.
3. Set **Publish** to **Code**, **Runtime stack** to **.NET 10 (LTS)**, **Operating System** to **Linux**, **Region** to **UAE North**, and **Pricing plan** to **Free F1**. Do not select Container: the GitHub workflow publishes the compiled .NET app.
4. Review the estimated price and configuration. Do not click **Create** if Azure shows a region-policy warning or any unexpected paid SKU.
5. After creation, open **Settings > Environment variables** (or **Configuration > Application settings**) and add the settings below. Use actual Neon values and strong, unique secrets; never commit them to Git.

Required API application settings:

```text
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ConnectionStrings__DefaultConnection=<Neon pooled Npgsql connection string>
ConnectionStrings__MigrationConnection=<Neon direct, non-pooled Npgsql connection string>
JwtSettings__SecretKey=<random secret, at least 32 bytes>
JwtSettings__Issuer=<API issuer, e.g. https://YOUR-API-HOST.azurewebsites.net>
JwtSettings__Audience=<frontend HTTPS origin>
Admin__Email=<your admin email>
Admin__Password=<a strong, unique admin password>
Cors__Origins__0=<frontend HTTPS origin>
```

Use the .NET/Npgsql connection string format from Neon (`Host=...;Port=5432;Database=...;Username=...;Password=...;SSL Mode=VerifyFull;Channel Binding=Require`). Use Neon's pooled URL for `DefaultConnection` and the direct URL without `-pooler` for `MigrationConnection`. The API applies EF Core migrations at startup using the direct connection. Never paste either URL or any secret into chat or GitHub files.

6. Set `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true` so HTTPS forwarding works behind App Service's proxy. Add a temporary HTTPS `Cors__Origins__0` value only if needed to start before the Vercel URL is known; replace it with the exact Vercel origin before testing browser API calls.
7. For the workflow below, download the app's publish profile. If Azure reports publishing authentication is disabled, enable **SCM Basic Auth Publishing Credentials** under the app's configuration, download the profile, store it only as the GitHub Actions secret described below, and do not share the XML file.

### Configure API deployment from GitHub

The workflow at [`.github/workflows/deploy-azure-api.yml`](./.github/workflows/deploy-azure-api.yml) builds the .NET 10 API and deploys it when backend files change on `main`, or when run manually.

In the GitHub repository:

1. Add repository variable `AZURE_WEBAPP_NAME` with the exact Web App name.
2. Add repository secret `AZURE_WEBAPP_PUBLISH_PROFILE` containing the downloaded publish profile XML.
3. Commit and push the workflow. Open **Actions** and verify **Deploy API to Azure App Service** succeeds.
4. Check `https://<actual-app-host>/health`. It should return HTTP 200 with `{"status":"ok"}` only when the database is reachable. Production startup fails visibly if required configuration is missing or migrations fail.

### Deploy the website to Vercel

1. Import the GitHub repository into Vercel.
2. Set **Root Directory** to `frontend`, **Build Command** to `npm run build`, and **Output Directory** to `dist`.
3. Set the production environment variable `VITE_API_URL` to the exact API origin, for example `https://<actual-app-host>`.
4. Deploy. The Vercel rewrite in [`frontend/vercel.json`](./frontend/vercel.json) keeps React Router routes working on refresh.
5. Copy the final Vercel HTTPS origin (no path or trailing slash) into both `Cors__Origins__0` and `JwtSettings__Audience` in the Azure app settings. Save to restart the API.
6. Verify the home page, projects, courses, contact form, admin login, and dashboard on the deployed domain.

### Deployment caveats

- The API currently stores uploaded PDFs on the app's local filesystem. Do not rely on production PDF uploads until durable object storage is configured.
- F1 is a free, quota-limited tier and is not a production-supported SLA plan. Upgrade only after checking the exact regional price and student credit remaining.
- The Azure wizard may show a randomized secure hostname. Use the actual hostname shown after creation for the issuer, frontend `VITE_API_URL`, and health check.
- Never put Neon connection strings, JWT secrets, admin passwords, or publish-profile XML in source files or commit them to GitHub.

## Validation

```powershell
cd frontend
npm run lint
npm run build

cd ..\backend-api
dotnet build
```
