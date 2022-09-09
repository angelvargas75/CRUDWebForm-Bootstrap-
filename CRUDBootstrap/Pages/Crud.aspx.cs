using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRUDBootstrap.Pages
{
    public partial class Crud : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string nom = "";
        public static string sID = "-1";
        public static string sOpc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Habilita la validacion tradicional
            Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            // Obtener el ID
            if (!Page.IsPostBack)  // Entra cuando es un redirect
            {
                if (Request.QueryString["nom"] != null && Request.QueryString["id"] != null)
                {
                    sID = Request.QueryString["id"].ToString();
                    nom = Request.QueryString["nom"].ToString();
                    CargarDatos();
                    txtDate.TextMode = TextBoxMode.DateTime;
                }
                if (Request.QueryString["op"] != null)
                {
                    sOpc = Request.QueryString["op"].ToString();
                    switch (sOpc)
                    {
                        case "C":
                            lblTitulo.Text = "Ingresar nuevo usuario";
                            btnNuevo.Visible = true;
                            break;
                        case "R":
                            lblTitulo.Text = "Consulta de usuario";
                            break;
                        case "U":
                            lblTitulo.Text = "Actualizar usuario";
                            btnActualizar.Visible = true;
                            break;
                        case "D":
                            lblTitulo.Text = "Eliminar usuario";
                            btnEliminar.Visible = true;
                            break;
                        default:
                            lblTitulo.Text = "Opcion invalida";
                            break;
                    }
                }
            }
        }

        void CargarDatos()
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_read", con);
            con.Open();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = nom;
            da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            txtNombre.Text = row[1].ToString();
            txtEdad.Text = row[2].ToString();
            txtEmail.Text = row[3].ToString();
            DateTime date = (DateTime)row[4];
            txtDate.Text = date.ToString("dd/MM/yyyy");
            con.Close();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_create", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = txtNombre.Text;
                cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = txtEdad.Text;
                cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = txtEmail.Text;
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = txtDate.Text;
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Redirect("Index.aspx");
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: " + ex);
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_update", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = txtNombre.Text;
            cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = txtEdad.Text;
            cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = txtEmail.Text;
            cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = txtDate.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_delete", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }
}