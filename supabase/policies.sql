-- Enable Row Level Security for all tables and create policies.
-- Replace <ADMIN_USER_ID> with your Supabase authenticated admin user id.

alter table profile enable row level security;
create policy "public read profile" on profile for select using (published = true);
create policy "admin manage profile" on profile for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table site_settings enable row level security;
create policy "public read settings" on site_settings for select using (published = true);
create policy "admin manage settings" on site_settings for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table projects enable row level security;
create policy "public read projects" on projects for select using (published = true);
create policy "admin manage projects" on projects for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table project_images enable row level security;
create policy "public read project_images" on project_images for select using (published = true);
create policy "admin manage project_images" on project_images for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table photos enable row level security;
create policy "public read photos" on photos for select using (published = true);
create policy "admin manage photos" on photos for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table skills enable row level security;
create policy "public read skills" on skills for select using (published = true);
create policy "admin manage skills" on skills for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table notes enable row level security;
create policy "public read notes" on notes for select using (published = true);
create policy "admin manage notes" on notes for all using (auth.uid() = '<ADMIN_USER_ID>');

alter table links enable row level security;
create policy "public read links" on links for select using (published = true);
create policy "admin manage links" on links for all using (auth.uid() = '<ADMIN_USER_ID>');
