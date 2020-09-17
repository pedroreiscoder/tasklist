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
    //Relatório de cobertura disponível em Coverage/index.html
    public class TaskListsControllerTests
    {
        private readonly Mock<ITaskListsRepo> _mockTaskListsRepo;
        private readonly Mock<ITagsRepo> _mockTagsRepo;
        private readonly Mock<ITaskTagsRepo> _mockTaskTagsRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TaskListsController>> _mockLogger;
        private readonly TaskListsController _controller;

        public TaskListsControllerTests()
        {
            _mockTaskListsRepo = new Mock<ITaskListsRepo>();
            _mockTagsRepo = new Mock<ITagsRepo>();
            _mockTaskTagsRepo = new Mock<ITaskTagsRepo>();
            _mapper = Mapper.Configuration.CreateMapper();
            _mockLogger = new Mock<ILogger<TaskListsController>>();
            _controller = new TaskListsController(_mockTaskListsRepo.Object, 
                                                  _mockTagsRepo.Object, 
                                                  _mockTaskTagsRepo.Object, 
                                                  _mapper, 
                                                  _mockLogger.Object);
        }

        [Fact]
        public void GetTaskLists_WhenCalled_ReturnsOk()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskLists()).Returns(new List<TaskList>() { new TaskList(), new TaskList() });

            var okResult = _controller.GetTaskLists();

            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTaskLists_WhenCalled_ReturnsAllTaskLists()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskLists()).Returns(new List<TaskList>() { new TaskList(), new TaskList() });

            var okResult = _controller.GetTaskLists().Result as OkObjectResult;

            var taskLists = Assert.IsAssignableFrom<IEnumerable<TaskListReadDto>>(okResult.Value);

            Assert.Equal(2, taskLists.Count());
        }

        [Fact]
        public void GetTaskListById_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            var notFoundResult = _controller.GetTaskListById(5);

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetTaskListById_ExistingIdPassed_ReturnsOk()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());

            var okResult = _controller.GetTaskListById(5);

            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTaskListById_ExistingIdPassed_ReturnsTaskList()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList() { Id = 5 });

            var okResult = _controller.GetTaskListById(5).Result as OkObjectResult;

            var taskList = Assert.IsType<TaskListReadDto>(okResult.Value);

            Assert.Equal(5, taskList.Id);
        }

        [Fact]
        public void PostTaskList_ValidObjectPassed_ReturnsCreatedResponse()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList() { Id = 5 });

            var createdResponse = _controller.PostTaskList(new TaskListCreateDto());

            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostTaskList_ValidObjectPassed_ReturnedResponseHasCreatedTaskList()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList() { Id = 5, Name = "Lista do Pedro" });

            var createdResponse = _controller.PostTaskList(new TaskListCreateDto() { Name = "Lista do Pedro" }).Result as CreatedAtActionResult;

            var taskList = Assert.IsType<TaskListReadDto>(createdResponse.Value);

            Assert.Equal("Lista do Pedro", taskList.Name);
        }

        [Fact]
        public void PutTaskList_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            TaskListUpdateDto taskListUpdateDto = new TaskListUpdateDto()
            {
                Name = "Lista do Daniel"
            };

            var notFoundResult = _controller.PutTaskList(5, taskListUpdateDto);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PutTaskList_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());

            TaskListUpdateDto taskListUpdateDto = new TaskListUpdateDto()
            {
                Name = "Lista do Daniel"
            };

            var noContentResult = _controller.PutTaskList(5, taskListUpdateDto);

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void PatchTaskList_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            var notFoundResult = _controller.PatchTaskList(5, new JsonPatchDocument<TaskListUpdateDto>());

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PatchTaskList_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                                  It.IsAny<ValidationStateDictionary>(),
                                                  It.IsAny<string>(),
                                                  It.IsAny<object>()));

            _controller.ObjectValidator = objectValidator.Object;

            var noContentResult = _controller.PatchTaskList(5, new JsonPatchDocument<TaskListUpdateDto>());

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void DeleteTaskList_UnknownIdPassed_ReturnsNotFound()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns((TaskList)null);

            var notFoundResult = _controller.DeleteTaskList(5);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteTaskList_ExistingIdPassed_ReturnsNoContent()
        {
            _mockTaskListsRepo.Setup(r => r.GetTaskListById(It.IsAny<long>())).Returns(new TaskList());

            var noContentResult = _controller.DeleteTaskList(5);

            Assert.IsType<NoContentResult>(noContentResult);
        }
    }
}
