﻿ using System;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.Globalization;
using System.Reflection;

namespace Niche.CommandLine
{
    /// <summary>
    /// A disctinct operational mode for the application
    /// </summary>
    [DebuggerDisplay("Mode: {" + nameof(Name) + "}")]
    public class CommandLineMode
    {
        private readonly object _instance;

        private readonly MethodInfo _method;

        /// <summary>
        /// Gets the name of this mode
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a description of this option
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the CommandLineMode class
        /// </summary>
        /// <param name="driverType">Type of the driver</param>
        /// <param name="instance">Instance on which the mode was declared.</param>
        /// <param name="method">Method to invoke to activate the mode.</param>
        public CommandLineMode(Type driverType, object instance, MethodInfo method)
        {
            if (driverType == null)
            {
                throw new ArgumentNullException(nameof(driverType));
            }

            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _method = method ?? throw new ArgumentNullException(nameof(method));

            Debug.Assert(method.DeclaringType != null);
            if (!method.DeclaringType.GetTypeInfo().IsInstanceOfType(instance))
            {
                throw new ArgumentException("Expect method to be callable on instance", nameof(method));
            }

            if (!driverType.GetTypeInfo().IsAssignableFrom(method.ReturnType))
            {
                var message
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        "Expected method return type to be compatible with {0}",
                        driverType.Name);
                throw new ArgumentException(message, nameof(method));
            }

            Description = CommandLineOptionBase.FindDescription(method);
            Name = CamelCase.ToDashedName(method.Name);
        }

        /// <summary>
        /// Test to see if ths mode has the specified name
        /// </summary>
        /// <param name="name">Name to check for.</param>
        /// <returns>True if the names match, false if not.</returns>
        public bool HasName(string name)
        {
            return string.Equals(Name, name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Activate this switch when found
        /// </summary>
        public object Activate()
        {
            return _method.Invoke(_instance, null);
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
    }
}
