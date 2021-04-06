using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Kooboo.Sites.Commerce
{
    public static class Validator
    {

        public static void NotNull(object obj, string path)
        {
            if (obj == null) throw new Exception($"{path} can not be null");
        }

        public static void NotEmpty(object obj, string path)
        {
            if (obj == null) NotNull(obj, path);
            if (obj == default || obj.Equals("")) throw new Exception($"{path} can not be empty");
        }

        public static void StringRange(string str, string path, int min, int max)
        {
            if (str == null) NotNull(str, path);
            int num = str.Length;
            if (num < min || num > max) throw new Exception($"{path} length has to be greater than {min} and less than {max}");
        }

        public static void NumberRange(decimal num, string path, decimal min, decimal max)
        {
            if (num < min || num > max) throw new Exception($"{path} length has to be greater than {min} and less than {max}");
        }
    }
}
