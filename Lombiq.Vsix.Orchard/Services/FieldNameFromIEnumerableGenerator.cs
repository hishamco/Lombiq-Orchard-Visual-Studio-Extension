﻿using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public class FieldNameFromIEnumerableGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private const string IEnumerableNameRegexPattern = @"^IEnumerable[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority { get { return 15; } }


        public override bool CanGenerate(string dependency) { return Regex.IsMatch(dependency, IEnumerableNameRegexPattern); }

        public override string Generate(string dependency, bool useShortName)
        {
            // This implementation handles only the dependencies with IEnumerable<T> generic types. It places the
            // generic parameter right after the underscore using its plural form.
            var segments = GetGenericTypeSegments(dependency);

            var fieldNameWithoutUnderscore = useShortName ?
                GetShortName(segments.CleanedGenericParameterName) :
                GetCamelCased(segments.CleanedGenericParameterName);

            var pluralizedFieldNameWithoutUnderscore = PluralizationService
                .CreateService(CultureInfo.GetCultureInfo("en-GB"))
                .Pluralize(fieldNameWithoutUnderscore);

            return GetStringWithUnderscore(pluralizedFieldNameWithoutUnderscore);
        }
    }
}
