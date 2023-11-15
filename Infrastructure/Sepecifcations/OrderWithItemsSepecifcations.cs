using Core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sepecifcations
{
    public class OrderWithItemsSepecifcations : BaseSepecifcations<Order>

    {
        public OrderWithItemsSepecifcations(string email): base (order => order.BuyerEmail == email)
        {
            AddInclude(order => order.OrderDate);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);




        }
        public OrderWithItemsSepecifcations(int id ,string email)
            : base(order => order.BuyerEmail == email && order.Id == id)
        {
            AddInclude(order => order.OrderDate);
            AddInclude(order => order.DeliveryMethod);
          



        }



    }
}
