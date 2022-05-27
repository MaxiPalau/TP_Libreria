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
    /// Interaction logic for Editoriales.xaml
    /// </summary>
    public partial class Editoriales : Window
    {
        private string provId;
        private DataTable dtEditoriales = new DataTable("Editoriales");

        // Guarda el número de página actual
        private int paginacion_IndicePag = 1;

        // Guarda el tamaño de paginación, cantidad de registros que muestra
        private int paginacion_NumRegistrosPagina = 8;

        // Seleccionar el modo de paginacion segun la direccion deseada
        private enum ModoPaginacion { Primero = 1, Siguiente = 2, Anterior = 3, Ultimo = 4 };

        public Editoriales(string idProv)
        {
            InitializeComponent();
            this.provId = idProv;
            BuscarEditoriales();
        }

        private void BuscarEditoriales()
        {
            string consulta = "SELECT Editorial FROM Editoriales WHERE IdProveedor = @provId"; 
            SqlConnection miConexionSql = Conexion.GetConexionSql();
            try
            {
                //Refresh();

                SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);
                miComandoSql.Parameters.AddWithValue("provId", provId);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                try
                {
                    miAdaptadorSql.Fill(dtEditoriales);
                    if (dtEditoriales.Rows.Count > 0)
                    {
                        DataTable tablaTemp = new DataTable();

                        // Copio los datos de la tabla a un temporal
                        tablaTemp = dtEditoriales.Clone();

                        /* Si la cantidad total de registros es mas grande que el tamaño de paginacion
                         * importa los registros del 0 al tamaño de paginacion
                         * sino importa los registros del 0 al total
                         */
                        if (dtEditoriales.Rows.Count >= paginacion_NumRegistrosPagina)
                        {
                            for (int i = 0; i < paginacion_NumRegistrosPagina; i++)
                            {
                                tablaTemp.ImportRow(dtEditoriales.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtEditoriales.Rows.Count; i++)
                            {
                                tablaTemp.ImportRow(dtEditoriales.Rows[i]);
                            }
                        }

                        //Enlazo la tabla con el gridview
                        listaEditoriales.DataContext = tablaTemp.DefaultView;

                        //Elimino la tabla temporal
                        tablaTemp.Dispose();

                    }
                    else
                    {
                        //Refresh();
                        listaEditoriales.DataContext = null;
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

        private void PaginacionPersonalizada(int modo)
        {
            int totalRegistros = dtEditoriales.Rows.Count;
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
                        tablaTemp = dtEditoriales.Clone();
                        if (totalRegistros >= ((paginacion_IndicePag * tamanioPagina) + tamanioPagina))
                        {
                            for (int i = paginacion_IndicePag * tamanioPagina; i < ((paginacion_IndicePag * tamanioPagina) + tamanioPagina); i++)
                            {
                                tablaTemp.ImportRow(dtEditoriales.Rows[i]);
                            }
                        }
                        else
                        {
                            for (int i = paginacion_IndicePag * tamanioPagina; i < totalRegistros; i++)
                            {
                                tablaTemp.ImportRow(dtEditoriales.Rows[i]);
                            }
                        }

                        paginacion_IndicePag += 1;
                        listaEditoriales.DataContext = tablaTemp.DefaultView;
                        tablaTemp.Dispose();
                    }
                    break;
                case (int)ModoPaginacion.Anterior:
                    if (paginacion_IndicePag > 1)
                    {
                        DataTable tablaTemp = new DataTable();
                        tablaTemp = dtEditoriales.Clone();

                        paginacion_IndicePag -= 1;

                        for (int i = ((paginacion_IndicePag * tamanioPagina) - tamanioPagina); i < (paginacion_IndicePag * tamanioPagina); i++)
                        {
                            tablaTemp.ImportRow(dtEditoriales.Rows[i]);
                        }

                        listaEditoriales.DataContext = tablaTemp.DefaultView;
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
            int totalRegistros = dtEditoriales.Rows.Count;
            int tamanioPagina = paginacion_NumRegistrosPagina;

            string infoPaginacion = paginacion_IndicePag * tamanioPagina + " de " + totalRegistros;

            if (dtEditoriales.Rows.Count < (paginacion_IndicePag * tamanioPagina))
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

        // Boton cerrar
        private void Cerrar_Click(object sender, RoutedEventArgs e) =>
            Close();
    }
}
