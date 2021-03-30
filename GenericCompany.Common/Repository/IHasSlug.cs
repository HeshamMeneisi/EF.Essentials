namespace GenericCompany.Common.Repository
{
    public interface IHasSlug
    {
        public string Slug { get; set; }
        public string GetSlugSource();
    }
}
