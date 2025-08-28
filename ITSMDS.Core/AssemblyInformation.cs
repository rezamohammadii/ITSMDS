using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITSMDS.Core;

public record AssemblyInformation(string Product, string Description, string Version)
{
    public static readonly AssemblyInformation Current = new(typeof(AssemblyInformation).Assembly);
    private AssemblyInformation(Assembly assembly)
        : this(
                assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "ITSMDS",
                assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "IT Service Management System ",
                assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "1.0.0")
    { }
}
