
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using Point = System.Drawing.Point;
//using Microsoft.Office.Interop.PowerPoint;
//using Microsoft.Office.Core;
//using MsoTriStateAlias = Microsoft.Office.Core.MsoTriState;

namespace Quiz_2.Formss
{
    public partial class Questions : Form
    {
        public Questions()
        {
            InitializeComponent();
        }
        public static class ControlID
        {
            public static string sendtxt1 { get; set; }
            public static List<PictureBox> selectedPictureBoxes { get; set; }
            public static int u_id { get; set; }
            public static DataTable searssh { get; set; }
        }
        private void Questions_Load(object sender, EventArgs e)
        {
            ControlID.selectedPictureBoxes = new List<PictureBox>();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ControlID.selectedPictureBoxes = new List<PictureBox>();
            this.Hide();
            var form2 = new Tchr();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
        private void qshns_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.ico|PowerPoint Files|*.ppt;*.pptx|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string appDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions");

                if (!Directory.Exists(appDirectory))
                {
                    Directory.CreateDirectory(appDirectory);
                }

                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (IsImageFile(filePath))
                    {
                        // Copy image files to the Questions folder
                        string destinationPath = Path.Combine(appDirectory, Path.GetFileName(filePath));
                        File.Copy(filePath, destinationPath);
                    }
                    else if (IsPowerPointFile(filePath))
                    {
                        // Convert PowerPoint to images and save to the Questions folder
                        ConvertPowerPointToImages(filePath, appDirectory);
                    }
                    // Add additional file type checks if needed
                }

                MessageBox.Show("Files imported successfully!");
            }
        }
        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" ||
                   extension == ".png";
        }
        private bool IsPowerPointFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".ppt" || extension == ".pptx";
        }
        private void ConvertPowerPointToImages(string pptFilePath, string outputDirectory)
        {
            //try
            //{
            //    using (Presentation presentation = new Presentation())
            //    {
            //        presentation.LoadFromFile(pptFilePath);

            //        int slideIndex = 0;
            //        foreach (ISlide slide in presentation.Slides)
            //        {
            //            Image slideImage = slide.SaveAsImage();
            //            // Use slide name for file name, handling potential invalid characters
            //            string defaultName = $"Slide_{slideIndex}.png";
            //            string imageFileName = Path.Combine(outputDirectory,
            //                string.IsNullOrEmpty(slide.Name) ? defaultName : Path.GetInvalidFileNameChars().Aggregate(slide.Name, (current, c) => current.Replace(c, '_')) + ".png");


            //            slideImage.Save(imageFileName, System.Drawing.Imaging.ImageFormat.Png); // Use consistent format
            //            slideImage.Dispose(); // Dispose of image object
            //            slideIndex++;
            //        }
            //    } // Presentation object is disposed automatically here
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions appropriately
            //    MessageBox.Show("Error: " + ex.Message);
            //}
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = (PictureBox)sender;
            CheckBox checkBox = (CheckBox)clickedPictureBox.Controls["selectionCheckbox"];



            if (checkBox.Checked)
            {
                // Deselect the image
                checkBox.Visible = false;
                ControlID.selectedPictureBoxes.Remove(clickedPictureBox);
                checkBox.Checked = false;
                clickedPictureBox.BorderStyle = BorderStyle.None;
            }
            else
            {
                // Select the image
                checkBox.Visible = true;
                ControlID.selectedPictureBoxes.Add(clickedPictureBox);
                checkBox.Checked = true;
                clickedPictureBox.BorderStyle = BorderStyle.FixedSingle;
                clickedPictureBox.BackColor = Color.MediumSeaGreen;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string QuestionsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions");

            if (!Directory.Exists(QuestionsFolderPath))
            {
                Directory.CreateDirectory(QuestionsFolderPath);
                MessageBox.Show("Questions folder not found in app directory.", "Questions folder has been Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string imageFileName in Directory.GetFiles(QuestionsFolderPath, "*.jpg", SearchOption.AllDirectories)
                                                   .Union(Directory.GetFiles(QuestionsFolderPath, "*.png", SearchOption.AllDirectories))
                                                   .Union(Directory.GetFiles(QuestionsFolderPath, "*.jpeg", SearchOption.AllDirectories)))
            {
                Image image = Image.FromFile(imageFileName);
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = image;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Size = new Size(200, 200);
                pictureBox.Click += pictureBox1_Click; // Add click event for selecting images

                // Extract the filename without extension and set it as PictureBox name
                string imageName = Path.GetFileNameWithoutExtension(imageFileName);
                pictureBox.Name = imageName;

                CheckBox checkBox = new CheckBox();
                checkBox.Name = "selectionCheckbox";
                checkBox.Visible = false; // Initially hide the checkbox
                checkBox.Location = new Point(0, 0);
                checkBox.Size = new Size(100, 30);
                checkBox.Text = "Selected";
                checkBox.ForeColor = Color.Red;
                checkBox.Font = new Font("Arial", 12F, FontStyle.Bold);
                pictureBox.Controls.Add(checkBox);
                flowLayoutPanel1.Controls.Add(pictureBox);
            }

            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Questions folder is empty", "No Questions Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Tchr();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string filename = "path/to/your/presentation.pptx"; // Replace with your file path

            //    using (Microsoft.Office.Interop.PowerPoint.Application app = new Microsoft.Office.Interop.PowerPoint.Application())
            //    {
            //        Presentation presentation = app.Presentations.Open(filename, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

            //        int slideCount = presentation.Slides.Count;

            //        for (int i = 1; i <= slideCount; i++)
            //        {
            //            Slide slide = presentation.Slides[i];
            //            string slideName = Path.GetFileNameWithoutExtension(filename) + "_Slide" + i.ToString("00") + ".jpg";

            //            // Export with appropriate size and quality
            //            slide.Export(slideName, "JPG", 1024, 768); // Adjust width and height as needed
            //        }

            //        presentation.Close();
            //    }

            //    Console.WriteLine("Slides exported successfully!");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error: " + ex.Message);
            //}
        }
    }
}

