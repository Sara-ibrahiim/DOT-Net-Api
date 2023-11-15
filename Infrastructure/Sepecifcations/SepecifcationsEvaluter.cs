using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sepecifcations
{
    public class SepecifcationsEvaluter <T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,ISepecifcations<T> sepecifcations)
        {
            var query = inputQuery;
            if(sepecifcations.Criteria is not null ) 
                query = query.Where(sepecifcations.Criteria);

            if (sepecifcations.OrderBy is not null)
                query = query.OrderBy(sepecifcations.OrderBy);

            if (sepecifcations.OrderByDescending is not null)
                query = query.OrderByDescending(sepecifcations.OrderByDescending);


            if (sepecifcations.IsPaginated)
                query = query.Skip(sepecifcations.Skip).Take(sepecifcations.Take);  



            query = sepecifcations.Includes.Aggregate(query , (current,include)=> current.Include(include));

            return query;
        }

    
    }
}
