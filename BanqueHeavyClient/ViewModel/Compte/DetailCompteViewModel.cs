using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BanqueHeavyClient.Global;
using System.Windows;

namespace BanqueHeavyClient.ViewModel
{
    public class DetailCompteViewModel : BaseViewModel
    {
        #region Propriétés

        public Compte SelectedCompte { get; set; }

        private BanqueContainer RefContexte { get; set; }

        public ObservableCollection<OperationViewModel> Operations { get; set; }

        private ObservableCollection<OperationViewModel> _operationDisplay;
        public ObservableCollection<OperationViewModel> OperationsDisplay 
        {
            get
            {
                if(_operationDisplay != null)
                    return new ObservableCollection<OperationViewModel>(_operationDisplay.OrderBy(m => m.DateOperation));
                return new ObservableCollection<OperationViewModel>();
            }
            set
            {
                _operationDisplay = value;
            }
        }

        private OperationViewModel _selectedViewModelOperation;
        public OperationViewModel SelectedViewModelOperation
        {
            get { return _selectedViewModelOperation; }
            set { _selectedViewModelOperation = value; OnPropertyChanged("IsBoutonActif"); }
        }

        public string CurrentMonth { get; set; }

        public string CurrentYear { get; set; }

        public CategorieViewModel CategorieViewModel { get; set; }

        public string ColorMajoration { get; set; }

        #endregion

        #region Binding

        public string Ouverture
        {
            get { return SelectedCompte.compte_dateOuverture.ToShortDateString(); }
        }

        public string OrganismeCompte
        {
            get 
            {
                SelectedCompte.Organisme = RefContexte.Organisme.Where(m => m.organisme_id == SelectedCompte.organisme_organisme_id).SingleOrDefault();
                if (SelectedCompte.Organisme != null)
                    return String.Format("{0} ({1})", SelectedCompte.Organisme.organisme_nom, SelectedCompte.Organisme.organisme_abrev);
                return String.Empty;
            }
        }

        public string SoldePlusUn
        {
            get { return String.Format("{0:0.00}€", this.CalculateSoldePlusUn()); }
        }

        public string SoldeCourant
        {
            get { return String.Format("{0:0.00}€", Outil.GetSoldeCourant(SelectedCompte)); }
        }

        public String NomFormat
        {
            get { return String.Format("{0}",SelectedCompte.compte_nom); }
        }

        public string Majoration
        {
            get { return CalculateMajoration(); }
        }

        public Visibility IsBoutonActif
        {
            get
            {
                return (SelectedViewModelOperation != null ? Visibility.Visible : Visibility.Hidden);
            }
        }

        #endregion

        #region New and wen
        public DetailCompteViewModel(Compte compte, CategorieViewModel categorieVm)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            CategorieViewModel = categorieVm;
            SelectedCompte = compte;
            Year = new ObservableCollection<string>();
            AddYear();
            Month = new ObservableCollection<string>();
            AddMonth();

            ExtractOperation();

            SelectedYear = Year.IndexOf(DateTime.Now.Year.ToString());
            SelectedMonth = DateTime.Now.Month - 1;
        }
        #endregion
        
        #region Gestion des dates

        private ObservableCollection<String> _month;
        public ObservableCollection<String> Month
        {
            get { return _month; }
            set { _month = value; }
        }

