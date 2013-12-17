﻿using System;
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
            CompteViewModel = new CompteViewModel(CategorieViewModel);
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
    }
}
