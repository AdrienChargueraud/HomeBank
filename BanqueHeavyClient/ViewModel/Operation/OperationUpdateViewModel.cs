using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using System.Windows.Input;
using System.Windows;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel
{
    public class OperationUpdateViewModel : BaseViewModel
    {
        #region Propriétés

        public DetailCompteViewModel CompteVm { get; set; }
        public Operation OperationToCreate { get; set; }
        public string SelectedMode { get; set; }
        public string Montant { get; set; }
        public int Mode { get; set; }
        public BanqueContainer RefContexte { get; set; }

        #endregion

        #region New and wen

        public OperationUpdateViewModel(DetailCompteViewModel compteVm, int mode)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            CompteVm = compteVm;
            OperationToCreate = new Operation();
            OperationToCreate.operation_date = DateTime.Now;
            ListeCategories = RefContexte.Categorie.Where(m => m.categorie_actif == true && m.utilisateur_user_id == CompteVm.SelectedCompte.utilisateur_user_id).ToList();
            ListeType = new List<String>();
            ListeType.Add("Crédit");
            ListeType.Add("Débit");
            Mode = mode;
            if (mode == Convert.ToInt32(Enums.Mode.UPDATE))
            {
                OperationToCreate = compteVm.SelectedViewModelOperation.SelectedOperation;
                CategorieOperation = ListeCategories.Where(m => m.categorie_id == OperationToCreate.Categorie.categorie_id).SingleOrDefault();
                Montant = Convert.ToString(Math.Abs(OperationToCreate.operation_montant));
                if (OperationToCreate.operation_montant < 0)
                    SelectedMode = "Débit";
                else
                    SelectedMode = "Crédit";
            }
        }

        #endregion

        #region Binding

        public List<Categorie> ListeCategories { get; set; }

        public Categorie CategorieOperation { get; set; }

        public List<String> ListeType { get; set; }

        public string TitreFenetre
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "AJOUTER UNE OPERATION" : "MODIFIER UNE OPERATION"); 
            }
        }

        public string TitreBouton
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "Ajouter l'opération" : "Modifier l'opération");
            }
        }

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
                    _actionSave = new RelaiCommande(() => this.AddOperation());
                return _actionSave;
            }
        }

        #endregion

        #region Métier

        private void AddOperation()
        {
            Montant = Montant.Replace(".", ",");
            if (SelectedMode.Equals(Outil.GetEnumDescription(Enums.TypeOperation.DEBIT)))
                OperationToCreate.operation_montant = 0 - Convert.ToDouble(Montant);
            else
                OperationToCreate.operation_montant = Convert.ToDouble(Montant);

            var transaction = RefContexte.Database.BeginTransaction();
            if (Mode == Convert.ToInt32(Enums.Mode.NEW))
            {
                try
                {
                    OperationToCreate.compte_compte_id = CompteVm.SelectedCompte.compte_id;
                    OperationToCreate.categorie_categorie_id = CategorieOperation.categorie_id;
                    RefContexte.Operation.Add(OperationToCreate);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible d'ajouter l'opération précédemment créée!", "Opération", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }
            else
            {
                try
                {
                    var _optemp = RefContexte.Operation.Where(m => m.operation_id == OperationToCreate.operation_id).SingleOrDefault();
                    _optemp.operation_date = OperationToCreate.operation_date;
                    _optemp.operation_montant = OperationToCreate.operation_montant;
                    _optemp.categorie_categorie_id = CategorieOperation.categorie_id;
                    _optemp.operation_date = OperationToCreate.operation_date;
                    _optemp.Categorie = CategorieOperation;
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de modifier l'opération sélectionnée!", "Opération", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }

            transaction.Dispose();
            CompteVm.ExtractOperation();
            CompteVm.MiseAJourDate();
            CompteVm.MiseAJourOperation();
            //CompteVm.CategorieViewModel.ExtractOperations(); INUTILE (rafraichissement évènement binding)
            CloseWindows();
        }

        private void CloseWindows()
        {
            Application.Current.Windows[1].Close();
        }

        #endregion
    }
}
