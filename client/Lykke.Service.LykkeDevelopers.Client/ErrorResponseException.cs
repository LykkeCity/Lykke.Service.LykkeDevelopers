﻿using System;
using System.Net;
using Lykke.Common.Api.Contract.Responses;
using Refit;

namespace Lykke.Service.LykkeDevelopers.Client
{
    /// <summary>
    /// represents error message from LykkeDevelopers API service
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ErrorResponseException"/> with error message.
        /// </summary>
        /// <param name="message">The error message</param>
        public ErrorResponseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorResponseException"/> with response error details and API excepiton.
        /// </summary>
        /// <param name="error">The response error details</param>
        /// <param name="inner">The exception occured during calling service API.</param>
        public ErrorResponseException(ApiException inner, ErrorResponse error = null)
            : base(error != null ? (error.ModelErrors != null && error.ModelErrors.Count > 0 ? error.GetSummaryMessage() : error.ErrorMessage)
                 : inner.Message ?? string.Empty, inner)
        {
            Error = error;
            StatusCode = inner.StatusCode;
        }

        /// <summary>
        /// Gets a response error details.
        /// </summary>
        public ErrorResponse Error { get; }

        /// <summary>
        /// Gets a http response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}
