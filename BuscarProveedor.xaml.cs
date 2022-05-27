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
    /// Interaction logic for BuscarProveedor.xaml
    /// </summary>
    public partial class BuscarProveedor : Window
    {
        // Contenedor global de datos
        DataTable dtProveedores = new DataTable("Proveedores");

        // Guarda el número de página actual
        private int paginacion_IndicePag = 1;

        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_NumRegistrosPagina = 8;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero = 1, Siguiente = 2, Anterior = 3, Ultimo = 4 };

        private string columna = "";

        private string idProveedor = "";
        private string nombre = "";
        private string razonSocial = "";
        private string direccion = "";
        private string codPostal = "";
        private string telefono = "";
        private string email = "";

     public BuscarProveedor()
        {
            InitializeComponent();
        }

        private void PaginacionPersonalizada(int modo)
        {
            int totalRegistros = dtProveedores.Rows.Count;
            int tamanioPagina = paginacion_NumRegistrosPagina;

            if (totalRegistros <= tamanioPagina)
            {
                return;
            }

            switch (modo)
            {
                case (int)ModoPaginacion.Siguiente:
                    if (totalRegistros > (paginacion_IndicePag * tamanioPagina))
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtProveedores.Clone();
                        if (totalRegistros >= ((paginacion_IndicePag * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = paginacion_IndicePag * tamanioPagina; i < ((paginacion_IndicePag * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtProveedores.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = paginacion_IndicePag * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtProveedores.Rows[i]);
                            }
                        }

                        paginacion_IndicePag += 1;
                        listaProveedores.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (paginacion_IndicePag > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtProveedores.Clone();

                        paginacion_IndicePag -= 1;

                        for (int i = ((paginacion_IndicePag * tamanioPagina) - tamanioPagina); i < (paginacion_IndicePag * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtProveedores.Rows[i]);
                        }

                        listaProveedores.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Primero:
                    paginacion_IndicePag = 2;
                    PaginacionPersonalizada((int)ModoPaginacion.Anterior);
                    break;
                case (int)ModoPaginacion.Ultimo:
                    paginacion_IndicePag = (totalRegistros % tamanioPagina == 0) ? ((totalRegistros / tamanioPagina) - 1) : (totalRegistros / tamanioPagina);
                    PaginacionPersonalizada((int)ModoPaginacion.Siguiente);
                    break;
            }
            MostrarInfoPaginacion();

        }

        private void MostrarInfoPaginacion()
        {
            int totalRegistros = dtProveedores.Rows.Count;
            int tamanioPagina = paginacion_NumRegistrosPagina;

            string infoPaginacion = paginacion_IndicePag * tamanioPagina + " de " + totalRegistros;

            if (dtProveedores.Rows.Count < (paginacion_IndicePag * tamanioPagina))
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

        private void Refresh()
        {
            dtProveedores.Clear();
            paginacion_IndicePag = 1;
            MostrarInfoPaginacion();
        }

        private void Consulta()
        {
            string consulta = "";
            if (parametroBusqueda.Text == "")
            {
                consulta = "SELECT * FROM Proveedores";
            }
            else
            {
                consulta = "SELECT * FROM Proveedores WHERE Nombre" + " LIKE '%" + parametroBusqueda.Text + "%'";
            }
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            try
            {
                Refresh();

                SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                try
                {
                    miAdaptadorSql.Fill(dtProveedores);
                    if (dtProveedores.Rows.Count > 0)
                    {
                        DataTable tablaTemp = new DataTable();

                        // Copio los datos de la tabla a un temporal
                        tablaTemp = dtProveedores.Clone();

                        /* Si la cantidad total de registros es mas grande que el tamaño de paginacion
                         * importa los registros del 0 al tamaño de paginacion
                         * sino importa los registros del 0 al total
                         */
                        if (dtProveedores.Rows.Count >= paginacion_NumRegistrosPagina)
                        {
                            for (int i = 0; i < paginacion_NumRegistrosPagina; i++)
                            {
                                tablaTemp.ImportRow(dtProveedores.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtProveedores.Rows.Count; i++)
                            {
                                tablaTemp.ImportRow(dtProveedores.Rows[i]);
                            }
                        }

                        //Enlazo la tabla con el gridview
                        listaProveedores.DataContext = tablaTemp.DefaultView;

                        //Elimino la tabla temporal
                        tablaTemp.Dispose();

                    }
                    else
                    {
                        Refresh();
                        listaProveedores.DataContext = null;
                        string mensaje = "No seleccionó una opción válida";
                        MessageBox.Show(mensaje);
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
            catch (Exception e2)
            {
                MessageBox.Show(e2.ToString());
            }
            Conexion.Dispose(miConexionSql);
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            Consulta();
            listaProveedores.SelectedItems.Clear();
            botonModificar.IsEnabled = false;
            botonBorrar.IsEnabled = false;
            botonEditoriales.IsEnabled = false;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {

            ModificarProveedor ventanaModificarProveedor = new ModificarProveedor(idProveedor);
            ventanaModificarProveedor.Show();

            ventanaModificarProveedor.textNombre.Text = nombre;
            ventanaModificarProveedor.textRazonSocial.Text = razonSocial;
            ventanaModificarProveedor.textDireccion.Text = direccion;
            ventanaModificarProveedor.textCodPostal.Text = codPostal;
            ventanaModificarProveedor.textTelefono.Text = telefono;
            ventanaModificarProveedor.textEmail.Text = email;
            ventanaModificarProveedor.CambioModificaciones = false;
            ventanaModificarProveedor.Closed += (o, args) =>
            {
                Consulta();
            };
        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            BorrarProveedor ventanaBorrarProveedor = new BorrarProveedor(idProveedor);
            ventanaBorrarProveedor.Show();

            ventanaBorrarProveedor.textNombre.Text = nombre;
            ventanaBorrarProveedor.textRazonSocial.Text = razonSocial;
            ventanaBorrarProveedor.textDireccion.Text = direccion;
            ventanaBorrarProveedor.textCodPostal.Text = codPostal;
            ventanaBorrarProveedor.textTelefono.Text = telefono;
            ventanaBorrarProveedor.textEmail.Text = email;
            ventanaBorrarProveedor.Closed += (o, args) =>
            {
                Consulta();
            };
        }

        private void listaProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaProveedores.SelectedItems.Count > 0)
            {
                botonEditoriales.IsEnabled = true;
                if (Titulo.Content.ToString() == "Busque el proveedor a modificar")
                {
                    botonModificar.IsEnabled = true;
                }
                else if (Titulo.Content.ToString() == "Busque el proveedor a borrar")
                {
                    botonBorrar.IsEnabled = true;
                }
                
                foreach (DataRowView drv in listaProveedores.SelectedItems)
                {
                    idProveedor = drv.Row[0] != null ? drv.Row[0].ToString() : String.Empty;
                    nombre = drv.Row[1] != null ? drv.Row[1].ToString() : String.Empty;
                    razonSocial = drv.Row[2] != null ? drv.Row[2].ToString() : String.Empty;
                    direccion = drv.Row[3] != null ? drv.Row[3].ToString() : String.Empty;
                    codPostal = drv.Row[4] != null ? drv.Row[4].ToString() : String.Empty;
                    telefono = drv.Row[5] != null ? drv.Row[5].ToString() : String.Empty;
                    email = drv.Row[6] != null ? drv.Row[6].ToString() : String.Empty;
                }

                

                //string resultado = titulo + ", " + autor + ", " + editorial + ", " + isbn + ", " + edicion + ", " + anio + ", " + paginas + ", " + categoria + ", " + precio + ", " + stock;
            }

        }

        private void Editoriales_Click(object sender, RoutedEventArgs e)
        {
            Editoriales ventanaEditoriales = new Editoriales(idProveedor);
            ventanaEditoriales.Show();
        }
    }
}
