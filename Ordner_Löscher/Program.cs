using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Pfad des Hauptverzeichnisses, das überprüft werden soll
        string hauptOrdner = @"C:\Users\adrian.maldonado\Documents\C#\Bla";  // Passe diesen Pfad an dein System an

        // Aufruf der Funktion, um leere Ordner zu überprüfen und zu löschen
        UeberpruefenUndLoeschenLeererOrdner(hauptOrdner);
    }

    static void UeberpruefenUndLoeschenLeererOrdner(string verzeichnis)
    {
        // Alle Unterordner im angegebenen Verzeichnis abrufen
        string[] unterordner = Directory.GetDirectories(verzeichnis);

        foreach (string unterordnerPfad in unterordner)
        {
            try
            {
                // Rekursiv alle Unterordner im aktuellen Unterordner überprüfen
                UeberpruefenUndLoeschenLeererOrdner(unterordnerPfad);

                // Alle Dateien im aktuellen Unterordner abrufen
                string[] dateien = Directory.GetFiles(unterordnerPfad);

                // Überprüfen, ob der Unterordner weder Dateien noch weitere Unterordner enthält
                string[] unterordnerInnen = Directory.GetDirectories(unterordnerPfad);

                if (dateien.Length == 0 && unterordnerInnen.Length == 0)
                {
                    // Leeren Ordner löschen
                    Directory.Delete(unterordnerPfad);
                    Console.WriteLine($"Leerer Ordner gelöscht: {unterordnerPfad}");
                }
                else
                {
                    // Wenn der Ordner Dateien enthält, diese anzeigen
                    if (dateien.Length > 0)
                    {
                        Console.WriteLine($"Der Ordner {unterordnerPfad} enthält die folgenden Dateien:");
                        foreach (string datei in dateien)
                        {
                            Console.WriteLine($"- {datei}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung (z. B. bei Berechtigungsproblemen)
                Console.WriteLine($"Fehler beim Zugreifen oder Löschen des Ordners {unterordnerPfad}: {ex.Message}");
            }
        }
    }
}
