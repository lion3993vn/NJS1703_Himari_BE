namespace HimariServer.Repository.Entities
{
    public partial class BlogCategory : BaseEntity
    {
        public string? Name { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
