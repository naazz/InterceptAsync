using Interceptors.Interceptors.PreTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using Interceptors.Tests.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Interceptors.Tests.PreTreatment
{
    public class TestAppServicePreTreatmentExecutor : ITestAppServicePreTreatmentExecutor
    {
        public IRepository<TestPreTreatment> _repository;

        public TestAppServicePreTreatmentExecutor(IRepository<TestPreTreatment> pRepository)
        {
            this._repository = pRepository;
        }

        public async Task PreTreatment_Delete(DeleteIn pRequest)//async
        {
            List<TestPreTreatment> list = await this._repository.GetAll().Where(w => w.TestId == pRequest.Id).ToListAsync();

            foreach (TestPreTreatment testPreTreatment in list)
            {
                await this._repository.DeleteAsync(testPreTreatment);
            }
        }

        //public void PreTreatment_Delete(DeleteIn pRequest)//async
        //{
        //    List<TestPreTreatment> list = this._repository.GetAll().Where(w => w.TestId == pRequest.Id).ToList();

        //    foreach (TestPreTreatment testPreTreatment in list)
        //    {
        //        this._repository.Delete(testPreTreatment);
        //    }
        //}

    }
}
