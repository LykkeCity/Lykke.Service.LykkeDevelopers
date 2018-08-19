using Lykke.Common.Api.Contract.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.LykkeDevelopers.Client
{
    internal class ApiRunner
    {
        public async Task RunAsync(Func<Task> method)
        {
            try
            {
                await method();
            }
            catch (ApiException ex)
            {
                ThrowException(ex);
            }
        }

        public async Task<T> RunAsync<T>(Func<Task<T>> method)
        {
            try
            {
                return await method();
            }
            catch (ApiException ex)
            {
                ThrowException(ex);
                return default(T);
            }
        }

        public void ThrowException(ApiException ex)
        {
            if (ex.HasContent)
            {
                throw new ErrorResponseException(ex, GetErrorResponse(ex));
            }
            else
            {
                throw new ErrorResponseException(inner: ex);
            }
        }

        private static ErrorResponse GetErrorResponse(ApiException ex)
        {
            ErrorResponse errorResponse;

            try
            {
                errorResponse = ex.GetContentAs<ErrorResponse>();
            }
            catch (Exception)
            {
                errorResponse = null;
            }

            return errorResponse;
        }
    }
}
