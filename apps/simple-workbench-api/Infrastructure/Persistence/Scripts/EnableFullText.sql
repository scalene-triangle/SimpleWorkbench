-- Enable full-text search index for lexical query path (SQL Server).
-- Run this script in environments where SQL Server full-text feature is installed.

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'SimpleWorkbenchFtCatalog')
BEGIN
    CREATE FULLTEXT CATALOG SimpleWorkbenchFtCatalog AS DEFAULT;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID(N'dbo.Note'))
BEGIN
    CREATE FULLTEXT INDEX ON dbo.Note
    (
        Title LANGUAGE 1033,
        SearchText LANGUAGE 1033
    )
    KEY INDEX PK_Note
    ON SimpleWorkbenchFtCatalog
    WITH CHANGE_TRACKING AUTO;
END
GO
