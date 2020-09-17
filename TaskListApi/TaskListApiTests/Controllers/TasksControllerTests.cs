using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskListApi.Controllers;
using TaskListApi.Data;
using TaskListApi.Dtos;
using TaskListApi.Models;
using Xunit;

namespace TaskListApiTests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITasksRepo> _mockTasksRepo;
        private readonly Mock<ITaskListsRepo> _mockTaskListsRepo;
        private readonly Mock<ITagsRepo> _mockTagsRepo;
        private readonly Mock<ITaskTagsRepo> _mockTaskTagsRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TasksController>> _mockLogger;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockTasksRepo = new Mock<ITasksRepo>();
            _mockTaskListsRepo = new Mock<ITaskListsRepo>();
            _mockTagsRepo = new Mock<ITagsRepo>();
            _mockTaskTagsRepo = new Mock<ITaskTagsRepo>();
            _mapper = Mapper.Configuration.CreateMapper();
            _mockLogger = new Mock<ILogger<TasksController>>();
            _controller = new TasksController(_mockTasksRepo.Object, 
                                              _mockTaskListsRepo.Object, 
                                              _mockTagsRepo.Object, 
                                              _mockTaskTagsRepo.Object, 
                                              _mapper, 
                                              _mockLogger.Object);
        }

        [Fact]
        public void GetTasks_UnknownTaskListIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            var notFoundResult = _controller.GetTasks(5);

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTasks_ExistingTaskListIdPassed_ReturnsOk()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());
            _mockTasksRepo.Setup(r => r.GetTasks(It.IsAny<long>())).Returns(new List<Task>() { new Task(), new Task() });

            var okResult = _controller.GetTasks(5);

            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTasks_ExistingTaskListIdPassed_ReturnsAllTasks()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());
            _mockTasksRepo.Setup(r => r.GetTasks(It.IsAny<long>())).Returns(new List<Task>() { new Task(), new Task() });

            var okResult = _controller.GetTasks(5).Result as OkObjectResult;

            var tasks = Assert.IsAssignableFrom<IEnumerable<TaskReadDto>>(okResult.Value);

            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public void GetTaskById_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns((Task)null);

            var notFoundResult = _controller.GetTaskById(5);

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTaskById_ExistingIdPassed_ReturnsOk()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task() { Id = 5 });

            var okResult = _controller.GetTaskById(5);

            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTaskById_ExistingIdPassed_ReturnsTask()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task() { Id = 5 });

            var okResult = _controller.GetTaskById(5).Result as OkObjectResult;

            var task = Assert.IsType<TaskReadDto>(okResult.Value);

            Assert.Equal(5, task.Id);
        }

        [Fact]
        public void PostTask_UnknownTaskListIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            var notFoundResult = _controller.PostTask(new TaskCreateDto() { TaskListId = 5 });

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostTask_ExistingTaskListIdPassed_ReturnsCreatedResponse()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task() { Id = 5 });

            var createdResponse = _controller.PostTask(new TaskCreateDto() { TaskListId = 5 });

            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostTask_ExistingTaskListIdPassed_ReturnedResponseHasCreatedTask()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task() { Id = 5, Title = "Fazer teste técnico" });

            var createdResponse = _controller.PostTask(new TaskCreateDto() { Title = "Fazer teste técnico", TaskListId = 5 }).Result as CreatedAtActionResult;

            var task = Assert.IsType<TaskReadDto>(createdResponse.Value);
            Assert.Equal("Fazer teste técnico", task.Title);
        }

        [Fact]
        public void PutTask_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns((Task)null);

            TaskUpdateDto taskUpdateDto = new TaskUpdateDto()
            {
                Title = "Fazer teste técnico"
            };

            var notFoundResult = _controller.PutTask(5, taskUpdateDto);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PutTask_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task());

            TaskUpdateDto taskUpdateDto = new TaskUpdateDto()
            {
                Title = "Fazer teste técnico"
            };

            var noContentResult = _controller.PutTask(5, taskUpdateDto);

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void PatchTask_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns((Task)null);

            var notFoundResult = _controller.PatchTask(5, new JsonPatchDocument<TaskUpdateDto>());

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PatchTask_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task());

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                                  It.IsAny<ValidationStateDictionary>(),
                                                  It.IsAny<string>(),
                                                  It.IsAny<object>()));

            _controller.ObjectValidator = objectValidator.Object;

            var noContentResult = _controller.PatchTask(5, new JsonPatchDocument<TaskUpdateDto>());

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void DeleteTask_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns((Task)null);

            var notFoundResult = _controller.DeleteTask(5);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteTask_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTasksRepo.Setup(r => r.GetTaskById(It.IsAny<long>())).Returns(new Task());

            var noContentResult = _controller.DeleteTask(5);

            Assert.IsType<NoContentResult>(noContentResult);
        }
    }
}
