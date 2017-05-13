using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace itiblab3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(List<double> En)
        {
            InitializeComponent();
             GraphPane pane = zedGraphControl1.GraphPane;

            pane.XAxis.Title.Text = "Эпоха"; //подпись оси X
            pane.YAxis.Title.Text = "Ошибка"; //подпись оси Y
            
                pane.Title.Text = "Зависимость среднеквадратической ошибки от эпохи"; 
            

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list1 = new PointPairList(); // Для y
            
            int i = 1;

            for (double x = 1; x <= En.Count; x++) // нумерация с единицы
            {
                list1.Add(x, En[i - 1]); //расчитываем координаты
                
                i++;
            }
            
            
            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("", list1, Color.Black, SymbolType.Circle);
            

           
                myCurve.Line.IsVisible = true;  
               
            // !!!
            // Цвет заполнения отметок (ромбиков) - голубой
            myCurve.Symbol.Fill.Color = Color.White;
            

            // !!!
            // Тип заполнения - сплошная заливка
            myCurve.Symbol.Fill.Type = FillType.Solid;
                       
            myCurve.Symbol.Size = 4;

           
            zedGraphControl1.AxisChange();

            
            zedGraphControl1.Invalidate();
           
        }
        
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
