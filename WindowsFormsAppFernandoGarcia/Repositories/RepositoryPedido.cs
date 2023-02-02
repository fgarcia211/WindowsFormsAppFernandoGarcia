using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WindowsFormsAppFernandoGarcia.Models;

namespace WindowsFormsAppFernandoGarcia.Repositories
{
    public class RepositoryPedido
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryPedido()
        {
            this.cn = new SqlConnection("Data Source=LOCALHOST\\DESAROLLO;Initial Catalog=PRACTICAADO;Persist Security Info=True;User ID=SA;Password=MCSD2023");
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Pedido> GetPedidosCliente (string idcliente)
        {
            List<Pedido> listapedidos = new List<Pedido>();

            SqlParameter pamIDCliente = new SqlParameter("@IDCLIENTE", idcliente);
            this.com.Parameters.Add(pamIDCliente);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_IDCLIENTE";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read())
            {
                listapedidos.Add(new Pedido(
                    this.reader["CodigoPedido"].ToString(),
                    this.reader["CodigoCliente"].ToString(),
                    this.reader["FechaEntrega"].ToString(),
                    this.reader["FormaEnvio"].ToString(),
                    int.Parse(this.reader["Importe"].ToString())
                ));
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return listapedidos;
        }

        public Pedido getPedido(string id)
        {
            SqlParameter pamIDPedido = new SqlParameter("@ID", id);
            this.com.Parameters.Add(pamIDPedido);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_ID";

            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            this.reader.Read();

            Pedido pedido = new Pedido(
                this.reader["CodigoPedido"].ToString(),
                this.reader["CodigoCliente"].ToString(),
                this.reader["FechaEntrega"].ToString(),
                this.reader["FormaEnvio"].ToString(),
                int.Parse(this.reader["Importe"].ToString())
            );

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();

            return pedido;
        }

        public int DeletePedido(string idpedido, string idcliente)
        {
            SqlParameter pamIDPedido = new SqlParameter("@IDPEDIDO", idpedido);
            this.com.Parameters.Add(pamIDPedido);

            SqlParameter pamIDCliente = new SqlParameter("@IDCLIENTE", idcliente);
            this.com.Parameters.Add(pamIDCliente);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PEDIDOS";

            this.cn.Open();

            int eliminados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return eliminados;
        }

        public int InsertPedidos(string codcliente, string fechaentrega, string formaenvio, int importe)
        {
            SqlParameter pamIDCliente = new SqlParameter("@IDCLIENTE", codcliente);
            this.com.Parameters.Add(pamIDCliente);

            SqlParameter pamFecha = new SqlParameter("@FECHA", fechaentrega);
            this.com.Parameters.Add(pamFecha);

            SqlParameter pamEnvio = new SqlParameter("@FORMAENVIO", formaenvio);
            this.com.Parameters.Add(pamEnvio);

            SqlParameter pamImporte = new SqlParameter("@IMPORTE", importe);
            this.com.Parameters.Add(pamImporte);

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_PEDIDOS";

            this.cn.Open();

            int eliminados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return eliminados;

        }
    }
}
