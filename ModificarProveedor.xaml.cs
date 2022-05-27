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
    /// Interaction logic for ModificarProveedor.xaml
    /// </summary>
    public partial class ModificarProveedor : Window
    {
        private string idProveedor;
        private bool modificaciones;
        private DataTable dtEditorial = new DataTable("Editorial");
   
        public ModificarProveedor(string idProveedor)
        {
            InitializeComponent();
            this.idProveedor = idProveedor;
            CargaListaEditoriales();
        }

        public bool CambioModificaciones
        {
            set => modificaciones = value;
        }

        private void CargaListaEditoriales()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string select = "SELECT Editorial FROM Editoriales WHERE IdProveedor = @idProveedor";

            try
            {
                SqlCommand miComandoSql = new SqlCommand(select, miConexionSql);
                miComandoSql.Parameters.AddWithValue("@idProveedor", idProveedor);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                try
                {
                    miAdaptadorSql.Fill(dtEditorial);
                    if (dtEditorial.Rows.Count > 0)
                    {
                        //Enlazo la tabla con el gridview
                        listaEditoriales.DataContext = dtEditorial.DefaultView;
                        
                    }
                    else
                    {
                        listaEditoriales.DataContext = null;
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    miAdaptadorSql.Dispose();
                    miComandoSql.Dispose();
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
           Close();

        private void Modificacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            modificaciones = true;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (modificaciones == true)
            {
                Modificar();
            }
            else
            {
                MessageBox.Show("Debe modificar algun valor", "Modifique un valor", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Modificar()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string update = "UPDATE Proveedores SET Nombre=@nombre, Razon_Social=@razonSocial, Direccion=@direccion, Codigo_Postal=@codPostal, " +
                "Telefono=@telefono, Email=@email WHERE Id=@IdProveedor";

            try
            {
                SqlCommand miSqlCommand = new SqlCommand(update, miConexionSql);
                miSqlCommand.Parameters.AddWithValue("@nombre", textNombre.Text);
                miSqlCommand.Parameters.AddWithValue("@razonSocial", textRazonSocial.Text);
                miSqlCommand.Parameters.AddWithValue("@direccion", textDireccion.Text);
                miSqlCommand.Parameters.AddWithValue("@codPostal", textCodPostal.Text);
                miSqlCommand.Parameters.AddWithValue("@telefono", textTelefono.Text);
                miSqlCommand.Parameters.AddWithValue("@email", textEmail.Text);
                miSqlCommand.Parameters.AddWithValue("@IdProveedor", idProveedor);
                
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);

                using (miAdaptadorSql)
                {
                    try
                    {
                       int registrosModificados = miSqlCommand.ExecuteNonQuery();
                        if (registrosModificados == 1)
                        {
                            MessageBox.Show("Proveedor modificado con éxito.", "Modificado", MessageBoxButton.OK, MessageBoxImage.Information);
                            modificaciones = false;
                            botonCerrar.Content = "Cerrar";
                        }
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            modificaciones = true;
            ModificarEditorial ventanaModificarEditorial = new ModificarEditorial(idProveedor);
            ventanaModificarEditorial.Show();
            ventanaModificarEditorial.Closed += (o, args) => 
            {
                Refresh();
            };
        }

        private void Refresh()
        {
            dtEditorial.Clear();
            listaEditoriales.DataContext = null;
            CargaListaEditoriales();
        }
    }
}
