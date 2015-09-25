using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using IMPression;

namespace IMPressive
{
    public static class EH
    {
        public static void Throw(this Exception e)
        {
            if(e is ParseException || e.InnerException is ParseException)
            {
                MessageBox.Show((e.InnerException ?? (ParseException) e).Message, "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Erreur : " + (e.InnerException?.Message ?? e.Message), "Erreur", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }
        }
    }
}
