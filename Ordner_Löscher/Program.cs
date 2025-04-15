using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class Program : Form
{
    private ProgressBar progressBar;
    private Label statusLabel;
    private Button startButton;
    private int processedItems; // Declaración de la variable a nivel de clase

    public Program()
    {
        this.Text = "Ordner-Löschen Progress:";
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
        statusLabel.Text = "Zum beginnen bitte drücken.";
        this.Controls.Add(statusLabel);

        startButton = new Button();
        startButton.Text = "Beginn";
        startButton.Location = new System.Drawing.Point(150, 140);
        startButton.Click += StartButton_Click;
        this.Controls.Add(startButton);
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        string hauptOrdner = @"C:\Users\adrian.maldonado\Documents\C#\Bla";

        if (Directory.Exists(hauptOrdner))
        {
            processedItems = 0; // Reiniciar el contador antes de iniciar el proceso
            Thread processThread = new Thread(() => UeberpruefenUndLoeschenLeererOrdner(hauptOrdner));
            processThread.Start();
        }
        else
        {
            MessageBox.Show($"Der Ordner {hauptOrdner} Existiert nicht.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UeberpruefenUndLoeschenLeererOrdner(string verzeichnis)
    {
        try
        {
            string[] unterordner = Directory.GetDirectories(verzeichnis);
            int totalItems = unterordner.Length + 1; // Total de carpetas a procesar

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
                UpdateStatus($"Folder gelöscht: {verzeichnis}");
            }
            else
            {
                UpdateStatus($"Folder bearbeitet: {verzeichnis}");
            }

            processedItems++;
            UpdateProgress(processedItems, totalItems);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
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
            progressBar.Value = Math.Min((int)((double)processed / total * 100), 100); // Asegurar que no exceda 100
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