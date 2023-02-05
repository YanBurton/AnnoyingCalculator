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
using static Calculator.MainWindow;

namespace Calculator
{
    public static class MathService
    {
        public static double Calculate(double lastNumber, SelectedOperator selectedOperator, double newNum, bool moreOp, string expression)
        {
            double result = 0;
            if (!moreOp)
            {
                if (selectedOperator == SelectedOperator.Div && (lastNumber == 0 || newNum == 0))
                {
                    MessageBox.Show("Impossible operation detected");
                }

                switch (selectedOperator)
                {
                    case SelectedOperator.Sub:
                        result = lastNumber - newNum;
                        break;
                    case SelectedOperator.Add:
                        result = lastNumber + newNum;
                        break;
                    case SelectedOperator.Div:
                        result = lastNumber / newNum;
                        break;
                    case SelectedOperator.Mul:
                        result = lastNumber * newNum;
                        break;
                    default:
                        break;
                }
            }
            else //if there was more than one operator
            {
                MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
                sc.Language = "VBScript";

                object op = sc.Eval(expression);
                result = Convert.ToDouble(op);
            }

            return result;
        }
    }
}
