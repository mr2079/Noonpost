using Domain.Entites.Article;
using Domain.Entites.Comment;
using Domain.Entites.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Context;

public class NoonpostDbContext : DbContext
{
    public NoonpostDbContext(DbContextOptions<NoonpostDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
