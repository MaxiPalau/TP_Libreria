using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionSQL
{
    public class Conexion
    {
        public static SqlConnection GetConexionSql()
        {
            string miConexion = ConfigurationManager.ConnectionStrings["Libreria.Properties.Settings.BD_Practica_2ConnectionString"].ConnectionString;
            SqlConnection miConexionSql = new SqlConnection(miConexion);
            miConexionSql.Open();
            return miConexionSql;
        }

        public static void Dispose(SqlConnection miConexion)
        {
            if(miConexion.State == System.Data.ConnectionState.Open)
            {
                miConexion.Close();
            }
            miConexion.Dispose();
        }
    }
}
