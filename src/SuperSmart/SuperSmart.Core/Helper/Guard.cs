using SuperSmart.Core.Extension;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SuperSmart.Core.Helper
{
    /// <summary>
    /// The guard class which handles
    /// different types of validations
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// The not null validation, to check that the given
        /// object isn't null
        /// </summary>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        public static void NotNull(object @object, [CallerMemberName] string memberName = "")
        {
            if (@object != null)
            {
                return;
            }
            
            Guard.ThrowException(memberName, "cannot be null");
        }

        /// <summary>
        /// The not null or empty check, checks whenever
        /// the given object isn't null or if it's converted
        /// to a string it shouldn't be empty
        /// </summary>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        public static void NotNullOrEmpty(object @object, [CallerMemberName] string memberName = "")
        {
            Guard.NotNull(@object);

            if (!string.IsNullOrWhiteSpace(@object.ToString()))
            {
                return;
            }

            Guard.ThrowException(memberName, "cannot be empty");
        }

        /// <summary>
        /// Checks if the given object has a valid 
        /// model state
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        public static void ModelStateCheck<T>(T @object, [CallerMemberName] string memberName = "")
        {
            Guard.NotNull(@object, memberName);

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(@object, new ValidationContext(@object, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }
        }

        /// <summary>
        /// Helper to throw a exception for
        /// a given validation
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="exception"></param>
        private static void ThrowException(string memberName, string exception)
        {
            throw new PropertyExceptionCollection(memberName, memberName + " " + exception);
        }
    }
}