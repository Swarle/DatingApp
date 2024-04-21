using System.Collections;
using System.Net;
using AutoMapper;
using DatingApp.BL.DTO.UserDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.UserSpecification;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.BL.Services;

public class AdminService : IAdminService
{
    private readonly IRepository<AppUser> _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public AdminService(IRepository<AppUser> repository, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<IEnumerable<UserWithRolesDto>> GetUsersWithRolesAsync()
    {
        var userSpecification = new UserWithRolesSpecification();

        var users = await _repository.GetAllAsync(userSpecification);

        if (!users.Any())
            throw new HttpException(HttpStatusCode.NotFound);

        var usersDto = _mapper.Map<IEnumerable<UserWithRolesDto>>(users);

        return usersDto;
    }

    public async Task<IEnumerable<string>> EditRolesAsync(string username, string roles)
    {
        var selectedRoles = roles.Split(",").ToArray();

        var user = await _userManager.FindByNameAsync(username) ?? 
                   throw new HttpException(HttpStatusCode.NotFound, "Could not find user");

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest, "Failed to add to roles");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded)
            throw new HttpException(HttpStatusCode.BadRequest, "Failed to remove from roles");

        var rolesDto = await _userManager.GetRolesAsync(user);

        return rolesDto;
    }
}