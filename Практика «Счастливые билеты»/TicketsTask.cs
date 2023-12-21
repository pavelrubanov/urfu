using System.Numerics;

namespace Tickets
{
    internal class TicketsTask
    {
        private const int MaxTicketLength = 100;
        private const int MaxTicketSum = 2000;

        public static BigInteger Solve(int halfTicketLength, int totalTicketSum)
        {
            if (totalTicketSum % 2 != 0)
            {
                return BigInteger.Zero;
            }

            var happyTickets = InitializeTicketsContainer();

            var halfResult = CountTickets(happyTickets, halfTicketLength, totalTicketSum / 2);

            return halfResult * halfResult;
        }

        private static BigInteger[,] InitializeTicketsContainer()
        {
            var happyTickets = new BigInteger[MaxTicketLength + 1, MaxTicketSum + 1];

            for (var i = 0; i <= MaxTicketLength; i++)
            {
                for (var j = 0; j <= MaxTicketSum; j++)
                {
                    happyTickets[i, j] = BigInteger.MinusOne;
                }
            }

            return happyTickets;
        }

        private static BigInteger CountTickets(BigInteger[,] happyTickets, int ticketLength, int ticketSum)
        {
            if (happyTickets[ticketLength, ticketSum] >= BigInteger.Zero)
            {
                return happyTickets[ticketLength, ticketSum];
            }

            if (ticketSum == 0)
            {
                return BigInteger.One;
            }

            if (ticketLength == 0)
            {
                return BigInteger.Zero;
            }

            happyTickets[ticketLength, ticketSum] = BigInteger.Zero;

            for (var i = 0; i <= 9; i++)
            {
                if (ticketSum - i >= 0)
                {
                    happyTickets[ticketLength, ticketSum] += CountTickets(happyTickets, ticketLength - 1, ticketSum - i);
                }
            }

            return happyTickets[ticketLength, ticketSum];
        }
    }
}
