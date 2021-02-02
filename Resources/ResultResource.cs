using System.Collections.Generic;

namespace HR_Management.Resources
{
    public class ResultResource
    {
        public bool Success { get; private set; }
        public List<string> Messages { get; private set; }

        public ResultResource(List<string> messages, bool flag = false)
        {
            this.Messages = messages ?? new List<string>();
            this.Success = flag;
        }

        public ResultResource(string message, bool flag = false)
        {
            this.Messages = new List<string>();
            this.Success = flag;
            if (!string.IsNullOrWhiteSpace(message))
            {
                this.Messages.Add(message);
            }
        }
    }
}
