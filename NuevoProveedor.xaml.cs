using ConexionSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Libreria
{
    /// <summary>
    /// Interaction logic for NuevoProveedor.xaml
    /// </summary>
    public partial class NuevoProveedor : Window
    {
        DataTable dtEditorial = new DataTable("Editorial");
        public NuevoProveedor()
        {
            InitializeComponent();
            Columnas();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            int idProv;
            SqlConnection miConexionSql = Conexion.GetConexionSql();

            if (textNombre.Text == "")
            {
                MessageBox.Show("Por favor ingrese el nombre del proveedor.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (textTelefono.Text == "")
            {
                MessageBox.Show("Por favor ingrese el teléfono.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (textEmail.Text == "")
            {
                MessageBox.Show("Por favor ingrese la dirección de correo electrónico.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (dtEditorial.Rows.Count == 0)
            {
                MessageBox.Show("Por favor ingrese al menos una editorial.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                /*string insertar = "INSERT INTO Proveedores (Nombre, Razon_Social, Direccion, Codigo_Postal, Telefono, Email) " +
                "VALUES (@nombre, @razonSocial, @direccion,  @codigoPostal, @telefono, @email)";

                SqlCommand miSqlCommand = new SqlCommand(insertar, miConexionSql);
                miSqlCommand.Parameters.AddWithValue("@nombre", textNombre.Text);
                miSqlCommand.Parameters.AddWithValue("@razonSocial", textRazonSocial.Text);
                miSqlCommand.Parameters.AddWithValue("@direccion", textDireccion.Text);
                miSqlCommand.Parameters.AddWithValue("@codigoPostal", textCodPostal.Text);
                miSqlCommand.Parameters.AddWithValue("@telefono", textTelefono.Text);
                miSqlCommand.Parameters.AddWithValue("@email", textEmail.Text);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);*/
                SqlCommand miComandoSql = miConexionSql.CreateCommand();

                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_NuevoProveedor";
                miComandoSql.Parameters.AddWithValue("@nombre", textNombre.Text);
                miComandoSql.Parameters.AddWithValue("@razonSocial", textRazonSocial.Text);
                miComandoSql.Parameters.AddWithValue("@direccion", textDireccion.Text);
                miComandoSql.Parameters.AddWithValue("@codigoPostal", textCodPostal.Text);
                miComandoSql.Parameters.AddWithValue("@telefono", textTelefono.Text);
                miComandoSql.Parameters.AddWithValue("@email", textEmail.Text);
                miComandoSql.Parameters.Add("@idProveedor", SqlDbType.BigInt);
                miComandoSql.Parameters["@idProveedor"].Direction = ParameterDirection.Output;

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                try
                {
                    string editorialCarga ="";
                    int registrosInsertados = miComandoSql.ExecuteNonQuery();
                    idProv = Convert.ToInt32(miComandoSql.Parameters["@idProveedor"].Value);
                 
                    if (registrosInsertados == 1)
                    {
                        foreach (DataRow dr in dtEditorial.Rows)
                        {
                            SqlCommand miComandoSqlEditorial = miConexionSql.CreateCommand();
                            miComandoSqlEditorial.CommandType = CommandType.StoredProcedure;
                            miComandoSqlEditorial.CommandText = "SP_Editorial_Alta";
                            miComandoSqlEditorial.Parameters.AddWithValue("@idProveedor", idProv);
                            editorialCarga = dr["Editorial"].ToString();
                            miComandoSqlEditorial.Parameters.AddWithValue("@Editorial", editorialCarga);
                            miComandoSqlEditorial.ExecuteNonQuery();
                            miComandoSqlEditorial.Dispose();
                        }
                        MessageBox.Show("Proveedor guardado con éxito.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                        textNombre.Text = "";
                        textRazonSocial.Text = "";
                        textDireccion.Text = "";
                        textCodPostal.Text = "";
                        textTelefono.Text = "";
                        textEmail.Text = "";
                        textEditorial.Text = "";
                        listaEditoriales.DataContext = null;
                    }
                    else
                    {
                       MessageBox.Show("No se guardó el proveedor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception e1)
                {
                   MessageBox.Show(e1.ToString());
                }
                Conexion.Dispose(miConexionSql);
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
             Close();

        private void Columnas()
        {
            dtEditorial.Columns.Add("Editorial", typeof(String));
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (textEditorial.Text == "")
            {
                MessageBox.Show("Por favor ingrese el nombre de la editorial.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                string editorial = textEditorial.Text;
                if (dtEditorial.Rows.Count == 0)
                {
                    dtEditorial.Rows.Add(new Object[] { editorial });

                    listaEditoriales.DataContext = dtEditorial.DefaultView;
                }
                else
                {
                    bool repetido = false;
                    foreach (DataRow dr in dtEditorial.Rows)
                    {
                        if (dr["Editorial"].ToString() == editorial)
                        {
                            MessageBox.Show("Editorial repetida, verifique el nombre y vuelva a ingresarlo.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            repetido = true;
                        }
                    }
                    if (repetido == false)
                    {
                        dtEditorial.Rows.Add(new Object[] { editorial });
                    }
                    listaEditoriales.DataContext = dtEditorial.DefaultView;
                }
            }

        }
    }
}
