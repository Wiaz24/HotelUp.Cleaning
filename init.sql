DO $$
DECLARE
service RECORD;
BEGIN
FOR service IN
SELECT unnest(ARRAY['customer', 'cleaning', 'repair', 'kitchen', 'employee', 'payment', 'information']) AS name
    LOOP
    -- Create role
    EXECUTE format('CREATE ROLE %I_role;', service.name);

    -- Create schema with role as owner
    EXECUTE format('CREATE SCHEMA %I AUTHORIZATION %I_role;', service.name, service.name);
    
    -- Create user with password and grant role
    EXECUTE format('CREATE USER %I_user WITH PASSWORD ''%I_password'';', service.name, service.name);
    EXECUTE format('GRANT %I_role TO %I_user;', service.name, service.name);
    
    -- Grant usage and all privileges on schema to role
    EXECUTE format('GRANT USAGE ON SCHEMA %I TO %I_role;', service.name, service.name);
    EXECUTE format('GRANT ALL PRIVILEGES ON SCHEMA %I TO %I_role;', service.name, service.name);
    
    -- Grant all privileges on tables to role and set search path
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA %I GRANT ALL PRIVILEGES ON TABLES TO %I_role;', service.name, service.name);
    EXECUTE format('ALTER ROLE %I_role SET search_path TO %I', service.name, service.name);
    END LOOP;
END $$;

-- Revoke all privileges on public schema
REVOKE ALL ON SCHEMA public FROM PUBLIC;
-- ALTER SYSTEM SET track_commit_timestamp to "on"