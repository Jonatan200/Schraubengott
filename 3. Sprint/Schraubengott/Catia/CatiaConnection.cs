using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INFITF;
using MECMOD;
using PARTITF;
using HybridShapeTypeLib;

namespace Schraubengott
{
    class CatiaConnection
    {
       
        INFITF.Application hsp_catiaApp;
        MECMOD.PartDocument hsp_catiaPartDoc;
        MECMOD.Sketch hsp_catiaSkizze;

        ShapeFactory shapefac;
        HybridShapeFactory hybridshapefac;
        Pad schaft;
        Part part_Schraube;
        Sketches sketches_Schraube;
        Body body_Schraube;

        Sketch skizze_kopf;
        Sketch skizze_tasche;





        #region catiastart 
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
            //Dokument erstellen
            INFITF.Documents catDokuments1 = hsp_catiaApp.Documents;

            //Part erstellen
             hsp_catiaPartDoc =(PartDocument) catDokuments1.Add("Part");
        }
        private void ErzeugeAchsensystem()
        {
            object[] arr = new object[] {0.0, 0.0, 0.0,
                                         0.0, 1.0, 0.0,
                                         0.0, 0.0, 1.0 };
            hsp_catiaSkizze.SetAbsoluteAxisData(arr);
        }
        #endregion


        #region Zylinder 
        internal void SkizzeZylinderErstellen()
        {
            shapefac = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;
            HybridBodies catHybridbodys1 = hsp_catiaPartDoc.Part.HybridBodies;
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
             sketches_Schraube = catHybridBody1.HybridSketches;

            // Ebene festlegen ? 
            OriginElements catoriginElements = hsp_catiaPartDoc.Part.OriginElements;
            Reference catRef1 = (Reference)catoriginElements.PlaneYZ;
            
            //Zkizze in YZ Ebene hizufügen 
             hsp_catiaSkizze = sketches_Schraube.Add(catRef1);

            ErzeugeAchsensystem();

            hsp_catiaPartDoc.Part.Update();
        }


        internal void ZkizzeZylinder(Schraube [] arr)
        {
            part_Schraube = hsp_catiaPartDoc.Part;
            Bodies bodies = part_Schraube.Bodies;
            body_Schraube = part_Schraube.MainBody;

            part_Schraube.InWorkObject = part_Schraube.MainBody;
            
            hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;
            hsp_catiaSkizze.set_Name("Zylinder");
            Factory2D catfactory2D1 = hsp_catiaSkizze.OpenEdition();

            Circle2D Zylinder1 = catfactory2D1.CreateClosedCircle(0, 0, 0.5 * arr[0].durchmesser);

            hsp_catiaSkizze.CloseEdition();
        }

       
        internal void ErzeugeZylinder(Schraube [] arr)
        {

            Reference RefMySchaft = part_Schraube.CreateReferenceFromObject(hsp_catiaSkizze);
            schaft = shapefac.AddNewPadFromRef(RefMySchaft, Convert.ToDouble(arr[0].laenge));
            schaft.set_Name("Zylinder");
            hsp_catiaPartDoc.Part.Update();

            hsp_catiaPartDoc.Part.Update();
        }

        #endregion


