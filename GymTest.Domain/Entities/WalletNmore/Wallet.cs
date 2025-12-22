using GymTest.Domain.Entities.Commons;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTest.Domain.Entities.WalletNmore
{
    public class Wallet : BaseEntity
    {
        public decimal Balance { get; set; } = 1000;

        //Nav

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public virtual User User { get; set; }
        public long UserId { get; set; }
    }


    public class Transaction : BaseEntity
    {
        public ActionName Type { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

        //Nav
        public virtual Wallet Wallet { get; set; }
        public long WalletId { get; set; }
    };

    public enum ActionName
    {
        //برای متد واریز
        Charge,
        // برای متد برداشت
        DisCharge,
        
        // برای متد اسپند یا پرچ یا همون خرید
        Buy,
        Sell,
        Commission
    }
}
