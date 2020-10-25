using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace APPPPPPP
{
    public partial class BoxTarget : Form
    {
        public BoxTarget()
        {
            InitializeComponent();
        }
        string LastTarget = "";
        string connection = "Server = sql1110-lc-in.ger.corp.intel.com,3181; Database = DC_PreProd_Box_Validation; Uid = DC_PreProd_Box_Valid_rw; Password = wPyL3W1PcPe2pKs";
        SqlCommand command;
        string sql = null;

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatedV.Clear();
            SqlError.Clear();
            SqlDataReader dataReader;
            sql = $"SELECT  * FROM [DC_PreProd_Box_Validation].[dbo].[tblBox] where BoxID = {TxtBoxID.Text} ";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connection);
                sqlConnection.Open();
                command = new SqlCommand(sql, sqlConnection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    if (TxtTarget.Text == LastTarget)
                    {
                        LastTarget = (string)dataReader.GetValue(5);
                        TxtTarget.Text = (string)dataReader.GetValue(5);
                    }
                    else
                    {
                        var IsYes = MessageBox.Show($"Do you want to change the target to: {TxtTarget.Text} ?", "Changing target", MessageBoxButtons.YesNo);
                        if (IsYes == DialogResult.Yes)
                        {
                            Update_Target();
                            LastTarget = TxtTarget.Text;
                        }
                        else
                        {
                            TxtTarget.Text = (string)dataReader.GetValue(5);
                        }
                    }
                }
                if (!dataReader.HasRows)
                {
                    LastTarget = "";
                    TxtTarget.Clear();
                    SqlError.SetError(TxtBoxID, "Box not found");
                }
                dataReader.Close();
                command.Dispose();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                SqlError.SetError(TxtBoxID, ex.Message);
            }
        }
            private void Update_Target()
            {
                try
                {
                    SqlError.Clear();
                    SqlConnection sqlConnection = new SqlConnection(connection);
                    string query = "UPDATE [dbo].[tblBox] " +
                                    $" SET Target= '{TxtTarget.Text}'" +
                                    $" WHERE  boxid = {TxtBoxID.Text}";
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.Add("Target", SqlDbType.VarChar).Value = TxtTarget.Text;
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    UpdatedV.SetError(TxtTarget, "Target Updated!");
                }
                catch (Exception ex)
                {
                    SqlError.SetError(TxtBoxID, ex.Message);
                }
            } 
            private void TxtBoxID_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            private void TxtTarget_TextChanged(object sender, EventArgs e)
            {
                UpdatedV.Clear();
            }
    }
    }



