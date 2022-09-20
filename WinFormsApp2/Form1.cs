using Timer = System.Windows.Forms.Timer;

namespace WinFormsApp2
{
    public partial class Model
    {
        public int progressBarValue = 90;
        public int progressBarMax = 10000;
        public int progressForce = 4;
        public int progressReturnForce = 200;

        public void ValueDecrease(object sender, EventArgs e)
            => progressBarValue = progressBarValue > progressReturnForce ? progressBarValue - progressReturnForce : progressBarValue - 0;


        public void ValueIncrease(object sender, EventArgs e)
            => progressBarValue = progressBarValue < (progressBarMax - progressForce) ? progressBarValue + progressForce : progressBarValue + 0;
    }

    public partial class Form1 : Form
    {
        TableLayoutPanel startTable = new TableLayoutPanel();
        TableLayoutPanel stage1Table = new TableLayoutPanel();
        TableLayoutPanel stage2Table = new TableLayoutPanel();

        ProgressBar progressBar1 = new ProgressBar();
        ProgressBar progressBar2 = new ProgressBar();
        public Timer timerUpdate = new Timer();
        public Timer timerProgressReturn = new Timer();
        Model gameModel = new Model();
        PictureBox pictureBoxStage = new PictureBox();

        Point screenCenter = new Point(Screen.PrimaryScreen.Bounds.Size.Width / 2, Screen.PrimaryScreen.Bounds.Size.Height / 2);   

        public Form1()
        {
            InitializeComponent();

            timerUpdate.Tick += new EventHandler(update);
            timerUpdate.Interval = 10;
            timerUpdate.Start();

            timerProgressReturn.Tick += new EventHandler(gameModel.ValueDecrease);
            timerProgressReturn.Interval = 150;
            timerProgressReturn.Start();

            startTable.RowStyles.Clear();
            startTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            stage1Table.RowStyles.Add(new RowStyle(SizeType.Absolute, 1));
            startTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); //кнопка
            startTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            startTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            startTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            startTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            startTable.Dock = DockStyle.Fill;

            var startButton = new Button();
            startButton.Location = new Point(screenCenter.X - 2*startButton.Width, screenCenter.Y- 2*startButton.Height);
            startButton.Size = new Size(300, 50);
            startButton.Text = "Начать миссию";
            startButton.Click += stage1;
            Controls.Add(startButton);

            //startTable.BackColor = Color.Transparent;

            //startTable.Controls.Add(new Panel(), 0, 0);
            //startTable.Controls.Add(startButton, 1, 1);
            //startTable.Controls.Add(new Panel(), 2, 2);


            var backgroundPictureBox = new PictureBox();
            backgroundPictureBox.ImageLocation = System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal)
                + @"\Image.JPG";
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(backgroundPictureBox);

            //this.BackgroundImage = Image.FromFile(System.Environment.GetFolderPath
            //    (System.Environment.SpecialFolder.Personal)
            //    + @"\Image.JPG");

        }
        private void checkConditionProgress(object sender, EventArgs e)
        {
            if (gameModel.progressBarValue >= 9800)
            {
                stage2();
            }
        }
        private void checkWin(object sender, EventArgs e)
        {
            if (gameModel.progressBarValue >= 9800)
            {
                timerProgressReturn.Stop();
                MessageBox.Show("Вы взломали пентагон, прикиньте!");
            }
        }
        private void stage1(object sender, EventArgs e)
        {
            Controls.Clear();
            progressBar1.Location = new Point(10, 10);
            progressBar1.Size = new Size(150, 30);
            progressBar1.Maximum = gameModel.progressBarMax;
            Controls.Add(progressBar1);

            pictureBoxStage.ImageLocation = System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal)
                + @"\Image.JPG";
            pictureBoxStage.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxStage.Dock = DockStyle.Fill;
            pictureBoxStage.Location = new Point(0, 0);
            pictureBoxStage.Size = new Size(800, 800);
            Controls.Add(pictureBoxStage);
            pictureBoxStage.MouseMove += gameModel.ValueIncrease;
            pictureBoxStage.MouseMove += checkConditionProgress;
        }

        private void stage2()
        {
            Controls.Clear();
            gameModel.progressBarValue = 90;
            gameModel.progressForce = 800;
            progressBar1.Location = new Point(50, 50);
            progressBar1.Size = new Size(200, 30);
            progressBar1.Maximum = gameModel.progressBarMax;
            Controls.Add(progressBar1);

            pictureBoxStage.ImageLocation = System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal)
                + @"\image1.jpg";
            pictureBoxStage.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxStage.Location = new Point(0, 0);
            pictureBoxStage.Size = new Size(800, 800);
            pictureBoxStage.MouseMove -= gameModel.ValueIncrease;
            pictureBoxStage.MouseMove -= checkConditionProgress;
            pictureBoxStage.MouseClick += gameModel.ValueIncrease;
            pictureBoxStage.MouseClick += checkWin;
            Controls.Add(pictureBoxStage);
        }

        private void update(object sender, EventArgs e)
        {
            progressBar1.Value = gameModel.progressBarValue;
        }
        
        protected override void OnFormClosing(FormClosingEventArgs eventArgs)
        {
            var result = MessageBox.Show("Действительно закрыть?", ":(", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                eventArgs.Cancel = true;
        }
    }
}