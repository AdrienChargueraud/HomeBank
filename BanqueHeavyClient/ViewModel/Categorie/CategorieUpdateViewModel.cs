using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel
{
    public class CategorieUpdateViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        public int Mode { get; set; }
        public CategorieViewModel CategorieModele { get; set; }
        public Categorie LaCategorie { get; set; }

        #endregion

        #region New and wen

        public CategorieUpdateViewModel(int mode, CategorieViewModel model)  : base()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            Mode = mode;
            CategorieModele = model;
            LaCategorie = new Categorie();
            if (Mode == Convert.ToInt32(Enums.Mode.UPDATE))
                LaCategorie = CategorieModele.SelectionCategorie.MaCategorieSelectionne;
        }

        #endregion

        #region Binding

        public string TitreFenetre
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "AJOUTER UNE CATEGORIE" :"MODIFIER UNE CATEGORIE");
            }
        }

        public string TitreBouton
        {
            get
            {
                return ((Mode == Convert.ToInt32(Enums.Mode.NEW)) ? "Ajouter la catégorie" : "Modifier la catégorie");
            }
        }

        #endregion

        #region Déclencheur

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
                    LaCategorie.utilisateur_user_id = SessionUtilisateur.Instance.ConnectedUser.user_id;
                    RefContexte.Categorie.Add(LaCategorie);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible d'ajouter la catégorie précédemment créée!", "Catégorie", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }
            if (Mode == Convert.ToInt32(Enums.Mode.UPDATE))
            {
                try
                {
                    Categorie categorie = RefContexte.Categorie.Where(m => m.categorie_id == LaCategorie.categorie_id).SingleOrDefault();
                    categorie.categorie_actif = LaCategorie.categorie_actif;
                    categorie.categorie_nom = LaCategorie.categorie_nom;
                    categorie.utilisateur_user_id = SessionUtilisateur.Instance.ConnectedUser.user_id;
                    RefContexte.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de modifier la catégorie sélectionnée!", "Catégorie", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
            }

            transaction.Dispose();
            CategorieModele.ExtractOperations();
            CloseWindows();
        }

        #endregion
    }
}
