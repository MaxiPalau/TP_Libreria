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
    /// Interaction logic for BuscaLibro1.xaml
    /// </summary>
    public partial class BuscaLibro1 : Window
    {
        // conexion sql global
        //SqlConnection miConexionSql;
        //private SqlConnection miConexionSql = Conexion.GetConexionSql();

        // Contenedor global de datos
        DataTable dtLibros = new DataTable("Libros");
        
        // Guarda el número de página actual
        private int paginacion_IndicePag = 1;

        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_NumRegistrosPagina = 13;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero=1,Siguiente=2,Anterior=3,Ultimo=4};

        private string columna = "";

        private string idLibro = "";
        private string titulo = "";
        private string autor = "";
        private string editorial = "";
        private string isbn = "";
        private string edicion = "";
        private string anio = "";
        private string paginas = "";
        private string categoria = "";
        private string precio = "";
        private string stock = "";
        private bool ejecutar = false;

        public BuscaLibro1()
        {
            InitializeComponent();
            
            List<TipoBusqueda> listaBusquedas = new List<TipoBusqueda>();
            listaBusquedas.Add(new TipoBusqueda { NombreBusqueda = "Titulo" });
            listaBusquedas.Add(new TipoBusqueda { NombreBusqueda = "Autor" });
            listaBusquedas.Add(new TipoBusqueda { NombreBusqueda = "ISBN" });

            listaBusqueda.ItemsSource = listaBusquedas;

        }

        public class TipoBusqueda
        {
            public string NombreBusqueda
            {
                get;
                set;
            }
        }

        private void PaginacionPersonalizada(int modo)
        {
            int totalRegistros = dtLibros.Rows.Count;
            int tamanioPagina = paginacion_NumRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch(modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (paginacion_IndicePag * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();
                        if (totalRegistros >= ((paginacion_IndicePag * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = paginacion_IndicePag * tamanioPagina; i < ((paginacion_IndicePag * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i= paginacion_IndicePag*tamanioPagina; i<totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtLibros.Rows[i]);
                            }
                        }

                        paginacion_IndicePag += 1;
                        listaLibros.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (paginacion_IndicePag > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtLibros.Clone();

                        paginacion_IndicePag -= 1;

                        for (int i =((paginacion_IndicePag * tamanioPagina) - tamanioPagina); i < (paginacion_IndicePag * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtLibros.Rows[i]);
                        }

                        listaLibros.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    paginacion_IndicePag = 2;
                    PaginacionPersonalizada((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    paginacion_IndicePag = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) -1) : (totalRegistros / tamanioPagina);
                    PaginacionPersonalizada((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacion();

        }

        private void MostrarInfoPaginacion()
        {
            int totalRegistros = dtLibros.Rows.Count;
            int tamanioPagina = paginacion_NumRegistrosPagina;

            string infoPaginacion = paginacion_IndicePag * tamanioPagina + " de " + totalRegistros;

            if(dtLibros.Rows.Count < (paginacion_IndicePag*tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            etiquetaInfoPaginacion.Content = infoPaginacion;
            etiquetaNumPag.Content = paginacion_IndicePag.ToString();
            
        }

        private void botonPrimero_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPersonalizada((int)ModoPaginacion.Primero);
        }

        private void botonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPersonalizada((int)ModoPaginacion.Siguiente);
        }

        private void botonAnterior_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPersonalizada((int)ModoPaginacion.Anterior);
        }

        private void botonUltimo_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPersonalizada((int)ModoPaginacion.Ultimo);
        }

        private void listaBusqueda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (null != listaBusqueda.SelectedItem)
            {
               columna = ((TipoBusqueda)listaBusqueda.SelectedItem).NombreBusqueda.ToString();
            }
             
        }

        private void Refresh()
        {
            dtLibros.Clear();
            paginacion_IndicePag = 1;
            MostrarInfoPaginacion();
        }

        private void Consulta()
        {
            string consulta = "";
            if (parametroBusqueda.Text == "Ingrese el dato a buscar" || parametroBusqueda.Text =="" )
            {
                consulta = "SELECT l.Id, l.Titulo, a.Autor, e.Editorial, l.ISBN, l.Edicion, l.Anio, l.Paginas, c.Categoria, l.Precio, l.Stock FROM Libro l " +
                    "LEFT JOIN Autores a " +
                    "ON l.IdAutor = a.Id " +
                    "LEFT JOIN Editoriales e " +
                    "ON l.IdEditorial = e.Id  " +
                    "LEFT JOIN Categorias c " +
                    "ON l.IdCategoria = c.Id";
                ejecutar = true;
            }
            else if (columna == "Autor")
            {
                consulta = "SELECT l.Id, l.Titulo, a.Autor, e.Editorial, l.ISBN, l.Edicion, l.Anio, l.Paginas, c.Categoria, l.Precio, l.Stock FROM Libro l " +
                    "LEFT JOIN Autores a " +
                    "ON l.IdAutor = a.Id " +
                    "LEFT JOIN Editoriales e " +
                    "ON l.IdEditorial = e.Id  " +
                    "LEFT JOIN Categorias c " +
                    "ON l.IdCategoria = c.Id " +
                    "WHERE a." + columna + " LIKE '%" + parametroBusqueda.Text + "%'";
                ejecutar = true;
            }
            else if (columna == "ISBN" || columna == "Titulo")
            {
                consulta = "SELECT l.Id, l.Titulo, a.Autor, e.Editorial, l.ISBN, l.Edicion, l.Anio, l.Paginas, c.Categoria, l.Precio, l.Stock FROM Libro l " +
                    "LEFT JOIN Autores a " +
                    "ON l.IdAutor = a.Id " +
                    "LEFT JOIN Editoriales e " +
                    "ON l.IdEditorial = e.Id  " +
                    "LEFT JOIN Categorias c " +
                    "ON l.IdCategoria = c.Id " +
                    "WHERE l." + columna + " LIKE '%" + parametroBusqueda.Text + "%'";
                ejecutar = true;
            }
            else
            {
                ejecutar = false;
            }
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            try
            {
                Refresh();

                SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
                if (ejecutar == true)
                {
                    SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                    try
                    {
                        miAdaptadorSql.Fill(dtLibros);
                        if (dtLibros.Rows.Count > 0)
                        {
                            DataTable tablaTemp = new DataTable();

                            // Copio los datos de la tabla a un temporal
                            tablaTemp = dtLibros.Clone();

                            /* Si la cantidad total de registros es mas grande que el tamaño de paginacion
                             * importa los registros del 0 al tamaño de paginacion
                             * sino importa los registros del 0 al total
                             */
                            if (dtLibros.Rows.Count >= paginacion_NumRegistrosPagina)
                            {
                                for (int i = 0; i < paginacion_NumRegistrosPagina; i++)
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

                            //Enlazo la tabla con el gridview
                            listaLibros.DataContext = tablaTemp.DefaultView;

                            //Elimino la tabla temporal
                            tablaTemp.Dispose();

                        }
                        else
                        {
                            Refresh();
                            listaLibros.DataContext = null;
                            string mensaje = "No se encontraron libros";
                            MessageBox.Show(mensaje, "Libro no encontrado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                    MostrarInfoPaginacion();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un parámetro de búsqueda.", "Notificación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Consulta();
        }

        private void parametroBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parametroBusqueda.Text = "";
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {

            ModificarLibro ventanaModificarLibro = new ModificarLibro();
            ventanaModificarLibro.Show();

            ventanaModificarLibro.IdLibro1 = idLibro;
            ventanaModificarLibro.textTitulo.Text = titulo;
            ventanaModificarLibro.textAutor.Text = autor;
            ventanaModificarLibro.textEditorial.Text = editorial;
            ventanaModificarLibro.textIsbn.Text = isbn;
            ventanaModificarLibro.textEdicion.Text = edicion;
            ventanaModificarLibro.textAnio.Text = anio;
            ventanaModificarLibro.textPaginas.Text = paginas;
            ventanaModificarLibro.textCategoria.Text = categoria;
            ventanaModificarLibro.textPrecio.Text = precio;
            ventanaModificarLibro.textStock.Text = stock;
            ventanaModificarLibro.CambioModificaciones = false;
            ventanaModificarLibro.Closed += (o, args) =>
             {
                 Consulta();
                 botonModificar.IsEnabled = false;
             };

        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            BorrarLibro ventanaBorrarLibro = new BorrarLibro();
            ventanaBorrarLibro.Show();

            ventanaBorrarLibro.IdLibro1 = idLibro;
            ventanaBorrarLibro.textTitulo.Text = titulo;
            ventanaBorrarLibro.textAutor.Text = autor;
            ventanaBorrarLibro.textEditorial.Text = editorial;
            ventanaBorrarLibro.textIsbn.Text = isbn;
            ventanaBorrarLibro.textEdicion.Text = edicion;
            ventanaBorrarLibro.textAnio.Text = anio;
            ventanaBorrarLibro.textPaginas.Text = paginas;
            ventanaBorrarLibro.textCategoria.Text = categoria;
            ventanaBorrarLibro.textPrecio.Text = precio;
            ventanaBorrarLibro.textStock.Text = stock;
            ventanaBorrarLibro.Closed += (o, args) =>
            {
                Consulta();
            };


        }

        private void listaLibros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaLibros.SelectedItems.Count > 0)
            {
                botonModificar.IsEnabled = true;
                botonBorrar.IsEnabled = true;
                foreach (DataRowView drv in listaLibros.SelectedItems) 
                {
                    idLibro = drv.Row[0] != null ? drv.Row[0].ToString() : String.Empty;
                    titulo = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
                    autor = drv.Row[2] != null ? drv.Row[2].ToString() : String.Empty;
                    editorial = drv.Row[3] != null ? drv.Row[3].ToString() : String.Empty;
                    isbn = drv.Row[4] != null ? drv.Row[4].ToString() : String.Empty;
                    edicion = drv.Row[5] != null ? drv.Row[5].ToString() : String.Empty;
                    anio = drv.Row[6] != null ? drv.Row[6].ToString() : String.Empty;
                    paginas = drv.Row[7] != null ? drv.Row[7].ToString() : String.Empty;
                    categoria = drv.Row[8] != null ? drv.Row[8].ToString() : String.Empty;
                    precio = drv.Row[9] != null ? drv.Row[9].ToString() : String.Empty;
                    stock = drv.Row[10] != null ? drv.Row[10].ToString() : String.Empty;

                }
            }
           
        }
    }
}