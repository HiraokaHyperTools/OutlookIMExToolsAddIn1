using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class VCardHelperUsecase
    {
        public IEnumerable<VCardLine> Parse(string body)
        {
            foreach (var line in body.Replace("\r\n", "\n").Split('\n'))
            {
                int sep = line.IndexOf(':');
                if (1 <= sep)
                {
                    var left = line.Substring(0, sep);
                    var atts = new Dictionary<string, string>();
                    var cells = left.Split(';');
                    for (int x = 0; x < cells.Length; x++)
                    {
                        var cell = cells[x];
                        var set = cell.IndexOf('=');
                        if (1 <= set)
                        {
                            atts[cell.Substring(0, set)] = cell.Substring(set + 1);
                        }
                    }

                    yield return new VCardLine
                    {
                        OriginalLine = line,
                        Key = cells[0],
                        Attributes = atts,
                        Value = line.Substring(sep + 1),
                    };
                }
            }
        }

        [Obsolete("This way won't work!")]
        public IEnumerable<VCardLine> MakeOutlookSafe(IEnumerable<VCardLine> lines, Func<string, string> decode)
        {
            foreach (var line in lines)
            {
                var value = line.Value;
                var atts = line.Attributes;

                if (false
                    || line.Attributes.ContainsKey("CHARSET")
                    || line.Attributes.ContainsKey("ENCODING")
                    || line.Value.All(chr => 0x20 <= chr && chr <= 0x7e)
                )
                {
                    // pass thru
                }
                else
                {
                    atts = new Dictionary<string, string>(atts);
                    atts["CHARSET"] = "UTF-8";
                    atts["ENCODING"] = "QUOTED-PRINTABLE";

                    value = string.Concat(
                        Encoding.UTF8.GetBytes(decode(value))
                            .Select(it => $"={it:X2}")
                    );
                }

                yield return new VCardLine
                {
                    Key = line.Key,
                    Attributes = atts,
                    Value = value,
                    OriginalLine = line.OriginalLine,
                };
            }
        }

        public string GetString(IEnumerable<VCardLine> lines)
        {
            var writer = new StringWriter();
            foreach (var line in lines)
            {
                var atts = string.Concat(line.Attributes.Select(it => $";{it.Key}={it.Value}"));
                writer.WriteLine($"{line.Key}{atts}:{line.Value}");
            }
            return writer.ToString();
        }
    }
}