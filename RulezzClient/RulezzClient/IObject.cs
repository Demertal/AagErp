using System.Data.Linq.Mapping;

namespace RulezzClient
{
    interface IObject
    {
        [Column(Name = "title")]
        string Title { get; set; }

        [Column(Name = "id")]
        int Id { get; set; }
    }
}
