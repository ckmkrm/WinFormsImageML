using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
using Microsoft.ML.Vision;

namespace WinFormsImageML
{
    public partial class Form1 : Form
    {
        private readonly string projectRoot;
        private readonly string dataFolder;
        private readonly string trainFolder;
        private readonly string testFolder;
        private readonly string outputFolder;
        private readonly string modelPath;

        private MLContext mlContext;
        private ITransformer trainedModel;

        public Form1()
        {
            InitializeComponent();

            projectRoot = Path.GetFullPath(Path.Combine(Application.StartupPath, "..", "..", ".."));
            dataFolder = Path.Combine(projectRoot, "data");
            trainFolder = Path.Combine(dataFolder, "train");
            testFolder = Path.Combine(dataFolder, "test");
            outputFolder = Path.Combine(projectRoot, "output");
            modelPath = Path.Combine(outputFolder, "model.zip");

            Directory.CreateDirectory(outputFolder);

            mlContext = new MLContext(seed: 1);

            lblProjectRoot.Text = $"Project root: {projectRoot}";
            Log("Uygulama hazýr.");
        }

        private void Log(string text)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => txtLog.AppendText(text + Environment.NewLine)));
            }
            else
            {
                txtLog.AppendText(text + Environment.NewLine);
            }
        }

        private List<ImageData> LoadImageDataFromDirectory(string folder)
        {
            var data = new List<ImageData>();
            if (!Directory.Exists(folder))
                return data;

            var classDirs = Directory.GetDirectories(folder);
            foreach (var classDir in classDirs)
            {
                var label = Path.GetFileName(classDir);
                var files = Directory.GetFiles(classDir, "*.*", SearchOption.AllDirectories)
                            .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                        f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                        f.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
                foreach (var f in files)
                {
                    data.Add(new ImageData { ImagePath = f, Label = label });
                }
            }
            return data;
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            btnTrain.Enabled = false;
            try
            {
                await Task.Run(() => TrainModel());
            }
            catch (Exception ex)
            {
                Log("Eðitim hatasý: " + ex.Message);
            }
            finally
            {
                btnTrain.Enabled = true;
            }
        }

        private void TrainModel()
        {
            Log("Eðitim baþlýyor...");

            var trainData = LoadImageDataFromDirectory(trainFolder);
            var testData = LoadImageDataFromDirectory(testFolder);

            Log($"Train örnek sayýsý: {trainData.Count}");
            Log($"Test örnek sayýsý: {testData.Count}");

            if (trainData.Count == 0)
            {
                Log("Eðitim verisi yok. data/train altýnda sýnýf klasörleri ve resimler olduðundan emin ol.");
                return;
            }

            IDataView trainDataView = mlContext.Data.LoadFromEnumerable(trainData);
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(testData);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label")
                .Append(mlContext.Transforms.LoadRawImageBytes(outputColumnName: "Image", imageFolder: "", inputColumnName: nameof(ImageData.ImagePath)))
                .Append(mlContext.MulticlassClassification.Trainers.ImageClassification(new ImageClassificationTrainer.Options()
                {
                    FeatureColumnName = "Image",
                    LabelColumnName = "LabelAsKey",
                    Arch = ImageClassificationTrainer.Architecture.ResnetV250,
                    Epoch = 10,
                    BatchSize = 8,
                    LearningRate = 0.01f,
                    MetricsCallback = (metrics) => Log(metrics.ToString()),
                    ValidationSet = testDataView
                }))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));

            Log("Pipeline hazýr, Fit çalýþýyor (eðitim)...");

            trainedModel = pipeline.Fit(trainDataView);

            Log("Eðitim tamamlandý.");

            if (testData.Count > 0)
            {
                var predictions = trainedModel.Transform(testDataView);
                var metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
                Log($"MicroAccuracy: {metrics.MicroAccuracy:0.###}, MacroAccuracy: {metrics.MacroAccuracy:0.###}");
            }

            mlContext.Model.Save(trainedModel, trainDataView.Schema, modelPath);
            Log($"Model kaydedildi: {modelPath}");
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picOriginal.Image?.Dispose();
                picOriginal.Image = new Bitmap(ofd.FileName);
                txtImagePath.Text = ofd.FileName;
                Log($"Resim yüklendi: {ofd.FileName}");
            }
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            if (!File.Exists(modelPath))
            {
                Log("Model bulunamadý. Önce eðit veya model.zip dosyasýný output klasörüne koy.");
                MessageBox.Show("Model bulunamadý. Önce eðit veya model.zip dosyasýný output klasörüne koy.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtImagePath.Text) || !File.Exists(txtImagePath.Text))
            {
                MessageBox.Show("Lütfen bir resim yükleyin.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var mlContextLocal = new MLContext();
                ITransformer loadedModel = mlContextLocal.Model.Load(modelPath, out var schema);
                var predictor = mlContextLocal.Model.CreatePredictionEngine<ImageData, ImagePrediction>(loadedModel);

                var pred = predictor.Predict(new ImageData { ImagePath = txtImagePath.Text });
                lblPrediction.Text = $"Tahmin: {pred.PredictedLabel}";
                Log($"Tahmin yapýldý: {pred.PredictedLabel}");
            }
            catch (Exception ex)
            {
                Log("Tahmin hatasý: " + ex.Message);
            }
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            if (picOriginal.Image == null) return;
            var bmp = new Bitmap(picOriginal.Image);
            var gray = ConvertToGrayscale(bmp);
            picProcessed.Image?.Dispose();
            picProcessed.Image = gray;
            Log("Griye çevirme iþlemi uygulandý.");
        }

        private void btnEdges_Click(object sender, EventArgs e)
        {
            if (picOriginal.Image == null) return;
            var bmp = new Bitmap(picOriginal.Image);
            var gray = ConvertToGrayscale(bmp);
            var edges = SobelEdgeDetect(gray);
            picProcessed.Image?.Dispose();
            picProcessed.Image = edges;
            gray.Dispose();
            Log("Sobel kenar algýlama uygulandý.");
        }

        private Bitmap ConvertToGrayscale(Bitmap bmp)
        {
            var gray = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var p = bmp.GetPixel(x, y);
                    int l = (int)(0.299 * p.R + 0.587 * p.G + 0.114 * p.B);
                    gray.SetPixel(x, y, Color.FromArgb(l, l, l));
                }
            }
            return gray;
        }

        private Bitmap SobelEdgeDetect(Bitmap gray)
        {
            int w = gray.Width;
            int h = gray.Height;
            var outBmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);

            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int y = 1; y < h - 1; y++)
            {
                for (int x = 1; x < w - 1; x++)
                {
                    int sx = 0, sy = 0;
                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int val = gray.GetPixel(x + kx, y + ky).R;
                            sx += gx[ky + 1, kx + 1] * val;
                            sy += gy[ky + 1, kx + 1] * val;
                        }
                    }
                    int mag = (int)Math.Min(255, Math.Sqrt(sx * sx + sy * sy));
                    outBmp.SetPixel(x, y, Color.FromArgb(mag, mag, mag));
                }
            }
            return outBmp;
        }
    }
}
