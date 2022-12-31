using System;
using System.Text;
using System.Windows.Forms;

namespace VTableDumper
{
    class Program
    {
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
                    string[] args = splitLine.Trim().Replace("::", ":").Split(':'); // class, func/args

                    if (curClass == null)
                    {
                        curClass = args[0];

                        sb.AppendLine($"class {curClass} {{");
                        sb.AppendLine("public:");
                    }

                    sb.AppendLine($"    virtual void {args[1].Replace(" ", "")};");
                }
            }
            sb.AppendLine("}");
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }
}