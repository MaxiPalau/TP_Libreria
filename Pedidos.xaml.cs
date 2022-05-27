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
//using GeneraPDF;
//using System.Data.SqlClient;
//using ConexionSQL;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Table = iText.Layout.Element.Table;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using alineacion = iText.Layout.Properties.TextAlignment;
using parrafo = iText.Layout.Element.Paragraph;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Canvas;
using iText.Commons.Utils;
using iText.Kernel.Events;
using iText.IO.Image;
using rectangulo = iText.Kernel.Geom.Rectangle;
using borde = iText.Layout.Borders.Border;




//using System.Data;
//using System.Windows;

namespace Libreria
{
    /// <summary>
    /// Interaction logic for Pedidos.xaml
    /// </summary>
    public partial class Pedidos : Window
    {
        // Contenedor global de datos
        DataTable dtPedidos = new DataTable("Pedidos");
        DataTable dtDetalle = new DataTable("Detalle");

        private string idPedido = "";

        // Guarda el número de página actual
        private int indicePagPedidos = 1;
        private int indicePagDetalles = 1;

        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_CantRegistrosPagina = 10;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero = 1, Siguiente = 2, Anterior = 3, Ultimo = 4 };

        public Pedidos()
        {
            InitializeComponent();
            AbreVentana();
        }

