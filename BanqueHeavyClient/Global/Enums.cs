﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BanqueHeavyClient.Global
{
    public static class Enums
    {
        public enum TypeErreurMessage : int
        {
            ERREUR = 1,
            AVERTISSEMENT = 2,
            CONFIRMATION = 3,
            INFORMATION = 4
        };

        public enum TypeOperation
        {
            [Description("Débit")]
            DEBIT,
            
            [Description("Crédit")]
            CREDIT
        }

        public enum Mode : int
        {
            NEW = 0,
            UPDATE = 1
        }

        public enum Onglet : int
        {
            PROFIL = 0,
            COMPTE = 1,
            CATEGORIE = 2, 
            CATEGORIEPLAN = 3,
            ORGANISME = 4,
            PROCURATION = 5,
            UTILISATEUR = 6
        }
    }
}
