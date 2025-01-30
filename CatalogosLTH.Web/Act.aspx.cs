using DevExpress.Web;
using DevExpress.Xpo.DB;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class Act : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                List<string> listaPilares = new List<string>();
                DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
                //if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)

                
                /* session.ConnectionString = ConfigurationManager.
                    ConnectionStrings["ConnectionString"].ConnectionString;*/
                SqlDataSource1.ConnectionString = MySqlConnectionProvider.GetConnectionString("172.93.106.146", "lth", "output", "lth2");

            

                MySqlConnection myConnection = new MySqlConnection("server=172.93.106.146; user id=lth; password=output; database=lth2;");

                //String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel='1';";
                String qNiv = "SELECT nombreniv, idnivel FROM mdl_nivel;";
                String qPil = "SELECT nombrepil FROM mdl_pilar;";

                //MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
                MySqlDataAdapter adapterNivel = new MySqlDataAdapter(qNiv, myConnection);
                MySqlDataAdapter adapterPilar = new MySqlDataAdapter(qPil, myConnection);

                //DataSet myDataSet = new DataSet();
                //myDataAdapter.Fill(myDataSet, "vwactividad");
                string a = DropDownList1.SelectedValue.ToString();

                //
                MySqlCommand cmdRead = new MySqlCommand("SELECT DISTINCT idpilar FROM vwactividad WHERE idnivel='1'", myConnection);
               
                myConnection.Open();

                MySqlDataReader readPilar = cmdRead.ExecuteReader();
                while (readPilar.Read())
                {
                    listaPilares.Add(readPilar.GetInt32(0).ToString());
                }

                myConnection.Close();


                foreach (string itemPilar in listaPilares)
                {
                    String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel = '1' AND idpilar=@valuepilar";

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
                  
                    myDataAdapter.SelectCommand.Parameters.Add("@valuepilar", MySqlDbType.Int32).Value = itemPilar;

                    DataSet myDataSet = new DataSet();
                    myDataAdapter.Fill(myDataSet, "vwactividad");

                    ASPxGridView1.DataSource = myDataSet;
                    ASPxGridView1.DataBind();

                    GridView cntrlGrid = new GridView();
                    cntrlGrid.ID = "grid" + itemPilar;
                    cntrlGrid.DataSource = myDataSet;

                    cntrlGrid.DataBind();


                    foreach (GridViewRow item in cntrlGrid.Rows)
                    {
                        Button btns = new Button();
                        btns.Text = "Seleccionar";

                        btns.Visible = true;



                        btns.CommandName = "cmdSelect";
                        btns.Command += btns_Command;
                        // btn_Check.Click += new EventHandler(btn_Check_Click);
                        //btns.Click += new EventHandler(btns_Click);
                        btns.Click += btns_Click;
                        TableCell tb = new TableCell();
                        tb.Controls.Add(btns);
                        item.Cells.Add(tb);
                        //item.Cells[0].Controls.Add(btns);
                    }



                    Panel1.Controls.Add(cntrlGrid);


                }
                //



                DataSet dataNivel = new DataSet();
                adapterNivel.Fill(dataNivel);
                
                if (dataNivel.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < dataNivel.Tables[0].Rows.Count; i++)
                    {
                        ListEditItem itemList = new ListEditItem();
                        ListItem itemL = new ListItem();
                        itemL.Text = dataNivel.Tables[0].Rows[i]["nombreniv"].ToString();
                        itemL.Value = dataNivel.Tables[0].Rows[i]["idnivel"].ToString();
                        itemList.Value = dataNivel.Tables[0].Rows[i]["idnivel"].ToString();
                        itemList.Text = dataNivel.Tables[0].Rows[i]["nombreniv"].ToString();

                        DropDownList1.Items.Add(itemL);

                    }

                    DropDownList1.DataMember = "nombreniv";
                    DropDownList1.DataValueField = "idnivel";
                    DropDownList1.DataBind();
                }


               // GridView1.DataSource = myDataSet;
                //GridView1.DataBind();

                //MySQLDataGrid.DataSource = myDataSet;
                //MySQLDataGrid.DataBind();


            }

            else
            {
                List<string> listaPilares = new List<string>();
                string a = DropDownList1.SelectedValue.ToString();

                MySqlConnection myConnection = new MySqlConnection("server=172.93.106.146; user id=lth; password=output; database=lth2;");

                /*EVALUA SELECCION DE NIVEL
                String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel = @value";

                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
                myDataAdapter.SelectCommand.Parameters.Add("@value", MySqlDbType.Int32).Value = a;

                DataSet myDataSet = new DataSet();
                myDataAdapter.Fill(myDataSet, "vwactividad");*/


                /*SELECCION DE PILARES*/
                MySqlCommand cmdRead = new MySqlCommand("SELECT DISTINCT idpilar FROM vwactividad WHERE idnivel=@value2", myConnection);
                cmdRead.Parameters.Add("@value2", MySqlDbType.Int32).Value = a;
                myConnection.Open();

                MySqlDataReader readPilar = cmdRead.ExecuteReader();
                while (readPilar.Read())
                {
                    listaPilares.Add(readPilar.GetInt32(0).ToString());
                }

                myConnection.Close();


                foreach (string itemPilar in listaPilares)
                {
                    String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel = @value AND idpilar=@valuepilar";

                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
                    myDataAdapter.SelectCommand.Parameters.Add("@value", MySqlDbType.Int32).Value = a;
                    myDataAdapter.SelectCommand.Parameters.Add("@valuepilar", MySqlDbType.Int32).Value = itemPilar;

                    DataSet myDataSet = new DataSet();
                    myDataAdapter.Fill(myDataSet, "vwactividad");

                    GridView cntrlGrid = new GridView();
                    cntrlGrid.ID = "grid" + itemPilar;

                    cntrlGrid.DataSource = myDataSet;

                    cntrlGrid.AutoGenerateSelectButton = true;

                    cntrlGrid.DataBind();


                    foreach (GridViewRow item in cntrlGrid.Rows)
                    {
                        Button btns = new Button();
                        btns.Text = "Seleccionar";

                        btns.Visible = true;

                        btns.CommandName = "cmdSelect";
                        btns.Command += btns_Command;
                        // btn_Check.Click += new EventHandler(btn_Check_Click);
                        //btns.Click += new EventHandler(btns_Click);
                        btns.Click += btns_Click;
                        TableCell tb = new TableCell();
                        tb.Controls.Add(btns);
                        item.Cells.Add(tb);
                        //item.Cells[0].Controls.Add(btns);
                    }

                    //cntrlGrid.DataBind();



                    Panel1.Controls.Add(cntrlGrid);

                }
            }
           
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> listaPilares = new List<string>();
            string a = DropDownList1.SelectedValue.ToString();

            MySqlConnection myConnection = new MySqlConnection("server=172.93.106.146; user id=lth; password=output; database=lth2;");

            /*EVALUA SELECCION DE NIVEL
            String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel = @value";

            MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
            myDataAdapter.SelectCommand.Parameters.Add("@value", MySqlDbType.Int32).Value = a;

            DataSet myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "vwactividad");*/


            /*SELECCION DE PILARES*/
            MySqlCommand cmdRead=new MySqlCommand("SELECT DISTINCT idpilar FROM vwactividad WHERE idnivel=@value2",myConnection);
            cmdRead.Parameters.Add("@value2", MySqlDbType.Int32).Value = a;
            myConnection.Open();

            MySqlDataReader readPilar = cmdRead.ExecuteReader();
            while (readPilar.Read())
            {
                listaPilares.Add(readPilar.GetInt32(0).ToString());
            }

            myConnection.Close();


            foreach (string itemPilar in listaPilares)
            {
                String strSQL = "SELECT Code, Texto FROM vwactividad WHERE idnivel = @value AND idpilar=@valuepilar";

                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(strSQL, myConnection);
                myDataAdapter.SelectCommand.Parameters.Add("@value", MySqlDbType.Int32).Value = a;
                myDataAdapter.SelectCommand.Parameters.Add("@valuepilar", MySqlDbType.Int32).Value = itemPilar;

                DataSet myDataSet = new DataSet();
                myDataAdapter.Fill(myDataSet, "vwactividad");

                GridView cntrlGrid = new GridView();
                cntrlGrid.ID = "grid" + itemPilar;

                cntrlGrid.DataSource = myDataSet;

                cntrlGrid.AutoGenerateSelectButton = true;

                cntrlGrid.DataBind();


                foreach (GridViewRow item in cntrlGrid.Rows)
                {
                    Button btns = new Button();
                    btns.Text = "Seleccionar";
                    
                    btns.Visible = true;
                    
                    btns.CommandName = "cmdSelect";
                    btns.Command += btns_Command;
                   // btn_Check.Click += new EventHandler(btn_Check_Click);
                    //btns.Click += new EventHandler(btns_Click);
                    btns.Click += btns_Click;
                    TableCell tb = new TableCell();
                    tb.Controls.Add(btns);
                    item.Cells.Add(tb);
                    //item.Cells[0].Controls.Add(btns);
                }
              
               //cntrlGrid.DataBind();
              
                

                Panel1.Controls.Add(cntrlGrid);
                
            }
            
            //GridView1.DataSource = myDataSet;
            //GridView1.DataBind();
           
           /* MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT Code, Texto FROM vwactividad WHERE idnivel = ?value";
            cmd.Parameters.Add("?value", MySqlDbType.Int32).Value = a;
            //cmd.Parameters.Add("?address", MySqlDbType.VarChar).Value = "myaddress";
            cmd.ExecuteNonQuery();*/
        }

        void btns_Click(object sender, EventArgs e)
        {
            string s =e.ToString();
            string sf = sender.ToString();

            var closeLink = (Control)sender;
            GridViewRow row = (GridViewRow)closeLink.NamingContainer;
            //GridViewRow row = (GridViewRow)btns.NamingContainer;
            //string firstCellText = row.Cells[0].Text; // here we are

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btns_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "cmdSelect")
            {
                //This is to test  
                LinkButton lb = (LinkButton)sender;
                lb.Text = "OK";


            }
        } 
       
    }
}