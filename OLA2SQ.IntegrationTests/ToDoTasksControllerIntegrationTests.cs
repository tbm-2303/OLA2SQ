using Microsoft.AspNetCore.Mvc.Testing;
using OLA2SQ; 
using Xunit;
using System.Net.Http;
using OLA2SQ.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

public class ToDoTasksControllerIntegrationTests : IClassFixture<WebApplicationFactory<OLA2SQ.Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<OLA2SQ.Program> _factory;

    public ToDoTasksControllerIntegrationTests(WebApplicationFactory<OLA2SQ.Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        SeedData();
    }

    private void SeedData()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TodoContext>();

            // Clear existing tasks
            context.TodoTasks.RemoveRange(context.TodoTasks);
            context.SaveChanges();

            // Seed the data
            context.TodoTasks.AddRange(new List<ToDoTask>
        {
            new ToDoTask { Id = 1, Title = "Task 1", IsCompleted = false, DueDate = DateTime.UtcNow.AddDays(1) },
            new ToDoTask { Id = 2, Title = "Task 2", IsCompleted = true, DueDate = DateTime.UtcNow.AddDays(2) },
        });
            context.SaveChanges();
        }
    }


    [Fact]
    public async Task GetAllTasks_ReturnsOk_WhenTasksExist()
    {
        // Arrange
        var response = await _client.GetAsync("/api/ToDoTasks");

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var responseString = await response.Content.ReadAsStringAsync();
        // You can deserialize the responseString to verify the content if needed
    }

    [Fact]
    public async Task AddTask_ReturnsCreated()
    {
        // Arrange
        var newTask = new ToDoTask { Title = "New Task", IsCompleted = false, DueDate = DateTime.UtcNow.AddDays(1) };
        var response = await _client.PostAsJsonAsync("/api/ToDoTasks", newTask);

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var createdTask = await response.Content.ReadAsAsync<ToDoTask>();

        Assert.Equal("New Task", createdTask.Title);
    }

    [Fact]
    public async Task UpdateTask_ReturnsNoContent_WhenTaskExists()
    {
        // Arrange
        var existingTask = new ToDoTask { Id = 1, Title = "Updated Task", IsCompleted = true, DueDate = DateTime.UtcNow.AddDays(1) };
        var response = await _client.PutAsJsonAsync($"/api/ToDoTasks/{existingTask.Id}", existingTask);

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 204 No Content
    }

    [Fact]
    public async Task DeleteTask_ReturnsNoContent_WhenTaskExists()
    {
        // Arrange
        var response = await _client.DeleteAsync("/api/ToDoTasks/1"); // Assuming a task with ID 1 exists

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 204 No Content
    }
}
