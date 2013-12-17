using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueHeavyClient.Global;
using BanqueLogicLayer.Modele;

namespace BanqueHeavyClient.ViewModel.Procuration
{
    public class ProcurationDetailViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }

        private BanqueLogicLayer.Modele.Procuration _elementSelectionne;
        public BanqueLogicLayer.Modele.Procuration ElementSelectionne
        {
            get { return _elementSelectionne; }
            set { _elementSelectionne = value; }
        }

        #endregion

        public ProcurationDetailViewModel(BanqueLogicLayer.Modele.Procuration element)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ElementSelectionne = element;
        }

        #region Binding

        public string UserName
        {
            get { return RefContexte.Utilisateur.Where(m => m.user_id == ElementSelectionne.utilisateur_user_id).SingleOrDefault().user_name; }
        }

        public string NumCompte
        {
            get { return ElementSelectionne.Compte.compte_numero; }
        }

        public string NomCompte
        {
            get { return ElementSelectionne.Compte.compte_nom; }
        }

        public string OrgaCompte
        {
            get { return ElementSelectionne.Compte.Organisme.organisme_nom ; }
        }

        public string DroitSurCompte
        {
            get { return ElementSelectionne.proc_droit.ToString(); }
        }

        #endregion
    }
}
