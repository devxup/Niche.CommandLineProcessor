﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class CommandLineMode<T>
    {
        /// <summary>
        /// Gets the name of this mode
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a description of this option
        /// </summary>
        public string Description { get; private set; }


        public CommandLineMode(T instance, MethodInfo method)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (!method.DeclaringType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Expect method to be callable on instance");
            }

            if (!typeof(T).IsAssignableFrom(method.ReturnType))
            {
                var message
                    = string.Format("Expect method return type to be compatible with {0}", typeof(T).Name);
                throw new ArgumentException(message);
            }


            mInstance = instance;
            mMethod = method;
            Description = CommandLineOptionBase.FindDescription(method);

            Name = CamelCase.ToDashedName(method.Name);
        }

        public bool HasName(string name)
        {
            return string.Equals(Name, name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Activate this switch when found
        /// </summary>
        public T Activate()
        {
            return (T)mMethod.Invoke(mInstance, null);
        }

        /// <summary>
        /// Create help text for this option
        /// </summary>
        public IEnumerable<string> CreateHelp()
        {
            var text
    = string.Format(
        CultureInfo.CurrentCulture,
        "{0}\t\t{1}",
        Name,
        Description);

            yield return text;
        }

        private readonly object mInstance;
        private readonly MethodInfo mMethod;
    }
}
