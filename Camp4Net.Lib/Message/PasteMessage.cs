namespace Camp4Net.Message
{
    internal class PasteMessage : CampfireMessage
    {
        internal PasteMessage(string message)
        {
            BuildMessage(MessageType.PasteMessage, message);
        }
    }
}