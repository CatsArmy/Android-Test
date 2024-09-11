namespace Android_Test
{
    public static class OperationExtensions
    {
        private static Dictionary<Operation, string> operations = new Dictionary<Operation, string>();
        private static bool init = true;
        public static string GetValue(this Operation operation)
        {
            if (init)
            {
                operations.Add(Operation.Multiplication, " * ");
                operations.Add(Operation.Division, " / ");
                operations.Add(Operation.Addition, " + ");
                operations.Add(Operation.Subtraction, " - ");
                init = false;
            }

            return operations[operation];
        }
    }
}
