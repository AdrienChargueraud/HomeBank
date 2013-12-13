using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel.Utilisateur
{
    public class UtilisateurViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }

        #endregion

        #region New and wen

        public UtilisateurViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
        }

        #endregion
    }
}
