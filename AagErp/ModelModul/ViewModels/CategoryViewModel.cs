using ModelModul.Models;

namespace ModelModul.ViewModels
{
    public class CategoryViewModel: Category
    {
        public CategoryViewModel() { }

        public CategoryViewModel(Category obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IdParent = obj.IdParent;
            Parent = obj.Parent;
        }
    }
}
