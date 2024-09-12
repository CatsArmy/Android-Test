namespace Android_Test
{
    //Color Palettes https://www.realtimecolors.com/?colors=e5ebe6-050605-9fcba4-415d50-79a495&fonts=Inter-Inter
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private TextView InOutResult;

        private Button[] Numpad = new Button[10];

        private Button FlipPolarity;
        private Button AllClear;

        private Button Percent;
        private Button Dot;

        private Button Divide;
        private Button Multiply;
        private Button Subtraction;
        private Button Addition;
        private Button Equals;

        // (+/-)num1 (Action(['+','-','*','/']) (+/-)num2...
        //operations length is index -1 where index > 0
        private List<Operation> operations = [];
        private List<double> numbers = [];
        private bool hasDot = false;
        private bool dot = false;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Investigate Figma + google Relay to android XML/resource files.
            //Investigate more on how this shit works

            InOutResult = base.FindViewById<TextView>(Resource.Id.tvInOutResult);
            numbers.Clear();
            operations.Clear();
            numbers.Add(operations.Count);
            InOutResult.Text = Evaluate();

            for (int i = 0; i < Numpad.Length; i++)
            {
                //Resource.Id.btn0 is a int and btn1 is the same int + 1 and so on
                Numpad[i] = base.FindViewById<Button>(Resource.Id.btn0 + i);
                int x = int.Parse($"{i}");
                Numpad[i].Click += (sender, e) => OnClick_Numpad(sender, e, x);
            }

            FlipPolarity = base.FindViewById<Button>(Resource.Id.btnFlipPolarity);
            FlipPolarity.Click += OnClick_ToggleNegativity;
            AllClear = base.FindViewById<Button>(Resource.Id.btnAllClear);
            AllClear.Click += OnClick_AllClear!;
            Percent = base.FindViewById<Button>(Resource.Id.btnPercent);
            Percent.Click += OnClick_Percent;
            Divide = base.FindViewById<Button>(Resource.Id.btnDivide);
            Divide.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Division);
            Dot = base.FindViewById<Button>(Resource.Id.btnDot)!;
            Dot.Click += OnClick_Dot;
            Multiply = base.FindViewById<Button>(Resource.Id.btnMultiply);
            Multiply.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Multiplication);
            Subtraction = base.FindViewById<Button>(Resource.Id.btnSubtraction);
            Subtraction.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Subtraction);
            Addition = base.FindViewById<Button>(Resource.Id.btnAddition);
            Addition.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Addition);
            Equals = base.FindViewById<Button>(Resource.Id.btnEquals);
            Equals.Click += OnClick_Equals;
        }

        private void OnClick_ToggleNegativity(object sender, EventArgs e)
        {
            if (numbers.Count == operations.Count)
            {
                return;
            }
            if (numbers[operations.Count] == 0)
            {
                return;
            }
            const int flipPolarity = -1;
            numbers[operations.Count] *= flipPolarity;
            InOutResult.Text = Evaluate();
        }

        private void OnClick_Percent(object sender, EventArgs e)
        {
            InOutResult.Text = Evaluate();
        }

        private void OnClick_Dot(object sender, EventArgs e)
        {
            if (hasDot)
            {
                return;
            }
            dot = true;
            hasDot = true;
            InOutResult.Text = Evaluate();
        }

        private void OnClick_Operation(object sender, EventArgs e, Operation operation)
        {
            if (numbers.Count == operations.Count)
            {
                operations[operations.Count - 1] = operation;
                InOutResult.Text = Evaluate();
                return;
            }
            operations.Add(operation);
            dot = false;
            hasDot = false;
            InOutResult.Text = Evaluate();
        }

        private void OnClick_Equals(object sender, EventArgs e)
        {
            InOutResult.Text = Calculate();
        }

        public string Evaluate()
        {
            string result = string.Empty;
            if (numbers.Count <= 0) { return result; }
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                result += $"{numbers[i]}";
                if (i >= operations.Count)
                {
                    continue;
                }
                result += $" {operations[i].GetValue()} ";
            }
            result += $"{numbers[numbers.Count - 1]}";
            if (numbers.Count == operations.Count)
            {
                result += $"{operations[operations.Count - 1].GetValue()}";
            }
            return result;
        }
        public string Calculate()
        {
            if (numbers.Count <= 0)
            {
                return Evaluate();
            }

            double result = 0.0;
            double priority = 0.0;
            bool isPriority = false;
            bool priorityPolarity = true;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                switch (operations[i])
                {
                    case Operation.Addition:
                        if (isPriority)
                        {
                            result += priority;
                            priority = 0.0;
                            isPriority = false;
                            break;
                        }
                        result += numbers[i];
                        priorityPolarity = true;
                        break;

                    case Operation.Subtraction:
                        if (isPriority)
                        {
                            result -= priority;
                            priority = 0.0;
                            isPriority = false;
                            break;
                        }
                        result -= numbers[i];
                        priorityPolarity = false;
                        break;

                    case Operation.Multiplication:
                        if (isPriority)
                        {
                            priority *= numbers[i];
                            break;
                        }
                        priority = numbers[i] * numbers[++i];
                        isPriority = true;
                        break;

                    case Operation.Division:
                        if (isPriority)
                        {
                            priority /= numbers[i];
                            break;
                        }
                        priority = numbers[i] / numbers[++i];
                        isPriority = true;
                        break;
                }

                switch (priorityPolarity)
                {
                    case true:
                        result += priority;
                        break;

                    case false:
                        result -= priority;
                        break;
                }
            }
            return $"{result}";
        }

        private void OnClick_Numpad(object sender, EventArgs e, int number)
        {
            double value;
            switch (numbers.Count == operations.Count)
            {
                case true:
                    value = double.Parse(dot ? $"0.{number}" : $"{number}");
                    numbers.Add(value);
                    break;

                case false:
                    value = double.Parse($"{numbers[operations.Count]}{(dot ? '.' : "")}{number}");
                    numbers[operations.Count] = value;
                    break;
            }
            InOutResult.Text = Evaluate();
        }

        private void OnClick_AllClear(object sender, EventArgs e)
        {
            if (sender != AllClear)
            {
                return;

            }
            dot = false;
            hasDot = false;
            numbers.Clear();
            operations.Clear();
            numbers.Add(operations.Count);
            InOutResult.Text = $"{operations.Count}";
        }

    }
}





///:root[data-theme="light"] {
///--text: #141a15;
///  --background: #f9faf9;
///  --primary: #346039;
///  --secondary: #a2beb1;
///  --accent: #5b8677;
///}
///:root[data - theme = "dark"] {
///    --text: #e5ebe6;
///  --background: #050605;
///  --primary: #9fcba4;
///  --secondary: #415d50;
///  --accent: #79a495;
///}
///
/// 