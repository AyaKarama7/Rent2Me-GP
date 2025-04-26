using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent2Me.Models;

public class FeedbackService
{
    private readonly AppDbContext _dbContext;

    public FeedbackService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SubmitFeedback(UserFeedback feedback)
    {
        feedback.CreatedAt = System.DateTime.UtcNow;
        _dbContext.Feedbacks.Add(feedback);
        await _dbContext.SaveChangesAsync(); 
    }

    public async Task<List<UserFeedback>> GetReceivedFeedbacks(string userId, string license)
    {
        return await _dbContext.Feedbacks
            .Where(fb => fb.ToUserId == userId)
            .Where(c=>c.LicensePlate==license)
            .Include(fb => fb.FromUser) 
            .ToListAsync(); 
    }

}
