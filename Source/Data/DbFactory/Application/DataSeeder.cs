using Data.Interfaces.DbFactory.Application;
using Shared.Extensions;
using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DbFactory.Application
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IApplicationDbFactory applicationDbFactory)
        {
            using (var uow = await applicationDbFactory.BeginUnitOfWorkAsync())
            {
                var admin = uow.Users.Get().FirstOrDefault(u => u.UserName == "Admin");
                if(admin == null)
                {
                    admin = new User
                    {
                        UserName = "Admin",
                        Role = new Role
                        {
                            RoleName = "Admin",
                            Discription = "Admin"
                        },
                        Password = "Ab34Minuri@#".Sha256()
                    };                
                    uow.Users.Insert(admin);

                    var otherUser = new User
                    {
                        UserName = "OtherUser",
                        Role = new Role
                        {
                            RoleName = "OtherUser",
                            Discription = "Other User"
                        },
                        Password = "L3k5Minuri@#".Sha256()
                    };
                    uow.Users.Insert(otherUser);
                    await uow.SaveAsync();
                }

                var accountRandD = uow.Accounts.Get().FirstOrDefault(a => a.AccountName == "R&D");
                if(accountRandD == null)
                {
                    accountRandD = new Account
                    {
                        AccountName = "R&D",
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now
                    };
                    uow.Accounts.Insert(accountRandD);

                    var accountCanteen = new Account
                    {
                        AccountName = "Canteen",
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now
                    };
                    uow.Accounts.Insert(accountCanteen);

                    var accountCeosCar = new Account
                    {
                        AccountName = "CEO's car",
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now
                    };
                    uow.Accounts.Insert(accountCeosCar);

                    var accountMarketing = new Account
                    {
                        AccountName = "Marketing",
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now
                    };
                    uow.Accounts.Insert(accountMarketing);

                    var accountParkingsFines = new Account
                    {
                        AccountName = "Parkings Fines",
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now
                    };
                    uow.Accounts.Insert(accountParkingsFines);

                    var periodJan = new Period
                    {
                        Discription = "January 2017",
                        PeriodDate = new DateTime(2017,01,01),
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,                        
                    };
                    uow.Periods.Insert(periodJan);

                    var periodFeb = new Period
                    {
                        Discription = "February 2017",
                        PeriodDate = new DateTime(2017, 02, 01),
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                    };
                    uow.Periods.Insert(periodFeb);

                    var periodMarch = new Period
                    {
                        Discription = "March 2017",
                        PeriodDate = new DateTime(2017, 03, 01),
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                    };
                    uow.Periods.Insert(periodMarch);
                    await uow.SaveAsync();
                  
                    uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
                    {
                        AccountId = accountRandD.AccountId,
                        Balance = 10.56M,
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                        PeriodId = periodMarch.PeriodId
                    });
                  
                    uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
                    {
                        AccountId = accountCanteen.AccountId,
                        Balance = 98000M,
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                        PeriodId = periodMarch.PeriodId
                    });

                    uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
                    {
                        AccountId = accountCeosCar.AccountId,
                        Balance = 24000M,
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                        PeriodId = periodMarch.PeriodId
                    });
                    
                    uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
                    {
                        AccountId = accountMarketing.AccountId,
                        Balance = -19112M,
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                        PeriodId = periodMarch.PeriodId
                    });

                    uow.AccountPeriodBalances.Insert(new AccountPeriodBalance
                    {
                        AccountId = accountParkingsFines.AccountId,
                        Balance = 11000M,
                        CreatedBy = admin.UserId,
                        CreatedDate = DateTimeOffset.Now,
                        PeriodId = periodMarch.PeriodId
                    });
                    await uow.SaveAsync();
                }
            }
        }
    }
}
