using DevSandbox.Models;
using System.Collections.Generic;

namespace DevSandbox.Tests.SampleData
{
    public static class SampleCompany
    {
        public static Company Create()
        {
            return new Company
            {
                name = "ABC Sample Company",
                email = "info@abccompany.com",
                need_report = false,
                need_approval = false,
                region = "99",
                document_type = "A",
                SubGroups = new List<SubGroup>()
                {
                    new SubGroup
                    {
                        acronym = "XYZ",
                        long_name = "XYZ Group",
                        type = "Test type 1",
                        instructions = "Queue",
                        Translation = new Translation()
                    },
                    new SubGroup
                    {
                        acronym = "123",
                        long_name = "123 Group",
                        type = "Test type 88",
                        instructions = "Archive",
                        Translation = new Translation()
                    },
                    new SubGroup
                    {
                        acronym = "QRS",
                        long_name = "QRS Group",
                        type = "Test type 100",
                        instructions = "Queue",
                        Translation = new Translation()
                    },
                    new SubGroup
                    {
                        acronym ="DEF",
                        long_name = "Defalut Group",
                        type = "Test type 4",
                        instructions = "Process",
                        Translation = new Translation()
                    }
                }
            };

        }
    }
}
