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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            Response.Redirect("~/Pages/Crud.aspx?id=" + id + "&op=R");
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            Response.Redirect("~/Pages/Crud.aspx?id=" + id + "&op=U");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string id;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            Response.Redirect("~/Pages/Crud.aspx?id=" + id + "&op=D");
        }
    }
}