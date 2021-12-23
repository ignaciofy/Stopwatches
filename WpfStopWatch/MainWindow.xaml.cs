using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using WpfStopWatch.Models;

namespace WpfStopWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;
        public List<TimeStore> values { get; set; }
        public int Counter = 0;
        static Dictionary<CheckBox, Stopwatch> cbstopwatches;
        static Dictionary<CheckBox, TextBlock> cbtextBlocks;
        static List<CheckBox> checkboxes;
        static List<TextBlock> textBlocks;
        public MainWindow()
        {
            InitializeComponent();
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            checkboxes = new List<CheckBox> { cbStopWatch1, cbStopWatch2, cbStopWatch3, cbStopWatch4, cbStopWatch5 };
            textBlocks = new List<TextBlock> { ClockTextBlock1, ClockTextBlock2, ClockTextBlock3, ClockTextBlock4, ClockTextBlock5 };
            cbstopwatches = new Dictionary<CheckBox, Stopwatch>();
            cbtextBlocks = new Dictionary<CheckBox, TextBlock>();
            for (int i = 0; i < checkboxes.Count; i++)
            {
                cbstopwatches.Add(checkboxes[i], new Stopwatch());
                cbtextBlocks.Add(checkboxes[i], textBlocks[i]);
            }

        }

        void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            if (Environment.HasShutdownStarted)
            {
                MessageBox.Show("Total amount of time the system logged on:" + ClockTextBlock1.Text);
            }

        }

        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                // ...
                case SessionSwitchReason.SessionLock:
                    if (stopWatch.IsRunning)
                        stopWatch.Stop();
                    break;
                case SessionSwitchReason.SessionUnlock:
                    stopWatch.Start();
                    dt.Start();
                    break;
                case SessionSwitchReason.SessionLogoff:
                    MessageBox.Show("Total amount of time the system logged on:" + ClockTextBlock1.Text);
                    break;
                    // ...
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {

            foreach (var stopWatch in cbstopwatches.Where(x => x.Key.IsChecked == true))
            {
                if (stopWatch.Value.IsRunning)
                {
                    TimeSpan ts = stopWatch.Value.Elapsed;
                    currentTime = String.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                    cbtextBlocks.FirstOrDefault(x => x.Key == stopWatch.Key).Value.Text = currentTime;

                }
            }
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var stopWatch in cbstopwatches.Where(x => x.Key.IsChecked == true))
            {
                stopWatch.Value.Start();
                dt.Start();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var stopWatch in cbstopwatches.Where(x => x.Key.IsChecked == true))
            {
                if (stopWatch.Value.IsRunning)
                {
                    stopWatch.Value.Stop();
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
