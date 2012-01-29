﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using IniLookup = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>;

namespace Kudu.Core.Infrastructure
{
    public class IniFile
    {
        private readonly string _path;
        private IniLookup _sectionLookup;

        public IniFile(string path)
        {
            _path = path;
        }

        public string GetSectionValue(string section, string key)
        {
            ParseIniFile();

            Dictionary<string, string> valueLookup;
            string value;
            if (_sectionLookup.TryGetValue(section, out valueLookup) &&
                valueLookup.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }

        private void ParseIniFile()
        {
            if (_sectionLookup != null)
            {
                return;
            }

            var files = File.ReadAllLines(_path);
            ParseValues(files, out _sectionLookup);
        }

        internal static void ParseValues(IList<string> lines, out IniLookup iniFile)
        {
            iniFile = new IniLookup(StringComparer.OrdinalIgnoreCase);

            Dictionary<string, string> currentSection = null;
            foreach (var line in lines)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var value = line.Trim();
                if (value.Length > 2 &&
                    value[0] == '[' &&
                    value[value.Length - 1] == ']')
                {
                    // Get the section name
                    string sectionValue = value.Substring(1, value.Length - 2).Trim();

                    if (!String.IsNullOrEmpty(sectionValue))
                    {
                        // Create a new sectio
                        currentSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                        // Create a new section
                        iniFile[sectionValue] = currentSection;
                    }
                }
                else if (currentSection == null)
                {
                    // If there's no section then ignore the values
                    continue;
                }
                else
                {
                    var kvp = value.Split('=');
                    if (kvp.Length == 2)
                    {
                        var key = kvp[0];
                        var keyValue = kvp[1];

                        if (String.IsNullOrEmpty(key))
                        {
                            // Skip empty keys
                            continue;
                        }

                        // Add it to the list of keys
                        currentSection[key.Trim()] = keyValue.Trim();
                    }
                }

            }
        }
    }
}
