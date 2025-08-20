using OutlookIMExToolsAddIn1.Helpers;
using OutlookIMExToolsAddIn1.Usecases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OutlookIMExToolsAddIn1.Usecases
{
    public class ThunderbirdProfilesUsecase
    {
        private readonly ParseIniUsecase _parseIniUsecase;

        public ThunderbirdProfilesUsecase(ParseIniUsecase parseIniUsecase)
        {
            _parseIniUsecase = parseIniUsecase;
        }

        public List<ThunderbirdProfile> ListAll()
        {
            var list = new List<ThunderbirdProfile>();

            var profilesDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Thunderbird"
            );
            var profilesIniFile = Path.Combine(
                profilesDir,
                "profiles.ini"
            );
            if (File.Exists(profilesIniFile))
            {
                var profilesIni = _parseIniUsecase.ParseIni(File.ReadAllText(profilesIniFile, Encoding.Default));
                for (int y = 0; ; y++)
                {
                    if (profilesIni.TryGetValue($"Profile{y}", out var profile) && profile != null)
                    {
                        if (true
                            && profile.TryGetValue("Name", out var name)
                            && profile.TryGetValue("Path", out var path)
                            && profile.TryGetValue("IsRelative", out var isRelative)
                        )
                        {
                            var profileDir = (isRelative == "1")
                                ? Path.Combine(profilesDir, path)
                                : path
                                ;

                            if (Directory.Exists(profileDir))
                            {
                                list.Add(new ThunderbirdProfile(
                                    name,
                                    Path.GetFullPath(profileDir)
                                ));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return list;
        }
    }
}