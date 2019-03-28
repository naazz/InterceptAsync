using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Interceptors.Interceptors.Validation
{
    public interface IBaseValidator : ITransientDependency
    {
        //..........................####...######..######..######..##..##..######..######...####...##..##.................................
        //.........................##..##....##......##....##......###.##....##......##....##..##..###.##.................................
        //.........................######....##......##....####....##.###....##......##....##..##..##.###.................................
        //.........................##..##....##......##....##......##..##....##......##....##..##..##..##.................................
        //.........................##..##....##......##....######..##..##....##....######...####...##..##.................................
        //................................................................................................................................
        //Quand on implémente cette interface il est primordiale que les
        //Méthode qui sont déclarer dans la classe qui implémente respectent une nomenclature
        //
        //On doit avoir le nom de la méthode du appService (ou est contenue le validator) concatener du mot Validaiton
        //Par exemple le CategoryAppService contient un IBaseValidator qui est de type CategoryAppServiceValidator
        //on vas donc retrouver des méthodes du genre
        //GetValidation
        //PostValidation
        //PutValidation

    }
}
