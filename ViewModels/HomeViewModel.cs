using Ecommerce.Models;

namespace Ecommerce.ViewModels
{
    // Instead of passing multiple things separately, you just expand the ViewModel
    //  public List<Category> Categories { get; set; }
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        //public string SearchQuery { get; set; } = string.Empty;
        //public int PageNumber { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
        //public int TotalCount { get; set; }
        //public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
