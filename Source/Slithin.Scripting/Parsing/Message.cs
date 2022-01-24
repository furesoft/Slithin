namespace Slithin.Scripting.Parsing;

public enum MessageSeverity
{
    Error, Warning, Info, Hint
}

public class Message
{
    public Message(MessageSeverity severity, string text, int line, int column)
    {
        Severity = severity;
        Text = text;
        Column = column;
        Line = line;
    }

    public int Column { get; set; }
    public int Line { get; set; }
    public MessageSeverity Severity { get; set; }
    public string Text { get; set; }

    public static Message Error(string message, int line, int column)
    {
        return new Message(MessageSeverity.Error, message, line, column);
    }

    public static Message Info(string message, int line, int column)
    {
        return new Message(MessageSeverity.Info, message, line, column);
    }

    public static Message Warning(string message, int line, int column)
    {
        return new Message(MessageSeverity.Warning, message, line, column);
    }

    public override string ToString()
    {
        return $"{Severity}: {Text} at line {Line} column {Column}.";
    }
}
