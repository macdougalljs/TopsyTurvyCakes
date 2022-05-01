using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {
        private readonly IRecipesService recipesService;

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

        [BindProperty]  // Access data posted in request - model binding - will iterate over each properties of the object and attempt to match the name of each property with the name of a value in the request - so they MUST match!
        public Recipe Recipe
        {
            get; set;
        }

        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }

        public async Task OnGetAsync()  // one of the most powerful features // the ability to execute different code based on the http verb the request was made with
        {
            // load the data for the initial recipe

            Recipe = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe(); // if it's not found, create a new recipe instead

        }

        public async Task<IActionResult> OnPostAsync()
        {
            Recipe.Id = Id.GetValueOrDefault();
            await recipesService.SaveAsync(Recipe);
            return RedirectToPage("/Recipe", new
            {
                id = Recipe.Id
            });
            // check out razor pages documentation to see all the various actionResult types and usage

        }
    }
}
