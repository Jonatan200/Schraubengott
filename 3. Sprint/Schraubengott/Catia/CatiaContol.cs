using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;

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
                    System.Windows.MessageBox.Show("CATIA Part wird erstellt.", "", MessageBoxButton.OK); 
                    System.Threading.Thread.Sleep(5000);// 5 Sekunden Wartezeit

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
                    Process.Start("CNEXT.exe");
                    System.Windows.MessageBox.Show("CATIA wird gestartet. Nach dem Start können CATIA Parts erstellt werden.", "", MessageBoxButton.OK);
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
