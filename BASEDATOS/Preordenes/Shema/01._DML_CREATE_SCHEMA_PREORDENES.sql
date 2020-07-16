IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'PREORDENES')
BEGIN
    -- The schema must be run in its own batch!
    EXEC( 'CREATE SCHEMA PREORDENES' );
END
