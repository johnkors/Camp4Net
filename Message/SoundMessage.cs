namespace Camp4Net.Message
{
    internal class SoundMessage : CampfireMessage
    {
        internal SoundMessage(MessageSound sound)
        {
            BuildMessage(MessageType.SoundMessage, sound.ToString());
        }
    }
}