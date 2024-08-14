﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class UsersListViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public string Image { get; set; }
        public string RegisterCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FirstName { get; set; }
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string LastName { get; set; }
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }
        [DisplayName("ایمیل")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }
        [DisplayName("آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }
        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PhoneNumber { get; set; }
        [DisplayName("تاریخ تولد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime BirthDate { get; set; }
        [DisplayName("تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile File { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }
    }

    public class AddUserToRoleViewModel
    {
        #region Constructure

        public AddUserToRoleViewModel()
        {
            UserRoles = new List<UserRolesViewModel>();
        }

        public AddUserToRoleViewModel(string userId, IList<UserRolesViewModel> userRoles)
        {
            UserId = userId;
            UserRoles = userRoles;
        }

        #endregion

        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }

    public class UserRolesViewModel
    {
        #region Constructor

        public UserRolesViewModel()
        {
            
        }

        public UserRolesViewModel(string roleName)
        {
            RoleName = roleName;
        }

        #endregion

        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}