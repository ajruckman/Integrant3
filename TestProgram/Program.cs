using System.IO;
using Integrant.Colorant;
using Integrant.Colorant.Schema;
using Newtonsoft.Json;

namespace TestProgram
{
    internal static class Program
    {
        public class User
        {
            public bool   Boolean     { get; set; }
            public int    UserID      { get; set; }
            public string Name        { get; set; }
            public string PhoneNumber { get; set; }
            public string Email       { get; set; }
        }

        private static void Main()
        {
            // var structure = new Structure<User>();
            //
            // var testUser = new User
            // {
            //     Boolean     = true,
            //     UserID      = 12345,
            //     Name        = "A.J.",
            //     PhoneNumber = "111.222.3344",
            //     Email       = "aj@example.com",
            // };

            var generator = new Generator();

            var themes = new string[] {"Default", "Solids"};

            foreach (string theme in themes)
            {
                var t = JsonConvert.DeserializeObject<ThemeDefinition>
                (
                    File.ReadAllText($"Definitions/{theme}.json")
                );

                generator.Generate(t);

                string s = JsonConvert.SerializeObject(t, Formatting.Indented);

                File.WriteAllText($"Themes/{theme}/Compiled.json", s);

                //

                Writer.Write(t, "Integrant.Colorant");
            }
        }
    }
}