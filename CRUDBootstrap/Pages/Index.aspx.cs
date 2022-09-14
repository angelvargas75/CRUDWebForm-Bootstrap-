using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
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
            string opEncrypt = HttpUtility.UrlEncode(Encrypt("C"));
            Response.Redirect(string.Format($"~/Pages/Crud.aspx?op={opEncrypt}"));
            //Response.Redirect("~/Pages/Crud.aspx?op=C");
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string id;
            string nombre;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            nombre = selectRow.Cells[2].Text;
            string idEncrypt = HttpUtility.UrlEncode(Encrypt(id));
            string nombreEncrypt = HttpUtility.UrlEncode(Encrypt(nombre));
            string opEncrypt = HttpUtility.UrlEncode(Encrypt("U"));
            Response.Redirect(string.Format($"~/Pages/Crud.aspx?id={idEncrypt}&nom={nombreEncrypt}&op={opEncrypt}"));
            //Response.Redirect("~/Pages/Crud.aspx?id="+ id +"&nom=" + nombre + "&op=U");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string id;
            string nombre;
            Button btnConsultar = (Button)sender;
            GridViewRow selectRow = (GridViewRow)btnConsultar.NamingContainer;
            id = selectRow.Cells[1].Text;
            nombre = selectRow.Cells[2].Text;
            string idEncrypt = HttpUtility.UrlEncode(Encrypt(id));
            string nombreEncrypt = HttpUtility.UrlEncode(Encrypt(nombre));
            string opEncrypt = HttpUtility.UrlEncode(Encrypt("D"));
            Response.Redirect(string.Format($"~/Pages/Crud.aspx?id={idEncrypt}&nom={nombreEncrypt}&op={opEncrypt}"));
            //Response.Redirect("~/Pages/Crud.aspx?id=" + id + "&nom=" + nombre + "&op=D");
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


        // Metodo para encriptar parametros QueryString
        private string Encrypt(string clearText)
        {
            string encriptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pbd = new Rfc2898DeriveBytes(encriptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pbd.GetBytes(32);
                encryptor.IV = pbd.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}