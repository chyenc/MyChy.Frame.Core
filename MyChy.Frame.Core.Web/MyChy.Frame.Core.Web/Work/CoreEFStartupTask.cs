using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyChy.Frame.Core.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.Work
{
    public class CoreEFStartupTask
    {
        private readonly IServiceProvider _serviceProvider;

        public CoreEFStartupTask(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }


        public void RunS()
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<CoreDbContext>();

                try
                {
                   

                    db.Database.EnsureCreated();

                    db.Database.Migrate();

                    // db.Database.Migrate();

                    //await db.Database.EnsureCreatedAsync();
                }
                catch (System.NotImplementedException)
                {
                    db.Database.EnsureCreated();
                }

                // await EnsureData(db);

            }

        }
    }
}
