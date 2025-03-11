using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace HorizontalVirtualize.Client.Pages;

public partial class Home
{
    private static async ValueTask<ItemsProviderResult<Thing>> LoadDataAsync(ItemsProviderRequest request)
    {
        await Task.Yield();
        var data = Enumerable
            .Range(request.StartIndex, request.Count)
            .Select((x, index) =>
            {
                int fullIndex = request.StartIndex + index;
                string[] words = Enumerable.Repeat(fullIndex.ToString(), 3).ToArray();
                return new Thing(fullIndex, words);
            });
        return new ItemsProviderResult<Thing>(data, 200);
    }
}

class Thing
{
    public int Index { get; set; }
    public string[] Words { get; set; } = [];

    public Thing(int index, string[] words)
    {
        Index = index;
        Words = words;
    }

}