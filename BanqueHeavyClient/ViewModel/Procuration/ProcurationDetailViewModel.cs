using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanqueHeavyClient.ViewModel.Procuration
{
    public class ProcurationDetailViewModel : BaseViewModel
    {
        #region Propriétés

        public BanqueLogicLayer.Modele.Procuration ElementSelectionne { get; set; }

        #endregion

        public ProcurationDetailViewModel(BanqueLogicLayer.Modele.Procuration element)
        {
            ElementSelectionne = element;
        }
    }
}
