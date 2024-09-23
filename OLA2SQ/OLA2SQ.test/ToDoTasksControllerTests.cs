using Xunit;
using Microsoft.EntityFrameworkCore;
using OLA2SQ.Controllers;
using OLA2SQ.Models;

public class ToDoTasksControllerTests
{
    private readonly ToDoTasksController _controller;
    private readonly TodoContext _context;

    public ToDoTasksControllerTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TodoContext(options);
        _controller = new ToDoTasksController(_context);
    }



    [Fact]   //Test Adding a Task
    public async Task AddTask_ShouldIncreaseCount_WhenTaskIsAdded()
    {
        // Arrange
        var task = new ToDoTask { Title = "New Task", IsCompleted = false, DueDate = DateTime.Now.AddDays(1) };

        // Act
        await _controller.PostToDoTask(task);

        // Assert
        var tasks = await _context.TodoTasks.ToListAsync();
        Assert.Single(tasks); // Ensure only one task is present
    }
 

    [Fact]   //Test Removing a Task
    public async Task RemoveTask_ShouldDecreaseCount_WhenTaskIsRemoved()
    {
        // Arrange
        var task = new ToDoTask { Title = "Task to Remove", IsCompleted = false, DueDate = DateTime.Now.AddDays(1) };
        await _controller.PostToDoTask(task);

        // Act
        await _controller.DeleteToDoTask(task.Id);

        // Assert
        var tasks = await _context.TodoTasks.ToListAsync();
        Assert.Empty(tasks); // Ensure no tasks are present
    }

    [Fact]   //Test Retrieving a Task
    public async Task GetToDoTask_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new ToDoTask { Title = "Existing Task", IsCompleted = false, DueDate = DateTime.Now.AddDays(1) };
        await _controller.PostToDoTask(task);

        // Act
        var result = await _controller.GetToDoTask(task.Id);

        // Assert
        Assert.NotNull(result.Value); // Ensure the task is not null
        Assert.Equal("Existing Task", result.Value.Title);
    }

    [Fact]
    public async Task PutToDoTask_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var task = new ToDoTask { Title = "Old Task", IsCompleted = false, DueDate = DateTime.Now.AddDays(1) };
        await _controller.PostToDoTask(task);

        // Fetch the existing task from the context
        var existingTask = await _context.TodoTasks.FindAsync(task.Id);

        // Act: Update properties of the existing task
        existingTask.Title = "Updated Task";
        existingTask.IsCompleted = true;
        existingTask.DueDate = DateTime.Now.AddDays(2);

        await _controller.PutToDoTask(existingTask.Id, existingTask); // Pass the updated existingTask

        // Assert
        var result = await _controller.GetToDoTask(existingTask.Id);
        Assert.Equal("Updated Task", result.Value.Title);
        Assert.True(result.Value.IsCompleted);
    }
}