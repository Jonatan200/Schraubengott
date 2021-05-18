using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schraubengott
{
     internal class CatiaControl
     {
        CatiaControl()
        {
            try
            {

                CatiaConnection cc = new CatiaConnection();

                // Finde Catia Prozess
                if (cc.CATIALaeuft())
                {

                    // Öffne ein neues Part
                    cc.ErzeugePart();

                    // Erstelle eine Skizze
                    cc.LeereSkizzeErzeugen();

                    // Generiere ein Profil
                    cc.ErzeugeProfiel();

                    // Extrudiere Balken
                    cc.ErzeugeBalken();
                }
                else
                {
                    // Console.WriteLine("Laufende Catia Application nicht gefunden");
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Exception aufgetreten");
            }
              
        }

        public static void Catia_Starten()
        {
            new CatiaControl();
        }
    }
}
