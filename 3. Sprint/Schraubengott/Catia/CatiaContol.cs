using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schraubengott
{
     internal class CatiaControl
     {
        CatiaControl(Schraube[] arr)
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
                    cc.ErsteSkizzeErstellen();

                    // Generiere ein Profil
                    cc.ZkizzeZylinder(arr);

                    // Extrudiere Balken
                    cc.ErzeugeGewindestange();



                    cc.ZweiteSkizzeErzeugen();

                    cc.ZkizzeKopf();

                    cc.ErzeugeKopf();

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

        public static void Catia_Starten(Schraube[] arr)
        {
            new CatiaControl(arr);
        }
    }
}
