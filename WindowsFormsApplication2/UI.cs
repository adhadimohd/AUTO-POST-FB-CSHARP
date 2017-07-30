using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace FBResponder
{
    public partial class UI : Form
    {

         FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Program Files\gecko", "geckodriver.exe");
        
        IWebDriver driver;


        //        WebElement element = (new WebDriverWait(driver, 10))
        //.until(ExpectedConditions.presenceOfElementLocated(By.id(“element”)));


        // important xpath
        // .//a[contains(@href,'/adhadi03/posts/851759708299271?comment_id=')]
        // will return comment post time stamp. click here and we can fill reply


        // .//a[contains(@class,'UFICommentActorName')]
        // return all actor

        //PhantomJSDriver driver;

        bool loggedon = false;
        bool postloaded = false;
        bool commentsLoaded = false;
        int timerCount = 0;
        int allcomments;


        public UI()
        {
            InitializeComponent();

            service.HideCommandPromptWindow = true;
            service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";

            //var options = new FirefoxOptions();
           // options.AddArgument("--window-position=-4000,-4000");

            driver = new FirefoxDriver(service);
            //driver.Manage().Window.Size = new System.Drawing.Size(0, 0);
           // driver.Manage().Window.Position = new System.Drawing.Point(3000,5000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var t1 = new Task(() => login());
            t1.Start();

        }
        private void UpdateProgress(int message)
        {
            progressBar1.Increment(message);

        }


        private async void button2_Click(object sender, EventArgs e)
        {
            //var progress = new Progress<int>(UpdateProgress);
            await Task.Factory.StartNew(() =>
            {
                gotopost();
            });

        }

        // login to the page
        public void login()
        {
            driver.Navigate().GoToUrl("https://www.facebook.com");
            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            driver.FindElement(By.Id("email")).SendKeys("amg@adhadi.com");
            //System.Threading.Thread.Sleep(5000);
            driver.FindElement(By.Id("pass")).SendKeys("blackhole2004" + OpenQA.Selenium.Keys.Enter);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            loggedon = true;
            timer1.Enabled = true;
            toolStripStatusLabel1.Text = "Logged on";
           
            timer1.Start();

        }


        // bukak page post
        public async void loadpostpage()
        {

            await Task.Factory.StartNew(() =>
            {
                driver.Navigate().GoToUrl("https://www.facebook.com/adhadi03/posts/851759708299271");
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                allcomments = countAllComments();
                return 1;

            });

            postloaded = true;
            
        }

        public void updateLog()
        { }


        public int countAllComments()
        {

            String a;
            int TotalComments = 0;
            int repeat;
            try
            {
                IWebElement comments = driver.FindElement(By.XPath(".//div[contains(@class,'UFIRow UFIShareRow')]//em[contains(@data-intl-translation,'{count} Comments')]"));
                a = comments.GetAttribute("innerText");

                a = a.Replace(" Comments", "");
                TotalComments = Convert.ToInt16(a);
                allcomments = TotalComments;

                //Console.WriteLine("Total Comments = " + TotalComments);
                repeat = ((TotalComments - (TotalComments % 50)) / 50);
                //Console.WriteLine("Repeat : " + repeat);
            }
            catch (OpenQA.Selenium.NoSuchElementException err)
            {
                TotalComments = 0;
            }
            catch (OpenQA.Selenium.StaleElementReferenceException err)
            {
                TotalComments = 0;
            }

            return TotalComments;
        }


        public int loadpreviouscomment()
        {

            int x;
            x = commentscount();
            
            if (allcomments > 50 && x <=allcomments)
            { 
                try
                {
                    IWebElement link = driver.FindElement(By.XPath(".//a[contains(@class, 'UFIPagerLink')]"));
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

                    link.Click();
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

                }
                catch (OpenQA.Selenium.StaleElementReferenceException e)
                {
                    commentsLoaded = true;



                }
                catch (OpenQA.Selenium.NoSuchElementException e)
                {

                    commentsLoaded = true;
                
                }



                IList<IWebElement> links = driver.FindElements(By.XPath(".//a[contains(@data-testid, 'ufi_comment_timestamp')]")).ToList();
                int b;
                b = links.Count();
                //Console.WriteLine("Found " + a + " comments");
                return b;
            }
            else
            {
                return 0;
            }


        }

        public void gotopost()
        {



        }

        public int commentscount()
        {
            // //a[contains(@data-testid,'ufi_comment_timestamp')]


            IList<IWebElement> links = driver.FindElements(By.XPath(".//a[contains(@data-testid, 'ufi_comment_timestamp')]")).ToList();
            int a;
            a = links.Count();
            //Console.WriteLine("Found " + a + " comments");
            return a;



        }

        private  void timer1_Tick(object sender, EventArgs e)
        {

            toolStripStatusLabel2.Text = "Timer Count " + timerCount;
            timerCount++;

            if (loggedon)
            {
                //load page
                if (postloaded)
                {
                    if (!commentsLoaded)
                    {
                        Task.Factory.StartNew(() =>
                           {
                               var res = loadpreviouscomment();
                               toolStripStatusLabel2.Text = res + " comments loaded ";
                              
                           });

                        int x,y = 0;
                        x = commentscount();
                        y = countAllComments();
                        
                       if (x >= y)
                        {
                            timer1.Stop();
                            commentsLoaded = true;
                            //toolStripStatusLabel2.Text = res + " - all comments loaded ";
                        }



                    }
                    else
                    {
                        timer1.Stop();
                    }
                }
                else
                {
                    // jika post belum dibukak. 
                    loadpostpage();
                }

            }
            
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
          

           
        }

        private void UI_Load(object sender, EventArgs e)
        {

        }
    }
}
