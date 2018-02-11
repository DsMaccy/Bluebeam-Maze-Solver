using System;
using System.Threading;
using System.Windows.Forms;

namespace MazeGeneratorUI
{
    public partial class Form1 : Form
    {
        private delegate void GeneratorCompletionHandler(bool success);
        private GeneratorCompletionHandler generatorHandler;

        public Form1()
        {
            InitializeComponent();
            generatorHandler = generatorComplete;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            generateButton.Enabled = false;
            

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.SupportMultiDottedExtensions = false;
            saveDialog.AddExtension = true;
            saveDialog.OverwritePrompt = true;
            saveDialog.Filter = "Portable Network Graphic|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            saveDialog.DefaultExt = ".png";
            DialogResult results = saveDialog.ShowDialog();

            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(AsyncGenerator);
            Thread t = new Thread(threadStart);
            GeneratorParameters parameters = new GeneratorParameters();
            parameters.filesizetag = sizeComboBox.SelectedItem as string;
            parameters.outfilename = saveDialog.FileName;
            
            t.Start(sizeComboBox.SelectedValue);
        }

        private struct GeneratorParameters
        {
            public string filesizetag;
            public string outfilename;
        }
        private void AsyncGenerator(object param)
        {
            try
            {
                GeneratorParameters parameters = (GeneratorParameters)param;
                // MazeGenerator = 
                MazeGenerator.ImageSize size;
                if (parameters.filesizetag == "Tiny")
                {
                    size = MazeGenerator.ImageSize.VerySmall;
                }
                else if (parameters.filesizetag == "Small")
                {
                    size = MazeGenerator.ImageSize.Small;
                }
                else if (parameters.filesizetag == "Medium")
                {
                    size = MazeGenerator.ImageSize.Medium;
                }
                else if (parameters.filesizetag == "Large")
                {
                    size = MazeGenerator.ImageSize.Large;
                }
                else  // Default to medium in the case of a bad input
                {
                    size = MazeGenerator.ImageSize.Medium;
                }
                MazeGenerator.MazeGenerator generator = new MazeGenerator.MazeGenerator(size);
                generator.Image.Save(parameters.outfilename);
                Invoke(generatorHandler, true);
            }

            catch (Exception)
            {
                Invoke(generatorHandler, false);
            }
        }

        private void generatorComplete(bool success)
        {
            generateButton.Enabled = true;
            if (success)
            {
                MessageBox.Show("Maze generated successfully");
            }
            else
            {
                MessageBox.Show("Maze generation failed :(");
            }
        }
    }
}