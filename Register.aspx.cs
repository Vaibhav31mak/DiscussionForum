using System;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace DiscussionForum
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string data = args.Value;
            args.IsValid = false;

            if (data.Length > 6 && data.Length < 14 && data.Any(char.IsDigit) && data.Any(char.IsLower) && data.Any(char.IsUpper))
            {
                args.IsValid = true;
            }
        }

        protected void signup_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConTest"].ConnectionString);

            try
            {
                using (con)
                {
                    con.Open();

                    // Check if the username exists
                    bool regerr = false;
                    string query1 = "SELECT uname FROM UserDetails WHERE uname = @uname";
                    using (SqlCommand cmd1 = new SqlCommand(query1, con))
                    {
                        cmd1.Parameters.AddWithValue("@uname", username.Text);
                        SqlDataReader rdr = cmd1.ExecuteReader();
                        regerr = rdr.HasRows;
                        rdr.Close(); // Make sure to close the reader
                    }

                    // Check if the email exists
                    bool regerr1 = false;
                    string query2 = "SELECT email FROM UserDetails WHERE email = @Email";
                    using (SqlCommand cmd2 = new SqlCommand(query2, con))
                    {
                        cmd2.Parameters.AddWithValue("@Email", email.Text);
                        SqlDataReader rdr1 = cmd2.ExecuteReader();
                        regerr1 = rdr1.HasRows;
                        rdr1.Close(); // Make sure to close the reader
                    }

                    // Handle registration errors
                    if (regerr)
                    {
                        registererr.Visible = true;
                        registererr.Text = "Username Already Exists";
                    }
                    else if (regerr1)
                    {
                        registererr.Visible = true;
                        registererr.Text = "Email Already Exists";
                    }
                    else
                    {
                        // If no errors, insert user into the database
                        string query = "INSERT INTO UserDetails(fname, lname, email, contact, uname, pass, university, avatar) " +
                                       "VALUES (@FName, @LName, @Email, @Contact, @Uname, @Pass, @University, @Avatar)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@FName", fname.Text);
                            cmd.Parameters.AddWithValue("@LName", lname.Text);
                            cmd.Parameters.AddWithValue("@Email", email.Text);
                            cmd.Parameters.AddWithValue("@Contact", contact.Text);
                            cmd.Parameters.AddWithValue("@Uname", username.Text);
                            cmd.Parameters.AddWithValue("@Pass", pass.Text);
                            cmd.Parameters.AddWithValue("@University", university.Text);
                            cmd.Parameters.AddWithValue("@Avatar", "Avatar/Default.jpg");

                            cmd.ExecuteNonQuery(); // Execute the insertion
                        }

                        // Redirect to login page after successful registration
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch (Exception err)
            {
                Label1.Text = "Error: " + err.Message;
            }
        }
    }
}
