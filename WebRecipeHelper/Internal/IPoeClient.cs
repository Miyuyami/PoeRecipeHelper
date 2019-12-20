using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebRecipeHelper
{
    public interface IPoeClient
    {
        Task<List<PoeItem>> GetItemsAsync(string sessionId, string league, string realm, string accountName, string tabName);
    }
}
