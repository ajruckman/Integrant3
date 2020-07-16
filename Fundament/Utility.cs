using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Fundament
{
    public static class Utility
    {
        public static List<string> Qualifications()
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase method     = stackTrace.GetFrame(1).GetMethod();

            string fullName = method.DeclaringType.FullName;
            fullName = fullName.Substring(0, fullName.IndexOf('`'));

            var result = new List<string>();

            var last = "";
            
            foreach (string s in fullName.Split('.'))
            {
                result.Add(last + s);
                last += s + ".";
            }

            return result;
        }
    }
}