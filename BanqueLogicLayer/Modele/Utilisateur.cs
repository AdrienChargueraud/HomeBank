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
    
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            this.Comptes = new HashSet<Compte>();
            this.Categories = new HashSet<Categorie>();
        }
    
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
    
        public virtual ICollection<Compte> Comptes { get; set; }
        public virtual ICollection<Categorie> Categories { get; set; }
    }
}