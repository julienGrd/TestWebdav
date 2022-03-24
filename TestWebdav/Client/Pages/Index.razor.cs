using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using TestWebdav.Shared;

namespace TestWebdav.Client.Pages
{
    public partial class Index
    {
        private WebdavFileModel[]? files;

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            files = await Http.GetFromJsonAsync<WebdavFileModel[]>("WebdavFile");
        }
    }
}
