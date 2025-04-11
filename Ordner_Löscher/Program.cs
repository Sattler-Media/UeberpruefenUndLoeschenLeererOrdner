using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Pfad des Hauptverzeichnisses, das überprüft werden soll
        string hauptOrdner = @"C:\Users\adrian.maldonado\Documents\C#\Bla";  // Passe diesen Pfad an dein System an

        if (Directory.Exists(hauptOrdner))
        {
            // Aufruf der Funktion, um leere Ordner zu überprüfen und zu löschen
            UeberpruefenUndLoeschenLeererOrdner(hauptOrdner);
        }
        else
        {
            Console.WriteLine($"Das Verzeichnis {hauptOrdner} existiert nicht.");
        }
    }

    static void UeberpruefenUndLoeschenLeererOrdner(string verzeichnis)
    {
        try
        {
            // Alle Unterordner im angegebenen Verzeichnis abrufen
            string[] unterordner = Directory.GetDirectories(verzeichnis);

            foreach (string unterordnerPfad in unterordner)
            {
                // Rekursiv alle Unterordner im aktuellen Unterordner überprüfen
                UeberpruefenUndLoeschenLeererOrdner(unterordnerPfad);
            }

            // Alle Dateien im aktuellen Verzeichnis abrufen
            string[] dateien = Directory.GetFiles(verzeichnis);
            string[] unterordnerInnen = Directory.GetDirectories(verzeichnis);

            if (dateien.Length == 0 && unterordnerInnen.Length == 0)
            {
                // Leeren Ordner löschen
                Directory.Delete(verzeichnis);
                Console.WriteLine($"Leerer Ordner gelöscht: {verzeichnis}");
            }
            else
            {
                // Dateien im Ordner anzeigen
                ZeigeDateienImOrdner(verzeichnis, dateien);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Zugriff verweigert auf {verzeichnis}: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"E/A-Fehler bei {verzeichnis}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler bei {verzeichnis}: {ex.Message}");
        }
    }

    static void ZeigeDateienImOrdner(string ordnerPfad, string[] dateien)
    {
        if (dateien.Length > 0)
        {
            Console.WriteLine($"Der Ordner {ordnerPfad} enthält die folgenden Dateien:");
            foreach (string datei in dateien)
            {
                Console.WriteLine($"- {datei}");
            }
        }
    }
}