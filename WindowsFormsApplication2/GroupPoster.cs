using FBHelper.DataModel;
using FBHelper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBResponder
{
    public partial class GroupPoster : Form
    {
        public GroupPoster()
        {
            InitializeComponent();
        }


        public void LoadContent()
        {
            FB context = new FB();
            var c = from f in context.Contents select f;
            dataGridView1.DataSource = c.ToList();
        }

        private void loadCampaignGroup()
        {
            FB context = new FB();
            var c = from f in context.CampaignGroups
                    where f.campaignId==1 
                    select f;

            dataGridView2.DataSource = c.ToList();
        }

        private void GroupPoster_Load(object sender, EventArgs e)
        {
            LoadContent();
            loadCampaignGroup();
            loadtask();

        }

        private void btnAddContent_Click(object sender, EventArgs e)
        {
            GroupPosterContent g = new GroupPosterContent();
            g.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DeleteContent()
        {

            var toBeDeleted = (int)dataGridView1.SelectedRows[0].Cells[0].Value;


            using (var context = new FB())
            {
                context.Database.Log = Console.WriteLine;
                var content = new Content { Id = toBeDeleted };
                context.Contents.Attach(content);
                context.Contents.Remove(content);
                context.SaveChanges();
            }
        }

        private void prepareDeleteContent()
        {
            DeleteContent();
            //status("Item Deleted.");
            LoadContent();
            //status("Data reloaded.");
        }

        private void deleteitem(object sender, EventArgs e)
        {
            prepareDeleteContent();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                ContextMenu cm = new ContextMenu();

                //cm.MenuItems.Add("Load", new EventHandler(loadItem));
                //MenuItem m = new MenuItem("Edit");
                //m.Click += edititem;
                //cm.MenuItems.Add(m);

                cm.MenuItems.Add("Delete", new EventHandler(deleteitem));

                if (currentMouseOverRow >= 0)
                {
                    dataGridView1.Rows[currentMouseOverRow].Selected = true;

                    //toolStripStatusLabel1.Text = "Data row " + currentMouseOverRow + " Selected";
                    cm.Show(dataGridView1, new Point(e.X, e.Y));
                }


            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GroupFrm g = new GroupFrm();
            g.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Campaigner c = new Campaigner();
            c.Show();

            

        }

        private void btnCreateTask_Click(object sender, EventArgs e)
        {
            CreateTask();

        }




        private void CreateTask()
        {

            // insert task record with status 0

            // get campaignid
            // get all group related with the campaign
            //   
            // insert into posttasks with status =0


            string x = "";
            var g = new FB();

            //int groupmanagerid = 0;
            //groupmanagerid = Convert.ToInt16(comboBox1.SelectedValue);

            int campaignid = 1;
            string sql = "insert into posttasks (status, groupid, fbid, groupname) select distinct 0, g.id, g.fbid, g.Name from groupraws g "
                        + " inner join groupmanagersets gm on gm.GroupId = g.id "
                        + " where gm.GroupManagerId in (select id from campaigngroups where campaignid =1)";

            g.Database.ExecuteSqlCommand(sql);


            
            var c = from f in g.PostTasks
                    select f;

            var content = from ct in g.Contents
                          select ct;

            var cl = content.ToArray();
            int rc = 0;

            foreach (var i in c.ToList())
            {
                
                rc = rc + 1;
                if (rc>=cl.Count()) { rc = 0; }
                //Console.WriteLine(i.Id + " : " + cl[rc].Id);
                i.contentId = cl[rc].Id;
                g.SaveChanges();
            }
            //now fill in with post
            // check with group history, what post has been made.. if the id exist before this, and is within 3 hours, mark as danger


            // refresh datagrid1
            loadtask();
            
        }

        private void loadtask()
        {
            FB context = new FB();
            var c = from f in context.PostTasks select f;
            dataGridView3.DataSource = c.ToList();

            // info box
            // tulis task summary

        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            DeleteTask();
        }

        private void DeleteTask()
        {
            var g = new FB();


            string sql = "Truncate table PostTasks";
            g.Database.ExecuteSqlCommand(sql);

            // refresh datagrid1
            loadtask();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
