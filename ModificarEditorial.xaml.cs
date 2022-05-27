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
    /// Interaction logic for ModificarEditorial.xaml
    /// </summary>
    public partial class ModificarEditorial : Window
    {
        private string idProveedor;
        private string editorial;
        private string idEditorial;
        DataTable dtEditorial = new DataTable("Editorial");

        public ModificarEditorial(string idProveedor)
        {
            InitializeComponent();
            this.idProveedor = idProveedor;
            CargaListaEditorial();
        }

        private void CargaListaEditorial()
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

        private void Refresh()
        {
            dtEditorial.Clear();
            listaEditoriales.DataContext = null;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            
            if (textEditorial.Text == "")
            {
                MessageBox.Show("Por favor ingrese el nombre de la editorial.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                SqlConnection miConexionSql = Conexion.GetConexionSql();
                SqlCommand miComandoSql = miConexionSql.CreateCommand();
                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_Editorial_Alta";
                
                this.editorial = textEditorial.Text;
                if (dtEditorial.Rows.Count == 0)
                {
                    try
                    {
                        miComandoSql.Parameters.AddWithValue("@idProveedor", idProveedor);
                        miComandoSql.Parameters.AddWithValue("@Editorial", this.editorial);
                        miComandoSql.ExecuteNonQuery();
                        miComandoSql.Dispose();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }

                    Refresh();
                    CargaListaEditorial();
                    textEditorial.Text = "";
                }
                else
                {
                    bool repetido = false;
                    foreach (DataRow dr in dtEditorial.Rows)
                    {
                        if (dr["Editorial"].ToString() == this.editorial)
                        {
                            MessageBox.Show("Editorial repetida, verifique el nombre y vuelva a ingresarlo.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            repetido = true;
                        }
                    }
                    if (repetido == false)
                    {
                        miComandoSql.Parameters.AddWithValue("@idProveedor", idProveedor);
                        miComandoSql.Parameters.AddWithValue("@Editorial", this.editorial);
                        miComandoSql.ExecuteNonQuery();
                        miComandoSql.Dispose();
                        textEditorial.Text = "";
                    }
                    Refresh();
                    CargaListaEditorial();
                }
                botonAgregar.IsEnabled = false;
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView drv in listaEditoriales.SelectedItems)
            {
                this.idEditorial = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
            }
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = miConexionSql.CreateCommand();
            miComandoSql.CommandType = CommandType.StoredProcedure;
            miComandoSql.CommandText = "SP_Editorial_Borrar";

            try
            {
                miComandoSql.Parameters.AddWithValue("@idEditorial", idEditorial);
                miComandoSql.ExecuteNonQuery();
                miComandoSql.Dispose();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }


            listaEditoriales.DataContext = dtEditorial.DefaultView;
            listaEditoriales.SelectedItems.Clear();
            textEditorialModificar.Text = "";
            botonModificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;
            textEditorial.IsEnabled = true;
            Refresh();
            CargaListaEditorial();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (textEditorialModificar.Text == this.editorial)
            {
                MessageBox.Show("Debe modificar el nombre de la editorial.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                bool repetido = false;
                foreach (DataRow dr in dtEditorial.Rows)
                {
                    if (dr["Editorial"].ToString() == textEditorialModificar.Text)
                    {
                        MessageBox.Show("Editorial repetida, verifique el nombre y vuelva a ingresarlo.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        repetido = true;
                    }
                }
                if (repetido == false)
                {
                    foreach (DataRow dr in dtEditorial.Rows)
                    {
                        if (dr["Editorial"].ToString() == this.editorial)
                        {
                            this.idEditorial = dr["Id"].ToString();
                        }
                    }

                    SqlConnection miConexionSql = Conexion.GetConexionSql();
                    SqlCommand miComandoSql = miConexionSql.CreateCommand();
                    miComandoSql.CommandType = CommandType.StoredProcedure;
                    miComandoSql.CommandText = "SP_Editorial_Modificar";

                    try
                    {
                        miComandoSql.Parameters.AddWithValue("@id", idEditorial);
                        miComandoSql.Parameters.AddWithValue("@Editorial", textEditorialModificar.Text);
                        miComandoSql.ExecuteNonQuery();
                        miComandoSql.Dispose();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }

                    listaEditoriales.SelectedItems.Clear();
                    textEditorialModificar.Text = "";
                    botonBorrar.IsEnabled = false;
                    botonModificar.IsEnabled = false;
                    Refresh();
                    CargaListaEditorial();
                    textEditorial.IsEnabled = true;
                }             
            }          
        }

        private void listaEditoriales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            botonBorrar.IsEnabled = true;
            botonModificar.IsEnabled = true;
            foreach (DataRowView drv in listaEditoriales.SelectedItems)
            {
                this.editorial = drv.Row[0] != null ? drv.Row[0].ToString() : String.Empty;
            }
            textEditorialModificar.Text = this.editorial;
            textEditorial.IsEnabled = false;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {

        }

        // Boton cerrar
        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void textEditorial_TextChanged(object sender, TextChangedEventArgs e)
        {
            botonAgregar.IsEnabled = true;
        }
    }
}
