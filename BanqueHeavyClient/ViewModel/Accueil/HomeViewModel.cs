using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using BanqueHeavyClient.ViewModel.Organisme;
using System.Windows;
using BanqueHeavyClient.ViewModel.Procuration;

namespace BanqueHeavyClient.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region Initialisation
        public CompteViewModel CompteViewModel { get; set; }  
        public CategorieViewModel CategorieViewModel { get; set; }
        public ProfilViewModel ProfilViewModel { get; set; }
        public OperationPlanningViewModel OperationPlanningViewModel { get; set; }
        public OrganismeViewModel OrganismeViewModel { get; set; }
        public ProcurationViewModel ProcurationViewModel { get; set; }

        public HomeViewModel() : base()
        {
            SessionUtilisateur.Instance.Plannificateur.UpdateProgrammation();
            CategorieViewModel = new CategorieViewModel();
            CompteViewModel = new CompteViewModel();
            ProfilViewModel = new ProfilViewModel();
            OperationPlanningViewModel = new OperationPlanningViewModel();
            OrganismeViewModel = new OrganismeViewModel();
            ProcurationViewModel = new ProcurationViewModel();
        }
        #endregion

        #region Binding
        public Visibility IsAdmin
        {
            get { return SessionUtilisateur.Instance.ConnectedUser.user_isAdmin ? Visibility.Visible : Visibility.Hidden; }
        }
        #endregion

        #region Refresh
        public void Refresh(int onglet)
        {
            if (onglet.Equals(Convert.ToInt32(Enums.Onglet.PROCURATION)))
            {
                ProcurationViewModel = new ProcurationViewModel();
                OnPropertyChanged("ProcurationViewModel");
            }
            if (onglet.Equals(Convert.ToInt32(Enums.Onglet.COMPTE)))
            {
                CompteViewModel = new CompteViewModel();
                OnPropertyChanged("CompteViewModel");
            }
            if (onglet.Equals(Convert.ToInt32(Enums.Onglet.CATEGORIE)))
            {
                CategorieViewModel = new CategorieViewModel();
                OnPropertyChanged("CategorieViewModel");
            }
            if (onglet.Equals(Convert.ToInt32(Enums.Onglet.CATEGORIEPLAN)))
            {
                OperationPlanningViewModel = new OperationPlanningViewModel();
                OnPropertyChanged("OperationPlanningViewModel");
            }
            if (onglet.Equals(Convert.ToInt32(Enums.Onglet.ORGANISME)))
            {
                OrganismeViewModel = new OrganismeViewModel();
                OnPropertyChanged("OrganismeViewModel");
            }
        }
        #endregion
    }
}
