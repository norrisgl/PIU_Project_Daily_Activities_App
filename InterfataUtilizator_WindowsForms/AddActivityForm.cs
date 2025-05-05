using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using LibrarieModele;
using System.Drawing;

namespace InterfataUtilizator_WindowsForms
{
    public partial class AddActivityForm : Form
    {
        public Activity NewActivity { get; private set; }

        public AddActivityForm()
        {
            InitializeComponent();
            InitializeForm();
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
            var dtpData = new DateTimePicker { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5) };

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

                NewActivity = new Activity
                {
                    ActivityName = txtNume.Text,
                    Description = txtDescriere.Text,
                    DateAndTime = dtpData.Value,
                    ActType = selectedTypes
                };

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