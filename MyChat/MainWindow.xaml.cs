﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatClient cc;
        public MainWindow()
        {
            InitializeComponent();

            SetUsername();
        }

        private void SetUsername()
        {
            //var sp = new StackPanel
            //{
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center
            //};
            //var usernameText = new TextBox { Text = "user" + new Random().Next() };
            //var bt = new Button
            //{
            //    Content = "Enter username",
            //    Margin = new Thickness(5, 5, 5, 5)
            //};
            //bt
            loginButton.Click += (_, __) =>
            {
                if (usernameText.Text != string.Empty)
                {
                    cc = new ChatClient(usernameText.Text, ProcessEvent);
                    //tb_history.Text += "Username: " + tb.Text + '\n';
                    //Content = mainGrid;
                    mainGrid.Visibility = Visibility.Visible;
                    loginPanel.Visibility = Visibility.Collapsed;
                    cc.Connect(usernameText.Text);

                    // bt = null; tb = null; sp = null; // eb = null;
                }
            };
            //sp.Children.Add(tb);
            //sp.Children.Add(bt);
            //Content = sp;
        }
        void ProcessEvent(WsEvent e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(() =>
            {
                switch (e.Type)
                {
                    case WsEventsType.USR_NEW:
                        tb_history.Text += "New user joined: " + e.Payload + '\n';
                        break;
                    case WsEventsType.MSG:
                        var m = e.Payload.Split(WsEvent.Delimiter, 2);
                        tb_history.Text += m[0] + ": " + m[1] + '\n';
                        //tb_history.Text += e.Payload + '\n';
                        break;
                    case WsEventsType.ERROR:
                        tb_history.Text += "ERROR: " + e.Payload + '\n';
                        break;
                    case WsEventsType.USR_LEAVE:
                        tb_history.Text += e.Payload + " left chat\n";
                        break;
                    default:
                        tb_history.Text += "Unknown event: " + e + '\n';
                        break;
                }
            }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            cc.Disconnect();
            base.OnClosing(e);
        }

        private void bt_send_Click(object sender, RoutedEventArgs e)
        {
            //tb_history.Text += tx_out.Text + '\n';
            cc.Send(tx_out.Text);
            sc_scroll.ScrollToBottom();
            tx_out.Clear();
        }
    }
}
