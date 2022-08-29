using FinalAgain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.DAL
{
    public class AppDBContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public AppDBContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}
        }
        public DbSet<Position> Positions { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }
        public DbSet<Friendship> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Fooorm> Fooorms { get; set; }
        public DbSet<FooormAnswers> FooormAnswers { get; set; }
        public DbSet<ServerJoinRequest> ServerJoinRequests { get; set; }
        public DbSet<ServerMember> ServerMembers { get; set; }
    }
}
