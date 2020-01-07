namespace Service
{
    public class FibonacciService
    {
        public long CalculateFibonacci(long num)
        {
            if (num == 0 || num == 1)
            {
                return 1;
            }

            return CalculateFibonacci(num - 1) + CalculateFibonacci(num - 2);
        }
    }
}