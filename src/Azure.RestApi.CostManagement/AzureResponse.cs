﻿namespace Azure.RestApi.CostManagement;

using System.Net;
using Data;

public sealed class AzureResponse<T> where T : class
{
    private readonly ErrorResponse? _error;

    public AzureResponse(T content, HttpStatusCode statusCode, HttpRequestMessage requestMessage)
    {
        Content        = content;
        StatusCode     = statusCode;
        RequestMessage = requestMessage;
        RequestUri     = requestMessage.RequestUri!;
        IsSuccess      = true;
    }

    public AzureResponse(HttpStatusCode statusCode, HttpRequestMessage requestMessage, ErrorResponse error)
            : this(null!, statusCode, requestMessage)
    {
        _error    = error;
        IsSuccess = false;
    }

    public T Content { get; }

    public HttpStatusCode StatusCode { get; }

    public bool IsSuccess { get; }

    public HttpRequestMessage RequestMessage { get; }

    public Uri RequestUri { get; }

    public string ErrorMessage => _error?.ToString()!;
}