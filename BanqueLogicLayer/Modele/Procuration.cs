//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanqueLogicLayer.Modele
{
    using System;
    using System.Collections.Generic;
    
    public partial class Procuration
    {
        public int compte_compte_id { get; set; }
        public int utilisateur_user_id { get; set; }
        public int proc_droit { get; set; }
    
        public virtual Compte Compte { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
    }
}