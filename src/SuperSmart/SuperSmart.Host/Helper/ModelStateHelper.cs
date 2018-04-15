using SuperSmart.Core.Extension;
using System.Linq;
using System.Web.Mvc;

namespace SuperSmart.Host.Helper
{
    public static class ModelStateHelper
    {

        public static void Merge(this ModelStateDictionary modelState, PropertyExceptionCollection exceptionCollection)
        {
            if(exceptionCollection?.Exceptions == null)
            {
                return;
            }
            foreach (var item in exceptionCollection.Exceptions)
            {
                modelState.AddModelError(item.MemberNames.FirstOrDefault(), item.ErrorMessage);
            }
        } 
    }
}