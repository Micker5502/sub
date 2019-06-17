using System;
using System.Collections.Generic;
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
using System.Threading;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Xaml;
using Newtonsoft.Json;
namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int sub;
        string ytResult;

        Properties.Settings MySetting = new WpfApp1.Properties.Settings();
        
        SynchronizationContext mainThread;
        Thread myThread;
        bool power = false;
        WebRequest ytRequest;
        WebResponse ytResponse;
        StreamReader sr;
        Stream st;

        public MainWindow()
        {
            InitializeComponent();
            init();
            mainThread = SynchronizationContext.Current;
            power = true;
            
            startThread();
        }
        public void init()
        {
            WebRequest.Create("https://www.googleapis.com/youtube/v3/channels?part=statistics&id=" + MySetting.Channel_ID + "&key=AIzaSyDWSLGOho3jUE5zTm-PJn9yu1TzPUUV_rs");
            try
            {
                if (MySetting.back_GroupPic != "")
                {
                    BitmapImage back_P = new BitmapImage(new Uri(MySetting.back_GroupPic, UriKind.Absolute));
                    back_Group.Source = back_P;
                }

                if (MySetting.logo_Pic != "")
                {
                    BitmapImage logoP = new BitmapImage(new Uri(MySetting.logo_Pic, UriKind.Absolute));
                    logo.Source = logoP;
                }

            }
            catch(Exception e)
            {
                MessageBox.Show(MySetting.back_GroupPic);
                back_Group.Source = new BitmapImage(new Uri("Picture/REM_Back2.png", UriKind.Relative));
                logo.Source = new BitmapImage(new Uri("Picture/Youtube_logo3.png", UriKind.Relative));
            }
            this.Top = MySetting.Win_PosY;
            this.Left = MySetting.win_PosX;
            RenderOptions.ProcessRenderMode = RenderMode.Default;
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 30 }
            );

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && MySetting.win_PosLock)
            {
                DragMove();
            }
        }

        public void startThread()
        {
            
            if (myThread != null && !myThread.IsAlive && myThread.ThreadState != ThreadState.Stopped)
                myThread.Start();
            else
            {
                myThread = new Thread(new ThreadStart(web_Refersh));
                myThread.IsBackground = true;
                startThread();
            }
        }
        private void check()
        {
            //power = false;
            //Console.WriteLine(ytResult);
            //Console.Write(ytResult);
            var data = JsonConvert.DeserializeObject<RootObject>(ytResult);
            //Console.WriteLine(data.items[0].statistics.subscriberCount);
            //int last = ytResult.IndexOf("訂閱人數");
            //int first_1 = ytResult.IndexOf("subscriber-count");
            //if (last != -1 && first_1 != -1)
            //{
            try
                {
                    //sub = Int32.Parse(ytResult.Substring(first_1 + 70, last - 27 - first_1 - 70).Replace(",", ""));
                    //sub = data.items[0].Statitics[0].subscriberCount;
                    sub = data.items[0].statistics.subscriberCount;
                }
                catch {
                    Console.WriteLine("GG");
                }
                //-----------------------------------------
                /*mainThread.Post(new SendOrPostCallback((obj) =>
                {
                    numberGroup.TurnStop(sub);
                }), "Finished!!");*/
                mainThread.Send(new SendOrPostCallback((obj) =>
                {
                    numberGroup.TurnStop(sub);
                }), "Finished!!");
            //-----------------------------------------
            //}
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MySetting.Win_PosY= this.Top ;
            MySetting.win_PosX=this.Left;
            power = false;
            MySetting.Save();
        }
        private void web_Refersh()
        {
            while (power)
            {
                MySetting = new WpfApp1.Properties.Settings();
                try
                {
                    ytRequest = WebRequest.Create("https://www.googleapis.com/youtube/v3/channels?part=statistics&id=" + MySetting.Channel_ID + "&key=AIzaSyDWSLGOho3jUE5zTm-PJn9yu1TzPUUV_rs");
                    ytRequest.Method = "GET";
                    ytResponse = ytRequest.GetResponse();
                    
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("頻道ID設定錯誤或沒有網路連接!"+e);
                    power = false;
                    
                }
                try
                {
                    st = ytResponse.GetResponseStream();
                    sr = new StreamReader(st,Encoding.UTF8);
                    ytResult = sr.ReadToEnd();
                    sr.Close();    
                    ytResponse.Close();
                    st.Close();
                }
                catch(Exception e) {
                    System.Windows.MessageBox.Show(e.ToString());
                }
                
                check();
                Console.WriteLine(RenderCapability.Tier.ToString());
                Thread.Sleep(8000);
            }
        }

        //----------------------------Alt + tab 隱藏---------------------------------------//
        void Window_Loaded(object sender, RoutedEventArgs e)
        {

            /*WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);*/
        }


        #region Window styles

        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }


        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        #endregion

        //-----------------------------------------------------------------------------------//
        //Menu_Item
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MySetting.win_PosLock == true)
            {
                MySetting.win_PosLock = false;
            }
            else MySetting.win_PosLock = true;

            MySetting.Save();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.Show();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            
            MySetting = new WpfApp1.Properties.Settings();
            ytRequest = WebRequest.Create("https://www.googleapis.com/youtube/v3/channels?part=statistics,snippet&id="+ MySetting.Channel_ID + "&key=AIzaSyDWSLGOho3jUE5zTm-PJn9yu1TzPUUV_rs");
            ytRequest.Method = "GET";
            try
            {

                BitmapImage back_P = new BitmapImage(new Uri(MySetting.back_GroupPic, UriKind.Absolute));
                back_Group.Source = back_P;
            }
            catch
            {
                
                back_Group.Source = new BitmapImage(new Uri("Picture/REM_Back2.png", UriKind.Relative));
            }
            try
            {
                
                BitmapImage logoP = new BitmapImage(new Uri(MySetting.logo_Pic, UriKind.Absolute));
                logo.Source = logoP;
            }
            catch
            {
                
                logo.Source = new BitmapImage(new Uri("Picture/Youtube_logo3.png", UriKind.Relative));
            }
            
            if (power == false)
            {
                startThread();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //MenuItem
    }
}
