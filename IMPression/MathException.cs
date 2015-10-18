using System;

namespace IMPression
{
    public class MathException : Exception
    {
        public MathException(string parsedString, int indexOfError, string message) : base(message)
        {
            ParsedString = parsedString;
            IndexOfError = indexOfError;
        }

        public new string Message
            =>
                $"Ligne : {ParsedString}" + (IndexOfError != 0 ? $"\nIndex : {IndexOfError}" : "") +
                $"\nMessage : {base.Message}";

        public string ParsedString { get; set; }
        public int IndexOfError { get; set; }
    }
}