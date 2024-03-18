﻿using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] IRepository Repository { get; set; } = null!;
        public List<Country>? Countries { get; set; }
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var responseHttp = await Repository.GetAsync<List<Country>>("api/countries");
            Countries = responseHttp.Response;
        }
    }
}
