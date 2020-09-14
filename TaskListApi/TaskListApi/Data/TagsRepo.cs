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
            return _context.Tags.ToList();
        }

        public Tag GetTagById(long id)
        {
            return _context.Tags.Find(id);
        }

        public void PostTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _context.Tags.Add(tag);
        }

        public void DeleteTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _context.Tags.Remove(tag);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
