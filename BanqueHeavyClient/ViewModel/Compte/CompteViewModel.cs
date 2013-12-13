using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Windows;

namespace BanqueHeavyClient.ViewModel
{
    public class CompteViewModel : BaseViewModel
    {
        #region Propriétés
        private ObservableCollection<DetailCompteViewModel> _comptes = new ObservableCollection<DetailCompteViewModel>();
        public ObservableCollection<DetailCompteViewModel> Comptes
        {
            get { return _comptes; }
            set
            {
                _comptes = value;
                OnPropertyChanged("Comptes");
            }
        }

        private DetailCompteViewModel _selectionCompte;
        public DetailCompteViewModel SelectionCompte
        {
            get { return _selectionCompte; }
            set
            {
                _selectionCompte = value;
                OnPropertyChanged("SelectionCompte");
                OnPropertyChanged("IsBoutonActif");
            }
        }

        public CategorieViewModel CategorieViewModel { get; set; }

        private BanqueContainer RefContexte { get; set; }

        #endregion

        #region New and wen

        public CompteViewModel(CategorieViewModel categorieVm)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            CategorieViewModel = categorieVm;
            UpdateComptesFromDatabase();
            if(Comptes.Count > 0)
                SelectionCompte = Comptes.ElementAt(0);
        }

        #endregion

        #region Binding

        public Visibility IsBoutonActif
        {
            get
            {
                return (SelectionCompte != null ? Visibility.Visible : Visibility.Hidden);
            }
        }

        #endregion

        #region Navigation

        private void ShowWindowAccount(int mode)
        {
            View.CompteUpdate view = new View.CompteUpdate();
            view.DataContext = new CompteUpdateViewModel(mode, this);
            view.ShowDialog();
        }

        #endregion

        #region Déclencheurs

        private RelaiCommande _addAccount;
        public ICommand AddAccount
        {
            get
            {
                if (_addAccount == null)
                    _addAccount = new RelaiCommande(() => this.ShowWindowAccount(Convert.ToInt32(Enums.Mode.NEW)));
                return _addAccount;
            }
        }

        private RelaiCommande _modifyAccount;
        public ICommand ModifyAccount
        {
            get
            {
                if (_modifyAccount == null)
                    _modifyAccount = new RelaiCommande(() => this.ShowWindowAccount(Convert.ToInt32(Enums.Mode.UPDATE)));
                return _modifyAccount;
            }
        }

        private RelaiCommande _deleteAccount;
        public ICommand DeleteAccount
        {
            get
            {
                if (_deleteAccount == null)
                    _deleteAccount = new RelaiCommande(() => this.DeleteFinalAccount());
                return _deleteAccount;
            }
        }

        #endregion

        #region Métier

        private void DeleteFinalAccount()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            try
            {
                var operationSupp = RefContexte.Operation.Where(m => m.compte_compte_id == SelectionCompte.SelectedCompte.compte_id).ToList();
                foreach (Operation op in operationSupp)
                    RefContexte.Operation.Remove(op);
                RefContexte.Compte.Remove(SelectionCompte.SelectedCompte);
                RefContexte.SaveChanges();
                transaction.Commit();
                UpdateComptesFromDatabase();
            }
            catch(Exception)
            {
                transaction.Rollback();
                new Erreur("Impossible de supprimer le compte!", "Compte", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }

            transaction.Dispose();
            if (Comptes.Count > 0)
                SelectionCompte = Comptes.ElementAt(0);
        }

        public void UpdateComptesFromDatabase()
        {
            Comptes = new ObservableCollection<DetailCompteViewModel>();
            foreach (Compte _cpt in RefContexte.Compte.Where(m => m.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList())
                Comptes.Add(new DetailCompteViewModel(_cpt, CategorieViewModel));
        }

        #endregion   
    }
}
