﻿using RDS.Core.Models.ViewModels.Company;

namespace RDS.Web.Handlers;

public class CompanyHandler(HttpClientService httpClientService) : ICompanyHandler
{
    private readonly Lazy<Task<HttpClient>> _httpClient = new(httpClientService.GetHttpClientAsync);

    private async Task<HttpClient> GetHttpClientAsync()
    {
        return await _httpClient.Value;
    }

    public async Task<Response<Company?>> CreateAsync(CreateCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/create")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao criar a empresa");    
    }

    public async Task<Response<CompanyUser?>> CreateUserAsync(CreateCompanyUserRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/create-user")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<CompanyUser?>>()
               ?? new Response<CompanyUser?>(null, 400, "Falha ao criar a empresa para o usuário");    
    }
    
    public async Task<Response<Company?>> UpdateAsync(UpdateCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"v1/companies/update")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao atualizar a empresa");    
    }

    public async Task<Response<Company?>> DeleteAsync(DeleteCompanyRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"v1/companies/delete")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Falha ao excluir a empresa");    
    }

    public async Task<PagedResponse<List<Company>>> GetAllAsync(GetAllCompaniesRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/all")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Company>>>()
               ?? new PagedResponse<List<Company>>(null, 400, "Não foi possível obter as empresas");    
    }
  
    public async Task<PagedResponse<List<Company>>> GetAllByUserIdAsync(GetAllCompaniesByUserIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allbyuserid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<Company>>>()
               ?? new PagedResponse<List<Company>>(null, 400, "Não foi possível obter as empresas");    
    }

    // public async Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByAdminAsync(GetAllCompaniesByUserIdRequest request)
    // {
    //     var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allcompanyidnamebyadmin")
    //     {
    //         Content = JsonContent.Create(request)
    //     };
    //     var httpClient = await GetHttpClientAsync();
    //     var result = await httpClient.SendAsync(requestMessage);
    //     return await result.Content.ReadFromJsonAsync<PagedResponse<List<CompanyIdNameViewModel>>>()
    //            ?? new PagedResponse<List<CompanyIdNameViewModel>>(null, 400, "Não foi possível obter as empresas");    
    // }
    //
    // public async Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByUserIdAsync(GetAllCompaniesByUserIdRequest request)
    // {
    //     var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allcompanyidnamebyuserid")
    //     {
    //         Content = JsonContent.Create(request)
    //     };
    //     var httpClient = await GetHttpClientAsync();
    //     var result = await httpClient.SendAsync(requestMessage);
    //     return await result.Content.ReadFromJsonAsync<PagedResponse<List<CompanyIdNameViewModel>>>()
    //            ?? new PagedResponse<List<CompanyIdNameViewModel>>(null, 400, "Não foi possível obter as empresas");    
    // }

    public async Task<PagedResponse<List<CompanyIdNameViewModel>>> GetAllCompanyIdNameByRoleAsync(GetAllCompaniesByUserIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/allcompanyidnamebyrole")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<CompanyIdNameViewModel>>>()
               ?? new PagedResponse<List<CompanyIdNameViewModel>>(null, 400, "Não foi possível obter as empresas"); 
    }

    public async Task<Response<Company?>> GetByIdAsync(GetCompanyByIdRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"v1/companies/byid")
        {
            Content = JsonContent.Create(request)
        };
        var httpClient = await GetHttpClientAsync();
        var result = await httpClient.SendAsync(requestMessage);
        return await result.Content.ReadFromJsonAsync<Response<Company?>>()
               ?? new Response<Company?>(null, 400, "Não foi possível obter a empresa");    
    }
}