using System;
using System.Collections.Generic;
using System.Configuration;
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
using ConexionSQL;

namespace Libreria
{
    /// <summary>
    /// Interaction logic for NuevoLibro.xaml
    /// </summary>
    public partial class NuevoLibro : Window
    {
        public NuevoLibro()
        {
            InitializeComponent();
            ComboboxEditoriales();
            ComboboxCategorias();
            
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string titulo = textTitulo.Text;
            string autor = textAutor.Text;
            string editorial = listaEditorial.Text;
            string isbn = textIsbn.Text;
            string precio = textPrecio.Text;
            string stock = textStock.Text;

            if (titulo == "")
            {
                MessageBox.Show("Por favor ingrese el título.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (autor == "")
            {
                MessageBox.Show("Por favor ingrese el autor.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (isbn == "")
            {
                MessageBox.Show("Por favor ingrese el ISBN.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (editorial == "")
            {
                MessageBox.Show("Por favor ingrese la editorial.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (precio == "")
            {
                MessageBox.Show("Por favor ingrese el precio.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (stock == "")
            {
                MessageBox.Show("Por favor ingrese el stock.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                SqlCommand miComandoSql = miConexionSql.CreateCommand();
                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_NuevoLibro";
                miComandoSql.Parameters.AddWithValue("@titulo", textTitulo.Text);
                miComandoSql.Parameters.AddWithValue("@autor", textAutor.Text);
                miComandoSql.Parameters.AddWithValue("@editorial", listaEditorial.Text);
                miComandoSql.Parameters.AddWithValue("@isbn", textIsbn.Text);
                miComandoSql.Parameters.AddWithValue("@edicion", textEdicion.Text);
                miComandoSql.Parameters.AddWithValue("@anio", textAnio.Text);
                miComandoSql.Parameters.AddWithValue("@paginas", textPaginas.Text);
                miComandoSql.Parameters.AddWithValue("@categoria", listaCategoria.Text);
                miComandoSql.Parameters.AddWithValue("@precio", textPrecio.Text);
                miComandoSql.Parameters.AddWithValue("@stock", textStock.Text);
                miComandoSql.Parameters.Add("@existe", SqlDbType.Int);
                miComandoSql.Parameters["@existe"].Direction = ParameterDirection.Output;
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);      
                {
                    try
                    {
                        int registrosInsertados = miComandoSql.ExecuteNonQuery();
                        int existe = Convert.ToInt32(miComandoSql.Parameters["@existe"].Value);
                        if (existe == 1)
                        {
                            MessageBox.Show("Libro existente, por favor actualice el stock", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            if (registrosInsertados >= 1)
                            {
                                MessageBox.Show("Libro guardado con éxito.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                                textTitulo.Text = "";
                                textAutor.Text = "";
                                listaEditorial.SelectedIndex = -1;
                                textIsbn.Text = "";
                                textEdicion.Text = "";
                                textAnio.Text = "";
                                textPaginas.Text = "";
                                listaCategoria. SelectedIndex = -1;
                                textPrecio.Text = "";
                                textStock.Text = "";
                            }
                        }
                        
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                }
                Conexion.Dispose(miConexionSql);

            }

        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void ComboboxEditoriales()
        {
            string consulta = "SELECT Editorial FROM Editoriales";
            DataSet dsEditoriales = new DataSet();
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
            SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
            try
            {
                miAdaptadorSql.Fill(dsEditoriales);
                listaEditorial.DisplayMemberPath = "Editorial";
                listaEditorial.SelectedValuePath = "Editorial";
                listaEditorial.ItemsSource = dsEditoriales.Tables[0].DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ComboboxCategorias()
        {
            string consulta = "SELECT Categoria FROM Categorias";
            DataSet dsCategorias = new DataSet();
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
            SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
            try
            {
                miAdaptadorSql.Fill(dsCategorias);
                listaCategoria.DisplayMemberPath = "Categoria";
                listaCategoria.SelectedValuePath = "Categoria";
                listaCategoria.ItemsSource = dsCategorias.Tables[0].DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
