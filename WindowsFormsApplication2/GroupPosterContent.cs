using System;
using System.Windows.Forms;
using FBHelper.DataModel;
using FBHelper.Data;

namespace FBResponder
{
    public partial class GroupPosterContent : Form
    {
        public GroupPosterContent()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void GroupPosterContent_Load(object sender, EventArgs e)
        {

        }

        private void saveContent()
        {
            int id = 0;
            var content = new Content
            {
                ContentText = txtContent.Text,
                Description = txtDescription.Text,
                CampaignId = 1,
                mediaurl01 = txtMediaUrl01.Text,
                mediaurl02 = txtMediaUrl02.Text,
                mediaurl03 = txtMediaUrl03.Text,
                mediaurl04 = txtMediaUrl04.Text,
                mediaurl05 = txtMediaUrl05.Text,
                isactive = chkIsActive.Checked = true ? true : false

            };

            using (var context = new FB())
            {
                context.Contents.Add(content);
                context.SaveChanges();
                id = content.Id;

            }

            if (id > 0)
            {
                MessageBox.Show("Data inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Close();
            }

            GroupPoster m = (GroupPoster)ActiveForm;
            m.LoadContent();
            
                        

        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveContent();
        }
    }
}
