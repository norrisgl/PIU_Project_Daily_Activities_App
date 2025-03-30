using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LibrarieModele;
using NivelStocareDate;

namespace InterfataUtilizator_WindowsForms
{
    public partial class ActivityTracker : Form
    {
        private const int MARGINE_STANGA = 50;
        private const int LATIME_CONTROL = 150;
        private const int DIMENSIUNE_PAS_Y = 30;
        private const int DIMENSIUNE_PAS_X = 200;

        private Label lblTitlu;
        private Label lblNume;
        private Label lblVarsta;
        private Label lblEmail;
        private Label lblActivitati;

        public ActivityTracker()
        {
            InitializeComponent();
            ConfigureazaFormular();
            IncarcaSiAfiseazaPersoane();
        }

        private void ConfigureazaFormular()
        {
            // Setări de bază pentru formular
            this.Text = "Activity Tracker";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 9, FontStyle.Regular);
            this.ForeColor = Color.DarkSlateBlue;
        }

        private void IncarcaSiAfiseazaPersoane()
        {
            List<Person> persoane = IncarcaPersoaneDinFisier();

            // Adaugă controalele de antet
            AdaugaAntet();

            // Adaugă datele persoanelor
            if (persoane.Count > 0)
            {
                AdaugaDatePersoane(persoane);
            }
            else
            {
                AdaugaMesajListaGoala();
            }
        }

        private List<Person> IncarcaPersoaneDinFisier()
        {
            try
            {
                string caleFisier = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persoane.txt");
                return FileHandler.ReadFromFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Person>();
            }
        }

        private void AdaugaAntet()
        {
            // Titlu principal
            lblTitlu = new Label();
            lblTitlu.Text = "Activity Tracker";
            lblTitlu.Location = new Point(MARGINE_STANGA, 20);
            lblTitlu.AutoSize = true;
            lblTitlu.Font = new Font("Arial", 14, FontStyle.Bold);
            this.Controls.Add(lblTitlu);

            // Antet coloane
            lblNume = new Label();
            lblNume.Text = "Nume";
            lblNume.Location = new Point(MARGINE_STANGA, 60);
            lblNume.Width = LATIME_CONTROL;
            lblNume.Font = new Font(lblNume.Font, FontStyle.Bold);
            this.Controls.Add(lblNume);

            lblVarsta = new Label();
            lblVarsta.Text = "Vârstă";
            lblVarsta.Location = new Point(MARGINE_STANGA + DIMENSIUNE_PAS_X, 60);
            lblVarsta.Width = LATIME_CONTROL / 2;
            lblVarsta.Font = new Font(lblVarsta.Font, FontStyle.Bold);
            this.Controls.Add(lblVarsta);

            lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Location = new Point(MARGINE_STANGA + 2 * DIMENSIUNE_PAS_X, 60);
            lblEmail.Width = LATIME_CONTROL + 40;
            lblEmail.Font = new Font(lblEmail.Font, FontStyle.Bold);
            this.Controls.Add(lblEmail);

            lblActivitati = new Label();
            lblActivitati.Text = "Activități";
            lblActivitati.Location = new Point(MARGINE_STANGA + 3 * DIMENSIUNE_PAS_X, 60);
            lblActivitati.Width = LATIME_CONTROL;
            lblActivitati.Font = new Font(lblActivitati.Font, FontStyle.Bold);
            this.Controls.Add(lblActivitati);
        }

