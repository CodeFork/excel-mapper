using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExcelMapper
{
    public static class ExcelImporterUtils
    {
#if NETSTANDARD2_0
        public static IEnumerable<ExcelClassMap> RegisterClassMapsInNamespace(this ExcelImporter importer,
            string namespaceString)
        {
            return RegisterClassMapsInNamespace(importer, Assembly.GetExecutingAssembly(), namespaceString);
        }
#endif

        public static IEnumerable<ExcelClassMap> RegisterClassMapsInNamespace(this ExcelImporter importer,
            Assembly assembly,
            string namespaceString)
        {
            if (namespaceString == null)
                throw new ArgumentNullException(nameof(namespaceString));

            if (namespaceString.Length == 0)
                throw new ArgumentException("The namespace cannot be empty.", nameof(namespaceString));

            if (!assembly.GetTypes().Any())
                throw new ArgumentException("The assembly doesn't have any types.", nameof(assembly));

            var classes = assembly
                .GetTypes()
                .Where(t => (t.IsAssignableFrom(typeof(ExcelClassMap)) || t.IsAssignableToGenericType(typeof(ExcelClassMap<>))) && t.Namespace == namespaceString);

            var objects = classes.Select(Activator.CreateInstance).OfType<ExcelClassMap>().ToList();

            if (!objects.Any())
                throw new ArgumentException("No classmaps found in this namespace.", nameof(assembly));

            foreach (var o in objects)
                importer.Configuration.RegisterClassMap(o);

            return objects;
        }

        internal static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}