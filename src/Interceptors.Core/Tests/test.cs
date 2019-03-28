using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Interceptors.Tests
{
    public class Test : Entity
    {
        public string Props1 { get; set; }

        public string Props2 { get; set; }

        public ICollection<TestPreTreatment> TestPreTreatments { get; set; } = new List<TestPreTreatment>();
    }
}
