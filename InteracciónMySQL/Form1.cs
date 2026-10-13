using System.Data;
using MySqlConnector;

namespace InteracciónMySQL
{
    public partial class Form1 : Form
    {
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=inventario_db;User ID=root;Password=ciberdbsql;";

        public Form1() => InitializeComponent();

        private async void Form1_Load(object? sender, EventArgs e) => await RefreshDataAsync();

        private async void refreshButton_Click(object? sender, EventArgs e) => await RefreshDataAsync();

        private async void addPersonalButton_Click(object? sender, EventArgs e)
        {
            var name = nameTextBox.Text.Trim();
            var role = roleTextBox.Text.Trim();
            if (!ValidateText(name, 80, "Nombre") || !ValidateText(role, 80, "Cargo"))
                return;

            try
            {
                await ExecuteAsync(
                    "INSERT INTO `CatPersonal` (`Nombre`, `Cargo`) VALUES (@nombre, @cargo);",
                    ("@nombre", name), ("@cargo", role));
                nameTextBox.Clear();
                roleTextBox.Clear();
                await RefreshDataAsync();
                SetStatus("Personal agregado correctamente.");
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private async void addProductButton_Click(object? sender, EventArgs e)
        {
            var description = descriptionTextBox.Text.Trim();
            if (!ValidateText(description, 40, "Descripción"))
                return;

            try
            {
                await ExecuteAsync(
                    "INSERT INTO `CatProducto` (`Descripcion`) VALUES (@descripcion);",
                    ("@descripcion", description));
                descriptionTextBox.Clear();
                await RefreshDataAsync();
                SetStatus("Producto agregado correctamente.");
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private async void addInventoryButton_Click(object? sender, EventArgs e)
        {
            if (productComboBox.SelectedValue is not int productId || productId <= 0)
            {
                MessageBox.Show("Seleccione un producto existente.", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var quantity = Decimal.ToInt32(quantityNumeric.Value);
            if (quantity < 1)
            {
                MessageBox.Show("La cantidad debe ser mayor que cero.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await ExecuteAsync(
                    "INSERT INTO `tblInventario` (`IDProducto`, `Cantidad`) VALUES (@producto, @cantidad);",
                    ("@producto", productId), ("@cantidad", quantity));
                await RefreshDataAsync();
                SetStatus("Entrada de inventario agregada correctamente.");
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                await connection.OpenAsync();

                personalGrid.DataSource = await ReadTableAsync(connection,
                    "SELECT `ID`, `Nombre`, `Cargo` FROM `CatPersonal` ORDER BY `ID`;");
                var products = await ReadTableAsync(connection,
                    "SELECT `ID`, `Descripcion` FROM `CatProducto` ORDER BY `Descripcion`;");
                productGrid.DataSource = products;
                productComboBox.DataSource = products.Copy();
                productComboBox.DisplayMember = "Descripcion";
                productComboBox.ValueMember = "ID";

                inventoryGrid.DataSource = await ReadTableAsync(connection,
                    "SELECT i.`ID`, i.`FechaAlta`, i.`IDProducto`, p.`Descripcion` AS `Producto`, i.`Cantidad` " +
                    "FROM `tblInventario` i INNER JOIN `CatProducto` p ON p.`ID` = i.`IDProducto` ORDER BY i.`ID`;");
                SetStatus("Conectado a inventario_db.");
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private static async Task<DataTable> ReadTableAsync(MySqlConnection connection, string sql)
        {
            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }

        private static async Task ExecuteAsync(string sql, params (string Name, object Value)[] parameters)
        {
            using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand(sql, connection);
            foreach (var parameter in parameters)
                command.Parameters.AddWithValue(parameter.Name, parameter.Value);
            await command.ExecuteNonQueryAsync();
        }

        private static bool ValidateText(string value, int maxLength, string fieldName)
        {
            if (value.Length == 0)
            {
                MessageBox.Show($"{fieldName} es obligatorio.", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (value.Length > maxLength)
            {
                MessageBox.Show($"{fieldName} no puede superar {maxLength} caracteres.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ShowDatabaseError(Exception exception)
        {
            var message = exception is MySqlException mysqlException
                ? mysqlException.Number switch
            {
                1045 => "No se pudo autenticar con MySQL. Verifique usuario y contraseña.",
                1049 => "La base de datos inventario_db no existe. Ejecute schema_inventario.sql.",
                1146 => "Falta una tabla del esquema. Ejecute schema_inventario.sql.",
                1452 => "El producto seleccionado ya no existe. Actualice los datos e inténtelo de nuevo.",
                _ => exception.Message
            }
                : exception.Message;
            SetStatus("Error de conexión o datos.");
            MessageBox.Show(message, "MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SetStatus(string message) => statusLabel.Text = message;
    }
}