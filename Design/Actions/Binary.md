# Action Binary

An Action Binary File is a extended WebAssembly binary file.
It consists of 3 extra sections:
- .ui for the ui definition if needed
- .image for the icon displayed in the UI
- .info Metadata about the action

### .info Section

    string ID
    string Name
    string Category
    bool IsAutomatable
    string Description

Metadata is serialized with MessagePack

### .ui Section
