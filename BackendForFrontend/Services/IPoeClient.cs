namespace BackendForFrontend.Services;

public interface IPoeClient {
    Task<List<PoeStashItem>> GetItemsAsync(string accountName, string sessionId, string realm, string league, string tabName);
}
