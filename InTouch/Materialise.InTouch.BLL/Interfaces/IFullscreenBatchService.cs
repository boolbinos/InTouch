using System.Collections.Generic;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IFullscreenBatchService
    {
        Task<List<FullscreenPostPartDTO>> CreateBatchAsync(List<PostDTO> posts);
        IEnumerable<FullscreenPostPartDTO> GetPostPartsWithImages(PostDTO post);
        FullscreenPostPartDTO GetPostPartWithVideo(PostDTO post);
        List<FullscreenPostPartDTO> SplitPost(PostDTO post);
    }
}