        private void AbreVentana()
        {
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            RefreshPedido();
            RefreshDetalle();
            try
            {
                SqlCommand miComandoSql = miConexionSql.CreateCommand();
                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_AbreVentana";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                try
                {
                    miAdaptadorSql.Fill(dtPedidos);
                    if (dtPedidos.Rows.Count > 0)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtPedidos.Clone();
                        if (dtPedidos.Rows.Count >= paginacion_CantRegistrosPagina)
                        {
                            for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                            {
                                tablaTemp.ImportRow(dtPedidos.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtPedidos.Rows.Count; i++)
                            {
                                tablaTemp.ImportRow(dtPedidos.Rows[i]);
                            }
                        }

                        listaPedidos.DataContext = tablaTemp.DefaultView;

                        tablaTemp.Dispose();
                    }
                    else
                    {
                        //Refresh();
                        listaPedidos.DataContext = null;
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
                MostrarInfoPaginacionPedidos();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Conexion.Dispose(miConexionSql);
            }
        }

        // Boton cerrar
        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        // Boton generar pdf
        private void GenerarPDF_Click(object sender, RoutedEventArgs e)
        {
            GenerarPdf();
           
        }

        // Doble click cuadro texto numero de pedido
        private void parametroBusqueda_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parametroBusqueda.Text = "";            
        }

        // Seleccion de elemento en la lista de pedidos
        private void listaPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetalle();
            botonBorrar.IsEnabled = true;
            botonModificar.IsEnabled = true;
            botonGenerarPDF.IsEnabled = true;
            SqlConnection miConexionSql = Conexion.GetConexionSql();

            if (listaPedidos.SelectedItems.Count > 0)
            {
                foreach (DataRowView dataRow in listaPedidos.SelectedItems)
                {
                    idPedido = dataRow.Row[0] != null ? dataRow[0].ToString() : String.Empty;
                }
                try
                {
                    SqlCommand miComandoSql = miConexionSql.CreateCommand();
                    miComandoSql.CommandType = CommandType.StoredProcedure;
                    miComandoSql.CommandText = "SP_ListaDetalles";
                    miComandoSql.Parameters.AddWithValue("@idPedido", idPedido);
                    SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                    try
                    {
                        miAdaptadorSql.Fill(dtDetalle);
                        if (dtDetalle.Rows.Count > 0)
                        {
                            DataTable tablaTemp3 = new DataTable();
                            tablaTemp3 = dtDetalle.Clone();
                            if (dtDetalle.Rows.Count >= paginacion_CantRegistrosPagina)
                            {
                                for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                                {
                                    tablaTemp3.ImportRow(dtDetalle.Rows[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < dtDetalle.Rows.Count; i++)
                                {
                                    tablaTemp3.ImportRow(dtDetalle.Rows[i]);
                                }
                            }
                            listaPedidosDetalle.DataContext = tablaTemp3.DefaultView;
                            tablaTemp3.Dispose();
                        }
                        else
                        {
                            RefreshDetalle();
                            listaPedidosDetalle.DataContext = null;
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
                    MostrarInfoPaginacionDetalle();
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
                finally
                {
                    Conexion.Dispose(miConexionSql);
                }
            }

           
        }

        // refresco de info de paginacion
        private void RefreshPedido()
        {
            dtPedidos.Clear();
            indicePagPedidos = 1;
            listaPedidos.DataContext = null;
            MostrarInfoPaginacionPedidos();

        }

        private void RefreshDetalle()
        {

            dtDetalle.Clear();
            indicePagDetalles = 1;
            listaPedidosDetalle.DataContext = null;
            MostrarInfoPaginacionDetalle();
        }

        // Boton buscar
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
            botonGenerarPDF.IsEnabled = false;
            if (parametroBusqueda.Text == "Ingrese el número de pedido a buscar")
            {
                MessageBox.Show("Debe ingresar un número de pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
            else
            {
                RefreshPedido();
                //RefreshDetalle();
                if (parametroBusqueda.Text == "")
                {
                    //listaPedidos.DataContext = null;
                    AbreVentana();
                }
                else
                {
                    idPedido = parametroBusqueda.Text;
                    SqlConnection miConexionSql = Conexion.GetConexionSql();

                    try
                    {
                        SqlCommand miComandoSql = miConexionSql.CreateCommand();
                        miComandoSql.CommandType = CommandType.StoredProcedure;
                        miComandoSql.CommandText = "SP_BuscarPedido";
                        miComandoSql.Parameters.AddWithValue("@Numero", idPedido);

                        SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                        try
                        {
                            miAdaptadorSql.Fill(dtPedidos);
                            if (dtPedidos.Rows.Count == 0)
                            {
                                MessageBox.Show("No se encontraron pedidos con ese número.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            if (dtPedidos.Rows.Count > 0)
                            {
                                DataTable tablaTemp = new DataTable();
                                tablaTemp = dtPedidos.Clone();
                                if (dtPedidos.Rows.Count >= paginacion_CantRegistrosPagina)
                                {
                                    for (int i = 0; i < paginacion_CantRegistrosPagina; i++)
                                    {
                                        tablaTemp.ImportRow(dtPedidos.Rows[i]);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dtPedidos.Rows.Count; i++)
                                    {
                                        tablaTemp.ImportRow(dtPedidos.Rows[i]);
                                    }
                                }

                                listaPedidos.DataContext = tablaTemp.DefaultView;

                                tablaTemp.Dispose();
                            }
                            else
                            {
                                RefreshPedido();
                                listaPedidos.DataContext = null;
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
                        MostrarInfoPaginacionPedidos();
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                    finally
                    {
                        Conexion.Dispose(miConexionSql);
                    }
                }
                

            }
            
            
        }


        // Boton nuevo
        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            NuevoPedido ventanaNuevoPedido = new NuevoPedido();
            ventanaNuevoPedido.Show();
        }

        // Boton modificar
        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarPedido ventanaModificarPedido = new ModificarPedido();
            ventanaModificarPedido.numeroPedidoText.Text = idPedido;
         
            ventanaModificarPedido.Show();
            ventanaModificarPedido.AbreVentana();
            ventanaModificarPedido.Closed += (o, args) =>
            {
                AbreVentana();
                botonBorrar.IsEnabled = false;
                botonModificar.IsEnabled = false;
                botonGenerarPDF.IsEnabled = false;
            };

        }


        // Boton borrar
        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            string nroPedido = "";
            string fecha = "";
            foreach (DataRowView drv in listaPedidos.SelectedItems)
            {
                nroPedido = drv.Row[0] != null ? drv[0].ToString() : String.Empty;
                fecha = drv.Row[1] != null ? drv[1].ToString() : String.Empty;
                
            }
            string mensaje = "Está seguro que quiere borrar el pedido Nº: " + nroPedido + ", de la fecha: " + fecha +"?";
            var resultado = MessageBox.Show(mensaje, "Borrar Pedido", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                SqlConnection miConexionSql = Conexion.GetConexionSql();
                SqlCommand miComandoSql = miConexionSql.CreateCommand();
                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_BorrarPedidoDetalle";
                miComandoSql.Parameters.AddWithValue("@numeroPedido", nroPedido);

                try
                {

                    miComandoSql.ExecuteNonQuery();

                    SqlCommand miComandoSql1 = miConexionSql.CreateCommand();
                    miComandoSql1.CommandType = CommandType.StoredProcedure;
                    miComandoSql1.CommandText = "SP_BorrarPedido";
                    miComandoSql1.Parameters.AddWithValue("@numeroPedido", nroPedido);

                    int rowDeletePedido = miComandoSql1.ExecuteNonQuery();
                    if (rowDeletePedido == 1)
                    {
                        MessageBox.Show("Pedido Nº: " + nroPedido + " borrado", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                        AbreVentana();
                        botonBorrar.IsEnabled = false;
                        botonModificar.IsEnabled = false;
                        botonGenerarPDF.IsEnabled = false;
                    }
                    miComandoSql1.Dispose();
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
            botonBorrar.IsEnabled = false;
            botonModificar.IsEnabled = false;
            botonGenerarPDF.IsEnabled = false;
        }


        // ----------------     PAGINACION     -------------------------

        // Paginación Lisview Pedidos
        private void PaginacionPedidos(int modo)
        {
            int totalRegistros = dtPedidos.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch (modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (indicePagPedidos * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtPedidos.Clone();
                        if (totalRegistros >= ((indicePagPedidos * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePagPedidos * tamanioPagina; i < ((indicePagPedidos * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtPedidos.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePagPedidos * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtPedidos.Rows[i]);
                            }
                        }

                        indicePagPedidos += 1;
                        listaPedidos.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePagPedidos > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtPedidos.Clone();

                        indicePagPedidos -= 1;

                        for (int i = ((indicePagPedidos * tamanioPagina) - tamanioPagina); i < (indicePagPedidos * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtPedidos.Rows[i]);
                        }

                        listaPedidos.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePagPedidos = 2;
                    PaginacionPedidos((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePagPedidos = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionPedidos((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacionPedidos();

        }

        private void MostrarInfoPaginacionPedidos()
        {
            int totalRegistros = dtPedidos.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            string infoPaginacion = indicePagPedidos * tamanioPagina + " de " + totalRegistros;

            if (dtPedidos.Rows.Count < (indicePagPedidos * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPagPedido.Content = infoPaginacion;
            NumPagPedido.Content = indicePagPedidos.ToString();

        }

        private void BotonPrimeroPedidos_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedidos((int)ModoPaginacion.Primero);
        }

        private void BotonSiguientePedidos_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedidos((int)ModoPaginacion.Siguiente);
        }

        private void BotonAnteriorPedidos_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedidos((int)ModoPaginacion.Anterior);
        }

        private void BotonUltimoPedidos_Click(object sender, RoutedEventArgs e)
        {
            PaginacionPedidos((int)ModoPaginacion.Ultimo);
        }




        // Paginación Listview  listaPedidosDetalle
  
        private void PaginacionDetalles(int modo)
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
                    if (totalRegistros > (indicePagDetalles * tamanioPagina))
                    {
                        DataTable tablaTemp1 = new DataTable();
                        tablaTemp1 = dtDetalle.Clone();
                        if (totalRegistros >= ((indicePagDetalles * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = indicePagDetalles * tamanioPagina; i < ((indicePagDetalles * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp1.ImportRow(dtDetalle.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = indicePagDetalles * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp1.ImportRow(dtDetalle.Rows[i]);
                            }
                        }

                        indicePagDetalles += 1;
                        listaPedidosDetalle.DataContext = tablaTemp1.DefaultView;
                        tablaTemp1.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (indicePagDetalles > 1)
                    {
                        DataTable tablaTemp1 = new DataTable();
                        tablaTemp1 = dtDetalle.Clone();

                        indicePagDetalles -= 1;

                        for (int i = ((indicePagDetalles * tamanioPagina) - tamanioPagina); i < (indicePagDetalles * tamanioPagina); i++)
                        {
                            tablaTemp1.ImportRow(dtDetalle.Rows[i]);
                        }

                        listaPedidosDetalle.DataContext = tablaTemp1.DefaultView;
                        tablaTemp1.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    indicePagDetalles = 2;
                    PaginacionDetalles((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    indicePagDetalles = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionDetalles((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacionDetalle();

        }

        private void MostrarInfoPaginacionDetalle()
        {
            int totalRegistros = dtDetalle.Rows.Count;
            int tamanioPagina = paginacion_CantRegistrosPagina;

            //string infoPaginacion = (((indicePagDetalles - 1) * tamanioPagina) + 1) + " a " + indicePagDetalles * tamanioPagina + " de " + totalRegistros;
            string infoPaginacion = indicePagDetalles * tamanioPagina + " de " + totalRegistros;

            if (dtDetalle.Rows.Count < (indicePagDetalles * tamanioPagina))
            {
                infoPaginacion = totalRegistros + " de " + totalRegistros;
            }
            InfoPagDetalle.Content = infoPaginacion;
            NumPagDetalle.Content = indicePagDetalles.ToString();

        }

        private void BotonPrimeroDetalles_Click(object sender, RoutedEventArgs e)
        {
            PaginacionDetalles((int)ModoPaginacion.Primero);
        }

        private void BotonSiguienteDetalles_Click(object sender, RoutedEventArgs e)
        {
            PaginacionDetalles((int)ModoPaginacion.Siguiente);
        }

        private void BotonAnteriorDetalles_Click(object sender, RoutedEventArgs e)
        {
            PaginacionDetalles((int)ModoPaginacion.Anterior);
        }

        private void BotonUltimoDetalles_Click(object sender, RoutedEventArgs e)
        {
            PaginacionDetalles((int)ModoPaginacion.Ultimo);
        }

        protected internal class ManejadorEventos : IEventHandler
        {
            public virtual void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage pagina = docEvent.GetPage();
                int cantPag = pdfDoc.GetNumberOfPages();
                int numeroPag = pdfDoc.GetPageNumber(pagina);
                
                PdfCanvas pdfCanvas = new PdfCanvas(pagina.NewContentStreamBefore(), pagina.GetResources(), pdfDoc);
                pdfCanvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.COURIER), 10)
                    .MoveText((pagina.GetPageSize().GetWidth()/2) - 56.69f, 40)
                    .ShowText("Página " + numeroPag + " de " + cantPag);
                pdfCanvas.Release();
            }
            internal ManejadorEventos(Pedidos _copia)
            {
                this._copia = _copia;
            }
            private readonly Pedidos _copia;
        }

        private void GenerarPdf()
        {
            DataTable dtPedidoCompleto = new DataTable("Pedido_Completo");


            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = miConexionSql.CreateCommand();
            try
            {

                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_Ver_Pedidos";
                miComandoSql.Parameters.AddWithValue("@numeroPedido", idPedido);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);
                miAdaptadorSql.Fill(dtPedidoCompleto);
                miAdaptadorSql.Dispose();
                miComandoSql.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Conexion.Dispose(miConexionSql);

            string fecha = dtPedidoCompleto.Rows[0][2] != null ? dtPedidoCompleto.Rows[0][2].ToString() : String.Empty;
            string proveedor = dtPedidoCompleto.Rows[0][1] != null ? dtPedidoCompleto.Rows[0][1].ToString() : String.Empty;


            string nombreArchivo = @"H:\Reportes\Pedido_" + idPedido + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".pdf";
            PdfWriter pdfWriter = new PdfWriter(nombreArchivo);
            PdfDocument docPdf = new PdfDocument(pdfWriter);
            docPdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new Pedidos.ManejadorEventos(this));
            PageSize ps = PageSize.A4;
            PdfPage pagina = docPdf.AddNewPage(ps);
            Document documento = new Document(docPdf, PageSize.A4);
            documento.SetMargins(56.69f, 56.69f, 56.69f, 56.69f);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

            Table tablaDetalles = new Table(UnitValue.CreatePercentArray(new float[] { 1, 0.7f, 0.4f, 1 })).UseAllAvailableWidth();

            parrafo nuevaLinea = new parrafo(new Text("\n")).SetFontSize(10);

            LineSeparator lineaHorizontal = new LineSeparator(new SolidLine());

            parrafo original = new parrafo("ORIGINAL").SetTextAlignment(alineacion.CENTER).SetFont(bold).SetFontSize(12);

            documento.Add(lineaHorizontal);
            documento.Add(nuevaLinea);
            rectangulo rec = new rectangulo(57.69f, 700.1f, 110, 60);
            PdfCanvas canvas = new PdfCanvas(pagina).Rectangle(56.69f, 641, 240.9f, 120);
           
            canvas.Rectangle(297.59f, 641, 240.9f, 120);
            canvas.Stroke();
            canvas.AddImageFittedIntoRectangle(ImageDataFactory.Create(@"..\..\Recursos\Fondo.jpg"), rec, true);
            canvas.BeginText().SetFontAndSize(bold, 14).MoveText(60, 688).ShowText("La Librería").EndText();
            canvas.BeginText().SetFontAndSize(font, 10).MoveText(60, 678).ShowText("Venta de libros y revistas").EndText();
            canvas.BeginText().SetFontAndSize(font, 7).MoveText(60, 670).ShowText("Corrientes 1234 - CABA - CP 1017").EndText();
            canvas.BeginText().SetFontAndSize(font, 7).MoveText(60, 660).ShowText("Tel: 1234-5678").EndText();
            canvas.BeginText().SetFontAndSize(font, 7).MoveText(60, 650).ShowText("Email: pedidos@lalibreria.com.ar").EndText();
            
            canvas.BeginText().SetFontAndSize(bold, 16).MoveText(ps.GetWidth() / 2 + (ps.GetWidth() / 4 - 56.69f), 748).ShowText("PEDIDO").EndText();
            canvas.BeginText().SetFontAndSize(font, 11).MoveText(ps.GetWidth() / 2 + 10, 728).ShowText("Nº: " + idPedido).EndText();
            canvas.BeginText().SetFontAndSize(font, 11).MoveText(ps.GetWidth() / 2 + 10, 714).ShowText("Fecha: " + fecha).EndText();
            canvas.BeginText().SetFontAndSize(font, 11).MoveText(ps.GetWidth() / 2 + 10, 700).ShowText("Proveedor: ").EndText();

            canvas.Release();
            Table proveed = new Table(1);
            proveed.SetFixedPosition(375, 675, 150);
            proveed.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).SetFont(font).SetFontSize(11).Add(new parrafo(proveedor)).SetBorder(borde.NO_BORDER));

            documento.Add(proveed);

            documento.Add(nuevaLinea);
            documento.Add(nuevaLinea);
            documento.Add(nuevaLinea);
            documento.Add(nuevaLinea);
            documento.Add(nuevaLinea);
            documento.Add(nuevaLinea);

            Cell celda3 = new Cell(0, 1).Add(new parrafo("Titulo")).SetFont(font).SetFontSize(11).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 1));
            tablaDetalles.AddHeaderCell(celda3);

            Cell celda4 = new Cell(1, 1).Add(new parrafo("ISBN")).SetFont(font).SetFontSize(11).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 1));
            tablaDetalles.AddHeaderCell(celda4);

            Cell celda5 = new Cell(2, 1).Add(new parrafo("Cantidad")).SetFont(font).SetFontSize(11).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 1));
            tablaDetalles.AddHeaderCell(celda5);

            Cell celda6 = new Cell(3, 1).Add(new parrafo("Editorial")).SetFont(font).SetFontSize(11).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 1));
            tablaDetalles.AddHeaderCell(celda6);


            for (int i = 0; i < dtPedidoCompleto.Rows.Count; i++)
            {

                tablaDetalles.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).SetFontSize(10).Add(new parrafo(
                    dtPedidoCompleto.Rows[i][5] != null ? dtPedidoCompleto.Rows[i][5].ToString() : String.Empty)));
                tablaDetalles.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).SetFontSize(10).Add(new parrafo(
                    dtPedidoCompleto.Rows[i][3] != null ? dtPedidoCompleto.Rows[i][3].ToString() : String.Empty)));
                tablaDetalles.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).SetFontSize(10).Add(new parrafo(
                    dtPedidoCompleto.Rows[i][4] != null ? dtPedidoCompleto.Rows[i][4].ToString() : String.Empty)));
                tablaDetalles.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).SetFontSize(10).Add(new parrafo(
                    dtPedidoCompleto.Rows[i][6] != null ? dtPedidoCompleto.Rows[i][6].ToString() : String.Empty)));
            }
            documento.Add(tablaDetalles);
            
            documento.Close();
            System.Diagnostics.Process.Start(nombreArchivo);
        }

    }

}
