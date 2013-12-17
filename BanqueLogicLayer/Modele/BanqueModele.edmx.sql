
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/16/2013 11:55:18
-- Generated from EDMX file: C:\Users\achargueraud\Documents\GitHub\HomeBank\BanqueLogicLayer\Modele\BanqueModele.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyAppV2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CatégorieOperation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Operation] DROP CONSTRAINT [FK_CatégorieOperation];
GO
IF OBJECT_ID(N'[dbo].[FK_CategorieOperationPlanning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationPlanning] DROP CONSTRAINT [FK_CategorieOperationPlanning];
GO
IF OBJECT_ID(N'[dbo].[FK_CompteOperation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Operation] DROP CONSTRAINT [FK_CompteOperation];
GO
IF OBJECT_ID(N'[dbo].[FK_CompteOperationPlanning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OperationPlanning] DROP CONSTRAINT [FK_CompteOperationPlanning];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganismeCompte]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Compte] DROP CONSTRAINT [FK_OrganismeCompte];
GO
IF OBJECT_ID(N'[dbo].[FK_UtilisateurCategorie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categorie] DROP CONSTRAINT [FK_UtilisateurCategorie];
GO
IF OBJECT_ID(N'[dbo].[FK_UtilisateurCompte]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Compte] DROP CONSTRAINT [FK_UtilisateurCompte];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categorie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorie];
GO
IF OBJECT_ID(N'[dbo].[Compte]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Compte];
GO
IF OBJECT_ID(N'[dbo].[Operation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Operation];
GO
IF OBJECT_ID(N'[dbo].[OperationPlanning]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OperationPlanning];
GO
IF OBJECT_ID(N'[dbo].[Organisme]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organisme];
GO
IF OBJECT_ID(N'[dbo].[Utilisateur]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utilisateur];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Organisme'
CREATE TABLE [dbo].[Organisme] (
    [organisme_id] int IDENTITY(1,1) NOT NULL,
    [organisme_nom] nvarchar(max)  NOT NULL,
    [organisme_actif] bit  NOT NULL,
    [organisme_abrev] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Compte'
CREATE TABLE [dbo].[Compte] (
    [compte_id] int IDENTITY(1,1) NOT NULL,
    [compte_numero] nvarchar(max)  NOT NULL,
    [organisme_organisme_id] int  NOT NULL,
    [utilisateur_user_id] int  NOT NULL,
    [compte_interet] float  NOT NULL,
    [compte_dateOuverture] datetime  NOT NULL,
    [compte_soldeDepart] float  NOT NULL,
    [compte_soldeCourant] float  NOT NULL,
    [compte_nom] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Operation'
CREATE TABLE [dbo].[Operation] (
    [operation_id] int IDENTITY(1,1) NOT NULL,
    [operation_montant] float  NOT NULL,
    [operation_date] datetime  NOT NULL,
    [compte_compte_id] int  NOT NULL,
    [categorie_categorie_id] int  NOT NULL
);
GO

-- Creating table 'Categorie'
CREATE TABLE [dbo].[Categorie] (
    [categorie_id] int IDENTITY(1,1) NOT NULL,
    [categorie_nom] nvarchar(max)  NOT NULL,
    [categorie_actif] bit  NOT NULL,
    [utilisateur_user_id] int  NOT NULL
);
GO

-- Creating table 'Utilisateur'
CREATE TABLE [dbo].[Utilisateur] (
    [user_id] int IDENTITY(1,1) NOT NULL,
    [user_name] nvarchar(max)  NOT NULL,
    [user_password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'OperationPlanning'
CREATE TABLE [dbo].[OperationPlanning] (
    [opeplan_id] int IDENTITY(1,1) NOT NULL,
    [opeplan_frequence] int  NOT NULL,
    [opeplan_dateOperation] datetime  NOT NULL,
    [opeplan_montant] float  NOT NULL,
    [compte_compte_id] int  NOT NULL,
    [categorie_categorie_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [organisme_id] in table 'Organisme'
ALTER TABLE [dbo].[Organisme]
ADD CONSTRAINT [PK_Organisme]
    PRIMARY KEY CLUSTERED ([organisme_id] ASC);
GO

-- Creating primary key on [compte_id] in table 'Compte'
ALTER TABLE [dbo].[Compte]
ADD CONSTRAINT [PK_Compte]
    PRIMARY KEY CLUSTERED ([compte_id] ASC);
GO

-- Creating primary key on [operation_id] in table 'Operation'
ALTER TABLE [dbo].[Operation]
ADD CONSTRAINT [PK_Operation]
    PRIMARY KEY CLUSTERED ([operation_id] ASC);
GO

-- Creating primary key on [categorie_id] in table 'Categorie'
ALTER TABLE [dbo].[Categorie]
ADD CONSTRAINT [PK_Categorie]
    PRIMARY KEY CLUSTERED ([categorie_id] ASC);
GO

-- Creating primary key on [user_id] in table 'Utilisateur'
ALTER TABLE [dbo].[Utilisateur]
ADD CONSTRAINT [PK_Utilisateur]
    PRIMARY KEY CLUSTERED ([user_id] ASC);
GO

-- Creating primary key on [opeplan_id] in table 'OperationPlanning'
ALTER TABLE [dbo].[OperationPlanning]
ADD CONSTRAINT [PK_OperationPlanning]
    PRIMARY KEY CLUSTERED ([opeplan_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [organisme_organisme_id] in table 'Compte'
ALTER TABLE [dbo].[Compte]
ADD CONSTRAINT [FK_OrganismeCompte]
    FOREIGN KEY ([organisme_organisme_id])
    REFERENCES [dbo].[Organisme]
        ([organisme_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganismeCompte'
CREATE INDEX [IX_FK_OrganismeCompte]
ON [dbo].[Compte]
    ([organisme_organisme_id]);
GO

-- Creating foreign key on [compte_compte_id] in table 'Operation'
ALTER TABLE [dbo].[Operation]
ADD CONSTRAINT [FK_CompteOperation]
    FOREIGN KEY ([compte_compte_id])
    REFERENCES [dbo].[Compte]
        ([compte_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompteOperation'
CREATE INDEX [IX_FK_CompteOperation]
ON [dbo].[Operation]
    ([compte_compte_id]);
GO

-- Creating foreign key on [categorie_categorie_id] in table 'Operation'
ALTER TABLE [dbo].[Operation]
ADD CONSTRAINT [FK_CatégorieOperation]
    FOREIGN KEY ([categorie_categorie_id])
    REFERENCES [dbo].[Categorie]
        ([categorie_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CatégorieOperation'
CREATE INDEX [IX_FK_CatégorieOperation]
ON [dbo].[Operation]
    ([categorie_categorie_id]);
GO

-- Creating foreign key on [utilisateur_user_id] in table 'Compte'
ALTER TABLE [dbo].[Compte]
ADD CONSTRAINT [FK_UtilisateurCompte]
    FOREIGN KEY ([utilisateur_user_id])
    REFERENCES [dbo].[Utilisateur]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UtilisateurCompte'
CREATE INDEX [IX_FK_UtilisateurCompte]
ON [dbo].[Compte]
    ([utilisateur_user_id]);
GO

-- Creating foreign key on [compte_compte_id] in table 'OperationPlanning'
ALTER TABLE [dbo].[OperationPlanning]
ADD CONSTRAINT [FK_CompteOperationPlanning]
    FOREIGN KEY ([compte_compte_id])
    REFERENCES [dbo].[Compte]
        ([compte_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompteOperationPlanning'
CREATE INDEX [IX_FK_CompteOperationPlanning]
ON [dbo].[OperationPlanning]
    ([compte_compte_id]);
GO

-- Creating foreign key on [categorie_categorie_id] in table 'OperationPlanning'
ALTER TABLE [dbo].[OperationPlanning]
ADD CONSTRAINT [FK_CategorieOperationPlanning]
    FOREIGN KEY ([categorie_categorie_id])
    REFERENCES [dbo].[Categorie]
        ([categorie_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategorieOperationPlanning'
CREATE INDEX [IX_FK_CategorieOperationPlanning]
ON [dbo].[OperationPlanning]
    ([categorie_categorie_id]);
GO

-- Creating foreign key on [utilisateur_user_id] in table 'Categorie'
ALTER TABLE [dbo].[Categorie]
ADD CONSTRAINT [FK_UtilisateurCategorie]
    FOREIGN KEY ([utilisateur_user_id])
    REFERENCES [dbo].[Utilisateur]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UtilisateurCategorie'
CREATE INDEX [IX_FK_UtilisateurCategorie]
ON [dbo].[Categorie]
    ([utilisateur_user_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------