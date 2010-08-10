namespace Camp4Net.Message
{
    internal enum MessageType
    {
        TextMessage, //(regular chat message),
        PasteMessage,// (pre-formatted message, rendered in a fixed-width font),
        SoundMessage,// (plays a sound as determined by the message, which can be either “rimshot”, “crickets”, or “trombone”),
        TweetMessage //(a Twitter status URL to be fetched and inserted into the chat)
    }
}