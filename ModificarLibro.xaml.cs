using ConexionSQL;
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

namespace Libreria
{
    /// <summary>
    /// Interaction logic for ModificarLibro.xaml
    /// </summary>
    public partial class ModificarLibro : Window
    {
        private string IdLibro;
        private bool modificaciones;
        
        public ModificarLibro()
        {
            InitializeComponent();
            ComboboxEditoriales();
            ComboboxCategorias();
        }

        public string IdLibro1 
        { 
            set => IdLibro = value;
        }

        public bool CambioModificaciones
        {
            set => modificaciones = value;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void Modificar()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string update = "UPDATE Libro SET Titulo=@titulo, IdAutor=(SELECT Id from Autores WHERE Autor like @autor)," +
                    "IdEditorial=(SELECT Id from Editoriales WHERE Editorial like @editorial), ISBN=@isbn, Edicion=@edicion, Anio=@Anio," +
                    " Paginas=@paginas, IdCategoria=(SELECT Id from Categorias WHERE Categoria like @categoria), Precio=@precio," +
                    " Stock=@stock WHERE Id=@ID";
            try
            {
                SqlCommand miSqlCommand = new SqlCommand(update, miConexionSql);
                miSqlCommand.Parameters.AddWithValue("@titulo", textTitulo.Text);
                miSqlCommand.Parameters.AddWithValue("@autor", textAutor.Text);
                miSqlCommand.Parameters.AddWithValue("@editorial", textEditorial.Text);
                miSqlCommand.Parameters.AddWithValue("@isbn", textIsbn.Text);
                miSqlCommand.Parameters.AddWithValue("@edicion", textEdicion.Text);
                miSqlCommand.Parameters.AddWithValue("@anio", textAnio.Text);
                miSqlCommand.Parameters.AddWithValue("@paginas", textPaginas.Text);
                miSqlCommand.Parameters.AddWithValue("@categoria", textCategoria.Text);
                miSqlCommand.Parameters.AddWithValue("@precio", textPrecio.Text);
                miSqlCommand.Parameters.AddWithValue("@stock", textStock.Text);
                miSqlCommand.Parameters.AddWithValue("@ID", IdLibro);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);

                using (miAdaptadorSql)
                {
                    try
                    {
                        int registrosModificados = miSqlCommand.ExecuteNonQuery();
                        
                        if (registrosModificados == 1)
                        {
                            var resultado = MessageBox.Show("Libro modificado con éxito.", "Modificado", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (resultado == MessageBoxResult.OK)
                            {
                                this.Close();
                            }
                            modificaciones = false;
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

        private void Modificacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            modificaciones = true;
        }

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

        private void listaCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaCategoria.SelectedItem != null)
            {
                textCategoria.Text = ((DataRowView)listaCategoria.SelectedItem).Row.ItemArray[0].ToString();
            }
            
        }

        private void listaEditorial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaEditorial.SelectedItem != null)
            {
                textEditorial.Text = ((DataRowView)listaEditorial.SelectedItem).Row.ItemArray[0].ToString();
            }

        }
    }
}
