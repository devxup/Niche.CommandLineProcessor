﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    ///  Wrapper class that handles a simple switch - something without a value
    /// </summary>
    public class CommandLineSwitch : CommandLineOptionBase
    {
        /// <summary>
        /// Gets the short form of this switch
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets the other short form of this switch
        /// </summary>
        public string AlternateShortName { get; private set; }

        /// <summary>
        /// Gets the long form of this switch
        /// </summary>
        public string LongName { get; private set; }

        /// <summary>
        /// Test to see if the specified method is a switch
        /// </summary>
        /// Switches are methods with no return type and no parameters that have
        /// a [Description] attribute.
        /// <param name="method">Method to test</param>
        /// <returns>True if it is a switch, false otherwise.</returns>
        public static bool IsSwitch(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.ReturnType == typeof(void)
                && method.GetParameters().Length == 0
                && method.GetCustomAttribute<DescriptionAttribute>() != null;
        }

        public static IEnumerable<CommandLineSwitch> CreateSwitches(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var methods = instance.GetType().GetMethods();

            var switches
                = methods.Where(IsSwitch)
                    .Select(m => new CommandLineSwitch(instance, m))
                    .ToList();

            return switches;
        }

        public CommandLineSwitch(object instance, MethodInfo method)
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

            mInstance = instance;
            mMethod = method;

            ShortName = "-" + CamelCase.ToShortName(method.Name);
            AlternateShortName = "/" + CamelCase.ToShortName(method.Name);
            LongName = "--" + CamelCase.ToDashedName(method.Name);
        }

        /// <summary>
        /// Activate this switch when found
        /// </summary>
        public override void Activate(Queue<string> arguments)
        {
            mMethod.Invoke(mInstance, null);
        }

        /// <summary>
        /// Add triggers to activate this option to the passed dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary that collects our triggers</param>
        public override void AddOptionsTo(Dictionary<string, CommandLineOptionBase> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            dictionary[ShortName] = this;
            dictionary[AlternateShortName] = this;
            dictionary[LongName] = this;
        }

        private readonly object mInstance;
        private readonly MethodInfo mMethod;
    }
}
