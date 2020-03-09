using DevExtreme.AspNet.Data.ResponseModel;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class PaginationResultDxExtensions
    {
        public static LoadResult ToDxLoadResult<T>(this PaginationResult<T> paginationResult)
        {
            return new LoadResult
            {
                data = paginationResult.Result,
                totalCount = paginationResult.ResultTotalCount
            };
        }
    }
}