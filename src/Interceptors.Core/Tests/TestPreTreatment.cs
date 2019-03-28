using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Interceptors.Tests
{
    public class TestPreTreatment : Entity
    {
        public string Props1 { get; set; }

        public int? TestId { get; set; }

        public Test Test { get; set; }
    }
}
