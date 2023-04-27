﻿using MessagePack;

namespace Slithin.ModuleSystem;

[MessagePackObject]
public class ScriptInfo
{
    [Key(2)] public string Category { get; set; }

    [Key(4)] public string Description { get; set; }

    [Key(0)] public string Id { get; set; }

    [Key(3)] public bool IsAutomatable { get; set; }
    [Key(5)] public bool IsListed { get; set; }

    [Key(6)] public bool IsVPL { get; set; }

    [Key(1)] public string Name { get; set; }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module