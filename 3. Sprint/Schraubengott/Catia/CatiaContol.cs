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
                /*  bool laufbedingung = false;

                  while(laufbedingung == false)
                  {
                      int z = 0,
                      i=z++;
                      if (z < 10)
                      {
                          cc.CATIALaeuft();
                      }

                      else
                      {
                          laufbedingung
                      }

                  }
                */


                bool catläuft = false;

                if (cc.CATIALaeuft() == false)
                {
                    Process.Start("CNEXT.exe");
                    //System.Windows.MessageBox.Show("CATIA wird gestartet. Nach dem Start können CATIA Parts erstellt werden.", "", MessageBoxButton.OK);

                    for (int c = 0; c < 5; c++)
                    {
                        System.Threading.Thread.Sleep(5000); //5 Sekunden Wartezeit#

                        if (cc.CATIALaeuft())
                        {
                            catläuft = true;
                            break;
                        }
                        if (c == 5)
                        {
                            System.Windows.MessageBox.Show("Ladezeit übeschritten, Bitte erneut versuchen, oder Catia manuell Starten", "");
                        }


                    }

                }
                else
                {
                    catläuft = true;
                }

                if (catläuft == true)
                { 
                #region Methodenaufrufe
                //System.Windows.MessageBox.Show("CATIA Part wird erstellt.", "", MessageBoxButton.OK);


                for (int i = 0; i < arr.Length; i++)
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

                    cc.ZkizzeKopf(arr, i);

                    cc.ErzeugeKopf(arr, i);           //Extrudiere Kopf

                    if (arr[i].typ == "Innensechskant")
                    {
                        cc.ZkizzeTasche(arr, i);

                        cc.TascheErzeugen(arr, i);

                    }

                    cc.Zeichnungsableitung();

                    cc.ErzeugeFase();
                }
                    #endregion
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
