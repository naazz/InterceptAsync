using Abp.Dependency;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interceptors.Interceptors.PostTreatment
{
    public interface IPostTreatmentExecutor<T> : ITransientDependency
    {
        List<T> GetEntities(List<int> pIds);
    }
}
