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
        CatiaControl(Schraube screw, int bestellnummer, string[] kundendaten)
        {
                        
                CatiaConnection cc = new CatiaConnection();

                bool catläuft = false;

                if (cc.CATIALaeuft() == false)
                {
                    Process.Start("CNEXT.exe");
                    //System.Windows.MessageBox.Show("CATIA wird gestartet. Nach dem Start können CATIA Parts erstellt werden.", "", MessageBoxButton.OK);

                    for (int c = 0; c < 15; c++)
                    {
                        System.Threading.Thread.Sleep(2000); //5 Sekunden Wartezeit#

                        if (cc.CATIALaeuft())
                        {
                            catläuft = true;
                            break;
                        }
                        if (c == 15)
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

                    // Öffne ein neues Part
                    cc.ErzeugePart();

                    //Zylinder

                    cc.SkizzeZylinderErstellen();      // Erstelle eine Skizze

                    cc.ZkizzeZylinder(screw);         // Generiere ein Profil

                    cc.ErzeugeZylinder(screw);        // Extrudiere Balken

                    cc.ErzeugeGewindehelix(screw);

                    //cc.ErzeugeGewindeFeature(arr);

                    cc.ErstelleSkizzeKopf(screw);       //Erstelle Skizze für den Kopf 

                    cc.ZkizzeKopf(screw);

                    cc.ErzeugeKopf(screw);           //Extrudiere Kopf

                    if (screw.typ == "Innensechskant")
                    {
                        cc.ZkizzeTasche(screw);

                        cc.TascheErzeugen(screw);

                    }

                    cc.Zeichnungsableitung(screw, bestellnummer, kundendaten);

                    //cc.ErzeugeFase();
                }
                    #endregion
                
         
            /*catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception aufgetreten");
            }     
            */
        }

        public static void Catia_Starten(Schraube arr, int bestellnummer, string[] kundendaten )
        {          
            new CatiaControl(arr, bestellnummer, kundendaten);       
        }
    }
}
