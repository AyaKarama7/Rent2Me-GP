using Microsoft.EntityFrameworkCore;
using Rent2Me.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rent2Me.Admin.Models
{
    public class SubscriptionPlanService
    {
        private readonly AppDbContext _context;

        public SubscriptionPlanService(AppDbContext context)
        {
            _context = context;
        }

        public List<SubscriptionPlan> GetAllPlans()
        {
            return _context.SubscriptionPlans.ToList();
        }

        public SubscriptionPlan GetPlanByName(string name)
        {
            return _context.SubscriptionPlans.Find(name);
        }

        public void AddPlan(SubscriptionPlan plan)
        {
            _context.SubscriptionPlans.Add(plan);
            _context.SaveChanges();
        }

        public void UpdatePlan(SubscriptionPlan plan)
        {
            _context.Entry(plan).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeletePlan(string name)
        {
            var plan = _context.SubscriptionPlans.Find(name);
            _context.SubscriptionPlans.Remove(plan);
            _context.SaveChanges();
        }
    }

}
