using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Security.Permissions;
using System.Security.Principal;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;


namespace WindowsFormsApplication1
{
    public partial class Konnectify : Form
    {
        bool connect = false;
        public Konnectify()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                RestartElevated();
            }
            SqlConnection cn2 = new SqlConnection(global::WindowsFormsApplication1.Properties.Settings.Default.Database1ConnectionString);

            try
            {
                string sql2 = "SELECT Ssid FROM History WHERE Id=1";
                string sql3 = "SELECT [Key] FROM History WHERE Id=1";
                SqlCommand exeSql2 = new SqlCommand(sql2, cn2);
                SqlCommand exeSql3 = new SqlCommand(sql3, cn2);
                cn2.Open();
                username.Text = (string)exeSql2.ExecuteScalar();
                password.Text = (string)exeSql3.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn2.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cn2 = new SqlConnection(global::WindowsFormsApplication1.Properties.Settings.Default.Database1ConnectionString);

            try
            {
                string sql2 = "SELECT Ssid FROM History WHERE Id=1";
                string sql3 = "SELECT [Key] FROM History WHERE Id=1";
                SqlCommand exeSql2 = new SqlCommand(sql2, cn2);
                SqlCommand exeSql3 = new SqlCommand(sql3, cn2);
                cn2.Open();
                username.Text = (string)exeSql2.ExecuteScalar();
                password.Text = (string)exeSql3.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn2.Close();
            }
            /*SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Kunal\Documents\Data.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) From Login where Username='"+ username.Text + "' and Password = '" + password.Text + "'",con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Wow!!!");
            }*/
            string ssid = username.Text, key = password.Text;
            if (!connect)
            {
                if (username.Text == null || username.Text == "")
                {
                    MessageBox.Show("SSID cannot be left blank !",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (password.Text == null || password.Text == "")
                    {
                        MessageBox.Show("Key value cannot be left blank !",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (key.Length >= 6)
                        {
                            Zedfi_Hotspot(ssid, key, true);
                            username.Enabled = false;
                            password.Enabled = false;
                            button1.Text = "Stop";
                            connect = true;
                        }
                        else
                        {
                            MessageBox.Show("Key should be more then or Equal to 6 Characters !",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                Zedfi_Hotspot(null, null, false);
                username.Enabled = true;
                password.Enabled = true;
                button1.Text = "Start";
                connect = false;
            }
        }
        private void Zedfi_Hotspot(string ssid, string key,bool status)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);

            if (process != null)
            {
                if (status)
                {
                    process.StandardInput.WriteLine("netsh wlan stop hostednetwork");
                    process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + ssid + " key=" + key);
                    process.StandardInput.WriteLine("netsh wlan start hosted network");
                    process.StandardInput.Close();
                }
                else
                {
                    process.StandardInput.WriteLine("netsh wlan stop hostednetwork");
                    process.StandardInput.Close();
                }
            }
        }
        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public void RestartElevated()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
            }
            catch
            {

            }

            System.Windows.Forms.Application.Exit();
        }

        private void save_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(global::WindowsFormsApplication1.Properties.Settings.Default.Database1ConnectionString);

            try
            {
                string sql = "INSERT INTO History (Id,Ssid,[Key]) VALUES (1,'" + username.Text + "','" + password.Text + "')";
                string sql2 = "UPDATE History SET Ssid='"+username.Text+"', [Key]='"+password.Text+"' WHERE Id=1;";
                SqlCommand exeSql = new SqlCommand(sql, cn);
                SqlCommand exeSql2 = new SqlCommand(sql2, cn);
                cn.Open();
                exeSql.ExecuteNonQuery();
                exeSql2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errorz", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection cn2 = new SqlConnection(global::WindowsFormsApplication1.Properties.Settings.Default.Database1ConnectionString);

            try
            {
                string sql2 = "SELECT Ssid FROM History WHERE Id=1";
                string sql3 = "SELECT [Key] FROM History WHERE Id=1";
                SqlCommand exeSql2 = new SqlCommand(sql2, cn2);
                SqlCommand exeSql3 = new SqlCommand(sql3, cn2);
                cn2.Open();
                username.Text = (string)exeSql2.ExecuteScalar();
                password.Text = (string)exeSql3.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn2.Close();
            }
        }

    }
}
