using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TaskListApi.Data;
using TaskListApi.Dtos;
using TaskListApi.Models;

namespace TaskListApi.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsRepo _repository;
        private readonly IMapper _mapper;

        public TagsController(ITagsRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TagReadDto>> GetTags()
        {
            IEnumerable<Tag> tags = _repository.GetTags();

            return Ok(_mapper.Map<IEnumerable<TagReadDto>>(tags));
        }

        [HttpPut("{id}")]
        public ActionResult PutTag(long id, TagUpdateDto tagUpdateDto)
        {
            Tag tag = _repository.GetTagById(id);

            if (tag == null)
                return NotFound();

            _mapper.Map(tagUpdateDto, tag);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchTag(long id, JsonPatchDocument<TagUpdateDto> jsonPatchDocument)
        {
            Tag tag = _repository.GetTagById(id);

            if (tag == null)
                return NotFound();

            TagUpdateDto tagUpdateDto = _mapper.Map<TagUpdateDto>(tag);
            jsonPatchDocument.ApplyTo(tagUpdateDto, ModelState);

            if (!TryValidateModel(tagUpdateDto))
                return ValidationProblem(ModelState);

            _mapper.Map(tagUpdateDto, tag);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTag(long id)
        {
            Tag tag = _repository.GetTagById(id);

            if (tag == null)
                return NotFound();

            _repository.DeleteTag(tag);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
