<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TodoApp.aspx.cs" Inherits="TodoApp.TodoApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>To-Do App</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h2 class="mb-4 text-center">📝 To-Do List</h2>

            <div class="input-group mb-3">
                <asp:TextBox ID="txtTask" runat="server" CssClass="form-control" Placeholder="Enter Task" />
                <asp:Button ID="btnAdd" runat="server" Text="Add Task" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
            </div>

            <asp:GridView ID="gvTasks" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="TaskID"
                OnRowEditing="gvTasks_RowEditing"
                OnRowUpdating="gvTasks_RowUpdating"
                OnRowCancelingEdit="gvTasks_RowCancelingEdit"
                OnRowCommand="gvTasks_RowCommand"
                CssClass="table table-bordered table-striped text-center">

                <Columns>
                    <asp:TemplateField HeaderText="Task">
                        <ItemTemplate>
                            <%# Eval("TaskDescription") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditTask" runat="server" Text='<%# Bind("TaskDescription") %>' CssClass="form-control" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Completed">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("IsCompleted")) ? "✔️" : "❌" %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField ShowEditButton="True" ShowCancelButton="True" UpdateText="Update" CancelText="Cancel" EditText="Edit" ButtonType="Button" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnToggle" runat="server" Text="Toggle" CssClass="btn btn-secondary btn-sm"
                                CommandName="ToggleComplete" CommandArgument="<%# Container.DataItemIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                                CommandName="DeleteTask" CommandArgument="<%# Container.DataItemIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>

</html>
