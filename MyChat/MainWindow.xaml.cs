using System;
using System.Collections.Generic;
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
            var sp = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            var tb = new TextBox { Text = "user" + new Random().Next() };
            var bt = new Button
            {
                Content = "Enter username",
                Margin = new Thickness(5, 5, 5, 5)
            };
            bt.Click += (_, __) =>
            {
                if (tb.Text != string.Empty)
                {
                    cc = new ChatClient(tb.Text, ProcessEvent);
                    //tb_history.Text += "Username: " + tb.Text + '\n';
                    Content = mainGrid;
                    cc.Connect(tb.Text);

                    // bt = null; tb = null; sp = null; // eb = null;
                }
            };
            sp.Children.Add(tb);
            sp.Children.Add(bt);
            Content = sp;
        }

        void ProcessEvent(WsEvent e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(() =>
            {
                switch (e.Type)
                {
                    case WsEventsType.USR_NEW:
                        tb_history.Text += "New user joined" + e.Payload + '\n';
                        break;
                    case WsEventsType.MSG:
                        tb_history.Text += e.Payload + '\n';
                        break;
                    case WsEventsType.ERROR:
                        tb_history.Text += "ERROR: " + e.Payload + '\n';
                        break;
                    default:
                        tb_history.Text += "Unknown event: " + e + '\n';
                        break;
                }
                //tb_history.Text += e + '\n';
            }));
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
