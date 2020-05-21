using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConsolaScrap.CAPADATOS
{
    class Conexion
    {
        static private string cadenaConexion = "Data Source=LAPTOP-0EN52ISV;Initial Catalog=Scrap;Integrated Security=SSPI";
          
        private SqlConnection con = new SqlConnection(cadenaConexion);
        
        public SqlConnection Conectar()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }

        public SqlConnection CerrarCon()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            return con;
        }
    }
}
