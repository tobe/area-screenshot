using System;
using System.Threading;
using System.Windows;

namespace LoLScoreboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Worker workerObject;
        Thread workerThread;

        public MainWindow() {
            InitializeComponent();

            workerObject = new Worker(this.screenshot);
            workerThread = new Thread(workerObject.TakeScreenshot);

            this.Dispatcher.Invoke(() =>{
                workerThread.Start();
            });
        }

        protected override void OnClosed(EventArgs e) {
            workerThread.Abort(); // Kill the background thread

            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
