# Danny Freethebikes — Personal Lab Notebook

A standalone Blazor WebAssembly app designed as a browser-first personal workshop site with a private admin dashboard.

## Local development

1. Install .NET SDK 8.x.
2. From the project root:
   ```bash
   cd /home/freethebikes/Documents/Development/PersonalSite/dannyFreethebikes
   dotnet restore
   dotnet build
   dotnet run
   ```
3. Open the app at `http://localhost:5000`.

## Supabase setup

1. Create a new Supabase project.
2. Open `supabase/schema.sql` and run the SQL to create tables.
3. Open `supabase/policies.sql` and replace `<ADMIN_USER_ID>` with your authenticated Supabase user id.
4. Enable Row Level Security for all tables, as defined in `policies.sql`.
5. Create a public storage bucket named `portfolio-assets`.
6. Configure bucket policies so the app can write objects with the anon key and public read access.

## Configuration

Update `wwwroot/appsettings.json` or create a local copy named `wwwroot/appsettings.local.json` with your Supabase values:

```json
{
  "Supabase": {
    "Url": "https://<your-project>.supabase.co",
    "AnonKey": "<YOUR_ANON_KEY>"
  }
}
```

> The app only uses the anon key in the browser. Do not store the Supabase service role key in the client.

## Admin user setup

1. Create a Supabase user in Authentication.
2. Copy the user's `uid` from the Supabase dashboard.
3. Replace `<ADMIN_USER_ID>` in `supabase/policies.sql`.
4. Use the same credentials on `/admin/login`.

## App structure

- `Pages/Index.razor` — public homepage
- `Pages/Admin.razor` — private admin dashboard
- `Pages/Login.razor` — login page
- `Components/` — UI components and admin editors
- `Services/` — Supabase client, auth, and content services
- `Models/` — typed data models
- `supabase/` — SQL schema and RLS policies
- `wwwroot/` — static assets and app configuration

## GitHub Pages deployment

This repository includes a workflow at `.github/workflows/deploy.yml` that builds the Blazor WebAssembly app and publishes `publish/wwwroot` to GitHub Pages.

To enable GitHub Pages:

1. Push to the `main` branch.
2. In the repository settings, enable GitHub Pages from the `gh-pages` branch.
3. The action deploys the static site automatically on every push.

## Render notes

This app is built as a static Blazor WebAssembly app for GitHub Pages.

If you later choose Render for a hosted ASP.NET Core version, you can deploy the same project with a small ASP.NET Core host around it, but the current architecture is intentionally static and browser-only.
