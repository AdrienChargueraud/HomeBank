using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueHeavyClient.Global;
using BanqueLogicLayer.Modele;
using System.Collections.ObjectModel;

namespace BanqueHeavyClient.ViewModel.Procuration
{
    public class ProcurationViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        public ObservableCollection<ProcurationDetailViewModel> ProcurationVersAutre { get; set; }
        public ObservableCollection<ProcurationDetailViewModel> ProcurationVersMoi { get; set; }
        public ProcurationDetailViewModel ProcVA_Selection { get; set; }
        public ProcurationDetailViewModel ProcVM_Selection { get; set; }

        #endregion

        #region New and wen

        public ProcurationViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ExtractProcurationAutreVersMoi();
            ExtractProcurationMoiVersAutre();
        }

        #endregion

        #region Metier

        private void ExtractProcurationMoiVersAutre()
        {
            var proc = RefContexte.Procuration.Where(m => m.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList();
            ProcurationVersAutre.Clear();
            foreach (var element in proc)
            {
                ProcurationVersAutre.Add(new ProcurationDetailViewModel(element));
            }
            OnPropertyChanged("ProcurationVersAutre");
        }

        private void ExtractProcurationAutreVersMoi()
        {
            var idList = RefContexte.Compte.Where(a => a.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).Select(m => m.compte_id).ToList();
            var proc = RefContexte.Procuration.Where(x => idList.Contains(x.compte_compte_id)).ToList();
            ProcurationVersMoi.Clear();
            foreach (var element in proc)
            {
                ProcurationVersMoi.Add(new ProcurationDetailViewModel(element));
            }
            OnPropertyChanged("ProcurationVersMoi");
        }

        #endregion
    }
}
