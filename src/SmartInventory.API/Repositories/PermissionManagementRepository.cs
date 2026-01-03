using SmartInventory.API.Data;
using SmartInventory.API.Domain.DTO;
using SmartInventory.API.Domain.Models;

namespace SmartInventory.API.Repositories;

/// <summary>
/// Purpose: Defines the functionality for creating, viewing and updating permissions
/// and roles, as well as assigning permissions and roles to users.
/// </summary>
public class PermissionManagementRepository(DatabaseContext context)
{
    /// <summary>
    /// Purpose: Connects the API with the database.
    /// </summary>
    private readonly DatabaseContext _context = context;

    /// <summary>
    /// Purpose: Fetches a role by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public RoleDto? GetRoleByName(string name)
    {
        if (!string.IsNullOrEmpty(name) && _context.Roles.FirstOrDefault(r => r.Name == name) is Role role)
        {
            return new RoleDto()
            {
                Id = role.Id
                ,
                Name = role.Name
                ,
                IsActive = role.IsActive
            };
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches a role by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoleDto? GetRoleById(int id)
    {
        if (id >= 0 && _context.Roles.FirstOrDefault(r => r.Id == id) is Role role)
        {
            return new RoleDto()
            {
                Id = role.Id
                ,
                Name = role.Name
                ,
                IsActive = role.IsActive                
            };
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches a permission by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PermissionDto? GetPermissionByName(string name)
    {
        if (!string.IsNullOrEmpty(name) && _context.Permissions.FirstOrDefault(r => r.Name == name) is Permission perm)
        {
            return new PermissionDto()
            {
                Id = perm.Id
                ,
                Name = perm.Name
                ,
                IsActive = perm.IsActive
                ,
                Description = perm.Description
            };
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches a permission by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public PermissionDto? GetPermissionById(int id)
    {
        if (id >= 0 && _context.Permissions.FirstOrDefault(r => r.Id == id) is Permission perm)
        {
            return new PermissionDto()
            {
                Id = perm.Id
                ,
                Name = perm.Name
                ,
                IsActive = perm.IsActive
                ,
                Description = perm.Description
            };
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches all active permissions.
    /// </summary>
    /// <returns></returns>
    public List<PermissionDto>? GetActivePermissions()
    {
        List<Permission> permissions = [.. from permission in _context.Permissions
                                        where permission.IsActive == true
                                        select permission];

        if (permissions.Count > 0)
        {
            List<PermissionDto> permissionDtos = [];

            foreach (var perm in permissions)
            {
                permissionDtos.Add(new()
                {
                    Id = perm.Id
                    ,
                    Name = perm.Name
                    ,
                    Description = perm.Description
                    ,
                    IsActive = perm.IsActive
                });
            }
            return permissionDtos;
        }
        return null;
    }
    
    /// <summary>
    /// Purpose: Fetches all inactive permissions.
    /// </summary>
    /// <returns></returns>
    public List<PermissionDto>? GetInActivePermissions()
    {
        List<Permission> permissions = [.. from permission in _context.Permissions
                                        where permission.IsActive == false
                                        select permission];

        if (permissions.Count > 0)
        {
            List<PermissionDto> permissionDtos = [];

            foreach (var perm in permissions)
            {
                permissionDtos.Add(new()
                {
                    Id = perm.Id
                    ,
                    Name = perm.Name
                    ,
                    Description = perm.Description
                    ,
                    IsActive = perm.IsActive
                });
            }
            return permissionDtos;
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches all active roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetActiveRoles()
    {
        List<Role> roles = [.. from role in _context.Roles
                            where role.IsActive == true
                            select role];

        if (roles.Count > 0)
        {
            List<RoleDto> rolesDto = [];

            foreach (var role in roles)
            {
                rolesDto.Add(new()
                {
                    Id = role.Id
                    ,
                    Name = role.Name
                    ,
                    IsActive = role.IsActive
                });
            }
            return rolesDto;
        }
        return null;
    }

    /// <summary>
    /// Purpose: Fetches all inactive roles.
    /// </summary>
    /// <returns></returns>
    public List<RoleDto>? GetInActiveRoles()
    {
        List<Role> roles = [.. from role in _context.Roles
                            where role.IsActive == false
                            select role];

        if (roles.Count > 0)
        {
            List<RoleDto> rolesDto = [];

            foreach (var role in roles)
            {
                rolesDto.Add(new()
                {
                    Id = role.Id
                    ,
                    Name = role.Name
                    ,
                    IsActive = role.IsActive
                });
            }
            return rolesDto;
        }
        return null;
    }

    /// <summary>
    /// Purpose: Activates or deactivates a role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public bool ToggleRoleStatus(int roleId)
    {
        if (_context.Roles.FirstOrDefault(r => r.Id == roleId) is Role role)
        {
            role.IsActive = !role.IsActive;
            _context.Update(role);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Purpose: Activates or deactivates a permission.
    /// </summary>
    /// <param name="permId"></param>
    /// <returns></returns>
    public bool TogglePermissionStatus(int permId)
    {
        if (_context.Permissions.FirstOrDefault(p => p.Id == permId) is Permission permission)
        {
            permission.IsActive = !permission.IsActive;
            _context.Update(permission);
            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Purpose: Adds a new role.
    /// </summary>
    /// <param name="newRole"></param>
    /// <returns></returns>
    public bool AddRole(RoleDto newRole)
    {
        if (!string.IsNullOrEmpty(newRole.Name) && _context.Roles.FirstOrDefault(r => r.Name == newRole.Name) == null)
        {
            _context.Roles.Add(new()
            {
                Id = 0
                ,
                Name = newRole.Name
                ,
                IsActive = true
                , Permissions = []
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Purpose: Adds a new permission.
    /// </summary>
    /// <param name="newPerm"></param>
    /// <returns></returns>
    public bool AddPermission(PermissionDto newPerm)
    {
        if (!string.IsNullOrEmpty(newPerm.Name) && _context.Permissions.FirstOrDefault(p => p.Name == newPerm.Name) == null)
        {
            _context.Permissions.Add(new()
            {
                Id = 0
                ,
                Name = newPerm.Name
                ,
                IsActive = true
                ,
                Description = newPerm.Description!
                ,
                Roles = []
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }

    /// <summary>
    /// Purpose: Assigns a permission to a role, enabling the role to perform some function in the
    /// system.
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    public bool AssignPermission(RoleDto role, PermissionDto permission)
    {
        if (_context.Roles.FirstOrDefault(r => r.Name == role.Name) is Role r &&
            _context.Permissions.FirstOrDefault(p => p.Name == permission.Name) is Permission p &&
            _context.RolePermissions.FirstOrDefault(rp => rp.RoleId == r.Id && rp.PermissionId == p.Id) == null)
        {
            _context.RolePermissions.Add(new()
            {
                RoleId = r.Id
                ,
                PermissionId = p.Id
                ,
                Permission = p
                ,
                Role = r
            });

            return _context.SaveChanges() > 0;
        }
        return false;
    }
    
    /// <summary>
    /// Fetches all permissions assigned to a user.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public List<PermissionDto>? GetAssignedPermissionsByUsername(string username)
    {
        if (!string.IsNullOrEmpty(username) && _context.Staff.FirstOrDefault(s => s.Username == username) is Staff staff)
        {
            List<RolePermission> rolePerms = [.. from perm in _context.RolePermissions
                                              where perm.RoleId == staff.RoleId
                                              select perm];

            List<PermissionDto> perms = [];

            foreach (RolePermission rolePerm in rolePerms)
            {
                PermissionDto perm = GetPermissionById(rolePerm.PermissionId)!;
                perms.Add(new()
                {
                    Id = perm.Id
                    ,
                    Name = perm.Name
                    ,
                    IsActive = perm.IsActive
                    ,
                    Description = perm.Description
                });
            }
            return perms;
        }
        return null;
    }
}