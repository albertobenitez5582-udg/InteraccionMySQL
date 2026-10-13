namespace InteracciónMySQL
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabs;
        private TabPage personalTab;
        private TabPage productTab;
        private TabPage inventoryTab;
        private DataGridView personalGrid;
        private DataGridView productGrid;
        private DataGridView inventoryGrid;
        private TextBox nameTextBox;
        private TextBox roleTextBox;
        private TextBox descriptionTextBox;
        private ComboBox productComboBox;
        private NumericUpDown quantityNumeric;
        private Button addPersonalButton;
        private Button addProductButton;
        private Button addInventoryButton;
        private Button refreshButton;
        private Label statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabs = new TabControl();
            personalTab = new TabPage();
            productTab = new TabPage();
            inventoryTab = new TabPage();
            personalGrid = CreateGrid();
            productGrid = CreateGrid();
            inventoryGrid = CreateGrid();
            nameTextBox = new TextBox { MaxLength = 80, Width = 220 };
            roleTextBox = new TextBox { MaxLength = 80, Width = 220 };
            descriptionTextBox = new TextBox { MaxLength = 40, Width = 220 };
            productComboBox = new ComboBox { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            quantityNumeric = new NumericUpDown { Minimum = 1, Maximum = 2147483647, Value = 1, Width = 120 };
            addPersonalButton = new Button { Text = "Agregar personal", AutoSize = true };
            addProductButton = new Button { Text = "Agregar producto", AutoSize = true };
            addInventoryButton = new Button { Text = "Agregar al inventario", AutoSize = true };
            refreshButton = new Button { Text = "Actualizar datos", AutoSize = true };
            statusLabel = new Label { AutoSize = true, Text = "Conectando...", Padding = new Padding(8, 8, 0, 0) };

            SuspendLayout();
            tabs.Dock = DockStyle.Fill;
            tabs.TabPages.AddRange(new[] { personalTab, productTab, inventoryTab });
            personalTab.Text = "Personal";
            productTab.Text = "Productos";
            inventoryTab.Text = "Inventario";
            ConfigurePersonalTab();
            ConfigureProductTab();
            ConfigureInventoryTab();

            refreshButton.Click += refreshButton_Click;
            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Height = 32;
            Controls.Add(tabs);
            Controls.Add(statusLabel);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 650);
            MinimumSize = new Size(850, 500);
            Text = "Inventario - MySQL";
            StartPosition = FormStartPosition.CenterScreen;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private static DataGridView CreateGrid() => new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false
        };

        private void ConfigurePersonalTab()
        {
            var input = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(8), WrapContents = false };
            input.Controls.Add(new Label { Text = "Nombre:", AutoSize = true, Padding = new Padding(0, 7, 4, 0) });
            input.Controls.Add(nameTextBox);
            input.Controls.Add(new Label { Text = "Cargo:", AutoSize = true, Padding = new Padding(14, 7, 4, 0) });
            input.Controls.Add(roleTextBox);
            input.Controls.Add(addPersonalButton);
            input.Controls.Add(refreshButton);
            addPersonalButton.Click += addPersonalButton_Click;
            personalTab.Controls.Add(personalGrid);
            personalTab.Controls.Add(input);
        }

        private void ConfigureProductTab()
        {
            var input = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(8), WrapContents = false };
            input.Controls.Add(new Label { Text = "Descripción:", AutoSize = true, Padding = new Padding(0, 7, 4, 0) });
            input.Controls.Add(descriptionTextBox);
            input.Controls.Add(addProductButton);
            addProductButton.Click += addProductButton_Click;
            productTab.Controls.Add(productGrid);
            productTab.Controls.Add(input);
        }

        private void ConfigureInventoryTab()
        {
            var input = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(8), WrapContents = false };
            input.Controls.Add(new Label { Text = "Producto:", AutoSize = true, Padding = new Padding(0, 7, 4, 0) });
            input.Controls.Add(productComboBox);
            input.Controls.Add(new Label { Text = "Cantidad:", AutoSize = true, Padding = new Padding(14, 7, 4, 0) });
            input.Controls.Add(quantityNumeric);
            input.Controls.Add(addInventoryButton);
            addInventoryButton.Click += addInventoryButton_Click;
            inventoryTab.Controls.Add(inventoryGrid);
            inventoryTab.Controls.Add(input);
        }
    }
}