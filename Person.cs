using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTask
{
    [Index("lastName", "gender")]
    public class Person
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
        public DateOnly wasBorn { get; set; }
        public string gender { get; set; }

    }
}
