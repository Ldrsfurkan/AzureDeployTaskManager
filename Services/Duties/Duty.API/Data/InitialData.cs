using Marten.Schema;

namespace Duty.API.Data
{
    public class InitialData
    { }
}
    /*  public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            await session.SaveChangesAsync();

            // if yazılcak tekrar tekrar olmasın diye
            if (await session.Query<DutyEntity>().AnyAsync())
                return;

            session.Store<DutyEntity>(GetInitialDuties());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<DutyEntity> GetInitialDuties()
        {
            return new List<DutyEntity>
            {
                new DutyEntity
                {
                    Id = 9001,
                    Description = "46157 Binek Oto",
                    MailDescription = "Bakalım",
                    Priority = 50,
                    Status = "Devam_Ediyor",
                    AssignedEmployeeId = 901,
                    Client = new Client
                    {
                        Id = 901,
                        Name = "ES"
                    },
                    CreatedAt = new DateTime(2026, 3, 4)
                },
                new DutyEntity
                {
                    Id = 9002,
                    Description = "47257 Bayi depo Stok takibi",
                    MailDescription = "Teknik tasarım dökümanı gönderilecek",
                    Priority = 25,
                    Status = "Devam_Ediyor",
                    AssignedEmployeeId = 902,
                    Client = new Client
                    {
                        Id = 901,
                        Name = "ES"
                    },
                    CreatedAt = new DateTime(2026, 3, 4)
                },
                new DutyEntity
                {
                    Id = 9003,
                    Description = "47542 Bayi Satış Durum Raporu",
                    MailDescription = "Bakalım",
                    Priority = 50,
                    Status = "Devam_Ediyor",
                    AssignedEmployeeId = 903,
                    Client = new Client
                    {
                        Id = 901,
                        Name = "ES"
                    },
                    CreatedAt = new DateTime(2026, 3, 4)
                },
                 new DutyEntity
                {
                    Id = 9004,
                    Description = "Arge Roketsan düzenlemeleri",
                    MailDescription = " ",
                    Priority = 60,
                    Status = "Devam_Ediyor",
                    AssignedEmployeeId = 903,
                    Client = new Client
                    {
                        Id = 902,
                        Name = "BEST"
                    },
                    CreatedAt = new DateTime(2026, 3, 4)
                },
                     new DutyEntity
                {
                    Id = 9005,
                    Description = "ES 47953 Kodsuz malzeme türet",
                    MailDescription = " ",
                    Priority = 30,
                    Status = "Test_Bekliyor",
                    AssignedEmployeeId = 902,
                    Client = new Client
                    {
                        Id = 901,
                        Name = "ES"
                    },
                    CreatedAt = new DateTime(2026, 3, 4)
                }
            };
        }

    }
      */
