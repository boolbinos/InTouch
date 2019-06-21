using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.WebSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.Mappers
{
    public class UserMapper
    {

        public static UserViewModel ConvertToUserViewModel(UserDTO userDTO)
        {
            if(userDTO == null)
            {
                throw new ArgumentNullException();
            }

            var userViewModel = new UserViewModel()
            {
                Id = userDTO.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                RoleName = userDTO.RoleDTO.Name,
                Avatar=userDTO.Avatar
            };

            return userViewModel;
        }

        public static List<UserViewModel> ConvertToUserViewModelCollection(IEnumerable<UserDTO> usersDTO)
        {
            if(usersDTO == null)
            {
                throw new ArgumentException();
            }

            List<UserViewModel> usersViewModel = new List<UserViewModel>();

            foreach(var userDTO in usersDTO)
            {
                usersViewModel.Add(ConvertToUserViewModel(userDTO));
            }

            return usersViewModel;
        }


        public static UserLikeViewModel ConvertToUserLikeViewModel(UserDTO userDTO)
        {
            if (userDTO==null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }

            var user = new UserLikeViewModel()
            {
                Id = userDTO.Id,
                FullName = userDTO.FirstName + ' ' + userDTO.LastName
            };

            return user;
        }
    }
}
