using System;
using System.Collections.Generic;
using System.Text;

namespace Memefy.Model
{
    public class FaceRectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Scores
    {
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }
    }

    public class EmotionModel
    {
        public FaceRectangle FaceRectangle { get; set; }
        public Scores Scores { get; set; }
    }
}
