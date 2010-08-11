using System;

namespace Camp4Net.Message
{
    internal abstract class CampfireMessage
    {
        protected string _jsonMessage;

        protected void BuildMessage(MessageType type, string message)
        {
            message = Clean(message);
            Validate(message);
            _jsonMessage = "{'message':{'type':'" + type + "', 'body':'" + message + "'}}";
        }

        private string Clean(string message)
        {
            return message.Replace("'", "");
        }

        private void Validate(string message)
        {
            if (message.Contains("'"))
            {
                throw new NotSupportedException("Message cannot contain single char ' ");
            }
        }

        public override string ToString()
        {
            return _jsonMessage;
        }
    }
}