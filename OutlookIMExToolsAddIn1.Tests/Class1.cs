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
        public void TestVCardHelperUsecaseOutlook()
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

        [Test]
        [TestCase("N:コム", "N:コム\r\n")]
        [TestCase("N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=E3=82=B3=E3=83=A0", "N:コム\r\n")]
        [TestCase("N;CHARSET=UTF-8;ENCODING=BASE64:44Kz44Og", "N:コム\r\n")]
        [TestCase("N;CHARSET=Shift_JIS;ENCODING=QUOTED-PRINTABLE:=83=52=83=80", "N:コム\r\n")]
        [TestCase("N;CHARSET=Shift_JIS;ENCODING=BASE64:g1KDgA==", "N:コム\r\n")]
        [TestCase("N;CHARSET=UTF-8;ENCODING=7BIT:com;dot;example", "N:com;dot;example\r\n")]
        public void TestVCardHelperUsecaseResolveCharsetAndEncoding(string input, string outputExpected)
        {
            var it = new VCardHelperUsecase();

            Assert.That(
                it.GetString(it.ResolveCharsetAndEncoding(it.Parse(input).Single())),
                Is.EqualTo(outputExpected)
            );
        }

        [Test]
        public void TestVCardHelperUsecaseResolveCharsetAndEncodingSpecial()
        {
            // These are some cases which NUnit won't execute well due to escaped unicode string.

            TestVCardHelperUsecaseResolveCharsetAndEncoding(
                "N;CHARSET=UTF-8;ENCODING=8BIT:\u00E3\u0082\u00B3\u00E3\u0083\u00A0",
                "N:コム\r\n"
            );

            TestVCardHelperUsecaseResolveCharsetAndEncoding(
                "N;CHARSET=Shift_JIS;ENCODING=8BIT:\u0083\u0052\u0083\u0080",
                "N:コム\r\n"
            );
        }

        [Test]
        public void TestVCardHelperUsecaseSplitAndUnescapeValueBySemic()
        {
            var it = new VCardHelperUsecase();

            Assert.That(
                it.SplitAndUnescapeValueBySemic("コム;イグ\\;ザンプル;ドット;;"),
                Is.EqualTo(
                    new string[]
                    {
                        "コム",
                        "イグ;ザンプル",
                        "ドット",
                        "",
                        "",
                    }
                )
            );

            Assert.That(
                it.SplitAndUnescapeValueBySemic("イグ;ザンプル ドット コム"),
                Is.EqualTo(
                    new string[]
                    {
                        "イグ",
                        "ザンプル ドット コム",
                    }
                )
            );

            Assert.That(
                it.SplitAndUnescapeValueBySemic("コム"),
                Is.EqualTo(
                    new string[]
                    {
                        "コム",
                    }
                )
            );
        }
    }
}
