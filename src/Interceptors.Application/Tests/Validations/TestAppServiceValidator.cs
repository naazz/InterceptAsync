using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Interceptors.Tests.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Interceptors.Tests.Validations
{
    public class TestAppServiceValidator : ITestAppServiceValidator
    {
        private readonly IRepository<Test> _repository;

        public TestAppServiceValidator(IRepository<Test> pRepository)
        {
            _repository = pRepository;
        }

        public async Task DeleteValidation(DeleteIn pRequest)
        {
            if (!await _repository.GetAll().AnyAsync(a => a.Id == pRequest.Id))
            {
                throw new AbpValidationException("Unexisting entity");
            }
        }

        //public void DeleteValidation(DeleteIn pRequest)
        //{
        //    if (!_repository.GetAll().Any(a => a.Id == pRequest.Id))
        //    {
        //        throw new AbpValidationException("Unexisting entity");
        //    }
        //}
    }
}
