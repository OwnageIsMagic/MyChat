using System;
using System.Collections.Generic;
using WebSocketSharp;
using System.Threading;
using System.Threading.Tasks;


//using Microsoft.AspNet.SignalR;
//using Microsoft.Owin.Cors;
//using Microsoft.Owin.Hosting;
//using Owin;

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
            ws.Connect();
            ws.Send(new WsEvent(WsEventsType.USR_NEW, username));
        }
        public void Send(string message)
        {
            ws.Send(new WsEvent(WsEventsType.MSG, message));
        }

        //private void OnMessage(object sender, MessageEventArgs ev)
        //{
        //  Display(new WsEvent(ev.Data));
        //switch (e.Type)
        //{
        //    case WsEventsType.MSG:

        //        break;
        //    case WsEventsType.SYS_EVENT:
        //        break;
        //    case WsEventsType.NEW_USR:
        //        break;
        //    case WsEventsType.ERROR:
        //        break;
        //    default:
        //        break;
        //}
        //}
    }

}
