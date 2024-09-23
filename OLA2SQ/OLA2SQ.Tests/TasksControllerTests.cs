using Microsoft.AspNetCore.Mvc;
using OLA2SQ.Controllers;
using OLA2SQ.Models;
using System.Collections.Generic;
using Xunit;

namespace OLA2SQ.Tests
{
    public class TasksControllerTests
    {
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _controller = new TasksController();
        }

        [Fact]
        public void GetTasks_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tasks = Assert.IsType<List<ToDoTask>>(okResult.Value);
            Assert.Empty(tasks);
        }

        [Fact]
        public void CreateTask_ValidTask_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var task = new ToDoTask
            {
                Title = "Test Task",
                IsCompleted = false,
                DueDate = System.DateTime.Now
            };

            // Act
            var result = _controller.CreateTask(task);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.IsType<ToDoTask>(createdResult.Value);
        }

        [Fact]
        public void UpdateTask_ValidId_ReturnsNoContent()
        {
            // Arrange
            var task = new ToDoTask
            {
                Title = "Test Task",
                IsCompleted = false,
                DueDate = System.DateTime.Now
            };
            var createdTaskResult = _controller.CreateTask(task).Result as CreatedAtActionResult;
            var createdTask = createdTaskResult.Value as ToDoTask;

            // Act
            createdTask.Title = "Updated Task";
            var result = _controller.UpdateTask(createdTask.Id, createdTask);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteTask_ValidId_ReturnsNoContent()
        {
            // Arrange
            var task = new ToDoTask
            {
                Title = "Test Task",
                Description = "Test Description",
                IsCompleted = false,
                DueDate = System.DateTime.Now
            };
            _controller.CreateTask(task); // Create a task to delete

            // Act
            var result = _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}