﻿namespace DevXpert.Academy.Financeiro.AntiCorruption
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);
        string GetCardHashKey(string serviceKey, string cartaoCredito);

        bool CommitTransaction(string cardHashKey, string orderId, decimal amount);
        bool RollbackTransaction(string orderId);
    }
}
