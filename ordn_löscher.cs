using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Pfad des Verzeichnisses, das überprüft werden soll
        string hauptOrdner = @"C:\pfad\zu\deinem\verzeichnis";  // Ändere diesen Pfad entsprechend deinem System

        // Aufruf der Funktion, um leere Ordner zu löschen
        LeereOrdnerEntfernen(hauptOrdner);
    }

    static void LeereOrdnerEntfernen(string verzeichnis)
    {
        // Alle Unterordner im angegebenen Verzeichnis abrufen
        string[] ordner = Directory.GetDirectories(verzeichnis);

        foreach (string ordnerPfad in ordner)
        {
            try
            {
                // Alle Dateien und Unterordner im aktuellen Ordner abrufen
                string[] dateienUndUnterordner = Directory.GetFileSystemEntries(ordnerPfad);

                // Wenn der Ordner keine Dateien oder Unterordner enthält
                if (dateienUndUnterordner.Length == 0)
                {
                    // Leeren Ordner löschen
                    Directory.Delete(ordnerPfad);
                    Console.WriteLine($"Leerer Ordner gelöscht: {ordnerPfad}");
                }
                else
                {
                    // Wenn der Ordner Dateien oder Unterordner enthält
                    Console.WriteLine($"Ordner mit Dateien: {ordnerPfad}");
                }
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung (z. B. bei Berechtigungsproblemen)
                Console.WriteLine($"Fehler beim Zugriff auf den Ordner {ordnerPfad}: {ex.Message}");
            }
        }
    }
}
