using System.Windows;
using System.Data;
using Microsoft.Data.SqlClient;
using LAB04;
using System.Text;

namespace LAB04
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-QMNREBJ\\SQLEXPRESS2017; Initial Catalog=Neptuno; Integrated Security=True; TrustServerCertificate=True; Encrypt=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Productos(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Producto> productos = new List<Producto>();
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("ListarProductos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            idproducto = reader.GetInt32(0),
                            nombreProducto = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            idProveedor = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            idCategoria = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            cantidadPorUnidad = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            precioUnidad = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                            unidadesEnExistencia = reader.IsDBNull(6) ? (short)0 : reader.GetInt16(6),
                            unidadesEnPedido = reader.IsDBNull(7) ? (short)0 : reader.GetInt16(7),
                            nivelNuevoPedido = reader.IsDBNull(8) ? (short)0 : reader.GetInt16(8),
                            suspendido = reader.IsDBNull(9) ? false : reader.GetInt16(9) == 1,
                            categoriaProducto = reader.IsDBNull(10) ? "" : reader.GetString(10)
                        });
                    }
                }

                dgProductos.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }


        private void Button_Categorias(object sender, RoutedEventArgs e)
        {
            List<Categoria> categorias = new List<Categoria>();
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("ListarCategorias", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categorias.Add(new Categoria
                    {
                        idcategoria = reader.GetInt32(0),
                        nombrecategoria = reader.GetString(1),
                        descripcion = reader.GetString(2),
                        Activo = reader.GetBoolean(3),
                        CodCategoria = reader.GetString(4)
                    });
                }
            }

            dgCategorias.ItemsSource = categorias;
        }

        private void Button_Buscar_Proveedor(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombreContacto = txtNombreContacto.Text.Trim();
                string ciudad = txtCiudad.Text.Trim();

                List<Proveedor> proveedores = new List<Proveedor>();

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("BuscarProveedores", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombrecontacto", string.IsNullOrEmpty(nombreContacto) ? (object)DBNull.Value : nombreContacto);
                    cmd.Parameters.AddWithValue("@ciudad", string.IsNullOrEmpty(ciudad) ? (object)DBNull.Value : ciudad);

                    conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        proveedores.Add(new Proveedor
                        {
                            idProveedor = reader.GetInt32(0),
                            nombreCompañia = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            nombrecontacto = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            cargocontacto = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            direccion = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            ciudad = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            region = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            codPostal = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            pais = reader.IsDBNull(8) ? "" : reader.GetString(8),
                            telefono = reader.IsDBNull(9) ? "" : reader.GetString(9),
                            fax = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            paginaprincipal = reader.IsDBNull(11) ? "" : reader.GetString(11)
                        });
                    }
                }

                dgProveedores.ItemsSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar proveedores: " + ex.Message);
            }
        }

        private void Button_Buscar_Pedidos_Fecha(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpFechaInicio.SelectedDate == null || dpFechaFin.SelectedDate == null)
                {
                    MessageBox.Show("Por favor, selecciona ambas fechas.");
                    return;
                }

                DateTime fechaInicio = dpFechaInicio.SelectedDate.Value;
                DateTime fechaFin = dpFechaFin.SelectedDate.Value;

                List<Pedido> pedidos = new List<Pedido>();

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("ListarPedidosPorFecha", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    StringBuilder debug = new StringBuilder();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        debug.AppendLine($"{i}: {reader.GetName(i)} - {reader.GetFieldType(i)}");
                    }
                    MessageBox.Show(debug.ToString(), "Columnas del resultado");

                    while (reader.Read())
                    {
                        pedidos.Add(new Pedido
                        {
                            IdPedido = reader.GetInt32(0),
                            IdCliente = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            NombreCliente = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            IdEmpleado = reader.GetInt32(3),
                            FechaPedido = reader.GetDateTime(4),
                            FechaEntrega = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            FechaEnvio = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                            FormaEnvio = reader.GetInt32(7),
                            Cargo = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            Destinatario = reader.IsDBNull(9) ? "" : reader.GetString(9),
                            DireccionDestinatario = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            CiudadDestinatario = reader.IsDBNull(11) ? "" : reader.GetString(11),
                            RegionDestinatario = reader.IsDBNull(12) ? "" : reader.GetString(12),
                            CodPostalDestinatario = reader.IsDBNull(13) ? "" : reader.GetString(13),
                            PaisDestinatario = reader.IsDBNull(14) ? "" : reader.GetString(14)
                        });
                    }
                }

                dgPedidos.ItemsSource = pedidos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar pedidos: " + ex.Message);
            }
        }

    }

}
