using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {
        private readonly IRecipesService recipesService;

        [BindProperty]
        public IFormFile Image
        {
            get; set;
        }

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
            var recipe = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();

            // to control what gets updated and when, each field in the database will need to be updated individually

            recipe.Name = Recipe.Name;
            recipe.Description = Recipe.Description;
            recipe.Ingredients = Recipe.Ingredients;
            recipe.Directions = Recipe.Directions;

            // image data

            if (Image != null)
            {
                // first copy to a temporary stream, and then read the images data from that stream
                using (var stream = new System.IO.MemoryStream())
                {
                    await Image.CopyToAsync(stream);
                    recipe.Image = stream.ToArray();
                    recipe.ImageContentType = Image.ContentType;
                }
            }

            await recipesService.SaveAsync(recipe);
            return RedirectToPage("/Recipe", new
            {
                id = recipe.Id
            });
            // check out razor pages documentation to see all the various actionResult types and usage
        }

        public async Task<IActionResult> OnPostDelete(Recipe recipe)
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }

     
    }
}
