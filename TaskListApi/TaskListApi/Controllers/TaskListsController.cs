using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TaskListsController> _logger;

        public TaskListsController
        (
            ITaskListsRepo taskListsRepo, 
            ITasksRepo tasksRepo, 
            ITagsRepo tagsRepo, 
            ITaskTagsRepo taskTagsRepo, 
            IMapper mapper,
            ILogger<TaskListsController> logger
        )
        {
            _taskListsRepo = taskListsRepo;
            _tasksRepo = tasksRepo;
            _tagsRepo = tagsRepo;
            _taskTagsRepo = taskTagsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retorna todas as listas de tarefas cadastradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TaskListReadDto>> GetTaskLists()
        {
            _logger.LogInformation("Consultando as listas de tarefas cadastradas");
            IEnumerable<TaskList> taskLists = _taskListsRepo.GetTaskLists();

            List<Tag> tags;
            TaskListReadDto taskListReadDto;
            List<TaskListReadDto> taskListReadDtos = new List<TaskListReadDto>();

            foreach (TaskList taskList in taskLists)
            {
                _logger.LogInformation("Mapeando a lista de tarefas de Id: {id} para a classe de DTO", taskList.Id);
                taskListReadDto = _mapper.Map<TaskListReadDto>(taskList);

                if (taskList.Tasks != null && taskList.Tasks.Count > 0)
                {
                    for (int i = 0; i < taskList.Tasks.Count; i++)
                    {
                        if (taskList.Tasks[i].TaskTags != null && taskList.Tasks[i].TaskTags.Count > 0)
                        {
                            _logger.LogInformation("Mapeando as tags da tarefa de Id: {id} para a classe de DTO", taskList.Tasks[i].Id);
                            tags = taskList.Tasks[i].TaskTags.Select(tt => tt.Tag).ToList();
                            taskListReadDto.Tasks[i].Tags = _mapper.Map<List<TagReadDto>>(tags);
                        }
                    }
                }

                _logger.LogInformation("Adicionando a lista de tarefas de Id: {id} na lista de retorno", taskList.Id);
                taskListReadDtos.Add(taskListReadDto);
            }

            return Ok(taskListReadDtos);
        }

        /// <summary>
        /// Retorna uma lista de tarefas específica.
        /// </summary>
        /// <param name="id">Id da lista de tarefas</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TaskListReadDto> GetTaskListById(long id)
        {
            _logger.LogInformation("Consultando a lista de tarefas de Id: {id}", id);
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
            {
                _logger.LogWarning("A lista de tarefas de Id: {id} não existe", id);
                return NotFound();
            }

            _logger.LogInformation("Mapeando a lista de tarefas de Id: {id} para a classe de DTO", id);
            TaskListReadDto taskListReadDto = _mapper.Map<TaskListReadDto>(taskList);

            if (taskList.Tasks != null && taskList.Tasks.Count > 0)
            {
                List<Tag> tags;

                for (int i = 0; i < taskList.Tasks.Count; i++)
                {
                    if (taskList.Tasks[i].TaskTags != null && taskList.Tasks[i].TaskTags.Count > 0)
                    {
                        _logger.LogInformation("Mapeando as tags da tarefa de Id: {id} para a classe de DTO", taskList.Tasks[i].Id);
                        tags = taskList.Tasks[i].TaskTags.Select(tt => tt.Tag).ToList();
                        taskListReadDto.Tasks[i].Tags = _mapper.Map<List<TagReadDto>>(tags);
                    }
                }
            }

            return Ok(taskListReadDto);
        }

        /// <summary>
        /// Permite a criação de uma lista de tarefas.
        /// </summary>
        /// <param name="taskListCreateDto">Lista de tarefas a ser cadastrada</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TaskListCreateDto> PostTaskList(TaskListCreateDto taskListCreateDto)
        {
            _logger.LogInformation("Mapeando a lista de tarefas a ser cadastrada para a model");
            TaskList taskList = _mapper.Map<TaskList>(taskListCreateDto);

            _logger.LogInformation("Cadastrando a lista de tarefas no banco de dados");
            _taskListsRepo.PostTaskList(taskList);
            _taskListsRepo.SaveChanges();

            if (taskListCreateDto.Tasks != null && taskListCreateDto.Tasks.Count > 0)
            {
                Tag tag;
                TaskTag taskTag;

                for (int i = 0; i < taskListCreateDto.Tasks.Count; i++)
                {
                    if (taskListCreateDto.Tasks[i].Tags != null && taskListCreateDto.Tasks[i].Tags.Count > 0)
                    {
                        taskTag = new TaskTag()
                        {
                            TaskId = taskList.Tasks[i].Id
                        };

                        _logger.LogInformation("Cadastrando as tags da tarefa de Id: {id} no banco de dados", taskList.Tasks[i].Id);
                        foreach (TagCreateDto tagCreateDto in taskListCreateDto.Tasks[i].Tags)
                        {
                            tag = _mapper.Map<Tag>(tagCreateDto);
                            _tagsRepo.PostTag(tag);
                            _tagsRepo.SaveChanges();

                            taskTag.TagId = tag.Id;
                            _taskTagsRepo.PostTaskTag(taskTag);
                            _taskTagsRepo.SaveChanges();
                        }
                    }
                }
            }

            _logger.LogInformation("Montando objeto de retorno");
            TaskList taskListCreated = _taskListsRepo.GetTaskListById(taskList.Id);
            TaskListReadDto taskListReadDto = _mapper.Map<TaskListReadDto>(taskListCreated);

            if (taskListCreated.Tasks != null && taskListCreated.Tasks.Count > 0)
            {
                List<Tag> tags;

                for (int i = 0; i < taskListCreated.Tasks.Count; i++)
                {
                    if (taskListCreated.Tasks[i].TaskTags != null && taskListCreated.Tasks[i].TaskTags.Count > 0)
                    {
                        tags = taskListCreated.Tasks[i].TaskTags.Select(tt => tt.Tag).ToList();
                        taskListReadDto.Tasks[i].Tags = _mapper.Map<List<TagReadDto>>(tags);
                    }
                }
            }

            return CreatedAtAction("GetTaskListById", new { id = taskListReadDto.Id }, taskListReadDto);
        }

        /// <summary>
        /// Permite a edição de uma lista de tarefas.
        /// </summary>
        /// <param name="id">Id da lista de tarefas</param>
        /// <param name="taskListUpdateDto">Lista de tarefas com os novos dados</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutTaskList(long id, TaskListUpdateDto taskListUpdateDto)
        {
            _logger.LogInformation("Consultando a lista de tarefas de Id: {id}", id);
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
            {
                _logger.LogWarning("A lista de tarefas de Id: {id} não existe", id);
                return NotFound();
            }

            _logger.LogInformation("Editando a lista de tarefas de Id: {id}", id);
            _mapper.Map(taskListUpdateDto, taskList);
            _taskListsRepo.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Permite a alteração de uma lista de tarefas.
        /// </summary>
        /// <param name="id">Id da lista de tarefas</param>
        /// <param name="jsonPatchDocument">Dado a ser alterado na forma de JsonPatchDocument</param>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchTaskList(long id, JsonPatchDocument<TaskListUpdateDto> jsonPatchDocument)
        {
            _logger.LogInformation("Consultando a lista de tarefas de Id: {id}", id);
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
            {
                _logger.LogWarning("A lista de tarefas de Id: {id} não existe", id);
                return NotFound();
            }

            _logger.LogInformation("Aplicando a alteração na classe de DTO");
            TaskListUpdateDto taskListUpdateDto = _mapper.Map<TaskListUpdateDto>(taskList);
            jsonPatchDocument.ApplyTo(taskListUpdateDto, ModelState);

            _logger.LogInformation("Validando a alteração feita na classe de DTO");
            if (!TryValidateModel(taskListUpdateDto))
                return ValidationProblem(ModelState);

            _logger.LogInformation("Alterando a lista de tarefas de Id: {id}", id);
            _mapper.Map(taskListUpdateDto, taskList);
            _taskListsRepo.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Permite a remoção de uma lista de tarefas.
        /// </summary>
        /// <param name="id">Id da lista de tarefas</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTaskList(long id)
        {
            _logger.LogInformation("Consultando a lista de tarefas de Id: {id}", id);
            TaskList taskList = _taskListsRepo.GetTaskListById(id);

            if (taskList == null)
            {
                _logger.LogWarning("A lista de tarefas de Id: {id} não existe", id);
                return NotFound();
            }

            _logger.LogInformation("Removendo a lista de tarefas de Id: {id}", id);
            _taskListsRepo.DeleteTaskList(taskList);
            _taskListsRepo.SaveChanges();

            return NoContent();
        }
    }
}
