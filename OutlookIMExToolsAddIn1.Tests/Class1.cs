using NUnit.Framework;
using OutlookIMExToolsAddIn1.Usecases;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OutlookIMExToolsAddIn1.Tests
{
    public class Class1
    {
        private string ResolveFiles(string path)
            => Path.Combine(TestContext.CurrentContext.WorkDirectory, "..", "..", "..", "Files", path);

        [Test]
        [Ignore("Private use")]
        public void TestVCardHelperUsecase()
        {
            var it = new VCardHelperUsecase();

            var latin1 = Encoding.GetEncoding("latin1");
            var vcf = File.ReadAllText(ResolveFiles("com_dot_example.vcf"), latin1);
            var lines = it.Parse(vcf).ToArray();
#pragma warning disable CS0618 // Type or member is obsolete
            var saferLines = it.MakeOutlookSafe(lines, text => Encoding.UTF8.GetString(latin1.GetBytes(text))).ToArray();
#pragma warning restore CS0618 // Type or member is obsolete
            File.WriteAllText("com_dot_example.rewrite.vcf", it.GetString(saferLines));
        }
    }
}
