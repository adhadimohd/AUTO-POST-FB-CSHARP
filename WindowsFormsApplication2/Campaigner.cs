using FBHelper.Data;
using FBHelper.DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Forms.Timer;

namespace FBResponder
{
    public partial class Campaigner : Form
    {

        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Program Files\gecko", "geckodriver.exe");
        IWebDriver driver;


        private System.Windows.Forms.Timer _timer;

        // The last time the timer was started
        private DateTime _startTime = DateTime.MinValue;

        // Time between now and when the timer was started last
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;

        // Time between now and the first time timer was started after a reset
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;

        // Whether or not the timer is currently running
        private bool _timerRunning = false;
        private int counter = 0;

 
        /// <summary>
        /// Handle the Timer's Tick event
        /// </summary>
        /// <param name="sender">System.Windows.Forms.Timer instance</param>
        /// <param name="e">EventArgs object</param>
        void _timer_Tick(object sender, EventArgs e)
        {
            // We do this to chop off any stray milliseconds resulting from 
            // the Timer's inherent inaccuracy, with the bonus that the 
            // TimeSpan.ToString() method will now show correct HH:MM:SS format
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);

            // The current elapsed time is the time since the start button was
            // clicked, plus the total time elapsed since the last reset
            _currentElapsedTime = timeSinceStartTime + _totalElapsedTime;
            counter++;

            // These are just two Label controls which display the current 
            // elapsed time and total elapsed time
            _totalElapsedTimeDisplay.Text = _currentElapsedTime.ToString();
            _currentElapsedTimeDisplay.Text = timeSinceStartTime.ToString();

            //toolStripStatusLabel1.Text

            if (counter == 60)
            {
                // Reset the elapsed time TimeSpan objects
                _totalElapsedTime = TimeSpan.Zero;
                _currentElapsedTime = TimeSpan.Zero;
                counter = 0;

                toolStripStatusLabel1.Text = "Processing post ";

                Task.Factory.StartNew(() =>
            {
                   var res = sendpost();
               

            });
            }
        }


        private int sendpost()
        {


            var context = new FB();
            var error = "";

            //var acc = new Account { Id = id };
            var task = context.PostTasks.First();
            int c_id = task.contentId;
            String postBtnIdentifier = ".//*[@data-testid='react-composer-post-button']";

            var groupfbid = task.fbid;
            //lblCurrentGroup.Text = task.groupname;



            driver.Navigate().GoToUrl("https://www.facebook.com/groups/" + groupfbid);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Thread.Sleep(2000);

            bool postingboxfound = false;
           // IWebElement childelem;
            //IWebElement parentelem= new IWebElement;

            var elem = "";


            try
            {
                elem  = ".//a[contains(@label, 'Start Discussion')]";
                 driver.FindElement(By.XPath(elem));
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
                //typeElem = 1;
   
                
                postingboxfound = true;
  


            }
            catch (NoSuchElementException e)
            {
                postingboxfound = false;
                error = "Write post box cannot be found;";
            }

            if (!postingboxfound)
                try
                {
                    elem = ".//a[contains(@label, 'Write Post')]";
                    driver.FindElement(By.XPath(elem));
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
                   

                    postingboxfound = true;
                  

                }
                catch (NoSuchElementException e)
                {
                    postingboxfound = false;
                    error = error + "Start Discussion box cannot be found;";
                }

            
            Actions action = new Actions(driver);
            action.MoveToElement(driver.FindElement(By.XPath(elem))).Perform();

            //action.SendKeys(MouseButtons.cli).Perform();

            //var content = context.Contents.Find(task.contentId);

            //if (postingboxfound)
            //{

            //    action.SendKeys(content.ContentText).Perform();
            //    IWebElement postBtn = driver.FindElement(By.XPath(postBtnIdentifier));
            //    postBtn.SendKeys(OpenQA.Selenium.Keys.Enter);
            //    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            //    Thread.Sleep(5000);
            //}
            //else
            //{
            //    // cant find post box. Delete from record
            //    // log cannot update
            //    error = error + "Cant find any post box";

            //}



            //PostTaskLog log = new PostTaskLog();
            //log.contenttext = content.ContentText;
            //log.fbid = task.fbid;
            //log.datecreated = System.DateTime.Now;
            //log.groupid = task.groupid;
            //log.groupname = task.groupname;
            //if (error == "") { log.poststatus = 1; }
            //            else { log.poststatus = 0; }
            //log.poststatusremark = error;

            //context.PostTaskLogs.Add(log);
            //context.SaveChanges();

            //context.Contents.Remove(content);
            //context.SaveChanges();


            return 0;
        }


        /// <summary>
        /// Handle Start/Stop button click
        /// </summary>
        /// <param name="sender">The Button control</param>
        /// <param name="e">EventArgs object</param>
        private void startButton_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Handle Reset button click
        /// </summary>
        /// <param name="sender">The Button control</param>
        /// <param name="e">EventArgs object</param>
        //private void resetButton_Click(object sender, EventArgs e)
        //{
        //    //// Stop and reset the timer if it was running
        //    //_timer.Stop();
        //    //_timerRunning = false;

        //    //// Reset the elapsed time TimeSpan objects
        //    //_totalElapsedTime = TimeSpan.Zero;

        //    //_currentElapsedTime = TimeSpan.Zero;
        //}

        public Campaigner()
        {
            InitializeComponent();


            service.HideCommandPromptWindow = true;
            service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";

            //var options = new FirefoxOptions();
            // options.AddArgument("--window-position=-4000,-4000");

            driver = new FirefoxDriver(service);


            // Set up a timer and fire the Tick event once per second (1000 ms)
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(_timer_Tick);

        }




        private void button1_Click(object sender, EventArgs e)
        {           // var id = 1;
            var context = new FB();

            //var acc = new Account { Id = id };
            var acc = context.Accounts.First();


            string loginid = acc.login;
            string password = acc.password;

            var t1 = new Task(() => login(loginid, password));
            t1.Start();
            toolStripStatusLabel1.Text = "Logging in...";


            // If the timer isn't already running
            if (!_timerRunning)
            {
                // Set the start time to Now
                _startTime = DateTime.Now;

                // Store the total elapsed time so far
                _totalElapsedTime = _currentElapsedTime;

                _timer.Start();
                _timerRunning = true;
            }
            else // If the timer is already running
            {
                _timer.Stop();
                _timerRunning = false;
            }


        }


        private void login(string login, string password)
        {



            bool LoggedOn = false;


            driver.Navigate().GoToUrl("https://en-gb.facebook.com");
            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            
            driver.FindElement(By.Id("email")).SendKeys(login);
            driver.FindElement(By.Id("pass")).SendKeys(password + OpenQA.Selenium.Keys.Enter);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LoggedOn = true;
            // timer1.Enabled = true;
            toolStripStatusLabel1.Text = "Logged on";


            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
        }

        private void Campaigner_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Stop and reset the timer if it was running
            _timer.Stop();
            _timerRunning = false;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            counter = 59;
        }

        private void campaignManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GroupPoster gp = new GroupPoster();
            gp.Show();

           
        }
    }
}
