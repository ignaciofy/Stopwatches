using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

namespace WpfStopWatch.Models
{
    public class TimeStore
    {
        public CheckBox CheckBox { get; set; }
        public TextBlock TextBlock { get; set; }
        public Stopwatch StopWatch { get; set; }
    }
}
