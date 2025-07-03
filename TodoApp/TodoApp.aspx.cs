using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace TodoApp
{
    public partial class TodoApp : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ToDoConn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTasks();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "INSERT INTO ToDoTasks (TaskDescription) VALUES (@TaskDescription)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TaskDescription", txtTask.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                txtTask.Text = string.Empty;
                LoadTasks();
            }
        }
        void LoadTasks()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM ToDoTasks";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTasks.DataSource = dt;
                gvTasks.DataBind();
            }
        }

        protected void gvTasks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvTasks.Rows[e.RowIndex];
            TextBox txtTask = (TextBox)row.FindControl("txtEditTask");

            string newDesc = txtTask.Text.Trim();

            ExecuteQuery("UPDATE ToDoTasks SET TaskDescription = @desc WHERE TaskID = @id",
                new SqlParameter("@desc", newDesc),
                new SqlParameter("@id", taskId));

            gvTasks.EditIndex = -1;
            LoadTasks();
        }

        protected void gvTasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTasks.EditIndex = -1;
            LoadTasks();
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
      
            if (e.CommandName == "ToggleComplete" || e.CommandName == "DeleteTask")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTasks.Rows[index];
                int taskId = Convert.ToInt32(gvTasks.DataKeys[index].Value);

                if (e.CommandName == "ToggleComplete")
                {
                    string query = "SELECT IsCompleted FROM ToDoTasks WHERE TaskID = @id";
                    bool isCompleted = false;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", taskId);
                        con.Open();
                        isCompleted = Convert.ToBoolean(cmd.ExecuteScalar());
                        con.Close();
                    }

                    ExecuteQuery("UPDATE ToDoTasks SET IsCompleted = @status WHERE TaskID = @id",
                        new SqlParameter("@status", !isCompleted),
                        new SqlParameter("@id", taskId));
                }

                if (e.CommandName == "DeleteTask")
                {
                    ExecuteQuery("DELETE FROM ToDoTasks WHERE TaskID = @id", new SqlParameter("@id", taskId));
                }

                LoadTasks();
            }
        }
        void ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void gvTasks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTasks.EditIndex = e.NewEditIndex;
            LoadTasks();
        }

    }
}