# Supabase Setup

This folder contains the schema and RLS policies for the Danny Freethebikes personal site.

## Steps

1. Create a new Supabase project.
2. Run `supabase/schema.sql` in the SQL editor to create tables.
3. Replace `<ADMIN_USER_ID>` in `supabase/policies.sql` with the `uid` of your admin user.
4. Run `supabase/policies.sql` to enable row-level security and create access policies.
5. Create a public storage bucket called `portfolio-assets`.
6. Configure the storage bucket so objects are publicly readable.
7. Add your Supabase URL and anon key to `wwwroot/appsettings.json` or `wwwroot/appsettings.local.json`.

## Notes

- All public queries only work when `published = true`.
- Admin insert/update/delete policies are enforced through Supabase RLS.
- The Blazor app is a static WebAssembly client, so the Supabase anon key is the only key used in the browser.
