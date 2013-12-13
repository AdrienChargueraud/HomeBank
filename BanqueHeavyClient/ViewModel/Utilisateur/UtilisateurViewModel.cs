using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Collections.ObjectModel;
using System.Windows;

namespace BanqueHeavyClient.ViewModel.Utilisateur
{
    public class UtilisateurViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        public ObservableCollection<DetailUtilisateurViewModel> UserReferentiel { get; set; }

        private DetailUtilisateurViewModel _userSelect;
        public DetailUtilisateurViewModel UserSelect
        {
            get { return _userSelect; }
            set { _userSelect = value; OnPropertyChanged("IsBoutonActif"); }
        }

        #endregion

        #region New and wen

        public UtilisateurViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ExtractAllUsers();
        }

        #endregion

        #region Binding
        public Visibility IsBoutonActif
        {
            get
            {
                return ((UserSelect != null) ? Visibility.Visible : Visibility.Hidden);
            }
        }
        #endregion

        #region Metier
        public void ExtractAllUsers()
        {
            UserReferentiel = new ObservableCollection<DetailUtilisateurViewModel>();

            OnPropertyChanged("UserReferentiel");
        }

        private void ShowWindowOperation(int mode)
        {
            //View.OrganismeUpdate view = new View.OrganismeUpdate();
            //view.DataContext = new UpdateOrganismeViewModel(this, mode);
            //view.ShowDialog();
        }

        private void DeleteUser()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            try
            {
               
                RefContexte.SaveChanges();
                transaction.Commit();

                ExtractAllUsers();
            }
            catch (Exception)
            {
                transaction.Rollback();
                new Erreur("Impossible de supprimer l'organisme sélectionné!", "Organisme", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }
            finally
            {
                transaction.Dispose();
            }

            if (UserReferentiel.Count > 0)
                UserSelect = UserReferentiel.ElementAt(0);
        }
        #endregion

    }
}
