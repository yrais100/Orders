﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoriesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        public List<Category>? Categories { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();

        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Category>>("api/categories");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsyn();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Categories = responseHttp.Response;
        }

        private async Task DeleteAsync(Category category)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Estas seguro de querer borrar el país {category.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<Category>($"api/categories/{category.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsyn();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con éxito.");
        }

    }
}
