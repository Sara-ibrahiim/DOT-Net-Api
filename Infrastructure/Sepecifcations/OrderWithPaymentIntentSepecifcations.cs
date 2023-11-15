using Core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sepecifcations
{
    public class OrderWithPaymentIntentSepecifcations : BaseSepecifcations<Order>
    {
        public OrderWithPaymentIntentSepecifcations(string paymentIntentId) 
            : base(order => order .PaymentIntentId == paymentIntentId)
        {


        }
    }
}
