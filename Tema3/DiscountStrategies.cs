using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce
{
    public class NoDiscount : IDiscountStrategy
    {
        public string Name => "Fara reducere";
        public decimal ApplyDiscount(decimal subTotal, List<CartItem> items) => 0m;
    }

    public class PercentageDiscount : IDiscountStrategy
    {
        private readonly decimal _percentage;
        private readonly string _campaignName;
        public PercentageDiscount(decimal percentage, string campaignName)
        {
            _percentage = percentage;
            _campaignName = campaignName;
        }
        public string Name => $"{_campaignName} ({_percentage}% reducere)";
        public decimal ApplyDiscount(decimal subTotal, List<CartItem> items)
            => Math.Round(subTotal * (_percentage / 100), 2);
    }

    public class FixedAmountDiscount : IDiscountStrategy
    {
        private readonly decimal _amount;
        private readonly string _voucherCode;
        public FixedAmountDiscount(decimal amount, string voucherCode)
        {
            _amount = amount;
            _voucherCode = voucherCode;
        }
        public string Name => $"Voucher {_voucherCode} (-{_amount:C})";
        public decimal ApplyDiscount(decimal subTotal, List<CartItem> items)
            => Math.Min(_amount, subTotal);
    }

    public class MinimumOrderDiscount : IDiscountStrategy
    {
        private readonly decimal _minimumOrder;
        private readonly decimal _discountPercentage;
        public MinimumOrderDiscount(decimal minimumOrder, decimal discountPercentage)
        {
            _minimumOrder = minimumOrder;
            _discountPercentage = discountPercentage;
        }
        public string Name => $"Reducere {_discountPercentage}% la comenzi peste {_minimumOrder:C}";
        public decimal ApplyDiscount(decimal subTotal, List<CartItem> items)
            => subTotal >= _minimumOrder ? Math.Round(subTotal * (_discountPercentage / 100), 2) : 0m;
    }

    public class BuyTwoGetOneFreeDiscount : IDiscountStrategy
    {
        private readonly string _targetCategory;
        public BuyTwoGetOneFreeDiscount(string targetCategory) { _targetCategory = targetCategory; }
        public string Name => $"2+1 Gratuit la categoria '{_targetCategory}'";
        public decimal ApplyDiscount(decimal subTotal, List<CartItem> items)
        {
            decimal discount = 0m;
            foreach (var item in items.Where(i => i.Product.Category == _targetCategory))
                discount += (item.Quantity / 3) * item.Product.Price;
            return discount;
        }
    }
}