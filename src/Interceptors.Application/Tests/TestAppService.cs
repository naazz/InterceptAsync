using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Interceptors.Interceptors.PreTreatment;
using Interceptors.Tests.Dtos;

namespace Interceptors.Tests
{
    public class TestAppService : ITestAppService
    {
        private readonly IRepository<Test> _repository;

        public TestAppService(IRepository<Test> pRepository)
        {
            _repository = pRepository;
        }

        [UxPreTreatment]
        public virtual async Task Delete(DeleteIn pRequest)
        {
            await this._repository.DeleteAsync(w => w.Id == pRequest.Id);
        }
    }
}
