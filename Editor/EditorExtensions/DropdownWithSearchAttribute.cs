using System;
using System.Diagnostics;
using TriInspector;

namespace EditorExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class DropdownWithSearchAttribute : Attribute
    {
        public string Values { get; }

        public TriMessageType ValidationMessageType { get; set; } = TriMessageType.Error;

        public DropdownWithSearchAttribute(string values)
        {
            Values = values;
        }
    }
}