//-----------------------------------------------------------------------
// <copyright file="TypeMatchIndexingRule.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.TypeSearch
{
    public class TypeMatchIndexingRule
    {
        public delegate bool TypeMatchIndexingRuleDelegate(ref TypeSearchInfo info, ref string errorMessage);

        public readonly string Name;
        private TypeMatchIndexingRuleDelegate rule;

        public TypeMatchIndexingRule(string name, TypeMatchIndexingRuleDelegate rule)
        {
            this.Name = name;
            this.rule = rule;
        }

        public bool Process(ref TypeSearchInfo info, ref string errorMessage)
        {
            return this.rule(ref info, ref errorMessage);
        }

        public override string ToString()
        {
            return "TypeMatchIndexingRule: " + this.Name;
        }
    }
}
#endif