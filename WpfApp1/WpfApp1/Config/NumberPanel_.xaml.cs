using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Config
{
    /// <summary>
    /// NumberPanel_.xaml 的交互逻辑
    /// </summary>
    public partial class NumberPanel_ : UserControl
    {
        private readonly int BASE_PERIOD = 10;
        private double _Speed = 1;

        public double Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        Storyboard storyboard1 = new Storyboard();
        Storyboard storyboard2 = new Storyboard();
        DoubleAnimation animation1 = new DoubleAnimation();
        DoubleAnimation animation2 = new DoubleAnimation();

        private string[] numberList = new string[] { "9","0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8" };
        public NumberPanel_()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            foreach (string i in numberList)
            {
                Number item = new Number();
                item.NumberValue = i;
                stackPanel_.Children.Add(item);
            }
        }

        public void TurnStart()
        {
            if (Speed <= 0)
            {
                return;
            }

            animation1.From = -60;
            animation1.To = -120 * 10 - 60;
            animation1.Duration = new Duration(TimeSpan.FromSeconds(BASE_PERIOD));
            animation1.SpeedRatio = Speed;
            animation1.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(animation1, stackPanel_.Name);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.TopProperty));
            storyboard1.Children.Add(animation1);
            

            storyboard1.Begin(this, true);
        }

        public void TurnStopAt(int number)
        {
            try
            {
                if (Speed <= 0)
                {
                    return;
                }
                double fromTop = (double)stackPanel_.GetValue(Canvas.TopProperty);

                double toTop = -120 * (((number + 22) % 10) + 18) - 60;

                if (fromTop - toTop > 120 * 10)
                {
                    fromTop -= 120 * 10;
                }

                animation2.From = fromTop;
                animation2.To = toTop;
                double numberCount = (fromTop - toTop) / 120;
                double duration = BASE_PERIOD * numberCount / 10;
                animation2.Duration = new Duration(TimeSpan.FromSeconds(5));
                animation2.SpeedRatio = 4;
                animation2.DecelerationRatio = 1;
                Storyboard.SetTargetName(animation2, stackPanel_.Name);
                Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.TopProperty));
                storyboard2.Children.Add(animation2);

                storyboard1.Stop(this);

                storyboard2.Begin(this, true);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        public bool IsStopped()
        {
            bool isStopped = storyboard2.GetCurrentState(this) != ClockState.Active;
            return isStopped;
        }

    }
}
