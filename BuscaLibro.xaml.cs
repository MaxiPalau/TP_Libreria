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
    /// Interaction logic for BuscaLibro.xaml
    /// </summary>
    public partial class BuscaLibro : Window
    {
       public BuscaLibro()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            try
            {
                string consulta = "SELECT * FROM Libro WHERE Titulo=@titulo";

                SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                using (miAdaptadorSql)
                {
                    miComandoSql.Parameters.AddWithValue("@titulo", textTitulo.Text);

                    DataTable tablaLibros = new DataTable();

                    miAdaptadorSql.Fill(tablaLibros);

                    textTitulo.Text = tablaLibros.Rows[0]["titulo"].ToString();
                    textAutor.Text = tablaLibros.Rows[0]["idautor"].ToString();
                    textIsbn.Text = tablaLibros.Rows[0]["isbn"].ToString();
                    textEditorial.Text = tablaLibros.Rows[0]["ideditorial"].ToString();
                    textEdicion.Text = tablaLibros.Rows[0]["edicion"].ToString();
                    textAnio.Text = tablaLibros.Rows[0]["anio"].ToString();
                    textPaginas.Text = tablaLibros.Rows[0]["paginas"].ToString();
                    textCategoria.Text = tablaLibros.Rows[0]["idcategoria"].ToString();
                    textPrecio.Text = tablaLibros.Rows[0]["precio"].ToString();
                    textStock.Text = tablaLibros.Rows[0]["stock"].ToString();

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }


    }
}
