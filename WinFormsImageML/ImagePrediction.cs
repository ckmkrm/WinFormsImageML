using Microsoft.ML.Data;

namespace WinFormsImageML
{
    public class ImagePrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        public float[] Score { get; set; }
    }
}
