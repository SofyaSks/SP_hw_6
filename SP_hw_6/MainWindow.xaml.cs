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

namespace SP_hw_6
{

    public partial class MainWindow : Window
    {
       

        CancellationTokenSource cancel = null;

        
        
        public MainWindow()
        {
            InitializeComponent();

            Random random = new Random();

            ProgressBar[] progresses = { progressBar1, progressBar2, progressBar3, progressBar4 };

            TextBlock[] textBlocks = { TextBlock_1, TextBlock_2, TextBlock_3, TextBlock_4 };

            FillProgressBars(progresses, textBlocks);

        }

        private void Button_Click_Copy(object sender, RoutedEventArgs e)
        {

            cancel = new CancellationTokenSource();
            var token = cancel.Token;
            try
            {
                Task task1 = Task.Run(() => FillProgressBarsValue(progressBar1, token));
                Task task2 = Task.Run(() => FillProgressBarsValue(progressBar2, token));
                Task task3 = Task.Run(() => FillProgressBarsValue(progressBar3, token));
                Task task4 = Task.Run(() => FillProgressBarsValue(progressBar4, token));               
                Task.WhenAll(task1, task2, task3, task4).ContinueWith(a => MessageBox.Show("Копирование завершено"));
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cancel.Dispose();
            }           
        }

       
        private void FillProgressBars(ProgressBar[] pb, TextBlock[] tb)
        {
            Random random = new Random();

            for (int i = 0; i < pb.Length; i++)
            {
                pb[i].Maximum = random.Next(100, 15000);
                tb[i].Text = pb[i].Maximum.ToString();
            }
        }

        private void FillProgressBarsValue(ProgressBar pb, CancellationToken token)
        {
            Dispatcher.Invoke(() =>
            {

                while (pb.Value < pb.Maximum)
                {
                    pb.Value++;                 

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            });
            
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            cancel.Cancel();
        }
    }
}
