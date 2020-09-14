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
    [Route("api/taskList")]
    [ApiController]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListsRepo _taskListsRepo;
        private readonly ITasksRepo _tasksRepo;
        private readonly ITagsRepo _tagsRepo;
        private readonly ITaskTagsRepo _taskTagsRepo;
        private readonly IMapper _mapper;

        public TaskListsController
        (
            ITaskListsRepo taskListsRepo, 
            ITasksRepo tasksRepo, 
            ITagsRepo tagsRepo, 
            ITaskTagsRepo taskTagsRepo, 
            IMapper mapper
        )
        {
            _taskListsRepo = taskListsRepo;
            _tasksRepo = tasksRepo;
            _tagsRepo = tagsRepo;
            _taskTagsRepo = taskTagsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskListReadDto>> GetTaskLists()
        {
            IEnumerable<TaskList> taskLists = _taskListsRepo.GetTaskLists();

            List<Tag> tags;
            TaskListReadDto taskListReadDto;
            List<TaskListReadDto> taskListReadDtos = new List<TaskListReadDto>();

            foreach (TaskList taskList in taskLists)
            {
                taskListReadDto = _mapper.Map<TaskListReadDto>(taskList);

                for (int i = 0; i < taskList.Tasks.Count; i++)
                {
                    tags = taskList.Tasks[i].TaskTags.Select(tt => tt.Tag).ToList();
                    taskListReadDto.Tasks[i].Tags = _mapper.Map<List<TagReadDto>>(tags);
                }

                taskListReadDtos.Add(taskListReadDto);
            }

            return Ok(taskListReadDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskListReadDto> GetTaskListById(long id)
        {
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
                return NotFound();

            TaskListReadDto taskListReadDto = _mapper.Map<TaskListReadDto>(taskList);

            List<Tag> tags;

            for (int i = 0; i < taskList.Tasks.Count; i++)
            {
                tags = taskList.Tasks[i].TaskTags.Select(tt => tt.Tag).ToList();
                taskListReadDto.Tasks[i].Tags = _mapper.Map<List<TagReadDto>>(tags);
            }

            return Ok(taskListReadDto);
        }

        [HttpPost]
        public ActionResult<TaskListCreateDto> PostTaskList(TaskListCreateDto taskListCreateDto)
        {
            TaskList taskList = _mapper.Map<TaskList>(taskListCreateDto);

            _taskListsRepo.PostTaskList(taskList);
            _taskListsRepo.SaveChanges();

            Models.Task task;
            Tag tag;
            TaskTag taskTag;

            foreach (TaskListTaskCreateDto taskListTaskCreateDto in taskListCreateDto.Tasks)
            {
                task = _mapper.Map<Models.Task>(taskListTaskCreateDto, opt => opt.AfterMap((src, dest) => dest.TaskListId = taskList.Id));
                _tasksRepo.PostTask(task);
                _tasksRepo.SaveChanges();

                taskTag = new TaskTag()
                {
                    TaskId = task.Id
                };

                foreach (TagCreateDto tagCreateDto in taskListTaskCreateDto.Tags)
                {
                    tag = _mapper.Map<Tag>(tagCreateDto);
                    _tagsRepo.PostTag(tag);
                    _tagsRepo.SaveChanges();
                    taskTag.TagId = tag.Id;
                    _taskTagsRepo.PostTaskTag(taskTag);
                    _taskTagsRepo.SaveChanges();
                }
            }

            TaskListReadDto taskListReadDto = _mapper.Map<TaskListReadDto>(taskList);

            return CreatedAtAction("GetTaskListById", new { id = taskListReadDto.Id }, taskListReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult PutTaskList(long id, TaskListUpdateDto taskListUpdateDto)
        {
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
                return NotFound();

            _mapper.Map(taskListUpdateDto, taskList);
            _taskListsRepo.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTaskList(long id)
        {
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
                return NotFound();

            _taskListsRepo.DeleteTaskList(taskList);
            _taskListsRepo.SaveChanges();

            return NoContent();
        }
    }
}
