using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BanqueHeavyClient.Global
{
    public class Erreur
    {
        #region Propriétés

        internal string MessageErreur { get; set; }
        internal string SourceErreur { get; set; }
        internal Enums.TypeErreurMessage TypeErreur { get; set; }

        #endregion

        #region New and wen

        public Erreur(string message, string source, Enums.TypeErreurMessage type)
        {
            MessageErreur = message;
            SourceErreur = source;
            TypeErreur = type;
        }

        #endregion

        #region Métier

        public void DisplayErreur()
        {
            string tmpFormat = String.Format("{0} ({1}) : {2}", SourceErreur, System.DateTime.Now, MessageErreur);
            MessageBoxResult result;
            switch (TypeErreur)
            {
                case Enums.TypeErreurMessage.ERREUR :
                    result = MessageBox.Show(tmpFormat, "Erreur HomeBank", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case Enums.TypeErreurMessage.AVERTISSEMENT :
                    result = MessageBox.Show(tmpFormat, "Avertissement HomeBank", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case Enums.TypeErreurMessage.INFORMATION :
                    result = MessageBox.Show(tmpFormat, "Information HomeBank", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        #endregion
    }
}
