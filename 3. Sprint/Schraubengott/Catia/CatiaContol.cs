using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schraubengott
{
     class CatiaControl
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
                    cc.ErzeugeProfil(20, 10);

                    // Extrudiere Balken
                    cc.ErzeugeBalken(300);
                }
                else
                {
                    // Console.WriteLine("Laufende Catia Application nicht gefunden");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception aufgetreten");
            }

        }

        static void Main(string[] args)
        {
            new CatiaControl();
        }
    }
}