        private void AdaugaDatePersoane(List<Person> persoane)
        {
            int topStart = 90;

            for (int i = 0; i < persoane.Count; i++)
            {
                // Nume
                var lblNumePers = new Label();
                lblNumePers.Text = persoane[i].Name;
                lblNumePers.Location = new Point(MARGINE_STANGA, topStart + i * DIMENSIUNE_PAS_Y);
                lblNumePers.Width = LATIME_CONTROL;
                lblNumePers.Tag = persoane[i]; // Stocheaza obiectul persoană pentru evenimentul de click
                lblNumePers.Cursor = Cursors.Hand;
                lblNumePers.Click += (s, e) => AfiseazaDetaliiPersoana((Person)((Label)s).Tag);
                this.Controls.Add(lblNumePers);

                // Vârstă
                var lblVarstaPers = new Label();
                lblVarstaPers.Text = persoane[i].Age.ToString();
                lblVarstaPers.Location = new Point(MARGINE_STANGA + DIMENSIUNE_PAS_X, topStart + i * DIMENSIUNE_PAS_Y);
                lblVarstaPers.Width = LATIME_CONTROL / 2;
                this.Controls.Add(lblVarstaPers);

                // Email
                var lblEmailPers = new Label();
                lblEmailPers.Text = persoane[i].Email;
                lblEmailPers.Location = new Point(MARGINE_STANGA + 2 * DIMENSIUNE_PAS_X, topStart + i * DIMENSIUNE_PAS_Y);
                lblEmailPers.Width = LATIME_CONTROL + 40;
                this.Controls.Add(lblEmailPers);

                // Număr activități
                var lblActivitatiPers = new Label();
                int nrActivitati = persoane[i].ActivityHandler?.Activities?.Count ?? 0;
                lblActivitatiPers.Text = nrActivitati.ToString();
                lblActivitatiPers.Location = new Point(MARGINE_STANGA + 3 * DIMENSIUNE_PAS_X, topStart + i * DIMENSIUNE_PAS_Y);
                lblActivitatiPers.Width = LATIME_CONTROL / 2;

                // Colorare diferită în funcție de numărul de activități
                if (nrActivitati > 3)
                    lblActivitatiPers.ForeColor = Color.DarkGreen;
                else if (nrActivitati == 0)
                    lblActivitatiPers.ForeColor = Color.Red;

                this.Controls.Add(lblActivitatiPers);
            }

            // Ajustează dimensiunea formularului în funcție de numărul de persoane
            this.Height = Math.Min(700, topStart + persoane.Count * DIMENSIUNE_PAS_Y + 50);
        }

        private void AdaugaMesajListaGoala()
        {
            var lblMesaj = new Label();
            lblMesaj.Text = "Nu există persoane în fișier sau fișierul nu a fost găsit";
            lblMesaj.Location = new Point(MARGINE_STANGA, 90);
            lblMesaj.AutoSize = true;
            lblMesaj.ForeColor = Color.Red;
            this.Controls.Add(lblMesaj);
        }

        private void AfiseazaDetaliiPersoana(Person persoana)
        {
            var detaliiForm = new Form();
            detaliiForm.Text = $"Detalii: {persoana.Name}";
            detaliiForm.Size = new Size(500, 400);
            detaliiForm.StartPosition = FormStartPosition.CenterParent;

            var tbDetalii = new TextBox();
            tbDetalii.Multiline = true;
            tbDetalii.Dock = DockStyle.Fill;
            tbDetalii.ScrollBars = ScrollBars.Vertical;
            tbDetalii.ReadOnly = true;
            tbDetalii.Font = new Font("Arial", 10);
            tbDetalii.Text = GenerareTextDetalii(persoana);

            detaliiForm.Controls.Add(tbDetalii);
            detaliiForm.ShowDialog();
        }

        private string GenerareTextDetalii(Person persoana)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Nume: {persoana.Name}");
            sb.AppendLine($"Vârstă: {persoana.Age}");
            sb.AppendLine($"Email: {persoana.Email}");
            sb.AppendLine("\nActivități:");

            if (persoana.ActivityHandler?.Activities != null && persoana.ActivityHandler.Activities.Count > 0)
            {
                foreach (var activitate in persoana.ActivityHandler.Activities)
                {
                    sb.AppendLine($"\n• {activitate.ActivityName}");
                    sb.AppendLine($"  Data: {activitate.DateAndTime}");
                    sb.AppendLine($"  Prioritate: {activitate.Priority}");
                    sb.AppendLine($"  Tip: {activitate.ActType}");
                    sb.AppendLine($"  Descriere: {activitate.Description}");
                }
            }
            else
            {
                sb.AppendLine("Nu există activități înregistrate");
            }

            return sb.ToString();
        }

        private void ActivityTracker_Load(object sender, EventArgs e)
        {
            // Evenimentul Load poate fi folosit pentru operații suplimentare
        }
    }
}