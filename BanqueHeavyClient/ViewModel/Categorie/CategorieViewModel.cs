using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using BanqueLogicLayer.Modele;
using System.Windows.Input;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.ViewModel
{
    public class CategorieViewModel : BaseViewModel
    {
        #region Propriétés

        private BanqueContainer RefContexte { get; set; }
        private ObservableCollection<DetailCategorieViewModel> _categories;
        public ObservableCollection<DetailCategorieViewModel> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        private DetailCategorieViewModel _selectionCategorie;
        public DetailCategorieViewModel SelectionCategorie 
        {
            get { return _selectionCategorie; }
            set
            {
                _selectionCategorie = value;
                OnPropertyChanged("SelectionCategorie");
            }
        }

        #endregion

        #region New and wen

        public CategorieViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
            ExtractOperations();
            if(Categories.Count > 0)
                SelectionCategorie = Categories.ElementAt(0);
        }

        #endregion

        #region Declencheurs

        private RelaiCommande _modifyCategory;
        public ICommand ModifyCategory
        {
            get
            {
                if (_modifyCategory == null)
                    _modifyCategory = new RelaiCommande(() => this.ShowWindowAccount(Convert.ToInt32(Enums.Mode.UPDATE)));
                return _modifyCategory;
            }
        }

        private RelaiCommande _addCategory;
        public ICommand AddCategory
        {
            get
            {
                if (_addCategory == null)
                    _addCategory = new RelaiCommande(() => this.ShowWindowAccount(Convert.ToInt32(Enums.Mode.NEW)));
                return _addCategory;
            }
        }

        private RelaiCommande _deleteCategory;
        public ICommand DeleteCategory
        {
            get
            {
                if (_deleteCategory == null)
                    _deleteCategory = new RelaiCommande(() => this.DeleteCategorieCollection());
                return _deleteCategory;
            }
        }

        #endregion

        #region Metier

        public void ExtractOperations()
        {
            Categories = new ObservableCollection<DetailCategorieViewModel>();
            foreach (Categorie _cat in RefContexte.Categorie.Where(m => m.utilisateur_user_id == SessionUtilisateur.Instance.ConnectedUser.user_id).ToList())
            {
                Categories.Add(new DetailCategorieViewModel(_cat));
            }
            OnPropertyChanged("Categories");
        }

        private void ShowWindowAccount(int mode)
        {
            View.CategorieUpdate view = new View.CategorieUpdate();
            view.DataContext = new CategorieUpdateViewModel(mode, this);
            view.ShowDialog();
            if (mode == 1)
                SelectionCategorie = Categories.ElementAt(Categories.Count - 1);
        }

        private void DeleteCategorieCollection()
        {
            if (SelectionCategorie.Utilisation == 0)
            {
                var transaction = RefContexte.Database.BeginTransaction();
                try
                {
                    RefContexte.Categorie.Remove(SelectionCategorie.MaCategorieSelectionne);
                    RefContexte.SaveChanges();
                    transaction.Commit();
                    Categories.Remove(SelectionCategorie);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Erreur("Impossible de supprimer la catégorie!", "Catégorie", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
                }
                transaction.Dispose();
            }
            else
            {
                new Erreur("Impossible de supprimer une catégorie qui contient des opérations!", "Catégorie", Enums.TypeErreurMessage.AVERTISSEMENT).DisplayErreur();
            }

            if (Categories.Count > 0 && SelectionCategorie == null)
                SelectionCategorie = Categories.ElementAt(0);
        }

        #endregion
    }
}
