using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VTableDumper
{
    class Program
    {
        static string[] ParseMethodArguments(string methodCall)
        {
            foreach (var arg in settings)
                methodCall = methodCall.Replace(arg.Key, arg.Value);

            string[] split = methodCall.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 1)
                return null;

            return split.Skip(1).ToArray();
        }

        public static Dictionary<string, string> settings = new Dictionary<string, string>()
        {
            { "Vec2", "Vector2<float>" },
            { "Vec3", "Vector3<float>" },
            { "Vec4", "Vector4<float>" },

            { "Vec2i", "Vector2<int>" },
            { "Vec3i", "Vector3<int>" },
            { "Vec4i", "Vector4<int>" },
        };

        [STAThread]
        static void Main(string[] a1)
        {
            string[] lines = Clipboard.GetText().Split('\n');

            StringBuilder sb = new StringBuilder();
            string curClass = null;

            int funcIndex = 0;
            foreach (string line in lines)
            {
                if (line.Contains("::") && line.Contains(";"))
                {
                    string splitLine = line.Split(';')[1].Trim();

                    string mainstr = splitLine.Substring(splitLine.IndexOf(':') + 2, splitLine.Length - splitLine.IndexOf(':') - 2);

                    if (curClass == null)
                    {
                        curClass = splitLine.Substring(0, splitLine.IndexOf(':'));

                        sb.AppendLine($"class {curClass} {{");
                        sb.AppendLine("public:");
                    }

                    string[] funcArgs = ParseMethodArguments(
                        mainstr.Replace(" ", "").Replace("const", " const")
                    );
                    string newFuncArgs = "";

                    if (funcArgs != null && funcArgs.Length > 0)
                    {
                        if (!(funcArgs.Length == 1 && funcArgs[0] == "void"))
                        {
                            int index = 1;
                            foreach (string arg in funcArgs)
                            {
                                newFuncArgs += arg + " a" + index + ", ";
                                index++;
                            }
                        }

                        //Console.WriteLine(newFuncArgs);
                    }

                    sb.AppendLine($"    virtual void {mainstr.Split('(')[0]}({newFuncArgs.Trim().Trim(',')});");
                }

                funcIndex++;
            }
            sb.AppendLine("}");
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }
}