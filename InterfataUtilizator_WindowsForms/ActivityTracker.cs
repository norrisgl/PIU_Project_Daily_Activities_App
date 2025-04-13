using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private const int MAX_NAME_LENGTH = 15;
        private const int MAX_AGE = 120;

        private Label[,] lblPersoane; // Tablou bidimensional pentru afișare
        private TextBox txtNume;
        private TextBox txtVarsta;
        private TextBox txtEmail;
        private TextBox txtCautare;
        private Label lblError;
        private Person ultimaPersoanaAdaugata;

        public ActivityTracker()
        {
            
            InitializeComponent();
            ConfigureazaFormular();
            InitializeazaControale();
            IncarcaSiAfiseazaPersoane();
        }


        private void ConfigureazaFormular()
        {
            this.Text = "Activity Tracker";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Arial", 9, FontStyle.Regular);
            this.ForeColor = Color.DarkSlateBlue;
        }

        private void    InitializeazaControale()
        {
            // Adaugă controale pentru introducere date
            var lblNumeInput = new Label { Text = "Nume:", Location = new Point(MARGINE_STANGA, 20) };
            txtNume = new TextBox { Location = new Point(MARGINE_STANGA + 100, 20), Width = LATIME_CONTROL };

            var lblVarstaInput = new Label { Text = "Vârstă:", Location = new Point(MARGINE_STANGA, 50) };
            txtVarsta = new TextBox { Location = new Point(MARGINE_STANGA + 100, 50), Width = LATIME_CONTROL / 2 };

            var lblEmailInput = new Label { Text = "Email:", Location = new Point(MARGINE_STANGA, 80) };
            txtEmail = new TextBox { Location = new Point(MARGINE_STANGA + 100, 80), Width = LATIME_CONTROL + 40 };

            var btnAdauga = new Button
            {
                Text = "Adaugă",
                Location = new Point(MARGINE_STANGA, 110),
                BackColor = Color.LightGreen
            };
            btnAdauga.Click += BtnAdauga_Click;

            var btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(MARGINE_STANGA + 100, 110),
                BackColor = Color.LightBlue
            };
            btnRefresh.Click += BtnRefresh_Click;

            lblError = new Label
            {
                ForeColor = Color.Red,
                Location = new Point(MARGINE_STANGA, 140),
                AutoSize = true
            };

            // Controale pentru căutare
            var lblCautare = new Label
            {
                Text = "Căutare:",
                Location = new Point(MARGINE_STANGA + 400, 20)
            };

            txtCautare = new TextBox
            {
                Location = new Point(MARGINE_STANGA + 500, 20),
                Width = LATIME_CONTROL,
                Margin = new Padding(3, 3, 3, 3), // Spațiere uniforma
                Font = new Font("Arial", 9),
                TextAlign = HorizontalAlignment.Left // Aliniere la stanga
            };

            var btnCautare = new Button
            {
                Text = "Caută",
                Location = new Point(MARGINE_STANGA + 650, 20),
                BackColor = Color.LightYellow
            };
            btnCautare.Click += BtnCautare_Click;

            // Adaugă controalele la formular
            this.Controls.AddRange(new Control[] { lblCautare, txtCautare, btnCautare });

            // Adaugă controalele la formular
            this.Controls.AddRange(new Control[] { lblNumeInput, txtNume, lblVarstaInput, txtVarsta,
                                                 lblEmailInput, txtEmail, btnAdauga, btnRefresh, lblError });
        }

        private void BtnAdauga_Click(object sender, EventArgs e)
        {
            if (ValideazaDatele())
            {
                var person = new Person(
                    PersID: 0, // ID-ul poate fi generat sau incrementat
                    nume: txtNume.Text,
                    varsta: int.Parse(txtVarsta.Text),
                    email: txtEmail.Text
                );

                // Adaugă persoana în fișier
                FileHandler.AppendToFile(person);
                ultimaPersoanaAdaugata = person;
                lblError.Text = "Persoană adăuga    tă cu succes!";
                lblError.ForeColor = Color.Green;

                // Șterge conținutul TextBox-urilor
                txtNume.Text = string.Empty;
                txtVarsta.Text = string.Empty;
                txtEmail.Text = string.Empty;

                // Reîncarcă și afișează lista actualizată de persoane
                IncarcaSiAfiseazaPersoane();
            }
        }

        private void BtnCautare_Click(object sender, EventArgs e)
        {
            string termenCautare = txtCautare.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(termenCautare))
            {
                IncarcaSiAfiseazaPersoane();
                return;
            }

            List<Person> persoane = IncarcaPersoaneDinFisier();
            List<Person> rezultate = persoane.Where(p =>
                p.Name.ToLower().Contains(termenCautare) ||
                p.Email.ToLower().Contains(termenCautare) ||
                p.Age.ToString().Contains(termenCautare) ||
                (p.ActivityHandler?.Activities?.Any(a => a.ActivityName.ToLower().Contains(termenCautare)) == true)).ToList();

            AfiseazaPersoane(rezultate);
            HighlightRezultateCautare(termenCautare);
        }

        private void HighlightRezultateCautare(string termenCautare)
        {
            if (lblPersoane == null) return;

            foreach (Label label in lblPersoane)
            {
                if (label.Text.ToLower().Contains(termenCautare.ToLower()))
                {
                    label.BackColor = Color.Yellow;
                    label.Font = new Font(label.Font, FontStyle.Bold);
                }
                else
                {
                    label.BackColor = SystemColors.Control;
                    label.Font = new Font(label.Font, FontStyle.Regular);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (ultimaPersoanaAdaugata != null)
            {
                txtNume.Text = ultimaPersoanaAdaugata.Name;
                txtVarsta.Text = ultimaPersoanaAdaugata.Age.ToString();
                txtEmail.Text = ultimaPersoanaAdaugata.Email;
                txtCautare.Text = string.Empty;
            }
            IncarcaSiAfiseazaPersoane();
        }

        private bool ValideazaDatele()
        {
            bool isValid = true;
            lblError.Text = "";

            // Validare nume
            if (string.IsNullOrWhiteSpace(txtNume.Text))
            {
                lblError.Text += "Numele este obligatoriu. ";
                txtNume.BackColor = Color.LightPink;
                isValid = false;
            }
            else if (txtNume.Text.Length > MAX_NAME_LENGTH)
            {
                lblError.Text += $"Numele nu poate depăși {MAX_NAME_LENGTH} caractere. ";
                txtNume.BackColor = Color.LightPink;
                isValid = false;
            }
            else
            {
                txtNume.BackColor = Color.White;
            }

            // Validare vârstă
            if (!int.TryParse(txtVarsta.Text, out int varsta) || varsta <= 0 || varsta > MAX_AGE)
            {
                lblError.Text += $"Vârsta trebuie să fie între 1 și {MAX_AGE}. ";
                txtVarsta.BackColor = Color.LightPink;
                isValid = false;
            }
            else
            {
                txtVarsta.BackColor = Color.White;
            }

            // Validare email
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                lblError.Text += "Email invalid. ";
                txtEmail.BackColor = Color.LightPink;
                isValid = false;
            }
            else
            {
                txtEmail.BackColor = Color.White;
            }

            return isValid;
        }

        private void IncarcaSiAfiseazaPersoane()
        {
            List<Person> persoane = IncarcaPersoaneDinFisier();
            AfiseazaPersoane(persoane);
        }

        private List<Person> IncarcaPersoaneDinFisier()
        {
            try
            {
                return FileHandler.ReadFromFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor: {ex.Message}", "Eroare",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Person>();
            }
        }

        private void AfiseazaPersoane(List<Person> persoane)
        {
            // Șterge controalele vechi de afișare
            if (lblPersoane != null)
            {
                foreach (var label in lblPersoane)
                {
                    this.Controls.Remove(label);
                }
            }

            if (persoane.Count == 0)
            {
                var lblMesaj = new Label
                {
                    Text = "Nu există persoane în fișier",
                    Location = new Point(MARGINE_STANGA, 180),
                    ForeColor = Color.Red
                };
                this.Controls.Add(lblMesaj);
                return;
            }

            // Inițializează tabloul bidimensional
            int rows = persoane.Count;
            int cols = 4; // Nume, Vârstă, Email, Nr. Activități
            lblPersoane = new Label[rows, cols];

            int topStart = 180;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    lblPersoane[i, j] = new Label();
                    lblPersoane[i, j].Location = new Point(
                        MARGINE_STANGA + j * DIMENSIUNE_PAS_X,
                        topStart + i * DIMENSIUNE_PAS_Y
                    );
                    lblPersoane[i, j].Width = j == 1 ? LATIME_CONTROL / 2 : LATIME_CONTROL;

                    // Completează datele
                    switch (j)
                    {
                        case 0: // Nume
                            lblPersoane[i, j].Text = persoane[i].Name;
                            lblPersoane[i, j].Tag = persoane[i];
                            lblPersoane[i, j].Cursor = Cursors.Hand;
                            lblPersoane[i, j].Click += (s, e) =>
                                AfiseazaDetaliiPersoana((Person)((Label)s).Tag);
                            break;
                        case 1: // Vârstă
                            lblPersoane[i, j].Text = persoane[i].Age.ToString();
                            break;
                        case 2: // Email
                            lblPersoane[i, j].Text = persoane[i].Email;
                            break;
                        case 3: // Nr. Activități
                            int nrActivitati = persoane[i].ActivityHandler?.Activities?.Count ?? 0;
                            lblPersoane[i, j].Text = nrActivitati.ToString();
                            lblPersoane[i, j].ForeColor = nrActivitati == 0 ? Color.Red :
                                                         (nrActivitati > 3 ? Color.DarkGreen : Color.Black);
                            break;
                    }

                    this.Controls.Add(lblPersoane[i, j]);
                }
            }

            // Ajustează dimensiunea formularului
            this.Height = Math.Min(Screen.PrimaryScreen.WorkingArea.Height,
                                 topStart + rows * DIMENSIUNE_PAS_Y + 50);
        }

        private void AfiseazaDetaliiPersoana(Person persoana)
        {
            var detaliiForm = new Form
            {
                Text = $"Detalii: {persoana.Name}",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var tbDetalii = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Arial", 10),
                Text = GenerareTextDetalii(persoana)
            };

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
    }
}