using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {

        [FromRoute]
        public long? Id
        {
            get; set;
        }   // may, or may not be populated depending on the URL the user has navigated to

        public bool IsNewReceipe
        {
            get
            {
                return Id == null;
            }
        }

        public Recipe Recipe
        {
            get; set;
        }
        public void OnGet()
        {
        }
    }
}
