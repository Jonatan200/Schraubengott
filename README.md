

# Schraubengott

Das Von Griffendor It-Solutions entwickelte Schraubendimensionierungsprogramm soll einen einfachen Dimensionierungs- und Bestellvorgang verschiedenster Schrauben erm�glichen.

Das vorliegende Programm bietet dabei eine solide Grundlage f�r ein anwenderfreundliches Programm mit einem breiten Spektrum an wichtigen Funktionen, die eine praktische Nutzung m�glich machen sollen.
So sch�tzt Beispielsweise ein mit einer Liste aller Kunden verkn�pftes Einlogfenster for unauthorisierten Zugriffen. Zudem erm�glicht ein E-Mailclient das direkte Versenden einer Bestellung.
Au�erdem erm�glicht das Programm dem Kunden das Sichern der Schrauben in Form einer automatisch gernerierten Excel oder als Catia-Modell inclusive Zeichnungsableitung. 

Da es sich bei dem Programm um ein Prototypen handelt ist der Umfang der Funktionen zurzeit eingeschrenk. Beispielsweise beschrenken sich die konfigurierbaren Schrauben auf Metrische Schrauben mit einem 
Au�en- bzw. Innensechskannt. Au�erdem k�nnen derzeit maximal 5 Schrauben gleichzeitig erstellt werden . Zudem fehlen kundenspeziefische Elemente, wie eine Online-Datenbank , in der die Zugangsdaten zum Einloggen gespeichert werden k�nnen. 



## Aufbau und Nutzung des Programms 

Das Programm ist grundlegend aus drei Ansichten aufgebaut. Nach dem Einloggen �ffnet sich standartm��ig eine Konfigurationsseite, auf der Schrauben durch Ausw�hlen der gew�nschten Parameter erstellt werden k�nnen. 
Sandartm��ig ist die 1. Schraube ausgew�hlt. Um eine weitere Schraube zu Konfigurieren muss diese zun�chst �ber den Button "Neue Schreube erstellen" erzeugt werden. Anschlie�end wird diese Schraube automatisch ausgew�hlt. 
Um die Konvigurationen zu Speichern muss der Button "Auswahl Speichern" bet�tigt werden. Nun werden die Daten der Schrauben auch auf der 2. Seite angezeigt. Um nachtr�glich eine Schraube zu ve�ndern muss die jeweilige Schraube  
oberhalb des Konfigurators ausgew�hlt werden. Anschlie�end ist es wichtig, die �nderungen durch erneutes dr�cken des Knopfes "Auswahl Speichern" zu sichern. 

�ber die oben angezeigten Tabs kann man nun in die �bersicht wechseln, in der alle konfigurierten Schrauben aufgelistet sind. Durch setzen der H�kchen an den gew�nschten Schrauben und bet�tigen des Knopfes " zum Warenkorb" k�nnen die Schrauben 
in den Warenkorb gelegt werden. Auch diese Auswahl kann nachtr�glich ge�ndert werden 

Der dritte Tab stellt den Warenkorb da. Hier werden alle ausgew�hlten Schrauben inklusieve Preis und Gewicht angezeigt. Zudem kann von hier aus eine Excel-�bersicht generiert, das Angebot versendet oder das Catia-Medell erstellt werden. Um ein Catia-Modell 
zu erstellen muss zun�chst die jeweilige schraube ausgew�hlt werden, ehe �ber "Catia Erstellen" der Vorgang abgeschlossen werden kann. 

Um das Programm zu schlie�en kann entweder der Knopf "Exit" oder das Rote Kreuz in der oberen rechten Ecke genutzt werden. !! Achtung: durch Schlie�en des Programms werden alle Konfigurationen gel�scht !! 



## Nutzng des Software 
Um die Software einwandfrei nutzen zu k�nnen m�ssen einige Vorgaben gegeben sein 
	*	Die Nutzung des Emailklients setzt eine bestehende Verbindung zum Internet vorraus 
	*	Um die Catia-Anwendung nutzen zu k�nnen muss Catia V5 (getsetet mit V5-6R2018) interliert und die Funktionen "Hybridkonstruktion erm�glichen" und "Geometrisches Set Erzeugen" aktiviert sein 



## Weiterentwicklung des Programms 
 
Eine umfangreiche Weiterentwicklung des Programms durch Griffendor IT-Solutions ist derzeit nicht Geplant. Da das Programm �ber eine MIT-Lizenz lizensiert ist ist allerdings jedem die Verwendung und weiterentwicklung des Programms gestettet. 
Dabei auftretende m�ngel oder durch das Programm entstehende Sch�den liegen ncht in der Verantwortung von Griffendor IT-Solutions. 

F�r die Catia-Api werden Folgende Catia-Bibleotheken ben�tigt:
	*	Interop.HybridShapeTypeLib
	*	Interop.INFITF
	*	Interop.MECMOD
	*	Interop.PARTITF
	*	Interop.DRAFTING
	*	Interop.ProductStructureTypeLib