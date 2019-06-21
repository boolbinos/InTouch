using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests.Mappers.Tests
{
    public class PostMapperTest
    {

        public PostMapperTest()
        {
        }

        [Fact]
        public void Mapper_ConvertToPost_should_throw_argument_null_exception_when_input_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => PostMapper.ConvertToPost((PostDTO)null));
        }

        [Fact]
        public void Mapper_ConvertToPostDTO_should_throw_argument_null_exception_when_input_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => PostMapper.ConvertToPostDTO(null));
        }

        [Fact]
        public void Mapper_ConvertToPostCollection_should_throw_argument_null_exception_when_input_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => PostMapper.ConvertToPostCollection(null));
        }

        [Fact]
        public void Mapper_ConvertToPostDTOCollection_should_throw_argument_null_exception_when_input_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => PostMapper.ConvertToPostDTOCollection(null));
        }

        [Fact]
        public void Mapper_should_convert_users_too()
        {
            var postDTO = new PostDTO();
            postDTO.UserDTO = new UserDTO();
            var post = PostMapper.ConvertToPost(postDTO);
            post.User = UserMapper.ConvertToUser(postDTO.UserDTO);
            Assert.NotNull(post.User);
        }


        [Fact]
        public void Mapper_ConverToPostCollection_should_return_posts_with_count_5_if_argument_has_count_5()
        {
            var postsDTO = new List<PostDTO>()
            {
                new PostDTO(),
                new PostDTO(),
                new PostDTO(),
                new PostDTO(),
                new PostDTO()
            };

            var posts = PostMapper.ConvertToPostCollection(postsDTO);

            Assert.Equal(posts.Count, 5);
        }

    }
}
