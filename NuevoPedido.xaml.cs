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
    /// Interaction logic for NuevoPedido.xaml
    /// </summary>
    public partial class NuevoPedido : Window
    {
        // Contenedor global de datos
        private DataTable dtLibros = new DataTable("Libros");
        private DataTable dtPedido = new DataTable("Pedido");

        // Guarda el número de página actual
        private int indicePag = 1;
        private int indicePagPedido = 1;


        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_CantRegistrosPagina = 5;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero = 1, Siguiente = 2, Anterior = 3, Ultimo = 4 };

        private bool selectionChanged = false;

        private string titulo = "";
        private string isbn = "";
        private string proveedor = "";
        private string isbn2 = "";
        private string proveedorAnt = "";

        public NuevoPedido()
        {
            InitializeComponent();
            Columnas();
        }

        // Evento botón Cerrar
        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void parametroBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parametroBusqueda.Text = "";
        }


        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = "";
            string commandText = "";
            string campo = "";
            Refresh();
            if (parametroBusqueda.Text == "" || parametroBusqueda.Text == "Ingrese el dato a buscar")
            {
                MessageBox.Show("Debe ingresar algún parámetro de búsqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                if (botonIsbn.IsChecked == true )
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
                                    Refresh();
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
                    MostrarInfoPaginacion();
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
            }         
        }

       

        private void BotonPrimero_Click(object sender, RoutedEventArgs e)
        {
            Paginacion((int)ModoPaginacion.Primero);
        }

        private void BotonAnterior_Click(object sender, RoutedEventArgs e)
        {
            Paginacion((int)ModoPaginacion.Anterior);
        }

        private void BotonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Paginacion((int)ModoPaginacion.Siguiente);
        }

        private void BotonUltimo_Click(object sender, RoutedEventArgs e)
        {
            Paginacion((int)ModoPaginacion.Ultimo);
        }

        private void Refresh()
        {
            dtLibros.Clear();
            indicePag = 1;
            MostrarInfoPaginacion();
        }

        private void Paginacion(int modo)
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
                    if (totalRegistros > (indicePag * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();
                        if (totalRegistros >= ((indicePag * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePag * tamanioPagina; i < ((indicePag * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePag * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }

                        indicePag += 1;
                        listaLibrosPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePag > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();

                        indicePag -= 1;

                        for (int i = ((indicePag * tamanioPagina) - tamanioPagina); i < (indicePag * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtLibros.Rows[i]);
                        }

                        listaLibrosPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePag = 2;
                    Paginacion((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePag = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    Paginacion((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacion();

        }

        private void MostrarInfoPaginacion()
        {
            int totalRegistros = dtLibros.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            string infoPaginacion = indicePag * tamanioPagina + " de " + totalRegistros;

            if (dtLibros.Rows.Count < (indicePag * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPag.Content = infoPaginacion;
            NumPag.Content = indicePag.ToString();

        }

        private void listaLibrosPedido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChanged = true;
            foreach (DataRowView drv in listaLibrosPedido.SelectedItems)
            {
                titulo = drv.Row[0] != null ? drv.Row[0].ToString() : String.Empty;
                isbn = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
                proveedor = drv.Row[3] != null ? drv.Row[3].ToString() : String.Empty;
            }

            if ((dtPedido.Rows.Count == 0 && proveedorAnt == "") || proveedorAnt == proveedor)
            {
                botonAgregar.IsEnabled = true;
                cantidadLibro.IsEnabled = true;
                cantidadLibro.Text = "";
            }
            else 
            {
                MessageBox.Show("Debe seleccionar un libro del mismo proveedor, " +
                    "\n por favor genere otro pedido para este libro.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }


        /* ----------------------------------------------  LISTA DE PEDIDOS AGREGADOS ----------------------------------------*/
        private void Columnas()
        {
            dtPedido.Columns.Add("Titulo", typeof(String));
            dtPedido.Columns.Add("ISBN", typeof(String));
            dtPedido.Columns.Add("Cantidad", typeof(String));
            dtPedido.Columns.Add("Proveedor", typeof(String));
        }

        private void RefreshPedido()
        {
            indicePagPedido = 1;
            MostrarInfoPaginacionPedido();
        }

        private void Generar_Click(object sender, RoutedEventArgs e)
        {
            int pedidoId;
            string isbnCarga = "";
            int cantidad = 0;

            if (dtPedido.Rows.Count == 0)
            {
                MessageBox.Show("No hay libros para realizar el pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SqlConnection miConexionSql = Conexion.GetConexionSql();
                try
                {
                    SqlCommand miComandoSql = miConexionSql.CreateCommand();
                    
                    miComandoSql.CommandType = CommandType.StoredProcedure;
                    miComandoSql.CommandText = "SP_GenerarPedido";
                    miComandoSql.Parameters.AddWithValue("@Proveedor", proveedorAnt);
                    miComandoSql.Parameters.Add("@idpedido", SqlDbType.BigInt);
                    miComandoSql.Parameters["@idpedido"].Direction = ParameterDirection.Output;

                    SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                    
                    try
                    {
                        miComandoSql.ExecuteNonQuery();
                        pedidoId = Convert.ToInt32(miComandoSql.Parameters["@idpedido"].Value);
                        
                        

                        foreach (DataRow dr in dtPedido.Rows)
                        {
                            SqlCommand miComandoSqlDetalle = miConexionSql.CreateCommand();
                            miComandoSqlDetalle.CommandType = CommandType.StoredProcedure;
                            miComandoSqlDetalle.CommandText = "SP_GenerarPedidoDetalle";
                            miComandoSqlDetalle.Parameters.AddWithValue("@pedidoId", pedidoId);
                            isbnCarga = dr["ISBN"].ToString();
                            cantidad = int.Parse(dr["Cantidad"].ToString());
                            miComandoSqlDetalle.Parameters.AddWithValue("@ISBN", isbnCarga);
                            miComandoSqlDetalle.Parameters.AddWithValue("@cant", cantidad);
                            miComandoSqlDetalle.ExecuteNonQuery();
                            miComandoSqlDetalle.Dispose();
                        }
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    Conexion.Dispose(miConexionSql);
                    MessageBox.Show("Pedido Generado", "Pedido", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            int cantidad = 0;

            if (selectionChanged == false)
            {
                MessageBox.Show("Debe seleccionar un libro", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (cantidadLibro.Text == "" || int.Parse(cantidadLibro.Text) <= 0)
            {
                MessageBox.Show("Debe ingresar una cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            {
                cantidad = int.Parse(cantidadLibro.Text);

                if (dtPedido.Rows.Count == 0)
                {
                    dtPedido.Rows.Add(new Object[] { titulo, isbn, cantidad.ToString(), proveedor });

                    listaPedido.DataContext = dtPedido.DefaultView;
                }
                else
                {
                    bool repetido = false;
                    foreach (DataRow dr in dtPedido.Rows)
                    {
                        if (dr["ISBN"].ToString() == isbn)
                        {
                            int suma;
                            suma = cantidad + int.Parse(dr["Cantidad"].ToString());
                            dr.SetField("Cantidad", suma.ToString());
                            repetido = true;
                        }
                    }
                    if (repetido == false)
                    {
                        dtPedido.Rows.Add(new Object[] { titulo, isbn, cantidad.ToString(), proveedor });
                    }
                    listaPedido.DataContext = dtPedido.DefaultView;
                }
                try
                {
                    if (dtPedido.Rows.Count > 0)
                    {
                        DataTable tablaTemp1 = new DataTable();
                        tablaTemp1 = dtPedido.Clone();
                        if (dtPedido.Rows.Count >= paginacion_CantRegistrosPagina)
                        {
                            for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                            {
                                tablaTemp1.ImportRow(dtPedido.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtPedido.Rows.Count; i++)
                            {
                                tablaTemp1.ImportRow(dtPedido.Rows[i]);
                            }

                        }
                        listaPedido.DataContext = tablaTemp1.DefaultView;
                        tablaTemp1.Dispose();
                    }
                    else
                    {
                        RefreshPedido();
                        listaPedido.DataContext = null;
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                MostrarInfoPaginacionPedido();               
            }

            cantidadLibro.Text = "";
            proveedorAnt = proveedor;
            listaLibrosPedido.SelectedItems.Clear();
            botonAgregar.IsEnabled = false;
            cantidadLibro.IsEnabled = false;
            selectionChanged = false;
        }

        private void PaginacionPedido(int modo)
        {
            int totalRegistros = dtPedido.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch (modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (indicePagPedido * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtPedido.Clone();
                        if (totalRegistros >= ((indicePagPedido * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePagPedido * tamanioPagina; i < ((indicePagPedido * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtPedido.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePagPedido * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtPedido.Rows[i]);
                            }
                        }

                        indicePagPedido += 1;
                        listaPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePagPedido > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtPedido.Clone();

                        indicePagPedido -= 1;

                        for (int i = ((indicePagPedido * tamanioPagina) - tamanioPagina); i < (indicePagPedido * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtPedido.Rows[i]);
                        }

                        listaPedido.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePagPedido = 2;
                    PaginacionPedido((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePagPedido = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionPedido((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacionPedido();
        }

        private void MostrarInfoPaginacionPedido()
        {
            int totalRegistros = dtPedido.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            string infoPaginacion = indicePagPedido * tamanioPagina + " de " + totalRegistros;

            if (dtPedido.Rows.Count < (indicePagPedido * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPagPedido.Content = infoPaginacion;
            NumPagPedido.Content = indicePagPedido.ToString();

        }

        private void BotonPrimeroPedido_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedido((int)ModoPaginacion.Primero);
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
        }

        private void BotonAnteriorPedido_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedido((int)ModoPaginacion.Anterior);
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
        }

        private void BotonSiguientePedido_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedido((int)ModoPaginacion.Siguiente);
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
        }

        private void BotonUltimoPedido_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedido((int)ModoPaginacion.Ultimo);
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            int cantidad = 0;
            if (cantidadLibro.Text == "" || int.Parse(cantidadLibro.Text) <= 0)
            {
                MessageBox.Show("Dede ingresar una cantidad", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                cantidad = int.Parse(cantidadLibro.Text);
                foreach (DataRow dr in dtPedido.Rows)
                {
                    if (dr["ISBN"].ToString() == isbn2)
                    {
                        dr.SetField("Cantidad", cantidad.ToString());
                    }
                }
                listaPedido.DataContext = dtPedido.DefaultView;

                try
                {
                    if (dtPedido.Rows.Count > 0)
                    {
                        DataTable tablaTemp1 = new DataTable();
                        tablaTemp1 = dtPedido.Clone();
                        if (dtPedido.Rows.Count >= paginacion_CantRegistrosPagina)
                        {
                            for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                            {
                                tablaTemp1.ImportRow(dtPedido.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtPedido.Rows.Count; i++)
                            {
                                tablaTemp1.ImportRow(dtPedido.Rows[i]);
                            }

                        }
                        listaPedido.DataContext = tablaTemp1.DefaultView;
                        tablaTemp1.Dispose();
                    }
                    else
                    {
                        RefreshPedido();
                        listaPedido.DataContext = null;
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                MostrarInfoPaginacionPedido();
                botonModificar.IsEnabled = false;
                botonBorrar.IsEnabled = false;
            } 
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            
            dtPedido.AcceptChanges();
            foreach (DataRow dr in dtPedido.Rows)
            {
                if (dr["ISBN"].ToString() == isbn2)
                {
                    dr.Delete();
                }
            }
            dtPedido.AcceptChanges();
            listaPedido.DataContext = dtPedido.DefaultView;
            try
            {
                if (dtPedido.Rows.Count > 0)
                {
                    DataTable tablaTemp1 = new DataTable();
                    tablaTemp1 = dtPedido.Clone();
                    if (dtPedido.Rows.Count >= paginacion_CantRegistrosPagina)
                    {
                        for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                        {
                            tablaTemp1.ImportRow(dtPedido.Rows[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dtPedido.Rows.Count; i++)
                        {
                            tablaTemp1.ImportRow(dtPedido.Rows[i]);
                        }

                    }
                    listaPedido.DataContext = tablaTemp1.DefaultView;
                    tablaTemp1.Dispose();
                }
                else
                {
                    RefreshPedido();
                    listaPedido.DataContext = null;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            MostrarInfoPaginacionPedido();

            if (dtPedido.Rows.Count == 0)
            {
                proveedorAnt = "";
            }
            botonModificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;
        }

        private void listaPedido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            botonBorrar.IsEnabled = true;
            botonModificar.IsEnabled = true;
            cantidadLibro.IsEnabled = true;
            cantidadLibro.Text = "";
            foreach (DataRowView drv in listaPedido.SelectedItems)
            {
                isbn2 = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
            }
        }
    }
}
