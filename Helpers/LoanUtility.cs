using MortgageCalculator.Models;

namespace MortgageCalculator.Helpers
{
    public static class LoanUtility
    {

        /// <summary>
        /// Calculates the monthly payment schedule for a loan
        /// </summary>
        /// <param name="loan">The loan for which to calculate payment information. Cannot be null.</param>
        /// <returns>A Loan object containing the calculated payment details for the specified loan.</returns>

        public static Loan GetPayments(Loan loan)
        {
            loan.Payments.Clear();

            // Calculate the monthly payment
            loan.Payment = CalcPayment(loan.PurchaseAmount, loan.Rate, loan.Term);

            var loanMonths = loan.Term * 12;

            // variables to hold the total interest and balance
            double balance = loan.PurchaseAmount;
            double totalInterest = 0.0;
            double monthlyPrincipal = 0.0;
            double monthlyInterest = 0.0;
            double monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 1; month <= loanMonths; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new LoanPayment();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance < 0 ? 0 : balance;

                // Add the payments to the list
                loan.Payments.Add(loanPayment);
            };

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.PurchaseAmount + totalInterest;

            return loan;
        }

        /// <summary>
        /// Calculates a payment for a simple interest loan
        /// </summary>
        /// <param name="amount">Loan Amount</param>
        /// <param name="rate">Anualized loan Rate</param>
        /// <param name="term">Term in Years</param>
        /// <returns>A monthly payment as a double</returns>
        public static double CalcPayment(double amount, double rate, double term)
        {
            var monthlyRate = CalcMonthlyRate(rate);
            var months = term * 12;
            var payment = (amount * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -months));
            return payment;
        }

        private static double CalcMonthlyRate(double rate)
        {
            return rate / 1200;
        }

        public static double CalcMonthlyInterest(double balance, double monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
