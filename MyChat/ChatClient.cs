using System;
using System.Collections.Generic;
using WebSocketSharp;
using System.Threading;
using System.Threading.Tasks;

namespace MyChat
{
    class ChatClient
    {
        WebSocket ws;

        //public List<string> history;
        public Dictionary<string, string> Users = new Dictionary<string, string>();
        public string Username;
        private Action<WsEvent> Display;

        public ChatClient(string username, Action<WsEvent> display)
        {
            Username = username;
            Display = display;
        }

        public void Connect(string username)
        {
            Username = username;
            ws = new WebSocket("ws://localhost/chat");
            ws.OnMessage += (_, ev) => Display(new WsEvent(ev.Data));
            ws.OnClose += (_, ev) => Display(new WsEvent(WsEventsType.ERROR, "Disconnected"));
            ws.Connect();
            ws.Send(new WsEvent(WsEventsType.USR_NEW, username));
        }
        public void Send(string message)
        {
            ws.Send(new WsEvent(WsEventsType.MSG, message));
        }

        public void Disconnect()
        {
            ws.Close();
        }
    }

}
