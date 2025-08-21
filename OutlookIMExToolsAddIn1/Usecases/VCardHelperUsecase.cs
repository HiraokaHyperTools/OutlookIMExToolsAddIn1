using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class VCardHelperUsecase
    {
        private readonly StringComparer _comparer = StringComparer.InvariantCultureIgnoreCase;

        public IEnumerable<VCardLine> Parse(string body)
        {
            foreach (var line in body.Replace("\r\n", "\n").Split('\n'))
            {
                int sep = line.IndexOf(':');
                if (1 <= sep)
                {
                    var left = line.Substring(0, sep);
                    var atts = new Dictionary<string, string>(_comparer);
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
                    atts = new Dictionary<string, string>(atts, _comparer);
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

        public VCardLine ResolveCharsetAndEncoding(VCardLine line)
        {
            if (true
                && line.Attributes.TryGetValue("CHARSET", out string charsetText)
                && !string.IsNullOrEmpty(charsetText)
                && Encoding.GetEncoding(charsetText) is Encoding charset
                && charset != null
            )
            {
                VCardLine ApplyNewValue(string newValue)
                {
                    var atts = new Dictionary<string, string>(line.Attributes, _comparer);
                    atts.Remove("ENCODING");
                    atts.Remove("CHARSET");
                    return new VCardLine
                    {
                        Key = line.Key,
                        Attributes = atts,
                        Value = newValue,
                        OriginalLine = line.OriginalLine,
                    };
                }

                if (true
                    && line.Attributes.TryGetValue("ENCODING", out string encoding)
                    && !string.IsNullOrEmpty(encoding)
                )
                {
                    if (false) { }
                    else if (_comparer.Compare(encoding, "QUOTED-PRINTABLE") == 0)
                    {
                        var rawString = Regex.Replace(
                            line.Value ?? "",
                            "=[0-9a-fA-F]{2}",
                            match => new string(
                                (char)Convert.ToByte(match.Value.Substring(1), 16),
                                1
                            )
                        );
                        return ApplyNewValue(
                            charset.GetString(
                                Encoding.GetEncoding("latin1").GetBytes(rawString)
                            )
                        );
                    }
                    else if (_comparer.Compare(encoding, "BASE64") == 0)
                    {
                        var bytes = Convert.FromBase64String(line.Value ?? "");
                        return ApplyNewValue(charset.GetString(bytes));
                    }
                    else if (_comparer.Compare(encoding, "8BIT") == 0 || _comparer.Compare(encoding, "7BIT") == 0)
                    {
                        return ApplyNewValue(
                            charset.GetString(
                                Encoding.GetEncoding("latin1").GetBytes(line.Value)
                            )
                        );
                    }
                }
                else
                {
                    // Pass thru. Likely Photo or JPEG or something not like text.
                }
            }

            return line;
        }

        public string GetString(VCardLine line)
            => GetString(new VCardLine[] { line });

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

        public string[] SplitAndUnescapeValueBySemic(string value)
        {
            var cells = new List<string>();
            var sb = new StringBuilder(value.Length);

            for (int x = 0, cx = value.Length; x < cx;)
            {
                if (value[x] == '\\')
                {
                    x++;
                    if (x < cx)
                    {
                        var ch = value[x];
                        x++;
                        if (false) { }
                        else if (ch == 'n')
                        {
                            sb.Append("\n");
                        }
                        else if (ch == 'r')
                        {
                            sb.Append("\r");
                        }
                        else if (ch == '\\')
                        {
                            sb.Append("\\");
                        }
                        else if (ch == ';')
                        {
                            sb.Append(";");
                        }
                        else
                        {
                            sb.Append("\\");
                            sb.Append(ch);
                        }
                    }
                }
                else if (value[x] == ';')
                {
                    cells.Add(sb.ToString());
                    sb.Clear();
                    x++;
                }
                else
                {
                    sb.Append(value[x]);
                    x++;
                }
            }

            cells.Add(sb.ToString());

            return cells.ToArray();
        }

        internal bool ParseDate(string value, out DateTime date)
        {
            // 20001231
            if (true
                && value.Length == 8
                && char.IsDigit(value[0])
                && char.IsDigit(value[1])
                && char.IsDigit(value[2])
                && char.IsDigit(value[3])
                && char.IsDigit(value[4])
                && char.IsDigit(value[5])
                && char.IsDigit(value[6])
                && char.IsDigit(value[7])
                && DateTime.TryParse(
                    value.Substring(0, 4) + "-" + value.Substring(4, 2) + "-" + value.Substring(6, 2),
                    out date
                )
            )
            {
                return true;
            }
            else
            {
                date = default;
                return false;
            }
        }
    }
}