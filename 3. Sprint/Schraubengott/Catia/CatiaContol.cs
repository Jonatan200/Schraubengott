using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

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


                    //Zylinder

                    cc.SkizzeZylinderErstellen();      // Erstelle eine Skizze
                
                    cc.ZkizzeZylinder(arr);         // Generiere ein Profil

                    cc.ErzeugeZylinder(arr);        // Extrudiere Balken

                    cc.ErzeugeGewindehelix(arr);
                   // cc.ErzeugeGewindeFeature(arr);
                    
                    
                    cc.ErstelleSkizzeKopf(arr);       //Erstelle Skizze für den Kopf 

                    cc.ZkizzeKopf(arr);

                    cc.ErzeugeKopf(arr);           //Extrudiere Kopf

                    if (arr[0].typ == "Innensechskant")
                    {
                        cc.ZkizzeTasche(arr);

                        cc.TascheErzeugen(arr);
                        
                    }
                   
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
