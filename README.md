

# Schraubengott

Das Von Griffendor It-Solutions Entwickelte Schraubendimensionierungsprogramm soll einen einfachen Dimensionierungs- und Bestellvorgang verschiedenster Schrauben ermöglichen.

Das Vorliegende Programm bietet dabei eine soliede Grundlage für ein anwenderfreundliches Program mit einem breiten Spektrum an wichtigen Funktionen, die eine Praktische nutzung möglich machen sollen.
So schützt Beispielsweise ein mit einer Liste aller Kunden Ferknüpftes Einlogfenster for unauthorisierten Zugriffen. Zudem ermöglicht ein E-Mailclient das direkte Versenden einer Bestellung.
Zudem ermöglicht das Programm dem Kunden das Sichern der Schrauben in form einer Automatisch gernerierten Excel oder als Catia-Modell inclusive Zeichnungsableitung. 

Da es sich bei dem Programm um ein Prototypen handelt ist der Umfang der Funktionen zurzeit noch eingeschrenk. Beispielsweise beschrenken sich die konfigurierbaren Schrauben auf Metrische Schreuben mit einem 
Außen- bzw. Innensechskannt. Außerdem können derzeit maximal 5 Schrauben gleichzeitig erstellt werden könen. Außerdem fehlen Kundenspeziefische Elemente, wie eine Online-Datenbank , in der die Zugangsdaten zum Einloggen gespeichert werden können. 



## Aufbau und Nutzung des Programms 

Das Programm ist Grundlegend aus drei Ansichten aufgebaut. NAch dem Einloggen öffnet sich standartmäßig eine Konfigurationsseite, auf der Schrauben durch Auswählen der gewünschten Parameter erstellt werden können. 
Sandartmäßig ist die 1. Schraube ausgewählt. Um eine weitere Schraube zu Konfigurieren muss diese zunächst über den Button "Neue Schreube erstellen" erzeugt werden. Anschließend wird diese Schraube automatisch ausgewählt. 
Um die Konvigurationen zu Speichern muss der Button "Auswahl Speichern" betätigt werden. Nun werden die Daten der Schrauben auch auf der 2. Seite Angezeigt. Um nachträglich eine Schraube zu veändern muss die jeweilige Schraube in der 
Oberhalb der Konfiguration ausgewählt werden. Anschließend ist es wichtig, die Änderungen durch erneutes drücken des Knopfes "Auswahl Speichern" zu sichern. 

Über die oben angezeigten Tabs kann man nun in die Übersicht wechseln, in der alle Konfigurierten schrauben aufgelistet sind. Durch setzen der Häkchen an den Gewünschten Schrauben und betätigen des Knopfes " zum Warenkorb" können die Schrauben 
in den Warenkorb gelegt werden. Auch diese Auswahl kann nachträglich geändert werden 

Der dritte Tab stellt den Warenkorb da. Hier werden alle ausgewählten Schrauben inklusieve Preis und gewicht angezeigt. Zudem kann von hier aus eine Exel-Übersicht generiert, das Angebot versendet oder das Catia-Medell erstellt werden. Um ein Catia-Modell 
zu erstellen muss zunächst die jeweilige schraube ausgewählt werden, ehe über "Catia Erstellen" der Vorgang abgeschlossen werden kann. 

Um das Programm zu schließen kann entweder der Knopf "Exit" oder das Rote Kreuz in der oberen rechten Ecke genutzt werden. !! Achtung: durch Schließen des Programms werden alle Konfigurationen gelöscht !! 



## Nutzng des Software 
Um die Software einwandfrei nutzen zu können müssen einige Vorgaben gegeben sein 
	*	Die Nutzung des Emailklients setzt eine bestehende Verbindung zum Internet vorraus 
	*	Um die Catia-Anwendung nutzen zu können muss Catia V5 (getsetet mit V5-6R2018) interliert und die Funktionen "Hybridkonstruktion ermöglichen" und "Geometrisches Set Erzeugen" aktiviert sein 



## Weiterentwicklung des Programms 
 
Eine umfangreiche Weiterentwicklung des Programms durch Griffendor IT-Solutions ist derzeit nicht Geplant. Da das Programm über eine MIT-Lizenz lizensiert ist ist allerdings jedem die Verwendung und weiterentwicklung des Programms gestettet. 
Dabei auftretende mängel oder durch das Programm entstehende Schäden liegen ncht in der Verantwortung von Griffendor IT-Solutions. 

Für die Catia-Api werden Folgende Catia-Bibleotheken benötigt:
	*	Interop.HybridShapeTypeLib
	*	Interop.INFITF
	*	Interop.MECMOD
	*	Interop.PARTITF
	*	Interop.DRAFTING
	*	Interop.ProductStructureTypeLib