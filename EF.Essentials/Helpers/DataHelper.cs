using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.StaticFiles;

namespace EF.Essentials.Helpers
{
    public class DataHelper
    {
        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool IsAcronymOf(string acronym, string name)
        {
            acronym = acronym.Trim().ToLower();
            name = name.Trim().ToLower();
            var i = 0;
            var j = 0;

            while (i < acronym.Length && j < name.Length)
            {
                if (acronym[i] == ' ')
                {
                    i++;
                    continue;
                }

                while (j < name.Length && name[j] == ' ') j++;

                while (i < acronym.Length && j < name.Length && name[j] != ' ' && name[j] == acronym[i])
                {
                    i++;
                    j++;
                }

                while (j < name.Length && name[j] != ' ') j++;
            }

            return i == acronym.Length;
        }

        public static (string name, string acronym) GetNameParts(string s)
        {
            s = Regex.Replace(s, "\\s+", " ").Trim();
            var a = s.Split('-', '—');
            if (a.Length == 1)
            {
                if (s.ToUpper() == s && !s.Contains(" "))
                    return (null, s);
                return (s, null);
            }

            if (IsAcronymOf(a[0].Trim(), a[1].Trim()))
                return (a[1].Trim(), a[0].Trim());
            if (IsAcronymOf(a[1].Trim(), a[0].Trim()))
                return (a[0].Trim(), a[1].Trim());

            return (s, null);
        }

        public static bool CheckBracketsIntersect(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            var bracketA = new NumBracket(a);
            var bracketB = new NumBracket(b);
            return bracketA.Intersects(bracketB);
        }

        public static IEnumerable<int> GetBinetFibonacci(int start, int end)
        {
            var PHI = 1.618033988749;
            var R5 = Math.Sqrt(5);
            for (var i = start; i <= end; i++)
                yield return (int) Math.Round((Math.Pow(PHI, i) - Math.Pow(-PHI, -i)) / R5);
        }

        public static void CopyValues<T>(T source, T target)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                var existing = prop.GetValue(target);
                if (value != null && (existing?.GetHashCode() ?? 0) == 0)
                    prop.SetValue(target, value, null);
            }
        }

        public static string GetValidExt(string contentType, string[] allowedExt)
        {
            contentType = contentType.Trim();
            return new FileExtensionContentTypeProvider().Mappings.Where(p =>
                    p.Value == contentType && allowedExt.Contains(p.Key))
                .Select(m => m.Key).FirstOrDefault();
        }
    }
}
