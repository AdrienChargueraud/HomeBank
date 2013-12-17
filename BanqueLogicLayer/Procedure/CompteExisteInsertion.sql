USE [MyAppV2];
GO

CREATE PROCEDURE [dbo].[PS_CompteExisteInsertion]
	@CompteNom nvarchar(255),
	@CompteNumero nvarchar(255),
	@Res bit output
AS
BEGIN
	set @Res=isnull((select 1 from Compte where compte_numero = @CompteNumero AND compte_nom = @CompteNom),0)
END
GO


