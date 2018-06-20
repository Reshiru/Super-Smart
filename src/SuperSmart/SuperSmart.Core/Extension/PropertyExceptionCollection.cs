using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Extension
{
    /// <summary>
    /// The property exception class which will
    /// be used to throw "ModelState" errors
    /// </summary>
    public class PropertyExceptionCollection : Exception
    {
        /// <summary>
        /// Overrides the default constructor and 
        /// initializes the empty parameters
        /// </summary>
        public PropertyExceptionCollection()
        {
            this.Exceptions = new List<ValidationResult>();
        }

        /// <summary>
        /// Initializes a new excpetion collection 
        /// for the given property by its name and it's given message
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="message"></param>
        public PropertyExceptionCollection(string propertyName, string message) : this()
        {
            var excpetion = new ValidationResult(message, new List<string>()
            {
                propertyName
            });

            this.Exceptions.Add(excpetion);
        }

        /// <summary>
        /// Initializes a new exception collection for 
        /// a tuple of validation results
        /// </summary>
        /// <param name="validationResults">
        /// 1. string = propertyName, 
        /// 2. string = message
        /// </param>
        public PropertyExceptionCollection(List<Tuple<string, string>> validationResults) : this()
        {
            foreach (var item in validationResults)
            {
                this.Exceptions.Add(new ValidationResult(item.Item1, new List<string>()
                {
                    item.Item2
                }));
            }
        }

        /// <summary>
        /// Initializes a new property exception collection 
        /// with the given validation results
        /// </summary>
        /// <param name="validationResults"></param>
        public PropertyExceptionCollection(List<ValidationResult> validationResults) : this()
        {
            this.Exceptions = validationResults;
        }

        /// <summary>
        /// Initializes a new property exception collection
        /// with the given validation result
        /// </summary>
        /// <param name="validationResult"></param>
        public PropertyExceptionCollection(ValidationResult validationResult) : this()
        {
            this.Exceptions.Add(validationResult);
        }

        /// <summary>
        /// Validation errors which are added by
        /// the constructors
        /// </summary>
        public List<ValidationResult> Exceptions { get; private set; }
    }
}