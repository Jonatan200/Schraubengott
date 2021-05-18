﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INFITF;
using MECMOD;
using PARTITF;

namespace Schraubengott
{
    class CatiaConnection
    {
       
        INFITF.Application hsp_catiaApp;
        MECMOD.PartDocument hsp_catiaPart;
        Sketch hsp_catiaProfil;


        public bool CATIALaeuft()
        {
            try
            {
                object catiaObject = System.Runtime.InteropServices.Marshal.GetActiveObject(
                    "CATIA.Application");
                hsp_catiaApp = (INFITF.Application)catiaObject;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        internal void ErzeugePart()
        {
            Documents catDokument = hsp_catiaApp.Documents;

            //Datei erstellen
             hsp_catiaPart =(PartDocument) catDokument.Add("Part");
        }


        internal void LeereSkizzeErzeugen()
        {
            HybridBodies catHybridbodys1 = hsp_catiaPart.Part.HybridBodies;
            HybridBody catHybridBody1;
            try
            {
                catHybridBody1 = catHybridbodys1.Item("Geometrisches Set.1");
            }
            catch (Exception)
            {
               /* MessageBox.Show("Kein Geometrisches Set gefunden", "Fehler");MessageBoxButton.OK, MessageBoxImage.Error);
                */return;  
            }
            //Geometrisches Set Benennen 
            catHybridBody1.set_Name("Schraube Außensechskannt");

            //Skizze erstellen 
            Sketches catSketches = catHybridBody1.HybridSketches;

            // Ebene festlegen ? 
            OriginElements catoriginElements = hsp_catiaPart.Part.OriginElements;
            Reference catRef = (Reference) catoriginElements.PlaneYZ;
            
            //Zkizze in YZ Ebene hizufügen 
             hsp_catiaProfil = catSketches.Add(catRef);

            ErzeugeAchsensystem();

            hsp_catiaPart.Part.Update();
        }

        private void ErzeugeAchsensystem()
        {
            object[] arr = new object[] {0.0, 0.0, 0.0,
                                         0.0, 1.0, 0.0,
                                         0.0, 0.0, 1.0 };
            hsp_catiaProfil.SetAbsoluteAxisData(arr);
        }

        internal void ErzeugeProfiel()
        {
            hsp_catiaProfil.set_Name("Rechteck");
            Factory2D catfactory2D = hsp_catiaProfil.OpenEdition();

            Point2D catpoint2D1 = catfactory2D.CreatePoint(-50, 50);
            Point2D catpoint2D2 = catfactory2D.CreatePoint(50, 50);
            Point2D catpoint2D3 = catfactory2D.CreatePoint(50, -50);
            Point2D catpoint2D4 = catfactory2D.CreatePoint(-50, -50);

            Line2D catline2D1 = catfactory2D.CreateLine(-50, 50, 50, 50);
            catline2D1.StartPoint = catpoint2D1;
            catline2D1.EndPoint = catpoint2D2;

            Line2D catline2D2 = catfactory2D.CreateLine(50, 50, 50, -50);
            catline2D2.StartPoint = catpoint2D2;
            catline2D2.EndPoint = catpoint2D3;

            Line2D catline2D3 = catfactory2D.CreateLine(50, -50, -50, -50);
            catline2D3.StartPoint = catpoint2D3;
            catline2D3.EndPoint = catpoint2D4;

            Line2D catline2D4 = catfactory2D.CreateLine(-50, -50, -50, 50);
            catline2D4.StartPoint = catpoint2D4;
            catline2D4.EndPoint = catpoint2D1;


            hsp_catiaProfil.CloseEdition();
            hsp_catiaPart.Part.Update();
        }


        internal void ErzeugeBalken()
        {
            hsp_catiaPart.Part.InWorkObject = hsp_catiaPart.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory) hsp_catiaPart.Part.ShapeFactory;

            Pad newPad = shapFac.AddNewPad(hsp_catiaProfil,100);
            newPad.set_Name("Balken");
            hsp_catiaPart.Part.Update();
        }
        // 18:19 Minuten 








        /*
           public Boolean ErzeugePart()
           {
               INFITF.Documents catDocuments1 = hsp_catiaApp.Documents;
               hsp_catiaPart = catDocuments1.Add("Part") as MECMOD.PartDocument;
               return true;
           }

           public void ErstelleLeereSkizze()
           {
               // geometrisches Set auswaehlen und umbenennen
               HybridBodies catHybridBodies1 = hsp_catiaPart.Part.HybridBodies;
               HybridBody catHybridBody1;
               try
               {
                   catHybridBody1 = catHybridBodies1.Item("Geometrisches Set.1");
               }
               catch (Exception)
               {
                   MessageBox.Show("Kein geometrisches Set gefunden! " + Environment.NewLine +
                       "Ein PART manuell erzeugen und ein darauf achten, dass 'Geometisches Set' aktiviert ist.",
                       "Fehler", MessageBoxButton.OK, MessageBoxImage.Information);
                   return;
               }
               catHybridBody1.set_Name("Profile");
               // neue Skizze im ausgewaehlten geometrischen Set anlegen
               Sketches catSketches1 = catHybridBody1.HybridSketches;
               OriginElements catOriginElements = hsp_catiaPart.Part.OriginElements;
               Reference catReference1 = (Reference)catOriginElements.PlaneYZ;
               hsp_catiaProfil = catSketches1.Add(catReference1);

               // Achsensystem in Skizze erstellen 
               ErzeugeAchsensystem();

               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }

           private void ErzeugeAchsensystem()
           {
               object[] arr = new object[] {0.0, 0.0, 0.0,
                                        0.0, 1.0, 0.0,
                                        0.0, 0.0, 1.0 };
               hsp_catiaProfil.SetAbsoluteAxisData(arr);
           }

           public void ErzeugeProfil(Double b, Double h)
           {
               // Skizze umbenennen
               hsp_catiaProfil.set_Name("Rechteck");

               // Rechteck in Skizze einzeichnen
               // Skizze oeffnen
               Factory2D catFactory2D1 = hsp_catiaProfil.OpenEdition();

               // Rechteck erzeugen

               // erst die Punkte
               Point2D catPoint2D1 = catFactory2D1.CreatePoint(-50, 50);
               Point2D catPoint2D2 = catFactory2D1.CreatePoint(50, 50);
               Point2D catPoint2D3 = catFactory2D1.CreatePoint(50, -50);
               Point2D catPoint2D4 = catFactory2D1.CreatePoint(-50, -50);

               // dann die Linien
               Line2D catLine2D1 = catFactory2D1.CreateLine(-50, 50, 50, 50);
               catLine2D1.StartPoint = catPoint2D1;
               catLine2D1.EndPoint = catPoint2D2;

               Line2D catLine2D2 = catFactory2D1.CreateLine(50, 50, 50, -50);
               catLine2D2.StartPoint = catPoint2D2;
               catLine2D2.EndPoint = catPoint2D3;

               Line2D catLine2D3 = catFactory2D1.CreateLine(50, -50, -50, -50);
               catLine2D3.StartPoint = catPoint2D3;
               catLine2D3.EndPoint = catPoint2D4;

               Line2D catLine2D4 = catFactory2D1.CreateLine(-50, -50, -50, 50);
               catLine2D4.StartPoint = catPoint2D4;
               catLine2D4.EndPoint = catPoint2D1;

               // Skizzierer verlassen
               hsp_catiaProfil.CloseEdition();
               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }

           public void ErzeugeBalken(Double l)
           {
               // Hauptkoerper in Bearbeitung definieren
               hsp_catiaPart.Part.InWorkObject = hsp_catiaPart.Part.MainBody;

               // Block(Balken) erzeugen
               ShapeFactory catShapeFactory1 = (ShapeFactory)hsp_catiaPart.Part.ShapeFactory;
               Pad catPad1 = catShapeFactory1.AddNewPad(hsp_catiaProfil, l);

               // Block umbenennen
               catPad1.set_Name("Balken");

               // Part aktualisieren
               hsp_catiaPart.Part.Update();
           }
       */
    }

}