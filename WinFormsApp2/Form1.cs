using System.Net.Sockets;
using Timer = System.Windows.Forms.Timer;

namespace WinFormsApp2
{
    public partial class Model
    {
        public static Size sizeApp = new Size(1024, 768);
        public int progressBarValue = 90;
        public int progressBarMax = 10000;
        public int progressForce = 200;
        public int progressReturnForce = 200;

        public Random random = new Random();
        public int? RndIndex;
        public byte amountPictures1Stage;
        public bool Is1StagePictureTime;

        public void ValueDecrease(object sender, EventArgs e)
            => progressBarValue = progressBarValue > progressReturnForce ? progressBarValue - progressReturnForce : progressBarValue - 0;


        public void ValueIncrease(object sender, EventArgs e)
            => progressBarValue = progressBarValue < (progressBarMax - progressForce) ? progressBarValue + progressForce : progressBarValue + 0;
    }

    public partial class Form1 : Form
    {
        //defaultDirectory: "C:\Users\Александр\source\repos\Muksaflash\WinFormsApp2\WinFormsApp2\bin\Debug\net6.0-windows";
        
        GraphicsImage rocket = new GraphicsImage(0, 0, 0, 0);

        List<string> Backs1Stage = new List<string>()
            {
                "Background1Stage.PNG", "stage1Background2.PNG",
                "stage1Background3.PNG", "stage1Background4.PNG"
            };

        

        List<string> ProgressBars2Stage = new List<string>()
        {
            "ProgressBar\\Pr1.png", "ProgressBar\\Pr2.png","ProgressBar\\Pr3.png", "ProgressBar\\Pr4.png",
            "ProgressBar\\Pr5.png", "ProgressBar\\Pr6.png","ProgressBar\\Pr7.png", "ProgressBar\\Pr8.png"
        };
        public Graphics graphics;

        PictureBox progressBox2Stage = new PictureBox();
        ProgressBar progressBar1 = new ProgressBar();
        ProgressBar progressBar2 = new ProgressBar();
        Timer timerUpdate = new Timer();
        Timer timerProgressReturn = new Timer();
        Timer timerPictureUpate = new Timer();
        Timer timerPictureShift = new Timer();
        Timer timerFolderSpeed = new Timer();

        Model gameModel = new Model();
        PictureBox stage1Background = new PictureBox();
        PictureBox stage3Background = new PictureBox();
        PictureBox stage3BackgroundNew = new PictureBox();

        Point screenCenter = new Point(Model.sizeApp.Width / 2, Model.sizeApp.Height / 2);

        PictureBox stage1Picture1 = new PictureBox();

        PictureBox stage2Picture1 = new PictureBox();
        PictureBox stage2Picture2 = new PictureBox();
        PictureBox stage2Picture3 = new PictureBox();
        PictureBox stage2Picture4 = new PictureBox();
        PictureBox stage2Picture5 = new PictureBox();
        PictureBox stage2Picture6 = new PictureBox();

        PictureBox folderPictureBox = new PictureBox();

        List<PictureBox> stage2PictureBoxes;

        int distanceRocketX;
        int distanceRocketY;

