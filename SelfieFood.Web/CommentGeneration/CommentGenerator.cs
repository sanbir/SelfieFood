using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using SelfieFood.Common;

namespace SelfieFood.Web.CommentGeneration
{
    public class CommentGenerator
    {
        public async Task<string> GetComment(Face[] faces, Emotion[] emotions)
        {
            var people = faces
                .Zip(emotions, (face, emotion) => new Person(face.FaceAttributes, emotion.Scores))
                .ToList();

            switch (people.Count)
            {
                case 1:
                    return new OnePersonCommentGenerator().GetComment(people[0]);
                case 2:
                    return new PairOfPeopleCommentGenerator().GetComment(people[0], people[1]);
                default:
                    return people.Count > 2
                        ? new GroupOfPeopleCommentGenerator().GetComment(people)
                        : ResturantsResponse.NoPeopleFoundMessage;
            }

        }
    }
}