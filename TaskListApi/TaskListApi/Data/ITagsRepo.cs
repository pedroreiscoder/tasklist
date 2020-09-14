using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITagsRepo
    {
        IEnumerable<Tag> GetTags();
        Tag GetTagById(long id);
        void PostTag(Tag tag);
        void DeleteTag(Tag tag);
        void SaveChanges();
    }
}
