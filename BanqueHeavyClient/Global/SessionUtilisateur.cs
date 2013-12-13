using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using System.Data.Entity;
using System.Data.EntityModel;

namespace BanqueHeavyClient.Global
{
    public sealed class SessionUtilisateur
    {
        #region propriétés

        private static volatile SessionUtilisateur instance;
        private static object syncRoot = new Object();
        public BanqueContainer banqueContexte { get; set; }
        public Utilisateur ConnectedUser { get; set; }
        public Service.Plannification Plannificateur { get; set; }

        #endregion

        #region définition du singleton

        private SessionUtilisateur()
        {
            banqueContexte = new BanqueContainer();
            Plannificateur = new Service.Plannification();
        }

        public static SessionUtilisateur Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot) 
                    {
                        if (instance == null) 
                            instance = new SessionUtilisateur();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region métier

        public void LancerLaPlannification()
        {
            Plannificateur.UpdateProgrammation();
        }

        #endregion
    }
}
