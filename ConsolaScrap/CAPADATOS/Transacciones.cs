using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConsolaScrap.CAPADATOS
{
    class Transacciones
    {
        private Conexion conexion = new Conexion();
        private SqlCommand comando = new SqlCommand();
        private SqlDataReader leerDatos;


        public DataTable Listar(string sql)
        {
            DataTable Tabla = new DataTable();
            comando.Connection = conexion.Conectar();

            comando.CommandText = sql;
            comando.CommandType = CommandType.Text;

            leerDatos = comando.ExecuteReader();
            Tabla.Load(leerDatos);
            leerDatos.Close();
            conexion.CerrarCon();
            return Tabla;
        }


        public void InsertarProducto(string sql)
        {
            try
            {
                comando.Connection = conexion.Conectar();
                comando.CommandText = sql;
                comando.CommandType = CommandType.Text;
               
                comando.ExecuteNonQuery();
               
                conexion.CerrarCon();
                Console.WriteLine("----------------PRODUCTOS insertada guayyyyyyyyyyyyyyyyyyyyyyyyy");
            }
            catch (Exception e)
            {
                Console.WriteLine("Fallo en transacciones PRODUCTOS procedimiento---------------------------------------------------------" + e);
                comando.Parameters.Clear();
                conexion.CerrarCon();
            }
           
        }
        public void InsertarReseña( int idp, int idu, string reseña) 
        {
            try { 
            comando.Connection = conexion.Conectar();
            comando.CommandText = "InsertarReseña";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@idUsu", idu);
            comando.Parameters.AddWithValue("@idProd", idp);
            comando.Parameters.AddWithValue("@reseña", reseña);
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarCon();
            Console.WriteLine("-----------------reseña insertada guayyyyyyyyyyyyyyyyyyyyyyyyy");
            }catch(Exception e)
            {
                Console.WriteLine("Fallo en transacciones insertar procedimiento---------------------------------------------------------"+ e);
                comando.Parameters.Clear();
                conexion.CerrarCon();
            }
        }

        public void InsertarUsuario(string nombre)
        {
            try
            {
                comando.Connection = conexion.Conectar();
                comando.CommandText = "InsertarUsuario";
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.ExecuteNonQuery();
                comando.Parameters.Clear();
                conexion.CerrarCon();
                Console.WriteLine("-----------------USUARIOS insertada guayyyyyyyyyyyyyyyyyyyyyyyyy");
            }
            catch (Exception e)
            {
                Console.WriteLine("Fallo en transacciones USUARIOS procedimiento---------------------------------------------------------" + e);
                comando.Parameters.Clear();
                conexion.CerrarCon();
            }
        }

        public int Seleccionar(string sql) {
            SqlDataReader datos;
            comando.Connection = conexion.Conectar();
            comando.CommandText = sql;
            comando.CommandType = CommandType.Text;

            datos = comando.ExecuteReader();
            int id = 0;
            while (datos.Read())
            {
               id= ReadSingleRow((IDataRecord)datos);
            }

            datos.Close();
            conexion.CerrarCon();
            Console.WriteLine("conseguido---------------" +id);
            return id;
        }

        private static int ReadSingleRow(IDataRecord record)
        {
            string id= String.Format("{0}", record[0]);
            int idU = Int16.Parse(id);
            return idU;
        }
    }
}

