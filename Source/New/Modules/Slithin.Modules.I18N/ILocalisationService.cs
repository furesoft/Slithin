﻿namespace Slithin.Modules.I18N;

public interface ILocalisationService
{
    string GetString(string key);

    public string GetStringFormat(string key, params object[] args)
    {
        return string.Format(key, args);
    }
}
