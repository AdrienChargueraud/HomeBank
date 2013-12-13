using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Windows.Input;
using System.Windows;

namespace BanqueHeavyClient.ViewModel.OperationPlanning
{
    public class OperationPlanningUpdateViewModel :  BaseViewModel
    {
        #region Propriétés

        public int Mode { get; set; }
        public BanqueContainer RefContexte { get; set; }
        public List<Categorie> ListeCategories { get; set; }
        public List<Compte> ListeComptes { get; set; }
        public Categorie CategorieOperation { get; set; }
        public Compte CompteOperation { get; set; }
        public BanqueLogicLayer.Modele.OperationPlanning OperationToCreate { get; set; }
        public List<String> ListeType { get; set; }
        public OperationPlanningViewModel ViewModel { get; set; }
        public string SelectedMode {get; set;}

        #endregion

        #region New and wen

        public OperationPlanningUpdateViewModel(OperationPlanningViewModel viewModel, int mode)
        {
            ViewModel = viewModel;
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            Mode = mode;
            ListeCategories = RefContexte.Categorie.Where(m => m.categorie_actif == true && m.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList();
            ListeComptes = RefContexte.Compte.Where(m => m.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList();
            OperationToCreate = new BanqueLogicLayer.Modele.OperationPlanning();
            
            ListeType = new List<String>();
            ListeType.Add(Outil.GetEnumDescription(Enums.TypeOperation.CREDIT));
            ListeType.Add(Outil.GetEnumDescription(Enums.TypeOperation.DEBIT));

            if (mode == Convert.ToInt32(Enums.Mode.UPDATE))
            {
                OperationToCreate = viewModel.OperationSelectionPlan.OperationSelectionPlan;
                CategorieOperation = ListeCategories.Where(m => m.categorie_id == OperationToCreate.categorie_categorie_id).SingleOrDefault();
                CompteOperation = ListeComptes.Where(m => m.compte_id == OperationToCreate.compte_compte_id).SingleOrDefault();
                Frequence = Convert.ToString(OperationToCreate.opeplan_frequence);
                Montant = Convert.ToString(Math.Abs(OperationToCreate.opeplan_montant));
                if (OperationToCreate.opeplan_montant < 0)
                    SelectedMode = Outil.GetEnumDescription(Enums.TypeOperation.DEBIT);
                else
                    SelectedMode = Outil.GetEnumDescription(Enums.TypeOperation.CREDIT);
            }
            else
            {
                Montant = "0";
                SelectedMode = Outil.GetEnumDescription(Enums.TypeOperation.CREDIT);
                OperationToCreate.opeplan_dateOperation = System.DateTime.Now;
                Frequence = "0";
            }
        }

        #endregion

        #region Binding

        public string TitreFenetre
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "AJOUTER UNE OPERATION PLANIFIEE" : "MODIFIER UNE OPERATION PLANIFIEE"); 
            }
        }

        public string TitreBouton
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "Ajouter l'opération planifiée" : "Modifier l'opération planifiée");
            }
        }

        public string Montant { get; set; }

        public string Frequence { get; set; }

        #endregion

        #region Declencheurs

        private RelaiCommande _actionClose;
        public ICommand ActionClose
        {
            get
            {
                if (_actionClose == null)
                    _actionClose = new RelaiCommande(() => this.CloseWindows());
                return _actionClose;
            }
        }

        private RelaiCommande _actionSave;
        public ICommand ActionSave
        {
            get
            {
                if (_actionSave == null)
                    _actionSave = new RelaiCommande(() => this.AddOperationPlannifiee());
                return _actionSave;
            }
        }

        #endregion

        #region Métier

        private void AddOperationPlannifiee()
        {
            try
            {
                if (String.IsNullOrEmpty(Montant))
                    Montant = "0";
                Montant = Montant.Replace(".", ",");
                if (SelectedMode.Equals(Outil.GetEnumDescription(Enums.TypeOperation.DEBIT)))
                    OperationToCreate.opeplan_montant = 0 - Convert.ToDouble(Montant);
                else
                    OperationToCreate.opeplan_montant = Convert.ToDouble(Montant);
                Montant = Montant.Replace(",", ".");
            }
            catch (Exception)
            {
                Montant = "0";
                new Erreur("Erreur de saisie du montant", "Opération planifiée", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                return;
            }

            var freq = 0;
            try
            {
                freq = Convert.ToInt32(Math.Round(Convert.ToDecimal(Frequence)));
            }
            catch (Exception)
            {
                Frequence = "";
                new Erreur("La fréquence n'est pas un entier", "Opération planifiée", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                return;
            }

            var transaction = RefContexte.Database.BeginTransaction();
            if (Mode == Convert.ToInt32(Enums.Mode.NEW))
            {
                try
                {
                    OperationToCreate.compte_compte_id = CompteOperation.compte_id;
                    OperationToCreate.categorie_categorie_id = CategorieOperation.categorie_id;
                    OperationToCreate.opeplan_frequence = freq;
                    RefContexte.OperationPlanning.Add(OperationToCreate);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible d'ajouter l'opération planifiée précédemment créée!", "Opération planifiée", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                    return;
                }
            }
            else
            {
                try
                {
                    var operation = RefContexte.OperationPlanning.Where(m => m.opeplan_id == OperationToCreate.opeplan_id).SingleOrDefault();
                    operation.opeplan_frequence = freq;
                    operation.opeplan_montant = OperationToCreate.opeplan_montant;
                    operation.compte_compte_id = CompteOperation.compte_id;
                    operation.categorie_categorie_id = CategorieOperation.categorie_id;
                    operation.opeplan_dateOperation = OperationToCreate.opeplan_dateOperation;
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de modifier l'opération planifiée sélectionnée!", "Opération planifiée", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                    return;
                }
            }

            transaction.Dispose();
            ViewModel.ExtractOperationPlanning();
            CloseWindows();
        }

        private void CloseWindows()
        {
            Application.Current.Windows[1].Close();
        }

        #endregion
    }
}
