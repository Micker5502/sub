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

namespace WpfApp1.Config
{
    /// <summary>
    /// Number.xaml 的交互逻辑
    /// </summary>
    public partial class Number : UserControl
    {
        private string _NumberValue;
        public string _font_Family;
        /// <summary>
        /// 数字值
        /// </summary>
        public string NumberValue
        {
            get
            {
                return _NumberValue;
            }
            set
            {
                textBlockNumber.Text = value.ToString();
                _NumberValue = value;
            }
        }

        public Number()
        {
            InitializeComponent();
        }
    }
}
