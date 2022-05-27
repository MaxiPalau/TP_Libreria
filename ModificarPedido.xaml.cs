using ConexionSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ModificarPedido.xaml
    /// </summary>
    public partial class ModificarPedido : Window
    {
        private string numPedido;
        private string cantidadDetalle = "";
        private string isbnDetalle = "";
        private string proveedorDetalle = "";
        private string idPedido;


        private string isbnLibros = "";
        private string proveedorLibros = "";
        private bool selectionChanged = false;
        private DataTable dtDetalle = new DataTable("Detalle_Pedido");
        private DataTable dtLibros = new DataTable("Libros");

        // Guarda el número de página actual
        private int indicePagLibros = 1;
        private int indicePagDetalle = 1;

        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_CantRegistrosPagina = 5;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero = 1, Siguiente = 2, Anterior = 3, Ultimo = 4 };

        public ModificarPedido()
        {
            InitializeComponent();
        }

        public String NumeroPedido
        {
            set => numPedido = value;
        }

        public void AbreVentana()
        {
            this.numPedido = numeroPedidoText.Text;
            CargaAnioPedido();
            CargaListaDetalle();

            string consulta = "(SELECT Id FROM Pedidos WHERE NumPedido = @numeroPedido)";
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
            miComandoSql.Parameters.AddWithValue("@numeroPedido", numPedido);

            try
            {
                idPedido = miComandoSql.ExecuteScalar().ToString();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            finally
            {
                miComandoSql.Dispose();
                Conexion.Dispose(miConexionSql);
            }
            
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
           Close();

        private void CargaAnioPedido()
        {
            string consulta = "SELECT YEAR(Fecha) FROM Pedidos WHERE NumPedido = " + numPedido;
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);

            try
            {
                anioPedido.Text = miComandoSql.ExecuteScalar().ToString();             
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            finally
            {
                miComandoSql.Dispose();
                Conexion.Dispose(miConexionSql);
            }
            
        }

        private void CargaListaDetalle()
        {
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSqlDetalle = miConexionSql.CreateCommand();
            miComandoSqlDetalle.CommandType = CommandType.StoredProcedure;
            miComandoSqlDetalle.CommandText = "SP_ListaDetalles";
            miComandoSqlDetalle.Parameters.AddWithValue("@idPedido", numPedido);
            SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSqlDetalle);
            try
            {
                miAdaptadorSql.Fill(dtDetalle);
                if (dtDetalle.Rows.Count > 0)
                {
                    DataTable tablaTemp = new DataTable();
                    tablaTemp = dtDetalle.Clone();
                    if (dtDetalle.Rows.Count >= paginacion_CantRegistrosPagina)
                    {
                        for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                        {
                            tablaTemp.ImportRow(dtDetalle.Rows[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dtDetalle.Rows.Count; i++)
                        {
                            tablaTemp.ImportRow(dtDetalle.Rows[i]);
                        }
                    }
                    listaDetalle.DataContext = tablaTemp.DefaultView;
                    tablaTemp.Dispose();
                }
                else
                {
                    RefreshListaDetalle();
                    listaDetalle.DataContext = null;
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                miAdaptadorSql.Dispose();
                miComandoSqlDetalle.Dispose();
                Conexion.Dispose(miConexionSql);
            }
            MostrarInfoPaginacionListaDetalle();
        }


        private void RefreshListaDetalle()
        {

            dtDetalle.Clear();
            indicePagDetalle = 1;
            listaDetalle.DataContext = null;
            MostrarInfoPaginacionListaDetalle();
        }

        private void MostrarInfoPaginacionListaDetalle()
        {
            int totalRegistros = dtDetalle.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;       
            string infoPaginacion = indicePagDetalle * tamanioPagina + " de " + totalRegistros;

            if (dtDetalle.Rows.Count < (indicePagDetalle * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPagPedido.Content = infoPaginacion;
            NumPagPedido.Content = indicePagDetalle.ToString();

        }

        private void PaginacionListaDetalle(int modo)
        {
            int totalRegistros = dtDetalle.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch (modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (indicePagDetalle * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtDetalle.Clone();
                        if (totalRegistros >= ((indicePagDetalle * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePagDetalle * tamanioPagina; i < ((indicePagDetalle * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtDetalle.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePagDetalle * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtDetalle.Rows[i]);
                            }
                        }

                        indicePagDetalle += 1;
                        listaDetalle.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePagDetalle > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtDetalle.Clone();

                        indicePagDetalle -= 1;

                        for (int i = ((indicePagDetalle * tamanioPagina) - tamanioPagina); i < (indicePagDetalle * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtDetalle.Rows[i]);
                        }

                        listaDetalle.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePagDetalle = 2;
                    PaginacionListaDetalle((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePagDetalle = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionListaDetalle((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacionListaDetalle();

        }

        private void BotonPrimeroListaDetalle_Click(object sender, RoutedEventArgs e)
        {
            PaginacionListaDetalle((int)ModoPaginacion.Primero);
        }

        private void BotonAnteriorListaDetalle_Click(object sender, RoutedEventArgs e)
        {
            PaginacionListaDetalle((int)ModoPaginacion.Anterior);
        }

        private void BotonSiguienteListaDetalle_Click(object sender, RoutedEventArgs e)
        {
            PaginacionListaDetalle((int)ModoPaginacion.Siguiente);
        }

        private void BotonUltimoListaDetalle_Click(object sender, RoutedEventArgs e)
        {
            PaginacionListaDetalle((int)ModoPaginacion.Ultimo);
        }

        private void listaDetalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cantidadListaDetalle.IsEnabled = true;
            botonModificar.IsEnabled = true;
            botonBorrar.IsEnabled = true;
            foreach (DataRowView dataRow in listaDetalle.SelectedItems)
            {
                isbnDetalle = dataRow.Row[2] != null ? dataRow[2].ToString() : String.Empty;
                cantidadDetalle = dataRow.Row[3] != null ? dataRow[3].ToString() : String.Empty;
                cantidadListaDetalle.Text = cantidadDetalle;
            }
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (cantidadListaDetalle.Text == cantidadDetalle)
            {
                MessageBox.Show("No modificó la cantidad", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                botonCerrar.Content = "Salir";
                string update = "UPDATE DetallePedidos SET Cantidad = @cantidad WHERE IdPedido = " +
                    "(SELECT Id FROM Pedidos WHERE NumPedido = @numeroPedido) AND Detalle = @isbn";
                SqlConnection miConexionSql = Conexion.GetConexionSql();
                SqlCommand miComandoSql = new SqlCommand(update, miConexionSql);
                miComandoSql.Parameters.AddWithValue("@cantidad", cantidadListaDetalle.Text);
                miComandoSql.Parameters.AddWithValue("@numeroPedido", numPedido);
                miComandoSql.Parameters.AddWithValue("@isbn", isbnDetalle);

                try
                {
                    int rowUpdate = miComandoSql.ExecuteNonQuery();
                    if (rowUpdate == 1)
                    {
                        MessageBox.Show("Actualización correcta", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshListaDetalle();
                        CargaListaDetalle();
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    miComandoSql.Dispose();
                    Conexion.Dispose(miConexionSql);
                }
                botonModificar.IsEnabled = false;
                botonBorrar.IsEnabled = false;
                cantidadListaDetalle.Text = "";
            }
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            botonCerrar.Content = "Salir";
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = miConexionSql.CreateCommand();
            miComandoSql.CommandType = CommandType.StoredProcedure;
            miComandoSql.CommandText = "SP_BorrarRegistroPedidoDetalle";
            miComandoSql.Parameters.AddWithValue("@numeroPedido", numPedido);
            miComandoSql.Parameters.AddWithValue("@isbn", isbnDetalle);

            try
            {
                int rowDelete = miComandoSql.ExecuteNonQuery();
                if (rowDelete == 1)
                {
                    MessageBox.Show("Registro borrado", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshListaDetalle();
                    CargaListaDetalle();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            finally
            {
                miComandoSql.Dispose();
                Conexion.Dispose(miConexionSql);
            }
            botonModificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;
            cantidadListaDetalle.Text = "";
        }

        private void parametroBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parametroBusqueda.Text = "";
        }

        private void RefreshBuscar()
        {
            dtLibros.Clear();
            indicePagLibros = 1;
            MostrarInfoPaginacionLibros();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = "";
            string commandText = "";
            string campo = "";
            RefreshBuscar();
            if (parametroBusqueda.Text == "" || parametroBusqueda.Text == "Ingrese el dato a buscar")
            {
                MessageBox.Show("Debe ingresar algún parámetro de búsqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else if (botonIsbn.IsChecked == false && botonTitulo.IsChecked == false)
            {
                MessageBox.Show("Debe seleccionar algún parámetro de búsqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (botonIsbn.IsChecked == true)
                {
                    filtro = parametroBusqueda.Text;
                    commandText = "SP_BuscarLibroNuevoPedidoISBN";
                    campo = "@ISBN";
                }
                else if (botonTitulo.IsChecked == true)
                {
                    filtro = parametroBusqueda.Text;
                    commandText = "SP_BuscarLibroNuevoPedidoTitulo";
                    campo = "@Titulo";
                }
                SqlConnection miConexionSql = Conexion.GetConexionSql();
                try
                {
                    SqlCommand miComandoSql = miConexionSql.CreateCommand();
                    miComandoSql.CommandType = CommandType.StoredProcedure;
                    miComandoSql.CommandText = commandText;
                    miComandoSql.Parameters.AddWithValue(campo, filtro);

                    SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                    using (miAdaptadorSql)
                    {
                        try
                        {
                            int cont = miAdaptadorSql.Fill(dtLibros);
                            if (cont == 0)
                            {
                                listaLibrosPedido.DataContext = null;
                                MessageBox.Show("No se encontraron registros", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                            else
                            {
                                if (dtLibros.Rows.Count > 0)
                                {
                                    DataTable tablaTemp = new DataTable();
                                    tablaTemp = dtLibros.Clone();
                                    if (dtLibros.Rows.Count >= paginacion_CantRegistrosPagina)
                                    {
                                        for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                                        {
                                            tablaTemp.ImportRow(dtLibros.Rows[i]);
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < dtLibros.Rows.Count; i++)
                                        {
                                            tablaTemp.ImportRow(dtLibros.Rows[i]);
                                        }
                                    }
                                    listaLibrosPedido.DataContext = tablaTemp.DefaultView;
                                    tablaTemp.Dispose();
                                }
                                else
                                {
                                    RefreshBuscar();
                                    listaLibrosPedido.DataContext = null;
                                }
                            }
                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show(e2.ToString());
                        }
                        finally
                        {
                            miAdaptadorSql.Dispose();
                            miComandoSql.Dispose();
                        }
                    }
                    MostrarInfoPaginacionLibros();
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    Conexion.Dispose(miConexionSql);
                    selectionChanged = false;
                    botonAgregar.IsEnabled = false;
                    cantidadLibro.IsEnabled = false;
                }
                botonAgregar.IsEnabled = false;
            }
        }

        private void PaginacionLibros(int modo)
        {
            int totalRegistros = dtLibros.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch (modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (indicePagLibros * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();
                        if (totalRegistros >= ((indicePagLibros * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePagLibros * tamanioPagina; i < ((indicePagLibros * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePagLibros * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }

                        indicePagLibros += 1;
                        listaLibrosPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePagLibros > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();

                        indicePagLibros -= 1;

                        for (int i = ((indicePagLibros * tamanioPagina) - tamanioPagina); i < (indicePagLibros * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtLibros.Rows[i]);
                        }

                        listaLibrosPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePagLibros = 2;
                    PaginacionLibros((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePagLibros = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionLibros((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacionLibros();

        }

        private void MostrarInfoPaginacionLibros()
        {
            int totalRegistros = dtLibros.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            string infoPaginacion = indicePagLibros * tamanioPagina + " de " + totalRegistros;

            if (dtLibros.Rows.Count < (indicePagLibros * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPag.Content = infoPaginacion;
            NumPag.Content = indicePagLibros.ToString();

        }

        private void listaLibrosPedido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataRowView drvv in listaDetalle.Items)
            {
                proveedorDetalle = drvv.Row[1] != null ? drvv.Row[1].ToString() : String.Empty;
            }
            
            foreach (DataRowView drv in listaLibrosPedido.SelectedItems)
            {
                isbnLibros = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
                proveedorLibros = drv.Row[3] != null ? drv.Row[3].ToString() : String.Empty;
            }
            if ( proveedorLibros == proveedorDetalle)
            {
                botonAgregar.IsEnabled = true;
                cantidadLibro.IsEnabled = true;
                cantidadLibro.Text = "";
            }
            else
            {
                MessageBox.Show("Debe seleccionar un libro del mismo proveedor, " +
                    "\n por favor genere otro pedido para este libro.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                proveedorLibros = proveedorDetalle;
                listaLibrosPedido.SelectedItems.Clear();
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            string insert;
            botonCerrar.Content = "Salir";
            SqlConnection miConexionSql = Conexion.GetConexionSql();

            if (cantidadLibro.Text == "")
            {
                MessageBox.Show("No ingresó una cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                insert = "INSERT INTO DetallePedidos (IdPedido, Detalle, Cantidad) " +
                    "VALUES (@pedidoId, @isbn, @cant)";
                SqlCommand miComandoSql = new SqlCommand(insert, miConexionSql);
                miComandoSql.Parameters.AddWithValue("@cant", cantidadLibro.Text);
                miComandoSql.Parameters.AddWithValue("@pedidoId", idPedido);
                miComandoSql.Parameters.AddWithValue("@isbn", isbnLibros);

                try
                {
                    int rowInsert = miComandoSql.ExecuteNonQuery();
                    if (rowInsert == 1)
                    {
                        MessageBox.Show("Se agregó el libro al pedido", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshListaDetalle();
                        CargaListaDetalle();
                        cantidadLibro.Text = "";
                        proveedorLibros = proveedorDetalle;
                        botonAgregar.IsEnabled = false;
                        listaLibrosPedido.SelectedItems.Clear();
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    miComandoSql.Dispose();
                    Conexion.Dispose(miConexionSql);
                }
                botonAgregar.IsEnabled = false;
            }
            
            
        }

        private void BotonPrimeroListaLibros_Click(object sender, RoutedEventArgs e)
        {
            PaginacionLibros((int)ModoPaginacion.Primero);
        }

        private void BotonAnteriorListaLibros_Click(object sender, RoutedEventArgs e)
        {
            PaginacionLibros((int)ModoPaginacion.Anterior);
        }

        private void BotonSiguienteListaLibros_Click(object sender, RoutedEventArgs e)
        {
            PaginacionLibros((int)ModoPaginacion.Siguiente);
        }

        private void BotonUltimoListaLibros_Click(object sender, RoutedEventArgs e)
        {
            PaginacionLibros((int)ModoPaginacion.Ultimo);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
