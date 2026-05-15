-- Supabase database schema for Danny Freethebikes personal site

create extension if not exists pgcrypto;

create table profile (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    name text not null,
    handle text,
    tagline text,
    short_bio text,
    hero_intro text,
    email text,
    github_link text,
    linkedin_link text,
    published boolean not null default true
);

create table site_settings (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    homepage_headline text,
    homepage_subheadline text,
    featured_project_count int not null default 4,
    show_featured_projects boolean not null default true,
    show_skills boolean not null default true,
    show_field_notes boolean not null default true,
    show_photos boolean not null default true,
    show_interests boolean not null default true,
    show_principles boolean not null default true,
    show_contact boolean not null default true,
    status_text text,
    mode_text text,
    signal_text text,
    published boolean not null default true
);

create table projects (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    title text not null,
    slug text,
    short_description text,
    long_description text,
    status text,
    tags text,
    featured boolean not null default false,
    published boolean not null default false,
    sort_order int not null default 0,
    project_url text,
    github_url text,
    cover_image text,
    gallery_images jsonb default '[]'::jsonb
);

create table project_images (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    project_id uuid references projects(id) on delete cascade,
    image_url text,
    caption text,
    alt_text text,
    sort_order int not null default 0,
    published boolean not null default false
);

create table photos (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    title text not null,
    caption text,
    alt_text text,
    category text,
    related_project_id uuid references projects(id) on delete set null,
    sort_order int not null default 0,
    published boolean not null default false,
    image_url text
);

create table skills (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    name text not null,
    category text,
    proficiency_label text,
    sort_order int not null default 0,
    published boolean not null default false
);

create table notes (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    title text not null,
    short_note text,
    body text,
    tags text,
    date date,
    sort_order int not null default 0,
    published boolean not null default false
);

create table links (
    id uuid primary key default gen_random_uuid(),
    created_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    label text not null,
    url text not null,
    sort_order int not null default 0,
    published boolean not null default false
);
