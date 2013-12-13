using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel.Organisme
{
    public class DetailOrganismeViewModel :  BaseViewModel
    {
        #region propriétés

        public BanqueLogicLayer.Modele.Organisme OrganismeSelection { get; set; }
        private BanqueContainer RefContexte { get; set; }

        #endregion

        #region Binding

        public string NomOrganisme
        {
            get { return OrganismeSelection.organisme_nom; }
        }

        public string Actif
        {
            get { return (OrganismeSelection.organisme_actif ? "Oui" : "Non"); }
        }

        public string Abreviation
        {
            get { return OrganismeSelection.organisme_abrev; }
        }

        #endregion

        #region New and Wen

        public DetailOrganismeViewModel(BanqueLogicLayer.Modele.Organisme organisme)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            OrganismeSelection = organisme;
        }

        #endregion
    }
}
