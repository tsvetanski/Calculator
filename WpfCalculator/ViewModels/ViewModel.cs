namespace WpfCalculator
{
    using System;
    using System.Windows.Input;
    using WpfCalculator.Common;

    public class ViewModel : NotifyPropertyChangedClass
    {
        #region Declarations

        private CalculatorService calculator;

        private ICommand numberCommand;
        private ICommand operationCommand;
        private ICommand equalsCommand;
        private ICommand functionCommand;

        private double resultNumber;
        private double firstNumber;
        private double secondNumber;

        private bool isOperatorUsed = false;
        private bool isOperationPressed = false;
        private bool isOperationPerformed = false;
        private bool isDecimalPressed = false;

        private string operation;
        private string function;

        #endregion

        #region Properties

        public double FirstNumber
        {
            get
            {
                return firstNumber;
            }

            set
            {
                if (value == firstNumber)
                {
                    return;
                }
                    
                firstNumber = value;
                OnPropertyChanged();
            }
        }

        public double SecondNumber
        {
            get
            {
                return secondNumber;
            }

            set
            {
                if (value == secondNumber)
                {
                    return;
                }

                secondNumber = value;
                OnPropertyChanged();
            }
        }

        public double ResultNumber
        {
            get
            {
                return resultNumber;
            }

            set
            {
                if (value == resultNumber)
                {
                    return;
                }

                resultNumber = value;
                OnPropertyChanged();
            }
        }

        public CalculatorService Calculator
        {
            get
            {
                if (calculator == null)
                {
                    calculator = new CalculatorService();
                }

                return calculator;
            }
        }

        public ICommand NumberCommand
        {
            get
            {
                return this.numberCommand
                       ?? (this.numberCommand = new RelayCommand(this.NumberClicked));
            }
        }

        public ICommand OperationCommand
        {
            get
            {
                return this.operationCommand
                       ?? (this.operationCommand = new RelayCommand(this.OperationClicked));
            }
        }

        public ICommand EqualsCommand
        {
            get
            {
                return this.equalsCommand
                       ?? (this.equalsCommand = new RelayCommand(this.EqualsClicked));
            }
        }

        public ICommand FunctionCommand
        {
            get
            {
                return this.functionCommand
                       ?? (this.functionCommand = new RelayCommand(this.FunctionClicked));
            }
        }

        public bool IsOperationPressed { get => isOperationPressed; set => isOperationPressed = value; }

        public bool IsOperationPerformed { get => isOperationPerformed; set => isOperationPerformed = value; }

        public bool IsOperatorUsed { get => isOperatorUsed; set => isOperatorUsed = value; }

        public string Operation
        {
            get
            {
                return operation;
            }

            set
            {
                this.operation = value;
                OnPropertyChanged();
            }
        }

        public string Function { get => function; set => function = value; }

        public bool IsDecimalPressed { get => isDecimalPressed; set => isDecimalPressed = value; }

        #endregion

        #region Methods

        private void OperationClicked(object operationObject)
        {
            Operation = operationObject as string;
            if (Operation == null)
            {
                throw new ArgumentsException();
            }

            if (IsOperationPerformed && !IsOperationPressed)
            {
                if (SecondNumber != 0)
                {
                    EqualsClicked(operationObject);
                }
                else
                {
                    SecondNumber = 0;
                }
            }

            IsOperationPressed = true;
            IsOperatorUsed = true;
            IsOperationPerformed = false;
        }

        private void EqualsClicked(object someObject)
        {
            if (IsOperationPerformed)
            {
                try
                {
                    ResultNumber = Calculator.DoOperation(FirstNumber, SecondNumber, Operation);
                }
                catch (ArgumentException)
                {
                    ResultNumber = double.NaN;
                }

                FirstNumber = ResultNumber;
                SecondNumber = 0;
            }

            IsOperationPerformed = false;
            IsOperationPressed = true;
            IsOperatorUsed = false;
            Operation = string.Empty;
        }

        private void NumberClicked(object numberObject)
        {
            string numberString = numberObject as string;
            if (numberString == null)
            {
                throw new ArgumentException(MessageConstants.InvalidOperation);
            }

            int number = Convert.ToInt32(numberString);

            if (IsOperationPressed)
            {
                ResultNumber = 0;
            }

            IsOperationPerformed = true;
            IsOperationPressed = false;

            if (!IsOperatorUsed || FirstNumber == 0)
            {
                FirstNumber = (FirstNumber * 10) + number;
                ResultNumber = FirstNumber;
            }
            else
            {
                SecondNumber = (SecondNumber * 10) + number;
                ResultNumber = SecondNumber;
            }
        }

        private void FunctionClicked(object funcObject)
        {
            Function = funcObject as string;
            if (Function == null)
            {
                throw new ArgumentException(MessageConstants.InvalidOperation);
            }

            switch (Function)
            {
                case FunctionConstants.ClearAll:
                    FirstNumber = 0;
                    SecondNumber = 0;
                    ResultNumber = 0;
                    Operation = string.Empty;
                    IsOperatorUsed = false;
                    break;
                case FunctionConstants.ChangeSign:
                    if (IsOperatorUsed)
                    {
                        SecondNumber = SecondNumber * (-1);
                        ResultNumber = SecondNumber;
                    }
                    else
                    {
                        FirstNumber = FirstNumber * (-1);
                        ResultNumber = FirstNumber;
                    }

                    break;
                case FunctionConstants.Separator:
                    IsDecimalPressed = true;
                    break;
                case FunctionConstants.SingleDel:
                    if (IsOperatorUsed)
                    {
                        SecondNumber = DeleteLastEntity(SecondNumber.ToString());
                        ResultNumber = SecondNumber;
                    }
                    else
                    {
                        FirstNumber = DeleteLastEntity(FirstNumber.ToString());
                        ResultNumber = FirstNumber;
                    }

                    break;
            }
        }

        private double DeleteLastEntity(string result)
        {
            if (result.Length > 1)
            {
                result = result.Substring(0, result.Length - 1);
            }
            else
            {
                result = "0";
            }

            double zero = 0;
            if (double.TryParse(result, out zero))
            {
                return double.Parse(result);
            }

            return zero;
        }
        #endregion
    }
}
