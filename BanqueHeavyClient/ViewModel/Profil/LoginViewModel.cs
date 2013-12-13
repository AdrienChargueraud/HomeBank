using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;
using System.Windows.Input;
using System.Windows;

namespace BanqueHeavyClient.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Proprietés

        private BanqueContainer RefContexte { get; set; }

        #endregion

        #region binding

        public String Identifiant { get; set; }
        public String MotDePasse { get; set; }

        #endregion

        #region New and Wen

        public LoginViewModel()
        {
            RefContexte = SessionUtilisateur.Instance.banqueContexte;
        }

        #endregion

        #region declencheur

        private RelaiCommande _log;
        public ICommand Log
        {
            get
            {
                if (_log == null)
                    _log = new RelaiCommande(() => this.LogUsers());
                return _log;
            }
        }

        #endregion

        #region Metier

        private void LogUsers()
        {
            bool auth = false;
            if (Identifiant != null && MotDePasse != null)
            {
                var user = RefContexte.Utilisateur.Where(m => m.user_name.Equals(Identifiant)).SingleOrDefault();
                if (user != null)
                {
                    if (user.user_password.Equals(Outil.EncodeMotDePasse(MotDePasse)))
                    {
                        SessionUtilisateur.Instance.ConnectedUser = user;
                        auth = true;
                        Home view = new Home();
                        view.DataContext = new HomeViewModel();
                        Application.Current.Windows[0].Close();
                        view.ShowDialog();
                    }
                }
            }

            if (!auth)
            {
                new Erreur("Authentification impossible !", "Login", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }
        }

        #endregion
    }
}
