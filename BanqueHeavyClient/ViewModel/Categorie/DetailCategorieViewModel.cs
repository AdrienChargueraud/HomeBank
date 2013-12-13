using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using System.Windows.Input;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel
{
    public class DetailCategorieViewModel : BaseViewModel
    {
        #region propriétés

        public Categorie MaCategorieSelectionne { get; set; }
        private BanqueContainer Contexte { get; set; }

        private List<Operation> _operationsAssociee;
        public List<Operation> OperationAssociee {
            get
            {
                if (ModeFiltre.Equals(TypeFiltre.ElementAt(1)))
                    return _operationsAssociee.OrderByDescending(m => m.operation_date).ToList();
                else
                    return _operationsAssociee.OrderByDescending(m => Math.Abs(m.operation_montant)).ToList();
            }
            set
            {
                _operationsAssociee = value;
            }
        }

        private string _modeFiltre;
        public string ModeFiltre
        {
            get { return _modeFiltre; }
            set { _modeFiltre = value; OnPropertyChanged("OperationAssociee"); }
        }

        public List<String> TypeFiltre { get; set; }

        public Operation SelectedOperation { get; set; }

        public int NbOperationSysteme { get; set; }

        #endregion

        #region Binding

        public double Utilisation
        {
            get { return OperationAssociee.Count; }
        }

        public double TauxUtilisation
        {
            get 
            {
                if (NbOperationSysteme == 0)
                    return 0.0;
                return (OperationAssociee.Count / Convert.ToDouble(NbOperationSysteme))*100; 
            }
        }

        public double TauxUtilisationRate
        {
            get 
            {
                if (NbOperationSysteme == 0)
                    return 0.0;
                return (OperationAssociee.Count / Convert.ToDouble(NbOperationSysteme)); 
            }
        }

        public string Activite
        {
            get
            {
                if (MaCategorieSelectionne.categorie_actif)
                    return "ACTIF";
                return "INACTIF";
            }
        }

        public System.Windows.Media.Brush ForeColor
        {
            get
            {
                if (MaCategorieSelectionne.categorie_actif)
                    return System.Windows.Media.Brushes.Green;
                return System.Windows.Media.Brushes.Red;
            }
        }

        #endregion

        #region New and wen

        public DetailCategorieViewModel(Categorie categorie)
        {
            Contexte = SessionUtilisateur.Instance.banqueContexte;
            MaCategorieSelectionne = categorie;
            _operationsAssociee = MaCategorieSelectionne.Operations.ToList();
            NbOperationSysteme = Contexte.Operation.Count();
            TypeFiltre = new List<string>();
            TypeFiltre.Add("Les plus gros montants");
            TypeFiltre.Add("Les dernières opérations");
            ModeFiltre = TypeFiltre.ElementAt(0);
        }

        #endregion
    }
}
