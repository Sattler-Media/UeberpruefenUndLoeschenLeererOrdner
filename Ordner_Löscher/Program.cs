using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class Program : Form
{
    private ProgressBar progressBar;
    private Label statusLabel;
    private Button startButton;
    private int processedItems; // Deklaration der Variable auf Klassenebene

    public Program()
    {
        this.Text = "Ordner-Löschen Fortschritt:";
        this.Size = new System.Drawing.Size(400, 200);

        progressBar = new ProgressBar();
        progressBar.Location = new System.Drawing.Point(50, 50);
        progressBar.Size = new System.Drawing.Size(300, 30);
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        this.Controls.Add(progressBar);

        statusLabel = new Label();
        statusLabel.Location = new System.Drawing.Point(50, 100);
        statusLabel.Size = new System.Drawing.Size(300, 30);
        statusLabel.Text = "Zum Starten bitte drücken.";
        this.Controls.Add(statusLabel);

        startButton = new Button();
        startButton.Text = "Start";
        startButton.Location = new System.Drawing.Point(150, 140);
        startButton.Click += StartButton_Click;
        this.Controls.Add(startButton);
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        string hauptOrdner = @"C:\Users\adrian.maldonado\Documents\C#\Bla";

        if (Directory.Exists(hauptOrdner))
        {
            processedItems = 0; // Zähler vor dem Start des Prozesses zurücksetzen
            Thread processThread = new Thread(() => UeberpruefenUndLoeschenLeererOrdner(hauptOrdner));
            processThread.Start();
        }
        else
        {
            MessageBox.Show($"Der Ordner {hauptOrdner} existiert nicht.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UeberpruefenUndLoeschenLeererOrdner(string verzeichnis)
    {
        try
        {
            string[] unterordner = Directory.GetDirectories(verzeichnis);
            int totalItems = unterordner.Length + 1; // Gesamtanzahl der zu verarbeitenden Ordner

            foreach (string unterordnerPfad in unterordner)
            {
                UeberpruefenUndLoeschenLeererOrdner(unterordnerPfad);
                processedItems++;
                UpdateProgress(processedItems, totalItems);
            }

            string[] dateien = Directory.GetFiles(verzeichnis);
            string[] unterordnerInnen = Directory.GetDirectories(verzeichnis);

            if (dateien.Length == 0 && unterordnerInnen.Length == 0)
            {
                Directory.Delete(verzeichnis);
                UpdateStatus($"Ordner gelöscht: {verzeichnis}");
            }
            else
            {
                UpdateStatus($"Ordner bearbeitet: {verzeichnis}");
            }

            processedItems++;
            UpdateProgress(processedItems, totalItems);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Fehler: {ex.Message}");
        }
    }

    private void UpdateProgress(int processed, int total)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateProgress(processed, total)));
        }
        else
        {
            progressBar.Value = Math.Min((int)((double)processed / total * 100), 100); // Sicherstellen, dass der Wert 100 nicht überschreitet
        }
    }

    private void UpdateStatus(string message)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateStatus(message)));
        }
        else
        {
            statusLabel.Text = message;
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new Program());
    }
}