        public Form1()
        {


            DoubleBuffered = true;
            stage2PictureBoxes = new List<PictureBox>()
            {
                stage2Picture1, stage2Picture2, stage2Picture3, stage2Picture4, stage2Picture5, stage2Picture6,
            };

            InitializeComponent();

            timerUpdate.Tick += update;
            timerUpdate.Interval = 10;
            timerUpdate.Start();

            timerProgressReturn.Tick += gameModel.ValueDecrease;
            timerProgressReturn.Interval = 150;
            timerProgressReturn.Start();

            var startButton = new PictureBox();
            startButton.Image = Image.FromFile("error-wait.gif");
            startButton.SizeMode = PictureBoxSizeMode.StretchImage;
            startButton.Size = new Size(200, 30);
            startButton.MaximumSize = new Size(1000, 200);
            startButton.Location = new Point(screenCenter.X - startButton.Width, screenCenter.Y - startButton.Height);
            startButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            startButton.Click += stage1;
            Controls.Add(startButton);

            var backgroundPictureBox = new PictureBox();
            backgroundPictureBox.Image = Image.FromFile("mainBackground.jfif");
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(backgroundPictureBox);

            folderPictureBox.Size = new Size(DisplayRectangle.Width / 10, DisplayRectangle.Height / 10);
            folderPictureBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            folderPictureBox.MaximumSize = new Size(200, 200);
            folderPictureBox.MinimumSize = new Size(50, 50);
            folderPictureBox.Image = Image.FromFile("Folder.png");
            folderPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            folderPictureBox.Visible = false;

            progressBox2Stage.Location = new Point(10, 100);
            progressBox2Stage.Size = new Size(DisplayRectangle.Width / 5, DisplayRectangle.Height / 5);
            progressBox2Stage.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBox2Stage.MaximumSize = new Size(400, 300);
            progressBox2Stage.MinimumSize = new Size(120, 90);
            progressBox2Stage.Image = Image.FromFile("ProgressBar\\Pr1.png");
            progressBox2Stage.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void checkConditionProgress(object sender, EventArgs e)
        {
            if (gameModel.progressBarValue >= 9800)
            {
                stage3();
            }
        }
        private void checkWin(object sender, EventArgs e)
        {
            if (gameModel.progressBarValue >= 9800)
            {
                timerProgressReturn.Stop();
                stage3();
            }
        }
        private void stage1(object sender, EventArgs e)
        {
            Controls.Clear();

            timerUpdate.Tick += addPicture1Stage;

            progressBar1.Location = new Point(10, 100);
            progressBar1.Size = new Size(DisplayRectangle.Width / 5, DisplayRectangle.Height / 10);
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.MaximumSize = new Size(800, 100);
            progressBar1.MinimumSize = new Size(240, 30);
            progressBar1.Maximum = gameModel.progressBarMax;
            Controls.Add(progressBar1);

            stage1Background.Image = Image.FromFile("Background1Stage.PNG");
            stage1Background.SizeMode = PictureBoxSizeMode.StretchImage;
            stage1Background.Dock = DockStyle.Fill;
            Controls.Add(stage1Background);

            timerPictureUpate.Interval = 2000;
            timerPictureUpate.Tick += (x, y) => gameModel.Is1StagePictureTime = true;
            timerPictureUpate.Start();

            stage1Background.MouseMove += gameModel.ValueIncrease;
            stage1Background.MouseMove += checkConditionProgress;
        }

        private void stage2()
        {
            timerUpdate.Tick -= addPicture1Stage;
            timerUpdate.Tick += addProgressBar2Stage;

            timerPictureUpate.Interval = 200;

            timerFolderSpeed.Interval = 10;
            timerFolderSpeed.Tick += FlyFolder;
            timerFolderSpeed.Start();

            timerPictureShift.Interval = 2000;
            timerPictureShift.Tick += ShiftPicture;
            timerPictureShift.Start();

            Controls.Clear();

            gameModel.progressBarValue = 90;
            gameModel.progressForce = 2000;

            Controls.Add(progressBox2Stage);
            Controls.Add(folderPictureBox);

            SetPicture2Stage(stage2PictureBoxes[0], new Point(100, 300), AnchorStyles.Left | AnchorStyles.Bottom);
            SetPicture2Stage(stage2PictureBoxes[1], new Point(400, 470), AnchorStyles.Bottom);
            SetPicture2Stage(stage2PictureBoxes[2], new Point(700, 400), AnchorStyles.Bottom | AnchorStyles.Right);
            SetPicture2Stage(stage2PictureBoxes[3], new Point(720, 50), AnchorStyles.Right);
            SetPicture2Stage(stage2PictureBoxes[4], new Point(350, 0), AnchorStyles.Top);
            SetPicture2Stage(stage2PictureBoxes[5], new Point(420, 235), AnchorStyles.Left);

            stage1Background.ImageLocation = System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal)
                + @"\image1.jpg";
            stage1Background.SizeMode = PictureBoxSizeMode.StretchImage;
            stage1Background.Location = new Point(0, 0);
            stage1Background.Size = new Size(800, 800);
            stage1Background.MouseMove -= gameModel.ValueIncrease;
            stage1Background.MouseMove -= checkConditionProgress;
            Controls.Add(stage1Background);

        }

        private void stage3()
        {
            Controls.Clear();

            timerUpdate.Tick -= addProgressBar2Stage;
            timerPictureShift.Tick -= ShiftPicture;
            timerFolderSpeed.Tick -= FlyFolder;

            timerPictureShift.Tick += StartFlyRocket;
            timerPictureShift.Interval = 4000;
            timerPictureShift.Start();
            timerFolderSpeed.Tick += FlyRocket;
            timerFolderSpeed.Interval = 30;
            timerFolderSpeed.Start();

            timerUpdate.Tick += RefreshStage3Background;
            timerUpdate.Interval = 100;
            ////stage3Background.Refresh();





            string gifImage = "Stage3LongBack.gif";
            stage3Background.Image = Image.FromFile("error-wait.gif");
            stage3Background.SizeMode = PictureBoxSizeMode.StretchImage;
            stage3Background.Dock = DockStyle.Fill;
            stage3Background.BackColor = Color.Transparent;
            Controls.Add(stage3Background);


            stage3Background.Paint += Stage3BackgroundPaint;
            //stage3Background.Invalidate();
        }

        private void RefreshStage3Background(object? sender, EventArgs e)
        {
            stage3Background.Refresh();
        }

        private void StartFlyRocket(object? sender, EventArgs e)
        {
            rocket.x = DisplayRectangle.Width;
            rocket.y = DisplayRectangle.Height/2;
            rocket.width = 50;
            rocket.height = 100;
            var x = rocket.x;
            var y = rocket.y;
            distanceRocketX = (x) / 100;
            distanceRocketY = (y - DisplayRectangle.Height / 3) / 100;
        }