        private int _selectedMonth;
        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                MiseAJourDate();
                MiseAJourOperation();
            }
        }

        private ObservableCollection<String> _year;
        public ObservableCollection<String> Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                MiseAJourDate();
                MiseAJourOperation();
            }
        }

        internal void MiseAJourDate()
        {
            CurrentMonth = Month.ElementAt(_selectedMonth);
            CurrentYear = Year.ElementAt(_selectedYear);
            OnPropertyChanged("CurrentMonth");
            OnPropertyChanged("CurrentYear");
        }

        private void AddMonth()
        {
            Month.Add("Janvier");
            Month.Add("Février");
            Month.Add("Mars");
            Month.Add("Avril");
            Month.Add("Mai");
            Month.Add("Juin");
            Month.Add("Juillet");
            Month.Add("Août");
            Month.Add("Septembre");
            Month.Add("Octobre");
            Month.Add("Novembre");
            Month.Add("Décembre");
        }

        private void AddYear()
        {
            Year.Add("2012");
            Year.Add("2013");
            Year.Add("2014");
        }

        #endregion

        #region Declencheurs

        private RelaiCommande _addOperation;
        public ICommand AddOperation
        {
            get
            {
                if (_addOperation == null)
                    _addOperation = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.NEW)));
                return _addOperation;
            }
        }

        private RelaiCommande _deleteOperation;
        public ICommand DeleteOperation
        {
            get
            {
                if (_deleteOperation == null)
                    _deleteOperation = new RelaiCommande(() => this.DeleteOperationInCollection());
                return _deleteOperation;
            }
        }

        private RelaiCommande _modifyOperation;
        public ICommand ModifyOperation
        {
            get
            {
                if (_modifyOperation == null)
                    _modifyOperation = new RelaiCommande(() => this.ShowWindowOperation(Convert.ToInt32(Enums.Mode.UPDATE)));
                return _modifyOperation;
            }
        }

        #endregion

        #region Navigation

        private void ShowWindowOperation(int mode)
        {
            View.OperationUpdate view = new View.OperationUpdate();
            view.DataContext = new OperationUpdateViewModel(this, mode);
            view.ShowDialog();
        }

        #endregion

        #region Metier

        internal void ExtractOperation()
        {
            Operations = new ObservableCollection<OperationViewModel>();
            foreach (Operation _op in SelectedCompte.Operations)
                Operations.Add(new OperationViewModel(_op));
            OnPropertyChanged("SoldeCourant");
            OnPropertyChanged("Majoration");
        }

        internal string CalculateMajoration()
        {
            double ecart = SelectedCompte.compte_soldeCourant - SelectedCompte.compte_soldeDepart;
            double pourcentage = (100 * SelectedCompte.compte_soldeCourant) / SelectedCompte.compte_soldeDepart;
            if (ecart >= 0)
            {
                ColorMajoration = "green";
                OnPropertyChanged("ColorMajoration");
                return String.Format("+{0:0.00}€ (+{1:0.00}%)", ecart, Math.Round(pourcentage - 100, 2));
            }
            ColorMajoration = "red";
            OnPropertyChanged("ColorMajoration");
            return String.Format("{0:0.00}€ ({1:0.00}%)", ecart, Math.Round(pourcentage - 100, 2));
        }

        internal void MiseAJourOperation()
        {
            List<OperationViewModel> _optemp = Operations.Where(m => m.SelectedOperation.operation_date.Year.Equals(Convert.ToInt32(CurrentYear))
                                                                && m.SelectedOperation.operation_date.Month.Equals(_selectedMonth + 1)
                                                                ).ToList();
            OperationsDisplay = new ObservableCollection<OperationViewModel>(_optemp);
            OnPropertyChanged("OperationsDisplay");
            OnPropertyChanged("SoldePlusUn");
        }

        internal double CalculateSoldePlusUn()
        {
            DateTime datePlusUn = new DateTime(Convert.ToInt32(CurrentYear), Convert.ToInt32(SelectedMonth + 1), 1);
            List<OperationViewModel> _optemp = Operations.Where(m => m.SelectedOperation.operation_date <= datePlusUn).ToList();
            double value = _optemp.Sum(m => m.SelectedOperation.operation_montant);
            if (datePlusUn < SelectedCompte.compte_dateOuverture)
                return 0;
            return value + SelectedCompte.compte_soldeDepart;
        }

        private void DeleteOperationInCollection()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            if (SelectedViewModelOperation != null)
            {
                try
                {
                    RefContexte.Operation.Remove(SelectedViewModelOperation.SelectedOperation);
                    RefContexte.SaveChanges();
                    transaction.Commit();

                    ExtractOperation();
                    MiseAJourOperation();
                    MiseAJourDate();
                    CategorieViewModel.ExtractOperations();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Suppression de l'opération impossible!", "Operation", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }
            transaction.Dispose();
        }

        #endregion
    }
}
