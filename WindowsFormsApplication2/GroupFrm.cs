using FBHelper.Data;
using FBHelper.DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FBResponder
{
    public partial class GroupFrm : Form
    {



        public GroupFrm()
        {
            InitializeComponent();
            


        }

        private void GroupFrm_Load(object sender, EventArgs e)
        {
            
            bindCombo();
            LoadGroupRaws();
            LoadGroupByCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //check account name
            //exist - proceed
            //retrieve all add to GROUPRAWRECORD
            RefreshGroup();

        }

        private void login(string login, string password)
        {

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Program Files\gecko", "geckodriver.exe");
            IWebDriver driver;

            bool LoggedOn = false;

            service.HideCommandPromptWindow = true;
            service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";

            driver = new FirefoxDriver(service);
            driver.Manage().Window.Size = new System.Drawing.Size(400, 400);
            //driver.Manage().Window.Position = new System.Drawing.Point(this.Top,this.Left);

            //this.Activate();
            //button1.Focus();

            driver.Navigate().GoToUrl("https://en-gb.facebook.com");
            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Thread.Sleep(2000);

            driver.FindElement(By.Id("email")).SendKeys(login);
            //System.Threading.Thread.Sleep(5000);
            Thread.Sleep(2000);
            driver.FindElement(By.Id("pass")).SendKeys(password + OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(2000);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LoggedOn = true;
           // timer1.Enabled = true;
            toolStripStatusLabel1.Text = "Logged on";

            //timer1.Start();
            Thread.Sleep(2000);
            // begin scrolling group
            driver.Navigate().GoToUrl("https://www.facebook.com/groups/?category=membership");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Thread.Sleep(2000);


            IList<IWebElement> list = driver.FindElements(By.XPath(".//a[contains(@href,'/groups/') and contains(@data-hovercard,'/ajax/')]"));
            int oldList = list.Count;
            int newList = 0;
           


            int counter = 0;
            while (oldList != newList)
            {
                counter++;
                oldList = list.Count;
                 
                //Actions actions = new Actions(driver);
                ScrollToBottom(driver);
                //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                //Thread.Sleep(5000);

                list = driver.FindElements(By.XPath(".//a[contains(@href,'/groups/') and contains(@data-hovercard,'/ajax/')]"));
                newList = list.Count;
                Console.WriteLine(counter + " => Oldlist:Newlist = " + oldList + ":" + newList);

            }


            toolStripStatusLabel3.Text = "Total Groups: " + newList;


            // now all list received.. update the main database
            // put everything in temporary group database first
            string gname;
            string gid;
            string url;

            var g = new FB();

            //clearkan dulu temp database
            g.Database.ExecuteSqlCommand("truncate table GroupTemps");


            foreach (var e in list)
            {
                gname = e.GetAttribute("innerHTML");
                url = e.GetAttribute("href");
                url = url.Replace("https://www.facebook.com/groups/", "");
                url = url.Replace("/?ref=group_browse_new", "");
                gid = url;

                saveTempGroup(gname, gid);
            }


            // now check how many count in group tmp
            
            toolStripStatusLabel4.Text = "Database insert :" + g.GroupTemps.Count().ToString();

            string sql = "insert into GroupRAWS (name, fbid, ban, hope) select name, fbid, 0,1 from GroupTemps where fbid not in (select fbid from GroupRAWS)";
            g.Database.ExecuteSqlCommand(sql);

            //sql insert 
            // insert into GroupRAWS (name, fbid) select name, fbid from GroupTemps where fbid not in (select fbid from GroupRAWS)
            driver.Close();


        }

        private void ScrollToBottom(IWebDriver driver)
        {
            long scrollHeight = 0;

            do
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");

                if (newScrollHeight == scrollHeight)
                {
                    break;
                }
                else
                {
                    scrollHeight = newScrollHeight;
                    Thread.Sleep(400);
                }

                toolStripStatusLabel3.Text = "Searching available groups...";
            } while (true);
        }

        private void RefreshGroup()

        {
 

           // var id = 1;
            var context = new FB();

            //var acc = new Account { Id = id };
            var acc = context.Accounts.First();
            

            string loginid = acc.login;
            string password = acc.password;

            var t1 = new Task(() => login(loginid, password));
            t1.Start();
            toolStripStatusLabel1.Text = "Logging in...";

        }




        private void saveTempGroup(string groupname, string groupfbid)
        {

            var tmp = new GroupTemp
            {
                Name = groupname,
                fbid = groupfbid

            };

            using (var context = new FB())
            {

                context.GroupTemps.Add(tmp);
                context.SaveChanges();
                toolStripStatusLabel3.Text = "Group temp with id " + tmp.Id + " saved";
            }


        }


        private void LoadGroupRaws()
        {


            int groupmanagerid = 0;
            groupmanagerid = Convert.ToInt16(comboBox1.SelectedValue);

            var fb = new FB();

            //var gr = from g in fb.GroupRAWS
            //         where !(from gms in fb.GroupManagerSets 
            //                  where gms.GroupManagerId == groupmanagerid
            //                 select gms.GroupManagerId).Contains(g.Id)
            //         select g;

            var gr = fb.GroupRAWS.SqlQuery("select * from GroupRaws g " +
                " where g.id not in (select groupid from GroupManagerSets where GroupManagerId = @P0)", groupmanagerid );

            fb.Database.Log = Console.WriteLine;
            
            dataGridView1.DataSource = gr.ToList();

        }


        private void LoadGroupByCombo()
        {

            var fb = new FB();

            int groupmanagerid = 0;
            groupmanagerid = Convert.ToInt16(comboBox1.SelectedValue);

            var gr = from g in fb.GroupRAWS
                     join gms in fb.GroupManagerSets 
                     on g.Id equals gms.GroupId
                     where gms.GroupManagerId == groupmanagerid 
                     select new
                     {
                        gms.Id , g.Name, g.fbid
                     };

            fb.Database.Log = Console.WriteLine;
                
            dataGridView2.DataSource = gr.ToList();
        }

        private void bindCombo()
        {
            FB fb = new FB();
            var item = fb.GroupManagers.ToList();
            comboBox1.DataSource = item;


        }

        private void getAllGroupFromFB()
        {


        }

        private void displayAllCurrentGroup()
        {
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void status1(string txt)
        {
            //statusStrip1.Text
        }

        private void button2_Click(object sender, EventArgs e)
        {


            //int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
            //MessageBox.Show(id.ToString());

            string x="";
            var g = new FB();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                //dataGridView1.Rows.Remove(r);
                //dataGridView2.Rows.Add(r);
                x = x + ", " + row.Cells[0].Value;
            }

            x = x.TrimStart(',');
            x = x.TrimEnd(',');

            int groupmanagerid = 0;
            groupmanagerid = Convert.ToInt16(comboBox1.SelectedValue);



            string sql = "insert into GroupManagerSets "
                + " (Groupid, GroupManagerId) " 
                + "select id, " + groupmanagerid
                + " from GroupRaws where id in ("+ x + ")";

            g.Database.ExecuteSqlCommand(sql);

            // refresh datagrid1
            LoadGroupRaws();

            // refresh datagrid2
            LoadGroupByCombo();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupRaws();

            // refresh datagrid2
            LoadGroupByCombo();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

 

        private void btnRemoveFromSet_Click(object sender, EventArgs e)
        {
            string x = "";
            var g = new FB();

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                //dataGridView1.Rows.Remove(r);
                //dataGridView2.Rows.Add(r);
                x = x + ", " + row.Cells[0].Value;
            }

            x = x.TrimStart(',');
            x = x.TrimEnd(',');

            if (x.Length!=0)
            { 
            int groupmanagerid = 0;
            groupmanagerid = Convert.ToInt16(comboBox1.SelectedValue);



            string sql = "Delete from GroupManagerSets "
                     + " where id in (" + x + ")";

            g.Database.ExecuteSqlCommand(sql);

            // refresh datagrid1
            LoadGroupRaws();

            // refresh datagrid2
            LoadGroupByCombo();
        }

        }
    }
}
