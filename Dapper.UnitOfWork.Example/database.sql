-- Addresses
CREATE TABLE [dbo].[Addresses] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Street] NVARCHAR (50) NULL,
    [Region] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- People
CREATE TABLE [dbo].[People] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NULL,
    [Age]        NVARCHAR (50) NULL,
    [Address_Id] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [FK_People_Addresses] FOREIGN KEY ([Address_Id]) REFERENCES [dbo].[Addresses] ([Id])
);