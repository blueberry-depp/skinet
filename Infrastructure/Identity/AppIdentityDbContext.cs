using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    // IdentityDbContext: this gives us the ability to query a session with identity database when we create it
    // and we'll have different tables in there for all of the different identity classes and each one of those
    // is going to be a db set that we can query. Now we're going to be working with the user in this application,
    // that's all we really need identity for although it does give us extra inside there as well.
    // <AppUser>: that way we ensure that when we do create this new migration, it includes the properties that we added there as well.
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        // Make sure this is public. So that entity framework tools can get access to this method. Because we've got more
        // than one DB context in application. We're  need to specify the type inside here as well, then we pass options
        // into base constructor inside the IdentityDbContext. We don't need to add any DB sets inside here. Our IdentityDbContext
        // is taken care of all of this work for us and that's why we derive from IdentityDbContext.
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // because we're overriding this method, we'll just pass into the class we're deriving from and we get access to that using base,
            // if we don't do this, we can sometimes get issues with identity and the primary key it's using for the ID. of the app user field.
            base.OnModelCreating(builder);
        }
    }
}
