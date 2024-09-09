namespace Android_Test
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private TextView InOutResult;
        private Button[] Numpad = new Button[10];

        private Button ToggleNegitivity;
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
        private List<Operation> operations;
        private List<double> numbers;
        private int index = 0;
        private bool dot = false;
        private bool hasDot = false;
        string text = string.Empty;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Investigate Figma + google Relay to android XML/resource files.
            //Investigate more on how this shit works

            InOutResult = FindViewById<TextView>(Resource.Id.tvInOutResult)!;
            operations = new List<Operation>();
            numbers = new List<double>();
            index = 0;
            numbers[index] = index;
            InOutResult.Text = $"{numbers[index]}";

            for (int i = 0; i < Numpad.Length; i++)
            {
                //Resource.Id.btn0 is a int and btn1 is the same int + 1 and so on
                Numpad[i] = FindViewById<Button>(Resource.Id.btn0 + i)!;
                Numpad[i].Click += (sender, e) => OnClick_Numpad(sender, e, i);
            }

            ToggleNegitivity = FindViewById<Button>(Resource.Id.btnToggleNegitivity)!;
            ToggleNegitivity.Click += OnClick_ToggleNegitivity;
            AllClear = FindViewById<Button>(Resource.Id.btnAllClear)!;
            AllClear.Click += OnClick_AllClear!;
            Percent = FindViewById<Button>(Resource.Id.btnPercent)!;
            Percent.Click += OnClick_Percent;
            Divide = FindViewById<Button>(Resource.Id.btnDivide)!;
            //Divide.Click += OnClick_Division!;
            Divide.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Division)!;
            Dot = FindViewById<Button>(Resource.Id.btnDot)!;
            Dot.Click += OnClick_Dot;
            Multiply = FindViewById<Button>(Resource.Id.btnMultiply)!;
            //Multiply.Click += OnClick_Multiply;
            Multiply.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Multiplication);
            Subtraction = FindViewById<Button>(Resource.Id.btnSubtraction)!;
            //Subtraction.Click += OnClick_Subtraction;
            Subtraction.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Subtraction);
            Addition = FindViewById<Button>(Resource.Id.btnAddition)!;
            //Addition.Click += OnClick_Addition;
            Addition.Click += (sender, e) => OnClick_Operation(sender, e, Operation.Addition);
            Equals = FindViewById<Button>(Resource.Id.btnEquals)!;
            Equals.Click += OnClick_Equals;
        }

        private void OnClick_ToggleNegitivity(object sender, EventArgs e)
        {
            if (numbers.Count != index)
            {
                return;
            }
            if (numbers[index] == 0)
            {
                return;
            }
            const int flipPolarity = -1;
            numbers[index] *= flipPolarity;
        }

        private void OnClick_Percent(object sender, EventArgs e)
        {

        }

        private void OnClick_Dot(object sender, EventArgs e)
        {
            if (hasDot)
            {
                return;
            }
            dot = true;
            hasDot = true;
            //Correctly update output.
        }

        private void OnClick_Operation(object sender, EventArgs e, Operation operation)
        {
            if (numbers.Count != index && index > 0)
            {
                operations[index] = operation;
                return;
            }
            operations.Add(operation);
            index++;
            dot = false;
            hasDot = false;
        }

        private void OnClick_Equals(object sender, EventArgs e)
        {

        }

        private void OnClick_Numpad(object sender, EventArgs e, int number)
        {
            if (dot)
            {
                if (numbers.Count != index)
                    numbers.Add(double.Parse($"0.{number}"));
                else
                    numbers[index] = double.Parse($"{numbers[index]}.{number}");
                dot = false;
                return;
            }
            if (numbers.Count != index)
            {
                numbers.Add(double.Parse($"{numbers[index]}{number}"));
                return;
            }

            numbers[index] = double.Parse($"{numbers[index]}{number}");
        }

        private void OnClick_AllClear(object sender, EventArgs e)
        {
            if (sender != AllClear)
            {
                return;
            }
            operations.Clear();
            numbers.Clear();
            index = 0;
            dot = false;
            hasDot = false;
            text = string.Empty;
            InOutResult.Text = $"{numbers[index]}";
        }
        private enum Operation
        {
            Addition = '+',
            Subtraction = '-',
            Multiplication = '*',
            Division = '/'
        }
    }
}
