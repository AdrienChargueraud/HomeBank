using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using BanqueLogicLayer.Modele;
using System.Windows;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel
{
    public class CompteUpdateViewModel : BaseViewModel
    {
        #region Propriété

        public int Mode { get; set; }
        private BanqueContainer RefContexte { get; set; }
        public CompteViewModel CompteModele { get; set; }
        public Compte LeCompte { get; set; }
        public ICollection<BanqueLogicLayer.Modele.Organisme> ListeOrganisme { get; set; }
        public BanqueLogicLayer.Modele.Organisme OrganismeSelec { get; set; }

        #endregion

        #region New and wen

        public CompteUpdateViewModel(int mode, CompteViewModel model)
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            CompteModele = model;
            ListeOrganisme = RefContexte.Organisme.Where(m => m.organisme_actif == true && m.Utilisateur_user_id == CompteModele.SelectionCompte.SelectedCompte.utilisateur_user_id).ToList();
            Mode = mode;
            LeCompte = new Compte();
            if (Mode == Convert.ToInt32(Enums.Mode.UPDATE))
            {
                LeCompte = model.SelectionCompte.SelectedCompte;
                OrganismeSelec = ListeOrganisme.Where(m => m.organisme_id == LeCompte.organisme_organisme_id).SingleOrDefault();
            }
            else
            {
                LeCompte.compte_dateOuverture = System.DateTime.Now;
            }
        }

        #endregion

        #region Binding

        public string TitreFenetre
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "AJOUTER UN COMPTE" : "MODIFIER UN COMPTE");
            }
        }

        public string TitreBouton
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "Ajouter un nouveau compte" : "Modifier le compte");  
            }
        }

        #endregion

        #region Déclencheurs

        private RelaiCommande _actionSave;
        public ICommand ActionSave
        {
            get
            {
                if (_actionSave == null)
                    _actionSave = new RelaiCommande(() => this.UpdateDatabase());
                return _actionSave;
            }
        }

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

        #endregion

        #region Métier

        private void CloseWindows()
        {
            Application.Current.Windows[1].Close() ;
        }

        private void UpdateDatabase()
        {
            var transaction = RefContexte.Database.BeginTransaction();
            if (Mode == Convert.ToInt32(Enums.Mode.NEW))
            {
                try
                {
                    LeCompte.compte_numero = DateTime.Now.Ticks.ToString();
                    LeCompte.organisme_organisme_id = OrganismeSelec.organisme_id;
                    LeCompte.utilisateur_user_id = SessionUtilisateur.Instance.ConnectedUser.user_id; 
                    RefContexte.Compte.Add(LeCompte);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible d'ajouter le compte précédemment créé!", "Compte", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }
            if (Mode == Convert.ToInt32(Enums.Mode.UPDATE))
            {
                try
                {
                    var tmpCompte = RefContexte.Compte.SingleOrDefault(m => m.compte_id == LeCompte.compte_id);
                    tmpCompte.organisme_organisme_id = OrganismeSelec.organisme_id;
                    tmpCompte.compte_interet = LeCompte.compte_interet;
                    tmpCompte.compte_nom = LeCompte.compte_nom;
                    tmpCompte.compte_numero = LeCompte.compte_numero;
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de modifier le compte sélectionné!", "Catégorie", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }

            transaction.Dispose();
            CompteModele.UpdateComptesFromDatabase();
            CloseWindows();
        }

        #endregion
    }
}
