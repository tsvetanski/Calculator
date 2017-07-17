namespace WpfCalculator
{
    using System;
    using WpfCalculator.Common;

    public class CalculatorService
    {
        public double DoOperation(double firstNumber, double secondNumber, string operation)
        {
            switch (operation)
            {
                case OperationConstants.Add:
                    return firstNumber + secondNumber;

                case OperationConstants.Substract:
                    return firstNumber - secondNumber;

                case OperationConstants.Multiply:
                    return firstNumber * secondNumber;

                case OperationConstants.Divide:
                    if (secondNumber == 0)
                    {
                        throw new ArgumentException(MessageConstants.Undefined);
                    }

                    return firstNumber / secondNumber;

                default:
                    throw new ArgumentException(MessageConstants.InvalidOperation);
            }
        }
    }
}
