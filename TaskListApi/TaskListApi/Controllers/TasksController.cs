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
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepo _tasksRepo;
        private readonly ITaskListsRepo _taskListsRepo;
        private readonly ITagsRepo _tagsRepo;
        private readonly ITaskTagsRepo _taskTagsRepo;
        private readonly IMapper _mapper;

        public TasksController
        (
            ITasksRepo tasksRepo,
            ITaskListsRepo taskListsRepo,
            ITagsRepo tagsRepo,
            ITaskTagsRepo taskTagsRepo,
            IMapper mapper
        )
        {
            _tasksRepo = tasksRepo;
            _taskListsRepo = taskListsRepo;
            _tagsRepo = tagsRepo;
            _taskTagsRepo = taskTagsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskReadDto>> GetTasks(long taskListId)
        {
            TaskList taskList = _taskListsRepo.GetTaskListById(taskListId);

            if (taskList == null)
                return NotFound();

            IEnumerable<Models.Task> tasks = _tasksRepo.GetTasks(taskListId);

            List<Tag> tags;
            TaskReadDto taskDto;
            List<TaskReadDto> taskDtos = new List<TaskReadDto>();

            foreach (Models.Task task in tasks)
            {
                taskDto = _mapper.Map<TaskReadDto>(task);

                if (task.TaskTags != null && task.TaskTags.Count > 0)
                {
                    tags = task.TaskTags.Select(tt => tt.Tag).ToList();
                    taskDto.Tags = _mapper.Map<List<TagReadDto>>(tags);
                }
                
                taskDtos.Add(taskDto);
            }

            return Ok(taskDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskReadDto> GetTaskById(long id)
        {
            Models.Task task = _tasksRepo.GetTaskById(id);

            if (task == null)
                return NotFound();

            TaskReadDto taskReadDto = _mapper.Map<TaskReadDto>(task);

            if (task.TaskTags != null && task.TaskTags.Count > 0)
            {
                List<Tag> tags = task.TaskTags.Select(tt => tt.Tag).ToList();
                taskReadDto.Tags = _mapper.Map<List<TagReadDto>>(tags);
            }

            return Ok(taskReadDto);
        }

        [HttpPost]
        public ActionResult<TaskCreateDto> PostTask(TaskCreateDto taskCreateDto)
        {
            TaskList taskList = _taskListsRepo.GetTaskListById(taskCreateDto.TaskListId);

            if (taskList == null)
                return NotFound();

            Models.Task task = _mapper.Map<Models.Task>(taskCreateDto);

            _tasksRepo.PostTask(task);
            _tasksRepo.SaveChanges();

            if (taskCreateDto.Tags != null && taskCreateDto.Tags.Count > 0)
            {
                Tag tag;
                TaskTag taskTag = new TaskTag()
                {
                    TaskId = task.Id
                };

                foreach (TagCreateDto tagCreateDto in taskCreateDto.Tags)
                {
                    tag = _mapper.Map<Tag>(tagCreateDto);
                    _tagsRepo.PostTag(tag);
                    _tagsRepo.SaveChanges();

                    taskTag.TagId = tag.Id;
                    _taskTagsRepo.PostTaskTag(taskTag);
                    _taskTagsRepo.SaveChanges();
                }
            }

            Models.Task taskCreated = _tasksRepo.GetTaskById(task.Id);
            TaskReadDto taskReadDto = _mapper.Map<TaskReadDto>(taskCreated);

            if (taskCreated.TaskTags != null && taskCreated.TaskTags.Count > 0)
            {
                List<Tag> tags = taskCreated.TaskTags.Select(tt => tt.Tag).ToList();
                taskReadDto.Tags = _mapper.Map<List<TagReadDto>>(tags);
            }

            return CreatedAtAction("GetTaskById", new { id = taskReadDto.Id }, taskReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult PutTask(long id, TaskUpdateDto taskUpdateDto)
        {
            Models.Task task = _tasksRepo.GetTaskById(id);

            if (task == null)
                return NotFound();

            _mapper.Map(taskUpdateDto, task);
            _tasksRepo.SaveChanges();

            if (task.TaskTags != null && task.TaskTags.Count > 0)
            {
                _taskTagsRepo.DeleteTaskTags(task.TaskTags);
                _taskTagsRepo.SaveChanges();
            }

            if (taskUpdateDto.Tags != null && taskUpdateDto.Tags.Count > 0)
            {
                Tag tag;
                TaskTag taskTag = new TaskTag()
                {
                    TaskId = task.Id
                };

                foreach (TagUpdateDto tagUpdateDto in taskUpdateDto.Tags)
                {
                    tag = _mapper.Map<Tag>(tagUpdateDto);
                    _tagsRepo.PostTag(tag);
                    _tagsRepo.SaveChanges();

                    taskTag.TagId = tag.Id;
                    _taskTagsRepo.PostTaskTag(taskTag);
                    _taskTagsRepo.SaveChanges();
                }
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchTask(long id, JsonPatchDocument<TaskUpdateDto> jsonPatchDocument)
        {
            Models.Task task = _tasksRepo.GetTaskById(id);

            if (task == null)
                return NotFound();

            TaskUpdateDto taskUpdateDto = _mapper.Map<TaskUpdateDto>(task);
            jsonPatchDocument.ApplyTo(taskUpdateDto, ModelState);

            if (!TryValidateModel(taskUpdateDto))
                return ValidationProblem(ModelState);

            _mapper.Map(taskUpdateDto, task);
            _tasksRepo.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(long id)
        {
            Models.Task task = _tasksRepo.GetTaskById(id);

            if (task == null)
                return NotFound();

            _tasksRepo.DeleteTask(task);
            _tasksRepo.SaveChanges();

            return NoContent();
        }
    }
}
