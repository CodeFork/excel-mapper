﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExcelMapper.Mappings.MultiItems
{
    /// <summary>
    /// Describes a property map that maps multiple values of one or multiple cells to ICollection&lt;&gt;.
    /// </summary>
    /// <typeparam name="T">The element type of the ICollection to create.</typeparam>
    internal class ConcreteICollectionPropertyMap<T> : EnumerableExcelPropertyMap<T>
    {
        private Type CollectionType { get; }

        public ConcreteICollectionPropertyMap(Type type, MemberInfo member, SingleExcelPropertyMap<T> elementMapping) : base(member, elementMapping)
        {
            CollectionType = type;
        }

        protected override object CreateFromElements(IEnumerable<T> elements)
        {
            var value = (ICollection<T>)Activator.CreateInstance(CollectionType);

            foreach (var element in elements)
            {
                value.Add(element);
            }

            return value;
        }
    }
}
