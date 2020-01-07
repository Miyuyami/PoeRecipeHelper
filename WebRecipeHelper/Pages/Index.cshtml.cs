using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace WebRecipeHelper.Pages
{
    public class PaginatedList<T> : List<T>
    {

    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> Logger;
        private readonly IPoeClient Client;

        public readonly double OneUnitSize = 47.5;

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

        public int MaxDisplayedSetIndex => this.HighlightSets?.Count ?? 0;

        private int displayedSetIndex;
        public int DisplayedSetIndex
        {
            get => this.displayedSetIndex;
            set => this.displayedSetIndex = Math.Min(Math.Max(value, 0), this.MaxDisplayedSetIndex);
        }

        public bool CanPrevious => this.DisplayedSetIndex > 0;
        public bool CanNext => this.DisplayedSetIndex < this.MaxDisplayedSetIndex;

        public bool CanDisplayStash => this.Stash != null;
        public IEnumerable<PoeItem> Stash { get; set; }
        public bool CanDisplayHighlights => this.HighlightSets != null;
        public IImmutableList<IImmutableList<PoeItemQuality>> HighlightSets { get; set; }

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

            this.Stash = result;

            this.HighlightSets = this.GetSets(result, 40, 20).ToImmutableList();

            return this.Page();
        }

        private ImmutableList<PoeItemQuality>.Builder CreateNewBuilder() => ImmutableList.CreateBuilder<PoeItemQuality>();
        private IEnumerable<IImmutableList<PoeItemQuality>> GetSets(IEnumerable<PoeItem> items, int sum, int instantValue)
        {

            var enumerable =
                items.Select(i => (i, i.Properties.FirstOrDefault(p => p.Name == "Quality")))
                     .Where(t => t.Item2 != null)
                     .Select(t => new PoeItemQuality(t.i, t.Item2.Values.Select(t => Convert.ToInt32(Regex.Match(t.value, @"\+(\d+)\%").Groups[1].Value)).Sum()))
                     .OrderByDescending(t => t.Quality);

            var instants = enumerable.Where(p => p.Quality >= instantValue &&
                                                 p.Item.ExplicitMods.Count == 0);

            foreach (var i in instants)
            {
                var builder = this.CreateNewBuilder();
                builder.Add(i);
                yield return builder.ToImmutable();
            }

            var restSet = enumerable.Except(instants)
                                    .ToHashSet();
            IImmutableList<PoeItemQuality> result;
            do
            {
                if (restSet.Count == 0)
                {
                    break;
                }

                var list = this.Rec(restSet.ToList(), sum, 0);
                if (list == null)
                {
                    break;
                }

                result = list.ToImmutableList();
                restSet.ExceptWith(result);

                yield return result;
            } while (true);
        }

        private List<PoeItemQuality> Rec(List<PoeItemQuality> list, int sum, int index)
        {
            if (sum < 0)
            {
                return null;
            }

            List<PoeItemQuality> result;
            PoeItemQuality i;
            do
            {
                i = list[index++];

                if (sum - i.Quality == 0)
                {
                    return new List<PoeItemQuality>() { i };
                }

                if (index >= list.Count)
                {
                    return null;
                }

                result = this.Rec(list, sum - i.Quality, index);
            } while (result == null);

            result.Add(i);
            return result;
        }
    }
}
