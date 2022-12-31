using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VTableDumper
{
    class Program
    {
        static string[] ParseMethodArguments(string methodCall)
        {
            string[] split = methodCall.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 1)
                return null;

            return split.Skip(1).ToArray();
        }

        [STAThread]
        static void Main(string[] a1)
        {
            string[] lines = Clipboard.GetText().Split('\n');

            StringBuilder sb = new StringBuilder();
            string curClass = null;
            foreach (string line in lines)
            {
                if (line.Contains("::") && line.Contains(";"))
                {
                    string splitLine = line.Split(';')[1];
                    string[] args = splitLine.Trim().Replace("::", ":").Split(':'); // class, func(args)

                    if (curClass == null)
                    {
                        curClass = args[0];

                        sb.AppendLine($"class {curClass} {{");
                        sb.AppendLine("public:");
                    }

                    string[] funcArgs = ParseMethodArguments(args[1].Replace(" ", ""));
                    string newFuncArgs = "";

                    if (funcArgs.Length > 0)
                    {
                        int index = 1;
                        foreach (string arg in funcArgs)
                        {
                            newFuncArgs += arg + " a" + index + ", ";
                            index++;
                        }
                    }

                    sb.AppendLine($"    virtual void {args[1].Split('(')[0]}({newFuncArgs.Trim().Trim(',')});");
                }
            }
            sb.AppendLine("}");
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }
}