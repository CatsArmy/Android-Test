namespace Android_Test
{
    public static class OperationExtensions
    {
        private static Dictionary<Operation, string> operations = [];
        private static bool init = true;
        public static string GetValue(this Operation operation)
        {
            if (init)
            {
                operations.Add(Operation.Addition, " + ");
                operations.Add(Operation.Subtraction, " - ");
                operations.Add(Operation.Multiplication, " * ");
                operations.Add(Operation.Division, " / ");
                operations.Add(Operation.Modulus, " % ");
                init = false;
            }

            return operations[operation];
        }
    }
    public enum Operation
    {
        Addition = '+',
        Subtraction = '-',
        Multiplication = '*',
        Division = '/',
        Modulus = '%',
    }
}
