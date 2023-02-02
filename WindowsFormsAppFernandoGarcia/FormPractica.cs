using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppFernandoGarcia.Models;
using WindowsFormsAppFernandoGarcia.Repositories;

#region PROCEDURES_PRACTICA

/*CREATE PROCEDURE SP_CLIENTES 
AS
	SELECT * FROM CLIENTES
GO

CREATE PROCEDURE SP_CLIENTES_ID (@ID NVARCHAR(50))
AS
	SELECT * FROM CLIENTES WHERE CodigoCliente = @ID
GO

CREATE PROCEDURE SP_PEDIDOS_IDCLIENTE (@IDCLIENTE NVARCHAR(50)) 
AS
	SELECT * FROM PEDIDOS 
	WHERE CodigoCliente = @IDCLIENTE
GO

CREATE PROCEDURE SP_PEDIDOS_ID (@ID NVARCHAR(50))
AS
	SELECT * FROM PEDIDOS 
	WHERE CodigoPedido = @ID
GO

CREATE PROCEDURE SP_UPDATE_CLIENTES (@ID NVARCHAR(50), @EMPRESA NVARCHAR(50), @CONTACTO NVARCHAR(50), @CARGO NVARCHAR(50), @CIUDAD NVARCHAR(50), @TELEFONO INT)
AS
	UPDATE CLIENTES SET EMPRESA = @EMPRESA, CONTACTO = @CONTACTO, CARGO = @CARGO, CIUDAD = @CIUDAD, TELEFONO = @TELEFONO
	WHERE CodigoCliente = @ID
GO

CREATE PROCEDURE SP_INSERT_PEDIDOS (@IDCLIENTE NVARCHAR(50), @FECHA NVARCHAR(50), @FORMAENVIO NVARCHAR(50),@IMPORTE INT)
AS
	INSERT INTO PEDIDOS VALUES (CONVERT (date, SYSDATETIME()), @IDCLIENTE, CONVERT (datetime, @FECHA), @FORMAENVIO, @IMPORTE)
GO

CREATE PROCEDURE SP_DELETE_PEDIDOS (@IDPEDIDO NVARCHAR(50), @IDCLIENTE NVARCHAR(50))
AS
	DELETE FROM PEDIDOS
	WHERE CodigoPedido = @IDPEDIDO
	AND CodigoCliente = @IDCLIENTE
GO*/

#endregion

namespace WindowsFormsAppFernandoGarcia
{
    public partial class FormPractica : Form
    {
        List<Cliente> listaClientes;
        List<Pedido> listaPedidos;
        RepositoryCliente repoCliente;
        RepositoryPedido repoPedido;

        public FormPractica()
        {
            InitializeComponent();
            listaClientes = new List<Cliente>();
            listaPedidos = new List<Pedido>();
            repoCliente = new RepositoryCliente();
            repoPedido = new RepositoryPedido();
            this.CargaClientes();

        }

        private void CargaClientes()
        {
            listaClientes = repoCliente.GetAllClientes();

            this.cmbclientes.Items.Clear();

            foreach (Cliente cliente in listaClientes)
            {
                this.cmbclientes.Items.Add(cliente.Empresa);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;

            if (indice != -1 )
            {
                this.cargaDatosCliente(this.listaClientes[indice].CodCliente);
                this.cargaPedidosCliente(this.listaClientes[indice].CodCliente);
            }
        }

        private void cargaDatosCliente(string codcliente)
        {
            Cliente cliente = repoCliente.getCliente(codcliente);

            this.txtempresa.Text = cliente.Empresa;
            this.txtcargo.Text = cliente.Cargo;
            this.txtciudad.Text = cliente.Ciudad;
            this.txtcontacto.Text = cliente.Contacto;
            this.txttelefono.Text = cliente.Telefono.ToString();
        }

        private void cargaPedidosCliente(string codcliente)
        {
            listaPedidos = repoPedido.GetPedidosCliente(codcliente);

            this.lstpedidos.Items.Clear();
            
            foreach (Pedido pedido in listaPedidos)
            {
                this.lstpedidos.Items.Add(pedido.CodPedido);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = this.lstpedidos.SelectedIndex;

            if ( indice != -1 )
            {
                this.cargaDatosPedido(this.listaPedidos[indice].CodPedido);
            }
        }

        private void cargaDatosPedido(string codpedido)
        {
            Pedido pedido = repoPedido.getPedido(codpedido);

            this.txtcodigopedido.Text = pedido.CodPedido;
            this.txtfechaentrega.Text = pedido.FechaEntrega;
            this.txtformaenvio.Text = pedido.FormaEnvio;
            this.txtimporte.Text = pedido.Importe.ToString();
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;

            if (indice != -1)
            {
                Cliente clientemod = this.listaClientes[indice];

                repoCliente.UpdateCliente(
                    clientemod.CodCliente, 
                    this.txtempresa.Text, 
                    this.txtcontacto.Text,
                    this.txtcargo.Text, 
                    this.txtciudad.Text, 
                    int.Parse(this.txttelefono.Text)
                );

                this.CargaClientes();

                this.lstpedidos.Items.Clear();

                this.LimpiaInput();
            }
        }

        private void LimpiaInput()
        {
            this.txtempresa.Text = "";
            this.txtcontacto.Text = "";
            this.txtcargo.Text = "";
            this.txtciudad.Text = "";
            this.txttelefono.Text = "";
            this.txtcodigopedido.Text = "";
            this.txtfechaentrega.Text = "";
            this.txtformaenvio.Text = ""; 
            this.txtimporte.Text = ""; 
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            int indicePedido = this.lstpedidos.SelectedIndex;
            int indiceCliente = this.cmbclientes.SelectedIndex;

            if (indicePedido != -1 && indiceCliente != -1)
            {
                int borrado = repoPedido.DeletePedido(listaPedidos[indicePedido].CodPedido, listaClientes[indiceCliente].CodCliente);

                if (borrado == 1)
                {
                    MessageBox.Show("El pedido se ha borrado correctamente");

                    this.txtcodigopedido.Text = "";
                    this.txtfechaentrega.Text = "";
                    this.txtformaenvio.Text = "";
                    this.txtimporte.Text = "";

                    this.cargaPedidosCliente(listaClientes[indiceCliente].CodCliente);
                }
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            int indiceCliente = this.cmbclientes.SelectedIndex;

            if (indiceCliente != -1)
            {
                int insertado = repoPedido.InsertPedidos(listaClientes[indiceCliente].CodCliente, this.txtfechaentrega.Text, this.txtformaenvio.Text, int.Parse(this.txttelefono.Text));

                this.cargaPedidosCliente(listaClientes[indiceCliente].CodCliente);
            }
        }
    }
}
