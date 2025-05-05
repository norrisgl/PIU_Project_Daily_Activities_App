using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibrarieModele;

namespace InterfataUtilizator_WindowsForms.Forms
{
    public partial class PersonDetailsForm : Form
    {
        private readonly Person _persoana;
        private TextBox _tbDetalii;
        private DataGridView _dgvActivitati;

        public PersonDetailsForm(Person persoana)
        {
            _persoana = persoana;
            InitializeForm();
            InitializeControls();
            LoadData();
        }

        private void InitializeForm()
        {
            this.Text = $"Detalii: {_persoana.Name}";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeControls()
        {
            // 1. Panou pentru detalii
            var panelDetalii = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            _tbDetalii = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 9)
            };
            panelDetalii.Controls.Add(_tbDetalii);

            // 2. Separator
            var separator = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.Silver
            };

            // 3. Panou butoane
            var panelActiuni = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            var btnAdaugaActivitate = new Button
            {
                Text = "Adaugă Activitate Nouă",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGreen,
                Width = 180
            };

            var btnRefresh = new Button
            {
                Text = "Actualizează",
                FlatStyle = FlatStyle.Flat,
                Location = new Point(190, 0),
                Width = 100
            };

            panelActiuni.Controls.AddRange(new Control[] { btnAdaugaActivitate, btnRefresh });

            // 4. Panou activități
            var panelActivitati = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            _dgvActivitati = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            _dgvActivitati.Columns.AddRange(
                new DataGridViewTextBoxColumn { HeaderText = "Nume", FillWeight = 30 },
                new DataGridViewTextBoxColumn { HeaderText = "Data", FillWeight = 20 },
                new DataGridViewTextBoxColumn { HeaderText = "Tip", FillWeight = 30 },
                new DataGridViewTextBoxColumn { HeaderText = "Descriere", FillWeight = 20 }
            );

            panelActivitati.Controls.Add(_dgvActivitati);

            // Evenimente
            btnAdaugaActivitate.Click += BtnAdaugaActivitate_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Buton închidere
            var btnInchide = new Button
            {
                Text = "Închide",
                Dock = DockStyle.Bottom,
                Height = 40,
                DialogResult = DialogResult.OK
            };

            // Adăugare controale
            this.Controls.Add(panelActivitati);
            this.Controls.Add(panelActiuni);
            this.Controls.Add(separator);
            this.Controls.Add(panelDetalii);
            this.Controls.Add(btnInchide);
        }

        private void LoadData()
        {
            _tbDetalii.Text = GenerareTextDetalii(_persoana);
            _dgvActivitati.DataSource = _persoana.ActivityHandler?.Activities?.ToList();
        }

        private void BtnAdaugaActivitate_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddActivityForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK && addForm.NewActivity != null)
                {
                    _persoana.ActivityHandler?.Activities?.Add(addForm.NewActivity);
                    LoadData();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private string GenerareTextDetalii(Person persoana)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"=== DETALII PERSOANĂ ===");
            sb.AppendLine($"Nume: {persoana.Name}");
            sb.AppendLine($"Email: {persoana.Email ?? "-"}");
            sb.AppendLine($"Vârstă: {persoana.Age}");
            sb.AppendLine();

            sb.AppendLine("=== ACTIVITĂȚI ===");
            if (persoana.ActivityHandler?.Activities?.Count > 0)
            {
                foreach (var activitate in persoana.ActivityHandler.Activities.OrderBy(a => a.DateAndTime))
                {
                    sb.AppendLine($"• {activitate.ActivityName} ({activitate.DateAndTime:dd.MM.yyyy HH:mm})");
                    sb.AppendLine($"  Tip: {FormatActivityTypes(activitate.ActType)}");
                    if (!string.IsNullOrEmpty(activitate.Description))
                    {
                        sb.AppendLine($"  Descriere: {activitate.Description}");
                    }
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("Nu există activități înregistrate.");
            }

            return sb.ToString();
        }

        private string FormatActivityTypes(ActivityType types)
        {
            if (types == ActivityType.None)
                return "Niciun tip specificat";

            var selectedTypes = new List<string>();
            foreach (ActivityType type in Enum.GetValues(typeof(ActivityType)))
            {
                if (type != ActivityType.None && types.HasFlag(type))
                {
                    selectedTypes.Add(type.ToString());
                }
            }

            return selectedTypes.Any() ? string.Join(", ", selectedTypes) : "Niciun tip specificat";
        }
    }
}