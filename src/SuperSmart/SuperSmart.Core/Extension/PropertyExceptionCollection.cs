using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Extension
{
    public class PropertyExceptionCollection : Exception
    {
        public PropertyExceptionCollection()
        {
            this.Exceptions = new List<ValidationResult>();
        }
        public PropertyExceptionCollection(string propertyName, string message) : this()
        {
            this.Exceptions.Add(new ValidationResult(message, new List<string>()
            {
                propertyName
            }));
        }
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
        public PropertyExceptionCollection(List<ValidationResult> validationResults) : this()
        {
            this.Exceptions = validationResults;
        }
        public PropertyExceptionCollection(ValidationResult validationResult) : this()
        {
            this.Exceptions.Add(validationResult);
        }
        public List<ValidationResult> Exceptions { get; private set; }
    }
}
