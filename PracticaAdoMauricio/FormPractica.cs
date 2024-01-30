using PracticaAdoMauricio.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PracticaAdoMauricio.Models;

namespace PracticaAdoMauricio
{
    public partial class FormPractica : Form
    {
        RepositoryTienda repo;
        List<Cliente> clientes;
        List<Pedido> pedidos;
        public FormPractica()
        {
            InitializeComponent();
            clientes = new List<Cliente>();
            this.repo = new RepositoryTienda();
            this.CargarClientes();
        }

        private void CargarClientes()
        {
            this.clientes = this.repo.GetClientes();
            this.cmbclientes.Items.Clear();
            foreach (Cliente cliente in this.clientes)
            {
                this.cmbclientes.Items.Add(cliente.Empresa);
            }
        }

        private void CargarPedidos(string codigoCliente)
        {
            this.pedidos = this.repo.GetPedidosCliente(codigoCliente);
            this.lstpedidos.Items.Clear();
            foreach (Pedido pedido in this.pedidos)
            {
                this.lstpedidos.Items.Add(pedido.CodigoPedido);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.cmbclientes.SelectedIndex;
            if (index != -1)
            {
                Cliente cliente = this.clientes[index];
                this.txtempresa.Text = cliente.Empresa;
                this.txtcontacto.Text = cliente.Contacto;
                this.txtcargo.Text = cliente.Cargo;
                this.txtciudad.Text = cliente.Ciudad;
                this.txttelefono.Text = cliente.Telefono + "";
                this.CargarPedidos(cliente.CodigoCliente);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lstpedidos.SelectedIndex;
            if (index != -1)
            {
                Pedido pedido = this.pedidos[index];
                this.txtcodigopedido.Text = pedido.CodigoPedido;
                this.txtfechaentrega.Text = pedido.FechaEntrega.ToString("dd/MM/yyyy");
                this.txtformaenvio.Text = pedido.FormaEnvio;
                this.txtimporte.Text = pedido.Importe + "";
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            int index = this.cmbclientes.SelectedIndex;
            if (index != -1)
            {
                string codigoCliente = this.clientes[index].CodigoCliente;
                string codigoPedido = this.txtcodigopedido.Text;
                DateTime fechaEntrega = DateTime.Parse(this.txtfechaentrega.Text);
                string formaEnvio = this.txtformaenvio.Text;
                int importe = int.Parse(this.txtimporte.Text);
                int resultado = this.repo.InsertPedido(codigoPedido, codigoCliente, fechaEntrega, formaEnvio, importe);
                MessageBox.Show("Se ha insertado " + resultado + " registro");
                this.CargarPedidos(codigoCliente);
            }
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            int index = this.lstpedidos.SelectedIndex;
            if (index != -1)
            {
                Pedido pedido = this.pedidos[index];
                string codigoPedido = pedido.CodigoPedido;
                int resultado = this.repo.DeletePedido(codigoPedido);
                MessageBox.Show("Se ha borrado " + resultado + " registro");
                this.CargarPedidos(pedido.CodigoCliente);
            }
        }
    }
}
