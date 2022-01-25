﻿using System.Collections.Generic;
using LiteDB;

namespace Slithin.Core;

public class Settings
{
    public ObjectId _id { get; set; }

    public bool AutomaticScreenRecovery { get; set; }
    public bool AutomaticTemplateRecovery { get; set; }
    public bool AutomaticUpdates { get; set; }

    public Dictionary<string, string> CustomSettings { get; set; } = new();

    public string Get(string key)
    {
        if (CustomSettings.ContainsKey(key))
        {
            return CustomSettings[key];
        }

        return string.Empty;
    }

    public void Put(string key, string value)
    {
        if (CustomSettings.ContainsKey(key))
        {
            CustomSettings[key] = value;
        }
        else
        {
            CustomSettings.Add(key, value);
        }
    }
}
