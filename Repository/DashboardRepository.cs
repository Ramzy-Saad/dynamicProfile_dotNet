using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<List<Club>> GetAllUserClubs()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.GetUserID();
        if (string.IsNullOrEmpty(userId))
            return new List<Club>();
        return await _context.Clubs.Where(c => c.AppUserId == userId).ToListAsync();
    }

    public async Task<List<Race>> GetAllUserRaces()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.GetUserID();
        if (string.IsNullOrEmpty(userId))
            return new List<Race>();
        return await _context.Races.Where(c => c.AppUserId == userId).ToListAsync();
    }
}
