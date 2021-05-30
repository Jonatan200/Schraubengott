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

                    for (int i = 0;  i < arr.Length;  i++)
                    {
                        // Öffne ein neues Part
                        cc.ErzeugePart(i);


                        //Zylinder

                        cc.SkizzeZylinderErstellen();      // Erstelle eine Skizze

                        cc.ZkizzeZylinder(arr, i);         // Generiere ein Profil

                        cc.ErzeugeZylinder(arr, i);        // Extrudiere Balken

                        cc.ErzeugeGewindehelix(arr, i);

                        // 
                        //cc.ErzeugeGewindeFeature(arr);


                        cc.ErstelleSkizzeKopf(arr);       //Erstelle Skizze für den Kopf 

                        cc.ZkizzeKopf(arr ,i);

                        cc.ErzeugeKopf(arr,i);           //Extrudiere Kopf

                        if (arr[i].typ == "Innensechskant")
                        {
                            cc.ZkizzeTasche(arr ,i);

                            cc.TascheErzeugen(arr, i);

                        }

                        cc.Zeichnungsableitung();

                        cc.ErzeugeFase();
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
