using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace SelfieFood.Web.CommentGeneration
{
    public class Person
    {
        public Person(FaceAttributes faceAttributes, Scores emotionScores)
        {
            FaceAttributes = faceAttributes;
            EmotionScores = emotionScores;
        }

        public FaceAttributes FaceAttributes { get; private set; }
        public Scores EmotionScores { get; private set; }
    }
}