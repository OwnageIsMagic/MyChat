using System;
using System.Linq;

namespace MyChat
{
    public class WsEvent
    {
        public WsEventsType Type;
        public string Payload;
        private string msg;
        public WsEvent(string msg)
        {
            this.msg = msg;
            Process();
        }

        public WsEvent(WsEventsType type, string payload)
        {
            Type = type;
            Payload = payload;
        }

        private void Process()
        {
            Type = (WsEventsType)Enum.ToObject(typeof(WsEventsType), msg[0]);
            Payload = msg.Substring(1);
        }

        public override string ToString() => (char)Type + Payload;
        public static implicit operator string(WsEvent e)
        {
            return e.ToString();
        }
    }

    public enum WsEventsType
    {
        //SYS_EVENT = 1,
        MSG = 1,
        USR_NEW,
        USR_LEAVE,
        ERROR
    }
}