using System;
using System.Linq;

namespace DevXpert.Academy.Financeiro.AntiCorruption
{
    public class PayPalGateway : IPayPalGateway
    {
        public string GetCardHashKey(string serviceKey, string cartaoCredito)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public string GetPayPalServiceKey(string apiKey, string encriptionKey)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public bool CommitTransaction(string cardHashKey, string orderId, decimal amount)
        {
            return new Random().Next(200) > 0;
        }

        public bool RollbackTransaction(string orderId)
        {
            return new Random().Next(200) > 0;
        }
    }
}