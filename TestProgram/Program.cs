using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Colorant;
using Colorant.ColorGeneratorInterop;
using Colorant.Definition;
using Integrant.Fundament;
using Integrant.Fundament.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            // List<Color> r = generator.Generate();

            // structure.Register(new Member<User, bool>(nameof(User.Boolean)));
            // structure.Register(new Member<User, int>(nameof(User.UserID)));
            // structure.Register(new Member<User, string>(nameof(User.Name)));
            // structure.Register(new Member<User, string>(nameof(User.PhoneNumber),
            //                                             memberIsVisible: (s, v, m) => v.Name?.Length > 0));
            // structure.Register(new Member<User, string>(nameof(User.Email)));

            // structure.MemberSet = MemberSet.FromClass<User>(testUser);

            // foreach (IMember<User> memberSetMember in structure.MemberSet.Members)
            // {
            //     Console.WriteLine(memberSetMember.ID);
            // }

            // var tDefault = new ThemeDefinition
            // {
            //     Name = "Default",
            //     Blocks = new List<Block>
            //     {
            //         new Block
            //         {
            //             Name = "Background",
            //             IDs  = Enumerable.Range(0, 21).Select(v => v.ToString()).ToList(),
            //         },
            //         new Block
            //         {
            //             Name = "Border",
            //             IDs  = Enumerable.Range(0, 21).Select(v => v.ToString()).ToList(),
            //         },
            //         new Block
            //         {
            //             Name = "Accent",
            //             IDs  = Enumerable.Range(0, 21).Select(v => v.ToString()).ToList(),
            //         },
            //         new Block
            //         {
            //             Name = "Text",
            //             IDs  = Enumerable.Range(0, 21).Select(v => v.ToString()).ToList(),
            //         },
            //     },
            // };
            //
            // //
            //
            // var vDark = new Variant
            // {
            //     Name = "Dark",
            //     BlockSources = new Dictionary<string, VariantBlockColorSource>
            //     {
            //         {"Background", VariantBlockColorSource.Range},
            //         {"Border", VariantBlockColorSource.Range},
            //         {"Accent", VariantBlockColorSource.Range},
            //         {"Text", VariantBlockColorSource.Range},
            //     },
            //     BlockColorsRange = new Dictionary<string, ColorRange>
            //     {
            //         {
            //             "Background", new ColorRange
            //             {
            //                 // Steps         = 21,
            //                 HueStart      = 259,
            //                 HueEnd        = 250,
            //                 HueCurve      = "linear",
            //                 SatStart      = 10,
            //                 SatEnd        = 20,
            //                 SatCurve      = "easeOutQuart",
            //                 LumStart      = 0,
            //                 LumEnd        = 37,
            //                 LumCurve      = "linear",
            //                 HoverStepSize = 2,
            //                 FocusStepSize = 3,
            //             }
            //         },
            //     },
            // };
            //
            // var vLight = new Variant
            // {
            //     Name = "Light",
            //     BlockSources = Enumerable.Range(0, 21)
            //                              .ToDictionary(k => k.ToString(), v => VariantBlockColorSource.Range),
            // };
            //
            // var vMatrix = new Variant
            // {
            //     Name = "Matrix",
            //     BlockSources = Enumerable.Range(0, 21)
            //                              .ToDictionary(k => k.ToString(), v => VariantBlockColorSource.Range),
            // };
            //
            // var vPink = new Variant
            // {
            //     Name = "Pink",
            //     BlockSources = Enumerable.Range(0, 21)
            //                              .ToDictionary(k => k.ToString(), v => VariantBlockColorSource.Range),
            // };
            //
            // var vWhite = new Variant
            // {
            //     Name = "White",
            //     BlockSources = Enumerable.Range(0, 21)
            //                              .ToDictionary(k => k.ToString(), v => VariantBlockColorSource.Range),
            // };
            //
            // tDefault.Variants.AddRange(new[] {vDark, vLight, vMatrix, vPink, vWhite});
            //
            // //
            //
            // generator.Generate(tDefault);
            //
            // string s = JsonConvert.SerializeObject(tDefault, Formatting.Indented);
            //
            // Console.WriteLine(s);

            //

            var tDefault = JsonConvert.DeserializeObject<ThemeDefinition>
            (
                File.ReadAllText("Themes/Default/Definition.json")
            );

            generator.Generate(tDefault);

            string s = JsonConvert.SerializeObject(tDefault, Formatting.Indented);
            Console.WriteLine(s);
            
            File.WriteAllText("Themes/Default/Compiled.json", s);
        }
    }
}