using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Reflection;

namespace BanqueHeavyClient.Global
{
    public static class Outil
    {
        public static double GetSoldeCourant(Compte compte)
        {
            var _op = compte.Operations.Where(m => m.operation_date < System.DateTime.Now).Sum(m => m.operation_montant);
            compte.compte_soldeCourant = compte.compte_soldeDepart + _op;
            return compte.compte_soldeDepart + _op;
        }

        public static string EncodeMotDePasse(string mdp)
        {
            try
            {
                byte[] bytValue, bytHash;
                bytValue = System.Text.Encoding.UTF8.GetBytes(mdp);
                HashAlgorithm unHash = new SHA1CryptoServiceProvider();
                bytHash = unHash.ComputeHash(bytValue);
                unHash.Clear();
                string motDePasseCrypte = Convert.ToBase64String(bytHash);
                return motDePasseCrypte;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
