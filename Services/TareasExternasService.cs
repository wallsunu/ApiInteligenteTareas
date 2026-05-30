using System.Net;
using System.Net.Http.Json;
using ApiInteligenteTareas.DTOs;

namespace ApiInteligenteTareas.Services;

public class TareasExternasService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://jsonplaceholder.typicode.com/todos";

    public TareasExternasService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TareaExternaDto>> ObtenerTodasAsync()
    {
        var todos = await _httpClient.GetFromJsonAsync<List<JsonPlaceholderTodoDto>>(BaseUrl)
            ?? throw new HttpRequestException("La API externa no devolvió datos.");

        return todos.Select(Mapear).ToList();
    }

    public async Task<TareaExternaDto?> ObtenerPorIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Error al consultar la API externa. Código: {(int)response.StatusCode}.");

        var todo = await response.Content.ReadFromJsonAsync<JsonPlaceholderTodoDto>();
        return todo is null ? null : Mapear(todo);
    }

    private static TareaExternaDto Mapear(JsonPlaceholderTodoDto todo) => new()
    {
        ExternalId = todo.Id,
        Titulo = todo.Title,
        Completado = todo.Completed
    };
}
