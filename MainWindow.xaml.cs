using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libreria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            BuscaLibro1 ventanaBuscaLibro = new BuscaLibro1();

            ventanaBuscaLibro.Show();
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            NuevoLibro ventanaNuevoLibro = new NuevoLibro();

            ventanaNuevoLibro.Show();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            BuscaLibro1 ventanaBuscaModifica = new BuscaLibro1();

            ventanaBuscaModifica.botonModificar.Visibility = Visibility.Visible;
            ventanaBuscaModifica.Titulo.Content = "Busque el libro a modificar";
            ventanaBuscaModifica.Show();
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            BuscaLibro1 ventanaBuscaBorra = new BuscaLibro1();

            ventanaBuscaBorra.botonBorrar.Visibility = Visibility.Visible;
            ventanaBuscaBorra.Titulo.Content = "Busque el libro a eliminar";
            ventanaBuscaBorra.Show();
        }

        private void BuscarProveedor_Click(object sender, RoutedEventArgs e)
        {
            BuscarProveedor ventanaBuscarProveedor = new BuscarProveedor();
            ventanaBuscarProveedor.Show();
        }

        private void NuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            NuevoProveedor ventanaNuevoProveedor = new NuevoProveedor();
            ventanaNuevoProveedor.Show();
        }

        private void ModificarProveedor_Click(object sender, RoutedEventArgs e)
        {
            BuscarProveedor ventanaBuscaModificaProveedor = new BuscarProveedor();
            ventanaBuscaModificaProveedor.botonModificar.Visibility = Visibility.Visible;
            ventanaBuscaModificaProveedor.botonEditoriales.Visibility = Visibility.Hidden;
            ventanaBuscaModificaProveedor.Titulo.Content = "Busque el proveedor a modificar";
            ventanaBuscaModificaProveedor.Title = "Modificar proveedor";
            ventanaBuscaModificaProveedor.Show();
        }

        private void BorrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            BuscarProveedor ventanaBorrarProveedor = new BuscarProveedor();
            ventanaBorrarProveedor.botonBorrar.Visibility = Visibility.Visible;
            ventanaBorrarProveedor.botonEditoriales.Visibility = Visibility.Hidden;
            ventanaBorrarProveedor.Titulo.Content = "Busque el proveedor a borrar";
            ventanaBorrarProveedor.Title = "Borrar proveedor";
            ventanaBorrarProveedor.Show();
        }

        private void Pedidos_Click(object sender, RoutedEventArgs e)
        {
            Pedidos ventanaPedidos = new Pedidos();
            ventanaPedidos.Show();

        }

        private void Inventario_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