        #region Gewinde
        internal void ErzeugeGewindeFeature(Schraube [] arr)
        {
            Reference RefMantelfläche = part_Schraube.CreateReferenceFromBRepName("RSur:(Face:(Brp:(Pad.1;0:(Brp:(Sketch.1;1)));None:();Cf11:());WithTemporaryBody;WithoutBuildError;WithSelectingFeatureSupport;MFBRepVersion_CXR15)", schaft);
            Reference RefFrontfläche = part_Schraube.CreateReferenceFromBRepName("RSur:(Face:(Brp:(Pad.1;2);None:();Cf11:());WithTemporaryBody;WithoutBuildError;WithSelectingFeatureSupport;MFBRepVersion_CXR15)", schaft);

            //Gewinde Erzeugen, Parameter setzen 

            PARTITF.Thread thread1 = shapefac.AddNewThreadWithOutRef();
            thread1.Side = CatThreadSide.catRightSide;
            thread1.Diameter = arr[0].durchmesser;
            thread1.Depth = arr[0].gewindelaenge;
            thread1.Pitch = arr[0].gewindesteigung;
            thread1.LateralFaceElement = RefMantelfläche;
            thread1.LimitFaceElement = RefFrontfläche;


            part_Schraube.Update();
        }

        
        internal void ErzeugeGewindehelix(Schraube[] arr)
        {
            hybridshapefac = (HybridShapeFactory) part_Schraube.HybridShapeFactory;

            Sketch gewinde = Gewindeskizze(arr);

            HybridShapeDirection HelixDir = hybridshapefac.AddNewDirectionByCoord(1, 0, 0);
            Reference RefHelxDir = part_Schraube.CreateReferenceFromObject(HelixDir);

            HybridShapePointCoord Helixstartpunkt = hybridshapefac.AddNewPointCoord(0, 0, 0.5 * arr[0].durchmesser);
            Reference RefHelixstartpunkt = part_Schraube.CreateReferenceFromObject(Helixstartpunkt);

            HybridShapeHelix Helix = hybridshapefac.AddNewHelix(RefHelxDir, false, RefHelixstartpunkt, arr[0].gewindesteigung, arr[0].gewindelaenge, false, 0, 0, false);

            Reference RefHelix = part_Schraube.CreateReferenceFromObject(Helix);
            Reference RefGewinde = part_Schraube.CreateReferenceFromObject(gewinde);

            part_Schraube.Update();

            part_Schraube.InWorkObject = body_Schraube;
// Verundung noch einfügen 


            OriginElements catoriginElements = this.part_Schraube.OriginElements;
            Reference RefPlanezx = (Reference) catoriginElements.PlaneZX;

            Slot GewindeRille = shapefac.AddNewSlotFromRef(RefGewinde, RefHelix);

            Reference RefmyPad = part_Schraube.CreateReferenceFromObject(schaft);
            HybridShapeSurfaceExplicit GewindestangenSurface = hybridshapefac.AddNewSurfaceDatum(RefmyPad);
            Reference RefGewindestangenSurface = part_Schraube.CreateReferenceFromObject(GewindestangenSurface);

            GewindeRille.ReferenceSurfaceElement = RefGewindestangenSurface;

            Reference RefGewindeRille = part_Schraube.CreateReferenceFromObject(GewindeRille);

            part_Schraube.Update();

        }
        
        private Sketch Gewindeskizze(Schraube [] arr)
        {
            OriginElements catoriginElements = part_Schraube.OriginElements;
            Reference RefPlanezx = (Reference)catoriginElements.PlaneZX;

            Sketch Skizze_gewinde = sketches_Schraube.Add(RefPlanezx);
            part_Schraube.InWorkObject = Skizze_gewinde;
            Skizze_gewinde.set_Name("Gewinde");


            //Koordinaten Für gewindezkizze berechnen 
            double zInnen = 0.5 * arr[0].kerndurchmesser + arr[0].gewinderundung - Math.Sin((30 * Math.PI) / 180) * arr[0].gewinderundung;
            double xInnen = Math.Cos((30 * Math.PI) / 180) * arr[0].gewinderundung;


            double zAußen = 0.5 * arr[0].durchmesser;
            double xAußen = 2 * 0.1875 * arr[0].gewindesteigung;
            

            double zRadius =0.5 * (arr[0].kerndurchmesser + arr[0].gewinderundung);
            double xRadius = 0;

            Factory2D catfactory2D2 = Skizze_gewinde.OpenEdition();

            Point2D geweindepunkt1 = catfactory2D2.CreatePoint(zInnen, -xInnen);
            Point2D geweindepunkt2 = catfactory2D2.CreatePoint(zInnen, xInnen);

            Point2D geweindepunkt3 = catfactory2D2.CreatePoint(zAußen, xAußen);
            Point2D geweindepunkt4 = catfactory2D2.CreatePoint(zAußen, -xAußen);

            Point2D geweindepunkt5 = catfactory2D2.CreatePoint(zRadius, xRadius);

            Line2D linieOben = catfactory2D2.CreateLine(zInnen, xInnen, zAußen, xAußen);
            linieOben.StartPoint = geweindepunkt2;
            linieOben.EndPoint = geweindepunkt3;

            Line2D linieAußen = catfactory2D2.CreateLine(zAußen, xAußen, zAußen, -xAußen);
            linieAußen.StartPoint = geweindepunkt3;
            linieAußen.EndPoint = geweindepunkt4;

            Line2D linieUnten = catfactory2D2.CreateLine(zAußen, -xAußen, zInnen, -xInnen);
            linieUnten.StartPoint = geweindepunkt4;
            linieUnten.EndPoint = geweindepunkt1;

            Circle2D gewindeRundung = catfactory2D2.CreateCircle(zRadius, xRadius, arr[0].gewinderundung, 0, 0);
            gewindeRundung.CenterPoint = geweindepunkt5;
            gewindeRundung.EndPoint = geweindepunkt1;
            gewindeRundung.StartPoint = geweindepunkt2;




            Skizze_gewinde.CloseEdition();

            return Skizze_gewinde;

        }

