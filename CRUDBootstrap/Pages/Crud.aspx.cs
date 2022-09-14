using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
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
                sOpc = Decrypt(HttpUtility.UrlDecode(Request.QueryString["op"]));
                if (sOpc != null && sOpc.Equals("C"))
                {
                    lblTitulo.Text = "Ingresar nuevo usuario";
                    btnNuevo.Visible = true;
                    return;
                }
                sID = Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));
                nom = Decrypt(HttpUtility.UrlDecode(Request.QueryString["nom"]));
                if (sID != null && nom != null)
                {
                    CargarDatos();
                    txtDate.TextMode = TextBoxMode.DateTime;
                }
                if (sOpc != null)
                {
                    switch (sOpc)
                    {
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
                Response.Redirect("Index.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: " + ex);
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
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
                Response.Redirect("Index.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: " + ex);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_delete", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
                cmd.ExecuteNonQuery();
                con.Close();
                Response.Redirect("Index.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: " + ex);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }



        // Metodo para desencriptar parametros QueryString
        private string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}