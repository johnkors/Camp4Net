using Camp4Net.Message;

namespace Camp4Net
{
    public interface IMailService
    {
        string PostSound(MessageSound soundType);
        string PostText(string message);
        string PostPaste(string message);
    }
}