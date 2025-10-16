
using System.Net;
using eMotoCare.BO.DTO.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Extensions
{
    public static class ApiResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this ControllerBase controller, ApiResponse<T>? response)
        {
            if (response == null)
            {
                var err = ApiResponse<object>.ServerError("Null response from service");
                return controller.StatusCode((int)HttpStatusCode.InternalServerError, err);
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
                return controller.NoContent();

            return controller.StatusCode((int)response.StatusCode, response);
        }
    }
}