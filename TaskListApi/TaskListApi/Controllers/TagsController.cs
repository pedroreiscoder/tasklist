using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public ActionResult<TagReadDto> GetTagById(long id)
        {
            Tag tag = _repository.GetTagById(id);

            if (tag == null)
                return NotFound();

            return Ok(_mapper.Map<TagReadDto>(tag));
        }

        [HttpPost]
        public ActionResult<TagCreateDto> PostTag(TagCreateDto tagCreateDto)
        {
            Tag tag = _mapper.Map<Tag>(tagCreateDto);

            _repository.PostTag(tag);
            _repository.SaveChanges();

            TagReadDto tagReadDto = _mapper.Map<TagReadDto>(tag);

            return CreatedAtAction("GetTagById", new { id = tagReadDto.Id }, tagReadDto);
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
