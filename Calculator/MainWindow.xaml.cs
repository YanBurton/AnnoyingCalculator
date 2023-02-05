using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Res = "0";
        public enum SelectedOperator
        {
            Sub,
            Add,
            Div,
            Mul
        }

        double lastNumber = 0;
        //double result1 = 0;
        bool equal = false;
        SelectedOperator selectedOperator;

        bool finalOp = false;

        //To compute multiple operations (just an experiment with the help of my friendly neighbour the Internet)
        MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
        public string expression = "";
        int manyOp = 0;
        double firstNum = 0;

        //if equal is pressed multiple times, use same expression as last
        double equalNum = 0;

        //Initialize annoying mode
        bool annoy = false;
        bool didYouMeanIt = true;
        bool from2 = false;

        public MainWindow()
        {
            //Initialize calculator
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Activate annoying mode? (yes please do it)", "Choose wisely.", MessageBoxButtons.YesNo);
            if(dr == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Forms.MessageBox.Show("You asked for it!");
                //I mean you really did
                annoy = true;
            }
            InitializeComponent();
            this.DataContext = this;
            btnAC.AddHandler(Button.ClickEvent, new RoutedEventHandler(AC));
            btnNegative.AddHandler(Button.ClickEvent, new RoutedEventHandler(Negative));
            btnPer.AddHandler(Button.ClickEvent, new RoutedEventHandler(Percentage));
            btnDot.AddHandler(Button.ClickEvent, new RoutedEventHandler(Dot));
            result.Content = Res;

            //next lines of codes for multiple operations
            sc.Language = "VBScript";
            //AND IT WORKS!!!!
            /*
            object op = sc.Eval(expression);
            MessageBox.Show(op.ToString());*/

    }

        private void Operations(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            //Change button colour when clicked
            var converter = new System.Windows.Media.BrushConverter();
            //Color to convert to
            var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFE9505");

            switch (btn.Name)
            {
                case "btnMul":

                    //ANNOY
                    if (annoy)
                    {
                        DialogResult dr = System.Windows.Forms.MessageBox.Show("Did you really mean to click that?", "I'm sure you didn't", MessageBoxButtons.YesNo);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            DialogResult dr1 = System.Windows.Forms.MessageBox.Show("I'm like 99% sure you didn't", "Nah", MessageBoxButtons.YesNo);
                            if (dr1 == System.Windows.Forms.DialogResult.Yes)
                            {
                                System.Windows.Forms.MessageBox.Show("UGH fine. You selected DIVIDE.");
                                didYouMeanIt = false;

                                if (finalOp)
                                {
                                    expression = expression.Substring(0, expression.Length - 1);
                                }
                                //Initialize divide
                                selectedOperator = SelectedOperator.Div;
                                lastNumber = Double.Parse(Res);
                                Res = "0";

                                //Add to expression
                                if (equal)
                                {
                                    expression += firstNum.ToString();
                                    equal = false;
                                }
                                expression += "/";

                                //Count the number of operators
                                manyOp++;

                                //Chnage button colours
                                btnDiv.Foreground = brush;
                                btnDiv.Background = Brushes.White;
                                finalOp = true;
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("As I thought.");
                                didYouMeanIt = false;
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Huh you gave up easily...");
                            didYouMeanIt = false;
                        }
                    }
                    //ANNOY OFF
                    if (didYouMeanIt)
                    {
                        if (finalOp)
                        {
                            expression = expression.Substring(0, expression.Length - 1);
                        }

                        selectedOperator = SelectedOperator.Mul;
                        lastNumber = Double.Parse(Res);
                        Res = "0";

                        //Add to expression
                        if (equal)
                        {
                            expression += firstNum.ToString();
                            equal = false;
                        }
                        expression += "*";

                        //Count the number of operators
                        manyOp++;

                        //God im such a genius
                        //Chnage button colours
                        btnMul.Foreground = brush;
                        btnMul.Background = Brushes.White;
                        finalOp = true;
                    }
                    didYouMeanIt = true;
                    break;

                case "btnDiv":
                    //ANNOY ON
                    if (annoy)
                    {
                        System.Windows.Forms.MessageBox.Show("You DO know you clicked the PLUS sign, right?");

                        if (finalOp)
                        {
                            expression = expression.Substring(0, expression.Length - 1);
                        }

                        //Initialize PLUS
                        selectedOperator = SelectedOperator.Add;
                        lastNumber = Double.Parse(Res);
                        Res = "0";

                        //Add to expression
                        if (equal)
                        {
                            expression += firstNum.ToString();
                            equal = false;
                        }
                        expression += "+";

                        //Count the number of operators
                        manyOp++;

                        //Chnage button colours
                        btnPlus.Foreground = brush;
                        btnPlus.Background = Brushes.White;
                        finalOp = true;
                        didYouMeanIt = false;
                    }
                    if (didYouMeanIt)
                    {
                        if (finalOp)
                        {
                            expression = expression.Substring(0, expression.Length - 1);
                        }
                        selectedOperator = SelectedOperator.Div;
                        lastNumber = Double.Parse(Res);
                        Res = "0";

                        //Add to expression
                        if (equal)
                        {
                            expression += firstNum.ToString();
                            equal = false;
                        }
                        expression += "/";

                        //Count the number of operators
                        manyOp++;

                        //Chnage button colours
                        btnDiv.Foreground = brush;
                        btnDiv.Background = Brushes.White;
                        finalOp = true;
                    }
                    didYouMeanIt = true;
                    break;

                case "btnPlus":
                    if (finalOp)
                    {
                        expression = expression.Substring(0, expression.Length - 1);
                    }
                    selectedOperator = SelectedOperator.Add;
                    lastNumber = Double.Parse(Res);
                    Res = "0";                    

                    //Add to expression
                    if (equal)
                    {
                        expression += firstNum.ToString();
                        equal = false;
                    }
                    expression += "+";

                    //Count the number of operators
                    manyOp++;

                    //Chnage button colours
                    btnPlus.Foreground = brush;
                    btnPlus.Background = Brushes.White;
                    finalOp = true;
                    break;

                case "btnMin":
                    if (finalOp)
                    {
                        expression = expression.Substring(0, expression.Length - 1);
                    }
                    selectedOperator = SelectedOperator.Sub;
                    lastNumber = Double.Parse(Res);
                    Res = "0";

                    //Add to expression
                    if (equal)
                    {
                        expression += firstNum.ToString();
                        equal = false;
                    }
                    expression += "-";

                    //Count the number of operators
                    manyOp++;

                    //Chnage button colours
                    btnMin.Foreground = brush;
                    btnMin.Background = Brushes.White;
                    finalOp = true;
                    break;

                default:
                    break;
            }
        }

        private void Number(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            //For fun
            var converter = new System.Windows.Media.BrushConverter();
            //Color to convert to
            var brush1 = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFE9505");

            //Convert back to normal
            btnMul.Background = brush1;
            btnMul.Foreground = Brushes.White;
            btnDiv.Background = brush1;
            btnDiv.Foreground = Brushes.White;
            btnPlus.Background = brush1;
            btnPlus.Foreground = Brushes.White;
            btnMin.Background = brush1;
            btnMin.Foreground = Brushes.White;

            switch (btn.Name)
            {
                case "btn1":
                    if (annoy)
                    {
                        //Generate random colours
                        Button[] btns = { btnMul, btnDiv, btnPlus, btnMin, btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn0, btnAC, btnDot, btnEqual, btnNegative, btnPer};
                        var rand1 = new Random();

                        var probR2 = rand1.Next(256);
                        var probG2 = rand1.Next(256);
                        var probB2 = rand1.Next(256);

                        var probR3 = rand1.Next(256);
                        var probG3 = rand1.Next(256);
                        var probB3 = rand1.Next(256);

                        for (int i = 0; i < btns.Length; i++)
                        {
                            var probR = rand1.Next(256);
                            var probG = rand1.Next(256);
                            var probB = rand1.Next(256);

                            var probR1 = rand1.Next(256);
                            var probG1 = rand1.Next(256);
                            var probB1 = rand1.Next(256);

                            btns[i].Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(probR), Convert.ToByte(probG), Convert.ToByte(probB)));
                            btns[i].Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(probR1), Convert.ToByte(probG1), Convert.ToByte(probB1)));
                        }
                        result.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(probR2), Convert.ToByte(probG2), Convert.ToByte(probB2)));
                        this.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(Convert.ToByte(probR3), Convert.ToByte(probG3), Convert.ToByte(probB3)));

                    }
                    Res = !Res.Equals("0") && equal != true ? Res += "1" : Res = "1";
                    expression += "1";
                    //Could not manage to make it automatic
                    Refresh();
                    equal = false;

                    if (manyOp == 1)
                    {
                        equalNum = Double.Parse(Res);
                    }
                    from2 = false;
                    finalOp = false;
                    break;

                case "btn2":
                    if (annoy)
                    {
                        System.Windows.Forms.MessageBox.Show("If you pressed 2 because you meant to press 5, press 6. If you pressed 2 because you meant 2, press 4. Else, press 3. If you're lost, press 0.");
                        didYouMeanIt = false;
                        from2 = true;
                    }
                    if (didYouMeanIt)
                    {
                        Res = !Res.Equals("0") && equal != true ? Res += "2" : Res = "2";
                        expression += "2";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    didYouMeanIt = true;
                    break;

                case "btn3":
                    if(annoy && from2)
                    {
                        System.Windows.Forms.MessageBox.Show("Why did you press 3??");
                        didYouMeanIt = false;
                    }
                    if (didYouMeanIt)
                    {
                        Res = !Res.Equals("0") && equal != true ? Res += "3" : Res = "3";
                        expression += "3";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    didYouMeanIt = true;
                    from2 = false;
                    break;

                case "btn4":
                    if(annoy && from2)
                    {
                        System.Windows.Forms.MessageBox.Show("Congratulations, you pressed 2.");
                        didYouMeanIt = false;
                        Res = !Res.Equals("0") && equal != true ? Res += "2" : Res = "2";
                        expression += "2";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    if (didYouMeanIt)
                    {
                        Res = !Res.Equals("0") && equal != true ? Res += "4" : Res = "4";
                        expression += "4";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    didYouMeanIt = true;
                    from2 = false;
                    break;

                case "btn5":
                    if (annoy)
                    {
                        System.Windows.Forms.MessageBox.Show("If you meant to press 5, press 2.");
                        didYouMeanIt = false;
                    }

                    if (didYouMeanIt)
                    {
                        Res = !Res.Equals("0") && equal != true ? Res += "5" : Res = "5";
                        expression += "5";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    didYouMeanIt = true;
                    from2 = false;
                    break;

                case "btn6":
                    if (annoy && from2)
                    {
                        System.Windows.Forms.MessageBox.Show("Congratulations. You pressed 5.");
                        Res = !Res.Equals("0") && equal != true ? Res += "5" : Res = "5";
                        expression += "5";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                        didYouMeanIt = false;
                    }
                    if (didYouMeanIt)
                    {
                        Res = !Res.Equals("0") && equal != true ? Res += "6" : Res = "6";
                        expression += "6";
                        Refresh();
                        equal = false;

                        if (manyOp == 1)
                        {
                            equalNum = Double.Parse(Res);
                        }
                        finalOp = false;
                    }
                    from2 = false;
                    didYouMeanIt = true;
                    break;

                case "btn7":
                    if (annoy)
                    {
                        //randomize labels
                        Button[] btnNum = {btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btn0};
                        var rand2 = new Random();
                        for(int i = 0; i < 9; i++)
                        {
                            var ranNum = rand2.Next(10);
                            btnNum[i].Content = ranNum.ToString();
                        }
                    }

                    Res = !Res.Equals("0") && equal != true ? Res += "7" : Res = "7";
                    expression += "7";
                    Refresh();
                    equal = false;

                    if (manyOp == 1)
                    {
                        equalNum = Double.Parse(Res);
                    }
                    finalOp = false;
                    from2 = false;
                    break;

                case "btn8":
                    Res = !Res.Equals("0") && equal != true ? Res += "8" : Res = "8";
                    expression += "8";
                    Refresh();
                    equal = false;

                    if (manyOp == 1)
                    {
                        equalNum = Double.Parse(Res);
                    }
                    finalOp = false;
                    from2 = false;
                    break;

                case "btn9":
                    Res = !Res.Equals("0") && equal != true ? Res += "9" : Res = "9";
                    expression += "9";
                    Refresh();
                    equal = false;

                    if (manyOp == 1)
                    {
                        equalNum = Double.Parse(Res);
                    }
                    from2 = false;
                    finalOp = false;
                    break;

                case "btn0":
                    if(annoy && from2)
                    {
                        System.Windows.Forms.MessageBox.Show("It seems you are lost. Here are all the instructions.");
                        System.Windows.Forms.MessageBox.Show("Press 0, 1, 3, 4, 6, 7, 8, 9 normally. If you want to press 5, " +
                            "press 2. If you pressed 2 because you meant 5, press 6. if you did not mean to press 5, press 3. If you pressed 2, press 0 to see instructions" +
                            ". if you pressed 2 because you wanted to press 2, press 4.");
                        DialogResult dr = System.Windows.Forms.MessageBox.Show("Simple, no?", "Cmon", MessageBoxButtons.YesNo);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Windows.Forms.MessageBox.Show("Good.");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Ughh here we go again...");
                            System.Windows.Forms.MessageBox.Show("Press 0, 1, 3, 4, 6, 7, 8, 9 normally. If you want to press 5, " +
                            "press 2. If you pressed 2 because you meant 5, press 6. if you did not mean to press 5, press 3. If you pressed 2, press 0 to see instructions" +
                            ". if you pressed 2 because you wanted to press 2, press 4.");
                        }
                        didYouMeanIt = false;
                    }
                    Res = !Res.Equals("0") && equal != true ? Res += "0" : Res = "0";
                    expression += "0";
                    Refresh();
                    equal = false;

                    if (manyOp == 1)
                    {
                        equalNum = Double.Parse(Res);
                    }
                    from2 = false;
                    finalOp = false;
                    didYouMeanIt = true;
                    break;
            }
        }

        private void AC(object sender, RoutedEventArgs e)
        {
            lastNumber = 0;
            Res = "0";
            expression = "";
            firstNum = 0;
            manyOp = 0;
            equal = false;
            from2 = false;
            Refresh();
        }

        private void Negative(object sender, RoutedEventArgs e)
        {
            double temp = Convert.ToDouble(Res);
            expression = expression.Replace(Res, "");
            Res = (temp * -1).ToString();
            expression += Res;
            Refresh();
        }
        private void Dot(object sender, RoutedEventArgs e)
        {
            if (!Res.Contains("."))
            {
                Res += ".";
                expression += ".";
            }
            //Add warnings
            Refresh();
        }
        private void Percentage(object sender, RoutedEventArgs e)
        {
            if (expression.Equals("69"))
            {
                annoy = true;
                System.Windows.MessageBox.Show("Glad to see you back! MUHAHAHAHAHHA");
                lastNumber = 0;
                Res = "0";
                expression = "";
                firstNum = 0;
                manyOp = 0;
                equal = false;
                Refresh();
            }

            if (annoy && expression.Equals("4.20"))
            {
                annoy = false;
                System.Windows.MessageBox.Show("No. NO. NOOOO HOW!?!");
                lastNumber = 0;
                Res = "0";
                expression = "";
                firstNum = 0;
                manyOp = 0;
                equal = false;

                //Change back to normal
                var converter = new System.Windows.Media.BrushConverter();
                var brush2 = (System.Windows.Media.Brush)converter.ConvertFromString("#FF333333");
                var brush1 = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFE9505");
                var brush3 = (System.Windows.Media.Brush)converter.ConvertFromString("#FFA5A5A5");

                Button[] btnNum = { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
                Button[] btns = { btnMul, btnDiv, btnPlus, btnMin, btnEqual };
                Button[] btnSpecial = { btnAC, btnPer, btnNegative };

                for (int i = 0; i < 10; i++)
                {
                    btnNum[i].Content = i;
                    btnNum[i].Foreground = Brushes.White;
                    btnNum[i].Background = brush2;
                }

                for (int i = 0; i < btns.Length; i++)
                {
                    btns[i].Background = brush1;
                    btns[i].Foreground = Brushes.White;
                }

                for (int i = 0; i < btnSpecial.Length; i++)
                {
                    btnSpecial[i].Background = brush3;
                    btnSpecial[i].Foreground = Brushes.Black;
                }
                this.Background = Brushes.Black;
                result.Foreground = Brushes.White;
                btnDot.Foreground = Brushes.White;
                btnDot.Background = brush2;

                Refresh();
            }
 
                double temp = Convert.ToDouble(Res);
            if (lastNumber == 0)
            {
                Res = (temp / 100).ToString();
                firstNum = lastNumber;
                lastNumber = Convert.ToDouble(Res);
            }
            else
            {
                Res = (lastNumber * (temp / 100)).ToString();
            }
            Refresh();
        }

        private void Refresh()
        {
            result.Content = Res;
        }

        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            //will the equal method work?
            if (annoy)
            {
                var rand = new Random();
                var prob = rand.Next(3);

                switch (prob)
                {
                    case 0:
                        DialogResult dr = System.Windows.Forms.MessageBox.Show("Hey I wont give you the answer if you don't know it yourself. Do you know the answer?", "WAIT", MessageBoxButtons.YesNo);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Windows.Forms.MessageBox.Show("Hmmm... Fine");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Well I won't give it to you!");
                            didYouMeanIt = false;
                        }
                        break;

                    case 1:
                        System.Windows.Forms.MessageBox.Show("Wait I'm busy. Try again later.");
                        didYouMeanIt = false;
                        break;

                    case 2:
                        System.Windows.Forms.MessageBox.Show("I can be nice sometimes.");
                        break;

                    default:
                        break;
                }
            }
            
            if (didYouMeanIt)
            {
                bool moreOp = manyOp > 1;
                if (equal)
                {
                    //System.Windows.MessageBox.Show("passed" + lastNumber + "\n" + selectedOperator + "\n" + Res);
                    lastNumber = MathService.Calculate(Convert.ToDouble(Res), selectedOperator, equalNum, moreOp, expression);
                    Res = lastNumber.ToString();
                }
                else
                {
                    lastNumber = MathService.Calculate(lastNumber, selectedOperator, Convert.ToDouble(Res), moreOp, expression);
                    Res = lastNumber.ToString();
                }

                equal = true;
                manyOp = 0;
                firstNum = lastNumber;
                expression = "";
                Refresh();
            }
            didYouMeanIt = true;
        }
    }
}
