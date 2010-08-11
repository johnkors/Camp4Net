namespace Camp4Net.Message
{
    internal class TextMessage : CampfireMessage
    {
        internal TextMessage(string message)
        {
            BuildMessage(MessageType.TextMessage, message);
        }
    }
}