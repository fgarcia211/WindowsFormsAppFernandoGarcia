using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WindowsFormsAppFernandoGarcia.Models;

namespace WindowsFormsAppFernandoGarcia.Repositories
{
    public class RepositoryCliente
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryCliente()
        {
            this.cn = new SqlConnection("Data Source=LOCALHOST\\DESAROLLO;Initial Catalog=PRACTICAADO;Persist Security Info=True;User ID=SA;Password=MCSD2023");
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Cliente> GetAllClientes()
        {
            List<Cliente> listaclientes = new List<Cliente>();

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read())
            {
                listaclientes.Add(new Cliente(
                    this.reader["CodigoCliente"].ToString(),
                    this.reader["Empresa"].ToString(),
                    this.reader["Contacto"].ToString(),
                    this.reader["Cargo"].ToString(),
                    this.reader["Ciudad"].ToString(),
                    int.Parse(this.reader["Telefono"].ToString())
                ));
            }

            this.reader.Close();
            this.cn.Close();
            return listaclientes;
        }

        public Cliente getCliente(string id)
        {
            SqlParameter pamIDCliente = new SqlParameter("@ID", id);
            this.com.Parameters.Add(pamIDCliente);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES_ID";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            this.reader.Read();
            Cliente cliente = new Cliente(
                this.reader["CodigoCliente"].ToString(),
                this.reader["Empresa"].ToString(),
                this.reader["Contacto"].ToString(),
                this.reader["Cargo"].ToString(),
                this.reader["Ciudad"].ToString(),
                int.Parse(this.reader["Telefono"].ToString())
            );

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return cliente;
        }

        public int UpdateCliente(string codcliente, string empresa, string contacto, string cargo, string ciudad, int telefono)
        {
            SqlParameter pamIDCliente = new SqlParameter("@ID", codcliente);
            this.com.Parameters.Add(pamIDCliente);

            SqlParameter pamEmpresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamEmpresa);

            SqlParameter pamContacto = new SqlParameter("@CONTACTO", contacto);
            this.com.Parameters.Add(pamContacto);

            SqlParameter pamCargo = new SqlParameter("@CARGO", cargo);
            this.com.Parameters.Add(pamCargo);

            SqlParameter pamCiudad = new SqlParameter("@CIUDAD", ciudad);
            this.com.Parameters.Add(pamCiudad);

            SqlParameter pamTelefono = new SqlParameter("@TELEFONO", telefono);
            this.com.Parameters.Add(pamTelefono);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATE_CLIENTES";

            this.cn.Open();

            int modificados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return modificados;
        }
    }
}
