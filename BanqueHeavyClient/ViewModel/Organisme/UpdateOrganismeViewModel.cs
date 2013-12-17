using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BanqueHeavyClient.Global;
using BanqueLogicLayer.Modele;

namespace BanqueHeavyClient.ViewModel.Organisme
{
    public class UpdateOrganismeViewModel : BaseViewModel
    {
        #region Propriétés

        public OrganismeViewModel ViewModel { get; set; }
        public int Mode { get; set; }
        private BanqueContainer RefContexte { get; set; }
        public BanqueLogicLayer.Modele.Organisme OrganismeCreation { get; set; }

        #endregion

        #region New and Wen

        public UpdateOrganismeViewModel(OrganismeViewModel organismeViewModel, int mode)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            this.ViewModel = organismeViewModel;
            this.Mode = mode;
            if (Mode == Convert.ToInt32(Enums.Mode.NEW))
            {
                OrganismeCreation = new BanqueLogicLayer.Modele.Organisme();
            }
            else
            {
                OrganismeCreation = ViewModel.OrganismeSelect.OrganismeSelection;
            }
        }

        #endregion

        #region Binding

        public string TitreFenetre
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "AJOUTER UN ORGANISME" : "MODIFIER UN ORGANISME");
            }
        }

        public string TitreBouton
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "Ajouter l'organisme" : "Modifier l'organisme");
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
                    _actionSave = new RelaiCommande(() => this.SaveModification());
                return _actionSave;
            }
        }

        #endregion

        #region Métier

        private void CloseWindows()
        {
            Application.Current.Windows[1].Close();
        }

        private void SaveModification()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            if (Mode == Convert.ToInt32(Enums.Mode.NEW))
            {
                try
                {
                    if (RefContexte.Organisme.Where(m => m.organisme_nom.Equals(OrganismeCreation.organisme_nom)).ToList().Count > 0)
                    {
                        new Erreur("Impossible d'ajouter un organisme qui existe déjà!", "Organisme", Enums.TypeErreurMessage.AVERTISSEMENT).DisplayErreur();
                    }
                    RefContexte.Organisme.Add(OrganismeCreation);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible d'ajouter l'organisme précédemment créé!", "Organisme", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                    return;
                }
            }
            else
            {
                try
                {
                    if (RefContexte.Organisme.Where(m => m.organisme_nom.Equals(OrganismeCreation.organisme_nom)).ToList().Count > 0)
                    {
                        new Erreur("Impossible d'ajouter un organisme qui existe déjà!", "Organisme", Enums.TypeErreurMessage.AVERTISSEMENT).DisplayErreur();
                    }
                    var tmp = RefContexte.Organisme.Where(m => m.organisme_id == OrganismeCreation.organisme_id).SingleOrDefault();
                    tmp.organisme_nom = OrganismeCreation.organisme_nom;
                    tmp.organisme_abrev = OrganismeCreation.organisme_abrev;
                    tmp.organisme_actif = OrganismeCreation.organisme_actif;
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de modifier l'organisme sélectionné!", "Organisme", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                    return;
                }
            }

            transaction.Dispose();
            ViewModel.ExtractAllOrganisme();
            CloseWindows();
        }

        #endregion
    }
}
