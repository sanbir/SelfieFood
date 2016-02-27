using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfieFood.Web.CommentGeneration
{
    public class OnePersonCommentGenerator
    {
        public string GetComment(Person person)
        {
            string result = null;

            if (person.FaceAttributes.Age < 7)
            {
                throw new NotImplementedException();
            }

            return result;
        }
    }

}