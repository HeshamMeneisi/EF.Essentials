using System.Linq;
using System.Text.RegularExpressions;

namespace EF.Essentials.Helpers
{
    public class NameAcronym
    {
        public NameAcronym(string entityName)
        {
            (Name, Acronym) = DataHelper.GetNameParts(entityName);
            PredictAcronym();
        }

        public NameAcronym(string n, string a = null)
        {
            Acronym = a == null ? null : Regex.Replace(a, "\\s+", " ").Trim();
            Name = Regex.Replace(n, "\\s+", " ").Trim();
            PredictAcronym();
        }

        public string Acronym { get; private set; }
        public string Name { get; }

        private void PredictAcronym()
        {
            if (!string.IsNullOrEmpty(Acronym)) return;

            Acronym = string.Join("",
                Regex.Matches(Name, "[A-Z]").ToArray().Select(m => m.Value)).Trim();
        }

        public override string ToString()
        {
            return $"{Name} ({Acronym})";
        }
    }
}