        #endregion


                     

        internal void ErstelleSkizzeKopf(Schraube[] arr)
        {

            part_Schraube.InWorkObject = body_Schraube;

            OriginElements catoriginElements = hsp_catiaPartDoc.Part.OriginElements;
            Reference ref_Kopf = part_Schraube.CreateReferenceFromName("Selection_RSur:(Face:(Brp:(Pad.1;2);None:();Cf11:());Slot.1_ResultOUT;Z0;G8251)");

            skizze_kopf = sketches_Schraube.Add(ref_Kopf);
            skizze_kopf.set_Name("Kopf");
            // Ebene festlegen ? 
            
           hsp_catiaPartDoc.Part.Update();


        }

        internal void ZkizzeKopf(Schraube [] arr)
        {
            part_Schraube.InWorkObject = skizze_kopf;
            skizze_kopf.set_Name("Kopf");
            Factory2D shapefac = skizze_kopf.OpenEdition();

            if (arr[0].typ == "Außensechskant")
            {
                SechseckZeichnen(arr);
            }
            else
            {
                Circle2D Zylinder1 = shapefac.CreateClosedCircle(0, 0, 0.5 * arr[0].kopfdurchmesser);
            }


            skizze_kopf.CloseEdition();
            hsp_catiaPartDoc.Part.Update();
        }


