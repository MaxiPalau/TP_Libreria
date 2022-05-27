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
    /// Interaction logic for BorrarProveedor.xaml
    /// </summary>
    public partial class BorrarProveedor : Window
    {
        private string idProveedor;
        private string idEditorial;
        private DataTable dtEditorial = new DataTable("Editorial");

        public BorrarProveedor(string idProveedor)
        {
            InitializeComponent();
            this.idProveedor = idProveedor;
            CargaListaEditoriales();
        }

        private void CargaListaEditoriales()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string select = "SELECT Editorial, Id FROM Editoriales WHERE IdProveedor = @idProveedor";

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

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show("¿Está seguro que desea borrar el proveedor " + textNombre.Text + "?", "¿Borrar proveedor?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                Borrar();
            }

        }

        private void Borrar()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlConnection miConexionSql1 = Conexion.GetConexionSql();
            string delete = "DELETE FROM Proveedores WHERE Id=@IdProveedor";
            try
            {
                foreach (DataRow dr in dtEditorial.Rows)
                {
                    this.idEditorial = dr["id"].ToString();
                    SqlCommand miComandoSql1 = miConexionSql1.CreateCommand();
                    miComandoSql1.CommandType = CommandType.StoredProcedure;
                    miComandoSql1.CommandText = "SP_Editorial_Borrar";
                    miComandoSql1.Parameters.AddWithValue("@idEditorial", idEditorial);
                    miComandoSql1.ExecuteNonQuery();
                    miComandoSql1.Dispose();
                }
 
                SqlCommand miSqlCommand = new SqlCommand(delete, miConexionSql);
                miSqlCommand.Parameters.AddWithValue("@IdProveedor", idProveedor);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);

                using (miAdaptadorSql)
                {
                    int registrosBorrados = miSqlCommand.ExecuteNonQuery();
                    if (registrosBorrados == 1)
                    {
                        var resulta = MessageBox.Show("Proveedor " + textNombre.Text + " borrado.", "Borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (resulta == MessageBoxResult.OK)
                        {
                            botonBorrar.IsEnabled = false;
                            botonCancelar.Content = "Salir";
                            labelVentana.Content = "Proveedor borrado";
                        }
                    }
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            Conexion.Dispose(miConexionSql);
            Conexion.Dispose(miConexionSql1);
        }
    }
}
