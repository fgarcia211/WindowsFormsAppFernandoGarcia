using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppFernandoGarcia.Models
{
    public class Pedido
    {
        public string CodPedido { get; set; }
        public string CodCliente { get; set; }
        public string FechaEntrega { get; set; }
        public string FormaEnvio { get; set; }
        public int Importe { get; set; }

        public Pedido(string codpedido, string codcliente, string fechaentrega, string formaenvio, int importe)
        {
            this.CodPedido = codpedido;
            this.CodCliente = codcliente;
            this.FechaEntrega = fechaentrega;
            this.FormaEnvio = formaenvio;
            this.Importe = importe;
        }

        public Pedido()
        {

        }
    }
}
