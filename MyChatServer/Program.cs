using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

using System.Threading;

using MyChat;

namespace MyChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer();
            //wssv.Log.Level = LogLevel.Warn;
            wssv.AddWebSocketService<ChatBehaviour>("/chat");
            wssv.Start();
            Console.ReadKey(true);
            wssv.Stop();
        }

        class ChatBehaviour : WebSocketBehavior
        {
            public Dictionary<string, string> Users = new Dictionary<string, string>();
            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);
                Sessions.Broadcast(new WsEvent(WsEventsType.USR_LEAVE, Users[ID]));
                Users.Remove(ID);
            }

            //protected override void OnError(ErrorEventArgs e) => base.OnError(e);
            protected override void OnMessage(MessageEventArgs ev)
            {
                //base.OnMessage(e);
                var e = new WsEvent(ev.Data);
                Console.WriteLine(ev.Data);
                switch (e.Type)
                {
                    case WsEventsType.MSG:
                        if (Users.ContainsKey(ID))
                            Sessions.Broadcast(new WsEvent(WsEventsType.MSG,
                                $"{Users[ID]}\t{e.Payload}"));
                        else
                            Send(new WsEvent(WsEventsType.ERROR, "set username"));
                        break;
                    case WsEventsType.USR_NEW:
                        if (Users.ContainsKey(ID))
                            Send(new WsEvent(WsEventsType.ERROR, "username is occupied"));
                        else
                        {
                            Users.Add(ID, e.Payload);
                            Sessions.Broadcast(new WsEvent(WsEventsType.USR_NEW, e.Payload));
                        }
                        break;
                    default:
                        Console.WriteLine("Unk ev!!!");
                        break;
                }
            }

            //void SendToOthers(string data)
            //{
            //    foreach (var s in Sessions.Sessions)
            //    {
            //        if (s.ID == ID) continue;
            //        Sessions.SendTo(data, s.ID);
            //    }
            //}
        }

    }
}
