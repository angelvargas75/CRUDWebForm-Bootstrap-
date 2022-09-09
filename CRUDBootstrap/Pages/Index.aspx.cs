using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CRUDBootstrap.Pages
{
    public partial class Index : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        void CargarTabla()
        {
            SqlCommand cmd = new SqlCommand("sp_load", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvListado.DataSource = dt;
            dgvListado.DataBind();
            con.Close();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Crud.aspx?op=C");
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string id;
            string nombre;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            nombre = selectRow.Cells[2].Text;
            Response.Redirect("~/Pages/Crud.aspx?id="+ id +"&nom=" + nombre + "&op=U");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string id;
            string nombre;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            nombre = selectRow.Cells[2].Text;
            Response.Redirect("~/Pages/Crud.aspx?id=" + id + "&nom=" + nombre + "&op=D");
        }

        protected void btnBuscar_Click1(object sender, EventArgs e)
        {
            string filtro = txtFilter.Text;
            //int id = Convert.ToInt32(txtFilter.Text);
            SqlDataAdapter da = new SqlDataAdapter("sp_read", con);
            con.Open();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = filtro;
            //da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            dgvListado.DataSource = ds;
            dgvListado.DataBind();
            con.Close();
        }
    }
}