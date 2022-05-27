using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using ConexionSQL;
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
using System.Data;
using System.Windows;

namespace GeneraPDF
{
    
    class GenerarPDF
    {
        

        public static void GenerarPdf(int pruaba)
        {
            DataTable dtPedidoCompleto = new DataTable("Pedido_Completo");
            MessageBox.Show(pruaba.ToString());
            string nombreArchivo = @"H:\Reportes\pruebaLibreria_" + DateTime.Now.ToString("s") + ".pdf";
            PdfWriter pdfWriter = new PdfWriter(nombreArchivo);
            PdfDocument docPdf = new PdfDocument(pdfWriter);
            Document documento = new Document(docPdf, PageSize.A4);
            documento.SetMargins(15, 15, 15, 15);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
            Table tabla = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4, 3, 3, 2, 4 })).UseAllAvailableWidth();

            SqlConnection miConexionSql = Conexion.GetConexionSql();
            SqlCommand miComandoSql = miConexionSql.CreateCommand();
            try
            {

                miComandoSql.CommandType = CommandType.StoredProcedure;
                miComandoSql.CommandText = "SP_Ver_Pedidos";
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

            Cell celda1 = new Cell(1, 1).Add(new Paragraph("Nº Pedido")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda1);

            Cell celda2 = new Cell(1, 2).Add(new Paragraph("Fecha")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda2);

            Cell celda3 = new Cell(1, 3).Add(new Paragraph("Titulo")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda3);

            Cell celda4 = new Cell(1, 4).Add(new Paragraph("ISBN")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda4);

            Cell celda5 = new Cell(1, 5).Add(new Paragraph("Cantidad")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda5);

            Cell celda6 = new Cell(1, 6).Add(new Paragraph("Proveedor")).SetFont(font).SetFontSize(13).SetFontColor(DeviceGray.BLACK).
                SetBackgroundColor(DeviceGray.GRAY).SetTextAlignment(alineacion.CENTER).SetBorder(new SolidBorder(ColorConstants.GRAY, 2));
            tabla.AddHeaderCell(celda6);

            /*foreach (DataRowView drv in dtPedidoCompleto)
            {
                idPedido = dataRow.Row[0] != null ? dataRow[0].ToString() : String.Empty;
            }*/

            for (int i=0; i < dtPedidoCompleto.Rows.Count; i++)
            {
                tabla.AddCell(new Cell().SetTextAlignment(alineacion.CENTER).Add(new Paragraph((string)dtPedidoCompleto.Rows[i][0])));

            }

            documento.Add(tabla);
            documento.Close();
        }
    }
}
