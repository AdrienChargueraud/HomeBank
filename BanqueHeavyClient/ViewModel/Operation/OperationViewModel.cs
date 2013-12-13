using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;

namespace BanqueHeavyClient.ViewModel
{
    public class OperationViewModel : BaseViewModel
    {
        #region New and wen

        private Operation _selectedOperation;
        public Operation SelectedOperation
        {
            get { return _selectedOperation; }
            set { _selectedOperation = value; }
        }

        public OperationViewModel(Operation operation)
        {
            SelectedOperation = operation;
        }

        #endregion

        #region Binding View Element

        public string DateOperation
        {
            get { return SelectedOperation.operation_date.ToShortDateString(); }
        }

        public string TypeOperation
        {
            get { return SelectedOperation.Categorie.categorie_nom.ToString(); }
        }

        public string Credit
        {
            get { return CalculateCredit(); }
        }

        public string Debit
        {
            get { return CalculateDebit(); }
        }

        #endregion

        #region Metier

        private string CalculateDebit()
        {
            if (SelectedOperation.operation_montant < 0)
                return String.Format("{0:0.00}€", Math.Round(SelectedOperation.operation_montant, 2));
            return "";
        }

        private string CalculateCredit()
        {
            if (SelectedOperation.operation_montant > 0)
                return String.Format("{0:0.00}€", Math.Round(SelectedOperation.operation_montant, 2));
            return "";
        }

        #endregion
    }
}
