using ConexionSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for BorrarLibro.xaml
    /// </summary>
    public partial class BorrarLibro : Window
    {
        private string IdLibro;

        public BorrarLibro()
        {
            InitializeComponent();
        }

        public string IdLibro1
        {
            set => IdLibro = value;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) => 
            Close();

        private void Borrar()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            string delete = "DELETE FROM Libro WHERE Id=@IdLibro";
            try
            {
                SqlCommand miSqlCommand = new SqlCommand(delete, miConexionSql);
                miSqlCommand.Parameters.AddWithValue("@IdLibro", IdLibro);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);

                using (miAdaptadorSql)
                {
                    try
                    {
                        int registrosBorrados = miSqlCommand.ExecuteNonQuery();
                        if (registrosBorrados == 1)
                        {
                            var result1 = MessageBox.Show("Libro " + textTitulo.Text + " borrado.", "Borrado", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (result1 == MessageBoxResult.OK)
                            {
                                botonBorrar.IsEnabled = false;
                                botonCancelar.Content = "Salir";
                                labelVentana.Content = "Libro borrado";
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString(), "Error");
                    }
                }
            }
            catch(Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show("¿Está seguro que desea borrar el libro?", "¿Borrar libro?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (resultado == MessageBoxResult.Yes)
            {
                Borrar();
            }
            
        }
    }
}
