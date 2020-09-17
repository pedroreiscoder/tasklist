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
    public class TagsControllerTests
    {
        private readonly Mock<ITagsRepo> _mockRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TagsController>> _mockLogger;
        private readonly TagsController _controller;

        public TagsControllerTests()
        {
            _mockRepository = new Mock<ITagsRepo>();
            _mapper = Mapper.Configuration.CreateMapper();
            _mockLogger = new Mock<ILogger<TagsController>>();
            _controller = new TagsController(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public void GetTags_WhenCalled_ReturnsOk()
        {
            var okResult = _controller.GetTags();

            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetTags_WhenCalled_ReturnsAllTags()
        {
            _mockRepository.Setup(r => r.GetTags()).Returns(new List<Tag>() { new Tag(), new Tag() });

            var okResult = _controller.GetTags().Result as OkObjectResult;

            var tags = Assert.IsAssignableFrom<IEnumerable<TagReadDto>>(okResult.Value);

            Assert.Equal(2, tags.Count());
        }

        [Fact]
        public void PutTag_UnknownIdPassed_ReturnsNotFound()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns((Tag)null);

            TagUpdateDto tagUpdateDto = new TagUpdateDto()
            {
                Name = "estudos"
            };

            var notFoundResult = _controller.PutTag(5, tagUpdateDto);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PutTag_ValidIdPassed_ReturnsNoContent()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns(new Tag());

            TagUpdateDto tagUpdateDto = new TagUpdateDto()
            {
                Name = "estudos"
            };

            var noContentResult = _controller.PutTag(5, tagUpdateDto);

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void PatchTag_UnknownIdPassed_ReturnsNotFound()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns((Tag)null);

            var notFoundResult = _controller.PatchTag(5, new JsonPatchDocument<TagUpdateDto>());

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void PatchTag_ValidIdPassed_ReturnsNoContent()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns(new Tag());

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(), 
                                                  It.IsAny<ValidationStateDictionary>(), 
                                                  It.IsAny<string>(), 
                                                  It.IsAny<object>()));

            _controller.ObjectValidator = objectValidator.Object;

            var noContentResult = _controller.PatchTag(5, new JsonPatchDocument<TagUpdateDto>());

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public void DeleteTag_UnknownIdPassed_ReturnsNotFound()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns((Tag)null);

            var notFoundResult = _controller.DeleteTag(5);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void DeleteTag_ValidIdPassed_ReturnsNoContent()
        {
            _mockRepository.Setup(r => r.GetTagById(It.IsAny<long>())).Returns(new Tag());

            var noContentResult = _controller.DeleteTag(5);

            Assert.IsType<NoContentResult>(noContentResult);
        }
    }
}
