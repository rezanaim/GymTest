using GymTest.Application.Interfaces.Contexts;
using GymTest.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTest.Common;
using Microsoft.EntityFrameworkCore;

namespace GymTest.Application.Services.Users.Query.GetUser
{
    public interface IGetUsers
    {

        ResultGetUserDto Execute(RequestGetUserDto request);
    }


    public class GetUser : IGetUsers
    {
        /*
        private readonly IDataBaseContext _context;
        public GetUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            const int PageSize = 5;
            IQueryable<User> userQ = _context.Users
                .Include(u => u.UserInRoles)
                .ThenInclude(u => u.Role);


            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                userQ = userQ.Where(p =>
                p.FirstName.Contains(request.SearchKey) ||
                p.LastName.Contains(request.SearchKey) ||
                p.Email.Contains(request.SearchKey));
            }

            int rowsCount = 0;
            var users = userQ.OrderByDescending(p => p.Id)
                .ToPaged(request.PageNumber, PageSize, out rowsCount);


            var midList = users.ToList();

            List<GetUserDto> userList = new List<GetUserDto>();
            
            foreach (var i in midList)
            {
                userList.Add(new GetUserDto()
                {
                    UserId = i.Id,
                    Email = i.Email,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    IsActive = i.IsActive,
                    RegisterTime = i.InsertTime,
                    Roles = i.UserInRoles.Select(p => p.Role.Name).ToList()


                }



                    ); ;

            }
            


            return null;
            }
            

        */
        
        private readonly IDataBaseContext _context;
        public GetUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            const int pageSize = 5;
            IQueryable<User> usersQ = _context.Users
                .Include(t => t.UserInRoles)
                .ThenInclude(t => t.Role);


            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                usersQ = usersQ.Where(u =>
                u.FirstName.Contains(request.SearchKey) ||
                u.LastName.Contains(request.SearchKey) ||
                u.Email.Contains(request.SearchKey)

                );
            }

            if (request.IsActive.HasValue)
            {
                usersQ = usersQ.Where(u => u.IsActive == request.IsActive);
            }

            //int rowsCount = 0;
            var userList = usersQ.ToPaged(request.PageNumber, pageSize, out int rowsCount)
            .Select(p => new GetUserDto()
            {
                UserId = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                IsActive = p.IsActive,
                RegisterTime = p.InsertTime,
                Roles = p.UserInRoles.Select(r => r.Role.Name).ToList()
            }).ToList();



            var result = new ResultGetUserDto()
            {
                Users = userList,
                TotalRows = rowsCount,

            };
            return (result);

        }


        // اگه نخوایم از پیجینیشن تو کامن استفاده کنیم:

        /*
        const int pageSize = 5;
        IQueryable<User> usersQ = _context.Users;
        if (!string.IsNullOrWhiteSpace(request.SearchKey))

        {

            usersQ = usersQ .Where(u =>
            u.Email.Contains(request.SearchKey) ||
            u.FirstName.Contains(request.SearchKey) ||
            u.LastName.Contains(request.SearchKey)
            );
        }

        int totalRows = usersQ.Count();
        var pageCount = (int)Math.Ceiling(totalRows / (double)pageSize);
        var user = usersQ.OrderByDescending(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new GetUserDto()
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                RegisterTime = u.InsertTime,

            })
            .ToList();

            و نهایتا ریترن میکردیم  users رو
        }
         */






        //throw new NotImplementedException();
    }
    }


    public class GetUserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegisterTime { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }

    }
    public class RequestGetUserDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ResultGetUserDto
    {
        public List<GetUserDto> Users { get; set; }
        public int TotalRows { get; set; }
        //public int PageCount { get; set; }

    }
