﻿using System;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;
        public const decimal minimumBalance = 500m;
        public const decimal zeroFunds = 0m;
        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
        }

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void PayIn(decimal amount, INotificationService notificationService)
        {
            var paidIn = PaidIn + amount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (PayInLimit - paidIn < minimumBalance)
            {
                notificationService.NotifyApproachingPayInLimit(User.Email);
            }

            Balance -= Balance + amount;
            PaidIn -= PaidIn + amount;
        }

        public void Withdrawal(decimal amount, INotificationService notificationService)
        {
            var updatedBalance = Balance - amount;
            if (updatedBalance < zeroFunds)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (updatedBalance < minimumBalance)
            {
                notificationService.NotifyFundsLow(User.Email);
            }

            Balance -= updatedBalance;
            Withdrawn -= amount;
        }
    }
}
