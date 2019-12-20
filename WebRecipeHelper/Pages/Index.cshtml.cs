using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace WebRecipeHelper.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> Logger;
        private readonly IPoeClient Client;

        public List<SelectListItem> Leagues { get; } = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Metamorph",  Value ="Metamorph" },
            new SelectListItem { Text = "Metamorph Hardcore",  Value ="Hardcore Metamorph" },
            new SelectListItem { Text = "Metamorph SSF", Value = "SSF Metamorph" },
            new SelectListItem { Text = "Metamorph Hardcore SSF", Value = "SSF Metamorph HC" },
            new SelectListItem { Text = "Standard", Value = "Standard" },
            new SelectListItem { Text = "Standard SSF",  Value ="SSF Standard" },
            new SelectListItem { Text = "Hardcore",  Value ="Hardcore" },
            new SelectListItem { Text = "Hardcore SSF", Value = "SSF Hardcore" },
        };

        public List<SelectListItem> Realms { get; } = new List<SelectListItem>()
        {
            new SelectListItem { Text = "PC", Value = "pc" },
            new SelectListItem { Text = "Xbox",  Value ="xbox" },
            new SelectListItem { Text = "Sony",  Value ="sony" },
        };

        [BindProperty(SupportsGet = true), Required]
        public string SessionId { get; set; }
        [BindProperty(SupportsGet = true), Required]
        public string League { get; set; }
        [BindProperty(SupportsGet = true), Required]
        public string Realm { get; set; }
        [BindProperty(SupportsGet = true), Required]
        public string AccountName { get; set; }
        [BindProperty(SupportsGet = true), Required]
        public string TabName { get; set; }

        public string Result { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IPoeClient client)
        {
            this.Logger = logger;
            this.Client = client;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var result = await this.Client.GetItemsAsync(this.SessionId, this.League, this.Realm, this.AccountName, this.TabName);

            this.Result = String.Join("\n", result.Select(i => i.TypeLine));

            return this.Page();
        }
    }
}