        internal void SechseckZeichnen(Schraube[] arr)
        {


            double schlüsselweite = 0.5 * Convert.ToDouble(arr[0].schluesselbreite);
            double außenkreisSchraubenkopf = schlüsselweite / (Math.Sqrt(3) / 2);

            #region zeichnen 

            Factory2D catfactory2D = skizze_kopf.OpenEdition();

            Point2D point2D1 = catfactory2D.CreatePoint(0, 0);
            Point2D point2D2 = catfactory2D.CreatePoint(0, schlüsselweite);

            Circle2D circle2D1 = catfactory2D.CreateClosedCircle(0, 0, außenkreisSchraubenkopf);
            circle2D1.CenterPoint = point2D1;
            circle2D1.Construction = true;


            Circle2D circle2D2 = catfactory2D.CreateClosedCircle(0, 0, schlüsselweite);
            circle2D2.CenterPoint = point2D1;
            circle2D2.Construction = true;

            Point2D point2D3 = catfactory2D.CreatePoint(0.5 * außenkreisSchraubenkopf, schlüsselweite);
            Point2D point2D4 = catfactory2D.CreatePoint(-0.5 * außenkreisSchraubenkopf, schlüsselweite);
            Point2D point2D5 = catfactory2D.CreatePoint(-1 * außenkreisSchraubenkopf, 0);
            Point2D point2D6 = catfactory2D.CreatePoint(-0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            Point2D point2D7 = catfactory2D.CreatePoint(0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            Point2D point2D8 = catfactory2D.CreatePoint(außenkreisSchraubenkopf, -0);

            Line2D line2D3 = catfactory2D.CreateLine(0.5 * außenkreisSchraubenkopf, schlüsselweite, -0.5 * außenkreisSchraubenkopf, schlüsselweite);
            line2D3.StartPoint = point2D3;
            line2D3.EndPoint = point2D4;

            Line2D line2D4 = catfactory2D.CreateLine(-0.5 * außenkreisSchraubenkopf, schlüsselweite, -1 * außenkreisSchraubenkopf, 0);
            line2D4.StartPoint = point2D4;
            line2D4.EndPoint = point2D5;

            Line2D line2D5 = catfactory2D.CreateLine(-1 * außenkreisSchraubenkopf, 0, -0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            line2D5.StartPoint = point2D5;
            line2D5.EndPoint = point2D6;

            Line2D line2D6 = catfactory2D.CreateLine(-0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite, 0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite);
            line2D6.StartPoint = point2D6;
            line2D6.EndPoint = point2D7;

            Line2D line2D7 = catfactory2D.CreateLine(0.5 * außenkreisSchraubenkopf, -1 * schlüsselweite, außenkreisSchraubenkopf, 0);
            line2D7.StartPoint = point2D7;
            line2D7.EndPoint = point2D8;

            Line2D line2D8 = catfactory2D.CreateLine(außenkreisSchraubenkopf, 0, 0.5 * außenkreisSchraubenkopf, schlüsselweite);
            line2D8.StartPoint = point2D8;
            line2D8.EndPoint = point2D3;

            Line2D line2D9 = catfactory2D.CreateLine(0, 0, 0, schlüsselweite);
            line2D9.Construction = true;
            line2D9.StartPoint = point2D1;
            line2D9.EndPoint = point2D2;
        }


        internal void ErzeugeKopf(Schraube [] arr)
        {
            //double schraubenkopfhöhe = Convert.ToDouble(arr[0].kopfhöhe);
            hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;

            Pad pad_Kopf = shapFac.AddNewPad(skizze_kopf, arr[0].kopfhöhe);
            pad_Kopf.set_Name("Kopf");
            hsp_catiaPartDoc.Part.Update();
        }

        internal void ZkizzeTasche(Schraube [] arr)
        {
            part_Schraube.InWorkObject = body_Schraube;
            Reference ref_tasche = part_Schraube.CreateReferenceFromName("Selection_RSur:(Face:(Brp:((Brp:(Pad.2;1);Brp:(Pad.1;2)));None:();Cf11:());Pad.2_ResultOUT;Z0;G8251)");

            skizze_tasche = sketches_Schraube.Add(ref_tasche);
            skizze_tasche.set_Name("Tasche Innensechskannt");
            // Ebene festlegen ? 

            hsp_catiaPartDoc.Part.Update();


            // Inensechskannt Zeichnen 

            part_Schraube.InWorkObject = skizze_tasche;
            Factory2D shapefac = skizze_tasche.OpenEdition();

            SechseckZeichnen(arr);
            
        }

        internal void TascheErzeugen(Schraube[] arr)
        {
            hsp_catiaPartDoc.Part.InWorkObject = hsp_catiaPartDoc.Part.MainBody;
            ShapeFactory shapFac = (ShapeFactory)hsp_catiaPartDoc.Part.ShapeFactory;

            Pocket pocket_innensechskannt = shapFac.AddNewPocket(skizze_tasche, 0.5 * arr[0].kopfhöhe);
            pocket_innensechskannt.set_Name("Innensechskannt");
            hsp_catiaPartDoc.Part.Update();
        }

        internal void Zeichenableitung()
        {
            Drwa
        }
    }

}
#endregion