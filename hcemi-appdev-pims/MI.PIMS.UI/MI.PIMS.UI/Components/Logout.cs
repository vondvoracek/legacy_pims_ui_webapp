using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Components
{
    [ViewComponent(Name = "Logout")] 
    public class Logout: ViewComponent
    {
        public Logout(){}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
