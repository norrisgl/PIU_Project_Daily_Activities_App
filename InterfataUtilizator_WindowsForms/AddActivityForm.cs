using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using LibrarieModele;
using System.Drawing;
using NivelStocareDate;


namespace InterfataUtilizator_WindowsForms
{
    public partial class AddActivityForm : Form
    {
        public Activity NewActivity { get; private set; }
        private readonly Person _persoanaAsociata;
        public AddActivityForm(Person persoana)
        {
            InitializeComponent();
            InitializeForm();
            _persoanaAsociata = persoana;
        }

        private void InitializeForm()
        {
            // Setări de bază pentru formular
            this.Text = "Adaugă Activitate Nouă";
            this.Size = new Size(450, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Inițializează controalele
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Layout principal
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(15),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Controale pentru formular
            var lblNume = new Label { Text = "Nume activitate:", AutoSize = true, Anchor = AnchorStyles.Left };
            var txtNume = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5) };

            var lblDescriere = new Label { Text = "Descriere:", AutoSize = true, Anchor = AnchorStyles.Left };
            var txtDescriere = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 80, Margin = new Padding(0, 5, 0, 5) };

            var lblData = new Label { Text = "Data:", AutoSize = true, Anchor = AnchorStyles.Left };
            var dtpData = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy HH:mm", 
                ShowUpDown = true 
            };

            var lblTip = new Label { Text = "Tip activitate:", AutoSize = true, Anchor = AnchorStyles.Left };
            var flpTipuri = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true
            };

            // Adaugă checkbox-uri pentru fiecare tip de activitate
            foreach (ActivityType type in Enum.GetValues(typeof(ActivityType)))
            {
                if (type != ActivityType.None)
                {
                    var cb = new CheckBox
                    {
                        Text = type.ToString(),
                        Tag = type,
                        Margin = new Padding(0, 2, 10, 2),
                        AutoSize = true
                    };
                    flpTipuri.Controls.Add(cb);
                }
            }

            var lblPrioritate = new Label { Text = "Prioritate:", AutoSize = true, Anchor = AnchorStyles.Left };

            // Crează un FlowLayoutPanel sau alt container pentru RadioButton-uri
            var flpPrioritati = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Padding = new Padding(0, 2, 5, 2)
            };

            // Adaugă un RadioButton pentru fiecare nivel de prioritate
            foreach (PriorityLevel priority in Enum.GetValues(typeof(PriorityLevel)))
            {
                var rb = new RadioButton
                {
                    Text = priority.ToString(),
                    Tag = priority,
                    Margin = new Padding(0, 2, 5, 2),
                    AutoSize = true
                };

                // Setează RadioButton "Medium" ca selectat implicit
                if (priority == PriorityLevel.Medium) 
                {
                    rb.Checked = true;
                }

                flpPrioritati.Controls.Add(rb);
            }


            // Butoane
            var btnAdauga = new Button { Text = "Adaugă", DialogResult = DialogResult.OK };
            var btnAnuleaza = new Button { Text = "Anulează", DialogResult = DialogResult.Cancel };

            // Adaugă controale în tabel
            mainTable.Controls.Add(lblNume, 0, 0);
            mainTable.Controls.Add(txtNume, 1, 0);
            mainTable.Controls.Add(lblDescriere, 0, 1);
            mainTable.Controls.Add(txtDescriere, 1, 1);
            mainTable.Controls.Add(lblData, 0, 2);
            mainTable.Controls.Add(dtpData, 1, 2);
            mainTable.Controls.Add(lblTip, 0, 3);
            mainTable.Controls.Add(flpTipuri, 1, 3);
            mainTable.Controls.Add(lblPrioritate, 2, 3);
            mainTable.Controls.Add(flpPrioritati, 3, 3);

            // Panel pentru butoane
            var pnlButoane = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(0, 10, 0, 0) };
            pnlButoane.Controls.Add(btnAdauga);
            pnlButoane.Controls.Add(btnAnuleaza);

            // Pozitionare butoane
            btnAdauga.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAnuleaza.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAnuleaza.Location = new Point(btnAdauga.Left - btnAnuleaza.Width - 10, btnAdauga.Top);

            // Evenimente
            btnAdauga.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNume.Text))
                {
                    MessageBox.Show("Introduceți numele activității!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Construiește tipurile selectate
                ActivityType selectedTypes = ActivityType.None;
                foreach (CheckBox cb in flpTipuri.Controls)
                {
                    if (cb.Checked)
                    {
                        selectedTypes |= (ActivityType)cb.Tag;
                    }
                }

                // Creează noua activitate
                var newActivity = new Activity
                {
                    ActivityName = txtNume.Text,
                    Description = txtDescriere.Text,
                    DateAndTime = dtpData.Value,
                    ActType = selectedTypes
                };

                // 1. Obține persoana curentă (presupunând că o ai disponibilă)
                Person persoanaCurenta = _persoanaAsociata;

                // 2. Adaugă activitatea la persoană
                persoanaCurenta.ActivityHandler?.Activities?.Add(newActivity);

                // 3. Actualizează fișierul
                try
                {
                    // Presupunând că ai o clasă FileHandler cu metode pentru gestionarea fișierului
                    List<Person> persoane = FileHandler.ReadFromFile();

                    // Găsește și actualizează persoana în listă
                    var index = persoane.FindIndex(p => p.PersonID == persoanaCurenta.PersonID);
                    if (index != -1)
                    {
                        persoane[index] = persoanaCurenta;
                    }

                    // Salvează lista actualizată în fișier
                    FileHandler.WriteToFile("C:/Users/Noris/source/repos/ProiectPIU/DailyActivities_PIU/DailyActivities_PIU/bin/Debug/Persoane.txt", persoane);

                    MessageBox.Show("Activitate adăugată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvarea activității: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            // Adaugă toate controalele la formular
            this.Controls.Add(mainTable);
            this.Controls.Add(pnlButoane);
            this.AcceptButton = btnAdauga;
            this.CancelButton = btnAnuleaza;
        }

        private void AddActivityForm_Load(object sender, EventArgs e)
        {

        }
    }
}