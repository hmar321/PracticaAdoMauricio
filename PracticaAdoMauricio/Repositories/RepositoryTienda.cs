using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PracticaAdoMauricio.Models;
using System.Diagnostics.Metrics;

#region PROCEDIMIENTOS ALMACENADOS
//create procedure SP_INSERT_PEDIDO
//(@codigopedido nvarchar(50), @codigocliente nvarchar(50), @fechaentrega datetime, @formaenvio nvarchar(50),@importe int)
//as
//    insert into pedidos values
//	(@codigopedido, @codigocliente, @fechaentrega, @formaenvio, @importe)
//go

//create procedure SP_DELETE_PEDIDO
//(@codigopedido nvarchar(50))
//as
//    delete from pedidos where CodigoPedido = @codigopedido
//go
#endregion

namespace PracticaAdoMauricio.Repositories
{
    public class RepositoryTienda
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryTienda()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=NETCORE;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Cliente> GetClientes()
        {
            string sql = "select * from clientes";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<Cliente> clientes = new List<Cliente>();
            while (this.reader.Read())
            {
                string codigo = this.reader["CodigoCliente"].ToString();
                string emp = this.reader["Empresa"].ToString();
                string cont = this.reader["Contacto"].ToString();
                string carg = this.reader["Cargo"].ToString();
                string ciud = this.reader["Ciudad"].ToString();
                int tel = int.Parse(this.reader["Telefono"].ToString());
                Cliente cliente = new Cliente(codigo, emp, cont, carg, ciud, tel);
                clientes.Add(cliente);
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public List<Pedido> GetPedidosCliente(string codigoPedido)
        {
            string sql = "select * from pedidos where CodigoCLiente=@codigocliente";
            SqlParameter paramCodigo = new SqlParameter("@codigocliente", codigoPedido);
            this.com.Parameters.Add(paramCodigo);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<Pedido> pedidos = new List<Pedido>();
            while (this.reader.Read())
            {
                string cdPedido = this.reader["CodigoPedido"].ToString();
                string cdCli = this.reader["CodigoCliente"].ToString();
                DateTime entrega = DateTime.Parse(this.reader["FechaEntrega"].ToString());
                string formaEnv = this.reader["FormaEnvio"].ToString();
                int importe = int.Parse(this.reader["Importe"].ToString());
                Pedido pedido = new Pedido(cdPedido, cdCli, entrega, formaEnv, importe);
                pedidos.Add(pedido);
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public int InsertPedido(string cdPed, string cdCli, DateTime fchEntrega, string formaEnv, int importe)
        {
            string sql = "SP_INSERT_PEDIDO";
            SqlParameter paramCdPed = new SqlParameter("@codigopedido", cdPed);
            this.com.Parameters.Add(paramCdPed);
            SqlParameter paramCdCli = new SqlParameter("@codigocliente", cdCli);
            this.com.Parameters.Add(paramCdCli);
            SqlParameter paramFchEntre = new SqlParameter("@fechaentrega", fchEntrega);
            this.com.Parameters.Add(paramFchEntre);
            SqlParameter paramFormaEnv = new SqlParameter("@formaenvio", formaEnv);
            this.com.Parameters.Add(paramFormaEnv);
            SqlParameter paramImporte = new SqlParameter("@importe", importe);
            this.com.Parameters.Add(paramImporte);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            int result=this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }

        public int DeletePedido(string cdPed)
        {
            string sql = "SP_DELETE_PEDIDO";
            SqlParameter paramCdPed = new SqlParameter("@codigopedido", cdPed);
            this.com.Parameters.Add(paramCdPed);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }
    }
}
