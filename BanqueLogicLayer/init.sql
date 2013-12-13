
SET QUOTED_IDENTIFIER OFF;
GO
USE [MyAppV2];
GO

-- --------------------------------------------------
-- Insertion d'utilisateur de test
-- --------------------------------------------------

insert into Utilisateur([user_name], [user_password]) values('Adrien', 'hDCxMgqBa2AUFBNQx/BR8fWHwck=')
insert into Utilisateur([user_name], [user_password]) values('Toto', 'Toto')

-- --------------------------------------------------
-- Insertion d'une liste d'organisme
-- --------------------------------------------------

insert into Organisme([organisme_nom], [organisme_abrev], [organisme_actif]) values('Crédit Agricole', 'CA', 1)
insert into Organisme([organisme_nom], [organisme_abrev], [organisme_actif]) values('Caisse d''Epargne', 'CE', 1)
insert into Organisme([organisme_nom], [organisme_abrev], [organisme_actif]) values('Crédit Mutuel', 'CMMC', 0)

-- --------------------------------------------------
-- Insertion d'une liste de catégorie
-- --------------------------------------------------

insert into Categorie([categorie_nom],[categorie_actif],[utilisateur_user_id]) values('Salaire', 1, 1)
insert into Categorie([categorie_nom],[categorie_actif],[utilisateur_user_id]) values('Essence', 1, 1)
insert into Categorie([categorie_nom],[categorie_actif],[utilisateur_user_id]) values('Supermarché', 1, 1)
insert into Categorie([categorie_nom],[categorie_actif],[utilisateur_user_id]) values('Déménagement', 0, 1)
insert into Categorie([categorie_nom],[categorie_actif],[utilisateur_user_id]) values('Informatique', 1, 2)

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------