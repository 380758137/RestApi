using Api.Core.DomainModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyDbContext myDbContext,ILoggerFactory loggerFactory,int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                if (!myDbContext.Countries.Any())
                {
                    myDbContext.Countries.AddRange(new List<Country>
                    {
                        new Country{
                            EnglishName="China",
                            ChineseName="中华人民共和国",
                            Abbreviation="中国",
                            Cities=new List<City>
                            {
                                new City{ Name="北平"},
                                new City{Name="上海"},
                                new City{ Name="海参崴"},
                                new City{ Name="盛京"},
                            }
                        },
                        new Country{
                            EnglishName="USA",
                            ChineseName="美利坚合众国",
                            Abbreviation="美国",
                            Cities=new List<City>
                            {
                                new City{ Name="New York"},
                                new City{Name="Chicago"}
                            }
                        }
                    });
                    await myDbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(e.Message);
                    await SeedAsync(myDbContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
