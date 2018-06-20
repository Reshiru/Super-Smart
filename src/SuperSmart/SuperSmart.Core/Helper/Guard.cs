using SuperSmart.Core.Extension;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SuperSmart.Core.Helper
{
    public static class Guard
    {
        public static void NotNull(object @object, [CallerMemberName] string memberName = "")
        {
            if (@object != null)
            {
                return;
            }
            
            Guard.ThrowException(memberName, "cannot be null");
        }

        public static void NotNullOrEmpty(object @object, [CallerMemberName] string memberName = "")
        {
            Guard.NotNull(@object);

            if (!string.IsNullOrWhiteSpace(@object.ToString()))
            {
                return;
            }

            Guard.ThrowException(memberName, "cannot be empty");
        }

        public static void ModelStateCheck<T>(T @object, [CallerMemberName] string memberName = "")
        {
            Guard.NotNull(@object, memberName);

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(@object, new ValidationContext(@object, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }
        }

        private static void ThrowException(string memberName, string exception)
        {
            throw new PropertyExceptionCollection(memberName, memberName + " " + exception);
        }
    }
}