        private void FlyRocket(object? sender, EventArgs e)
        {
            if (rocket.x > 0)
            {
                rocket.x -= distanceRocketX + 10000 / rocket.x;
                rocket.y -= distanceRocketY - 1000 / rocket.y;
                rocket.width += 10;
                rocket.height += 10;
            }
            else
            {
                rocket.width = 0;
                rocket.height = 0;
            }
        }

        private void Stage3BackgroundPaint(object? sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            graphics.DrawImage(rocket.image, rocket.x, rocket.y, rocket.width, rocket.height);
        }

        private void ShiftPicture(object? sender, EventArgs e)
        {

            if (gameModel.RndIndex != null)
            {
                var RnIndex = (int)gameModel.RndIndex;
                stage2PictureBoxes[RnIndex].Image = Image.FromFile("1Picture2Stage.PNG");
                stage2PictureBoxes[RnIndex].MouseDown -= ChangeToWhite;
                stage2PictureBoxes[RnIndex].MouseClick -= ChangeToBlackRed;
                stage2PictureBoxes[RnIndex].MouseDown -= gameModel.ValueIncrease;
                stage2PictureBoxes[RnIndex].MouseClick -= checkWin;
                stage2PictureBoxes[RnIndex].MouseDown -= StartFlyFolder;
            }
            gameModel.RndIndex = gameModel.random.Next(stage2PictureBoxes.Count);
            var RndIndex = (int)gameModel.RndIndex;
            stage2PictureBoxes[RndIndex].Image = Image.FromFile("1Picture2StageRed.PNG");
            stage2PictureBoxes[RndIndex].MouseDown += ChangeToWhite;
            stage2PictureBoxes[RndIndex].MouseClick += ChangeToBlackRed;
            stage2PictureBoxes[RndIndex].MouseDown += gameModel.ValueIncrease;
            stage2PictureBoxes[RndIndex].MouseClick += checkWin;
            stage2PictureBoxes[RndIndex].MouseDown += StartFlyFolder;
        }

        private void ChangeToWhite(object sender, EventArgs e) =>
            stage2PictureBoxes[(int)gameModel.RndIndex].Image = Image.FromFile("2Picture2Stage.PNG");

        private void ChangeToBlackRed(object sender, EventArgs e) =>
            stage2PictureBoxes[(int)gameModel.RndIndex].Image = Image.FromFile("1Picture2StageRed.PNG");

        private void StartFlyFolder(object sender, EventArgs e)
        {
            folderPictureBox.Visible = true;
            folderPictureBox.Location = stage2PictureBoxes[(int)gameModel.RndIndex].Location;
        }

        private void FlyFolder(object sender, EventArgs e)
        {
            var x = folderPictureBox.Location.X;
            var y = folderPictureBox.Location.Y;
            var distX = (x - progressBox2Stage.Location.X) / 10;
            var distY = (y - progressBox2Stage.Location.Y) / 10;
            if (folderPictureBox.Location != progressBox2Stage.Location)
                folderPictureBox.Location = new Point(x - distX, y - distY);
            else
                folderPictureBox.Visible = false;
        }

        private void SetPicture2Stage(PictureBox frame, Point location, AnchorStyles anchor)
        {
            frame.Location = location;
            frame.Size = new Size(DisplayRectangle.Width / 4, DisplayRectangle.Height / 3);
            frame.Anchor = anchor;
            frame.MaximumSize = new Size(500, 500);
            frame.MinimumSize = new Size(100, 100);
            frame.Image = Image.FromFile("1Picture2Stage.PNG");
            frame.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(frame);
        }

        private void update(object sender, EventArgs e)
        {
            progressBar1.Value = gameModel.progressBarValue;
        }

        private void addPicture1Stage(object sender, EventArgs e)
        {
            var value = gameModel.progressBarValue;
            var borders = new int[Backs1Stage.Count];
            var distance = gameModel.progressBarMax / Backs1Stage.Count;
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i] = distance;
                if (i > 0) borders[i] = borders[i - 1] + distance;
                if (value < borders[i])
                {
                    ChangePicture(Backs1Stage[i], stage1Background);
                    return;
                }
            }
        }

        private void addProgressBar2Stage(object sender, EventArgs e)
        {
            var value = gameModel.progressBarValue;
            var borders = new int[ProgressBars2Stage.Count];
            var distance = gameModel.progressBarMax / ProgressBars2Stage.Count;
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i] = distance;
                if (i > 0) borders[i] = borders[i - 1] + distance;
                if (value < borders[i])
                {
                    ChangePicture(ProgressBars2Stage[i], progressBox2Stage);
                    return;
                }
            }
        }

        private void ChangePicture(string picture, PictureBox frame)
        {
            if (gameModel.Is1StagePictureTime == true)
            {
                frame.Image.Dispose();
                frame.Image = Image.FromFile(picture);
                gameModel.Is1StagePictureTime = false;
                timerPictureUpate.Start();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs eventArgs)
        {
            var result = MessageBox.Show("Действительно закрыть?", ":(", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                eventArgs.Cancel = true;
        }
    }
}