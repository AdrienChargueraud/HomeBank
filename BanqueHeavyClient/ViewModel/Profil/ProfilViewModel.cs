using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using BanqueLogicLayer.Modele;

namespace BanqueHeavyClient.ViewModel
{
    public class ProfilViewModel : BaseViewModel
    {
        public string BrowseFile { get; set; }

        public BitmapImage Image { get; set; }

        public BanqueLogicLayer.Modele.Utilisateur User { get; set; }

        public ProfilViewModel()
        {
            //User = Repository.Instance.GetUtilisateurByName("Chargueraud") ;
            //if (!String.IsNullOrEmpty(User.Image))
            //{
            //    BrowseFile = User.Image;
            //    BtnUpload();
            //}
            //else
            //    BrowseFile = "";
        }

        private RelaiCommande _parcourirLeDisque;
        public ICommand ParcourirLeDisque
        {
            get
            {
                if (_parcourirLeDisque == null)
                    _parcourirLeDisque = new RelaiCommande(() => this.OpenFileWindows());
                return _parcourirLeDisque;
            }
        }

        private RelaiCommande _uploader;
        public ICommand Uploader
        {
            get
            {
                if (_uploader == null)
                    _uploader = new RelaiCommande(() => this.BtnUpload());
                return _uploader;
            }
        }

        private void OpenFileWindows()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                BrowseFile = ofd.FileName;
                //User.Image = BrowseFile;
            }
        }

        private void BtnUpload()
        {
            if (BrowseFile.Trim().Length != 0)
            {
                Image = new BitmapImage();
                Image.BeginInit();
                Image.UriSource = new Uri(BrowseFile.Trim(), UriKind.Relative);
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.EndInit();
                OnPropertyChanged("Image");
            }
        }

        private RelaiCommande _sauvegarder;
        public ICommand Sauvegarder
        {
            get
            {
                if (_sauvegarder == null)
                    _sauvegarder = new RelaiCommande(() => this.SauvegarderProfil());
                return _sauvegarder;
            }
        }

        private void SauvegarderProfil()
        {
            //Repository.Instance.SaveUser(User);
        }
    }
}
