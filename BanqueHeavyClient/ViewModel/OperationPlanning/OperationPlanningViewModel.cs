using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Collections.ObjectModel;
using BanqueHeavyClient.ViewModel.OperationPlanning;
using System.Windows.Input;
using System.Windows;

namespace BanqueHeavyClient.ViewModel
{
    public class OperationPlanningViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        public ObservableCollection<DetailOperationPlanningViewModel> OperationsPlannifiees { get; set; }
        private DetailOperationPlanningViewModel _operationSelectionPlan;
        public DetailOperationPlanningViewModel OperationSelectionPlan
        {
            get { return _operationSelectionPlan; }
            set { _operationSelectionPlan = value; OnPropertyChanged("IsBoutonActif"); }
        }

        #endregion

        #region New and wen

        public OperationPlanningViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ExtractOperationPlanning();
        }

        #endregion

        #region Binding

        public Visibility IsBoutonActif
        {
            get
            {
                return ((OperationSelectionPlan != null) ? Visibility.Visible : Visibility.Hidden);
            }
        }

        #endregion

        #region Métier

        public void ExtractOperationPlanning()
        {
            OperationsPlannifiees = new ObservableCollection<DetailOperationPlanningViewModel>();
            var idList = RefContexte.Compte.Where(a => a.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).Select(m => m.compte_id).ToList();
            var _operationsPlannifiees = RefContexte.OperationPlanning.Where(x => idList.Contains(x.compte_compte_id)).ToList();
            foreach (var a in _operationsPlannifiees)
                OperationsPlannifiees.Add(new DetailOperationPlanningViewModel(a));
            OnPropertyChanged("OperationsPlannifiees");
        }

        private void ShowWindowOperation(int mode)
        {
            View.OperationPlanningUpdate view = new View.OperationPlanningUpdate();
            view.DataContext = new OperationPlanningUpdateViewModel(this, mode);
            view.ShowDialog();
        }

        private void DeleteOperationPlanning()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            try
            {
                RefContexte.OperationPlanning.Remove(OperationSelectionPlan.OperationSelectionPlan);
                RefContexte.SaveChanges();
                transaction.Commit();

                ExtractOperationPlanning();
            }
            catch (Exception)
            {
                transaction.Rollback();
                new Erreur("Impossible de supprimer l'opération planifiée!", "Opération planifiée", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }
            finally
            {
                transaction.Dispose();
            }

            if (OperationsPlannifiees.Count > 0)
                OperationSelectionPlan = OperationsPlannifiees.ElementAt(0);
        }

        #endregion

        #region Declencheurs

        private RelaiCommande _addPlanOperation;
        public ICommand AddPlanOperation
        {
            get
            {
                if (_addPlanOperation == null)
                    _addPlanOperation = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.NEW)));
                return _addPlanOperation;
            }
        }

        private RelaiCommande _deletePlanOperation;
        public ICommand DeletePlanOperation
        {
            get
            {
                if (_deletePlanOperation == null)
                    _deletePlanOperation = new RelaiCommande(() => this.DeleteOperationPlanning());
                return _deletePlanOperation;
            }
        }

        private RelaiCommande _modifyPlanOperation;
        public ICommand ModifyPlanOperation
        {
            get
            {
                if (_modifyPlanOperation == null)
                    _modifyPlanOperation = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.UPDATE)));
                return _modifyPlanOperation;
            }
        }

        #endregion
    }
}
