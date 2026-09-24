USE PortfolioDB;
GO

IF OBJECT_ID(N'dbo.Projects', N'U') IS NULL
CREATE TABLE dbo.Projects (
    Id            INT IDENTITY(1,1) CONSTRAINT PK_Projects PRIMARY KEY,
    Title         NVARCHAR(150)  NOT NULL,
    Slug          NVARCHAR(160)  NOT NULL CONSTRAINT UQ_Projects_Slug UNIQUE,
    Summary       NVARCHAR(500)  NOT NULL,
    Description   NVARCHAR(MAX)  NULL,
    TechStack     NVARCHAR(300)  NULL,
    ImageUrl      NVARCHAR(500)  NULL,
    GithubUrl     NVARCHAR(500)  NULL,
    LiveDemoUrl   NVARCHAR(500)  NULL,
    CaseStudyUrl  NVARCHAR(500)  NULL,
    IsFeatured    BIT            NOT NULL CONSTRAINT DF_Projects_IsFeatured DEFAULT 0,
    IsPublished   BIT            NOT NULL CONSTRAINT DF_Projects_IsPublished DEFAULT 1,
    SortOrder     INT            NOT NULL CONSTRAINT DF_Projects_SortOrder DEFAULT 0,
    CreatedAt     DATETIME2(0)   NOT NULL CONSTRAINT DF_Projects_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt     DATETIME2(0)   NOT NULL CONSTRAINT DF_Projects_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF OBJECT_ID(N'dbo.AdminUsers', N'U') IS NULL
CREATE TABLE dbo.AdminUsers (
    Id            INT IDENTITY(1,1) CONSTRAINT PK_AdminUsers PRIMARY KEY,
    Username      NVARCHAR(50)  NOT NULL CONSTRAINT UQ_AdminUsers_Username UNIQUE,
    PasswordHash  NVARCHAR(100) NOT NULL,
    CreatedAt     DATETIME2(0)  NOT NULL CONSTRAINT DF_AdminUsers_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Projects_Public')
CREATE INDEX IX_Projects_Public
    ON dbo.Projects (IsPublished, IsFeatured DESC, SortOrder)
    INCLUDE (Title, Slug, Summary);
GO