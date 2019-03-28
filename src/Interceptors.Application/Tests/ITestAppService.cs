using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Interceptors.Tests.Dtos;
using Interceptors.Interceptors.PreTreatment;
using Interceptors.Interceptors.Validation;
using Microsoft.EntityFrameworkCore;

namespace Interceptors.Tests
{
    public interface ITestAppService : IApplicationService, IBaseValidatedAppService, IPreTreatmentAppService
    {
        Task Delete(DeleteIn pRequest);
    }
}
