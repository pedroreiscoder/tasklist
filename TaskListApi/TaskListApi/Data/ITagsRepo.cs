using System.Collections.Generic;
using TaskListApi.Models;

namespace TaskListApi.Data
{
    public interface ITagsRepo
    {
        IEnumerable<Tag> GetTags();
        Tag GetTagById(long id);
        Tag PostTag(Tag tag);
        void PutTag(long id, Tag tag);
        Tag DeleteTag(long id);
    }
}
