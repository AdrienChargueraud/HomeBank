using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BanqueHeavyClient.ViewModel.Organisme
{
    public class OrganismeViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        public ObservableCollection<DetailOrganismeViewModel> OrganismeReferentiel { get; set; }

        private DetailOrganismeViewModel _organismeSelect;
        public DetailOrganismeViewModel OrganismeSelect
        {
            get { return _organismeSelect; }
            set { _organismeSelect = value; OnPropertyChanged("IsBoutonActif"); }
        }

        #endregion

        #region New and wen

        public OrganismeViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ExtractAllOrganisme();
        }

        #endregion

        #region Binding
        public Visibility IsBoutonActif
        {
            get
            {
                return ((OrganismeSelect != null) ? Visibility.Visible : Visibility.Hidden);
            }
        }
        #endregion

        #region Metier

        public void ExtractAllOrganisme()
        {
            OrganismeReferentiel = new ObservableCollection<DetailOrganismeViewModel>();
            foreach (var org in RefContexte.Organisme.Where(m => m.Utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList())
                OrganismeReferentiel.Add(new DetailOrganismeViewModel(org));
            OnPropertyChanged("OrganismeReferentiel");
        }

        private void ShowWindowOperation(int mode)
        {
            View.OrganismeUpdate view = new View.OrganismeUpdate();
            view.DataContext = new UpdateOrganismeViewModel(this, mode);
            view.ShowDialog();
        }

        private void DeleteOrganisme()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            try
            {
                if (RefContexte.Compte.Where(m => m.organisme_organisme_id == OrganismeSelect.OrganismeSelection.organisme_id).ToList().Count > 0)
                {
                    new Erreur("Impossible de supprimer un organisme qui est référencé dans un ou plusieurs comptes", "Organisme", Enums.TypeErreurMessage.AVERTISSEMENT).DisplayErreur();
                    transaction.Rollback();
                    return;
                }
                RefContexte.Organisme.Remove(OrganismeSelect.OrganismeSelection);
                RefContexte.SaveChanges();
                transaction.Commit();

                ExtractAllOrganisme();
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

            if (OrganismeReferentiel.Count > 0)
                OrganismeSelect = OrganismeReferentiel.ElementAt(0);
        }

        #endregion

        #region Declencheurs

        private RelaiCommande _addOrganisme;
        public ICommand AddOrganisme
        {
            get
            {
                if (_addOrganisme == null)
                    _addOrganisme = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.NEW)));
                return _addOrganisme;
            }
        }

        private RelaiCommande _deleteOrganisme;
        public ICommand DeleteOrganismeComm
        {
            get
            {
                if (_deleteOrganisme == null)
                    _deleteOrganisme = new RelaiCommande(() => this.DeleteOrganisme());
                return _deleteOrganisme;
            }
        }

        private RelaiCommande _modifyOrganisme;
        public ICommand ModifyOrganisme
        {
            get
            {
                if (_modifyOrganisme == null)
                    _modifyOrganisme = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.UPDATE)));
                return _modifyOrganisme;
            }
        }

        #endregion
    }
}
