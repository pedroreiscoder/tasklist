using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public class TagsRepo : ITagsRepo
    {
        private readonly AppDbContext _context;

        public TagsRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetTags()
        {
            return _context.Tags
                .Include(t => t.TaskTags)
                .ToList();
        }

        public Tag GetTagById(long id)
        {
            return _context.Tags
                .Include(t => t.TaskTags)
                .FirstOrDefault(t => t.Id == id);
        }

        public void PostTag(Tag tag)
        {
            _context.Tags.Add(tag);
        }

        public void DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
