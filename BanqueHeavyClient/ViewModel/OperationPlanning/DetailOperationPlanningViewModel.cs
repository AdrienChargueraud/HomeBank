using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel.OperationPlanning
{
    public class DetailOperationPlanningViewModel : BaseViewModel
    {
        #region propriétés

        public BanqueLogicLayer.Modele.OperationPlanning OperationSelectionPlan { get; set; }
        private BanqueContainer RefContexte { get; set; }

        #endregion

        #region Binding

        public string Compte
        {
            get
            {
                return OperationSelectionPlan.Compte.compte_nom;
            }
        }

        public string MontantOperation
        {
            get
            {
                return String.Format("{0:0.00}€", OperationSelectionPlan.opeplan_montant);
            }
        }

        public string TypeOperation
        {
            get
            {
                if (OperationSelectionPlan.opeplan_montant >= 0)
                    return Outil.GetEnumDescription(Enums.TypeOperation.CREDIT);
                return Outil.GetEnumDescription(Enums.TypeOperation.DEBIT);
            }
        }

        public string TypeCategorie
        {
            get
            {
                return (OperationSelectionPlan.Categorie != null ? OperationSelectionPlan.Categorie.categorie_nom : "");
            }
        }

        public string DateNextOperation
        {
            get
            {
                return String.Format("{0}", OperationSelectionPlan.opeplan_dateOperation.ToShortDateString());
            }
        }

        public string Frequence
        {
            get
            {
                return String.Format("{0} jour(s)", OperationSelectionPlan.opeplan_frequence);
            }
        }

        #endregion

        #region New and Wen

        public DetailOperationPlanningViewModel(BanqueLogicLayer.Modele.OperationPlanning operation)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            OperationSelectionPlan = operation;
        }

        #endregion
    }
}
