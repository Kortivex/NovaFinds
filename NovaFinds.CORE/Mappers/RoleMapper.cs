// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the RoleMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class RoleMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static RoleDto? ToDomain(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<RoleDto?> ToListDomain(IEnumerable<Role> roles)
        {
            var rolesDto = new List<RoleDto?>();
            foreach (var role in roles){ rolesDto.Add(ToDomain(role)); }

            return rolesDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static Role? ToDb(RoleDto role)
        {
            return new Role
            {
                Name = role.Name,
                NormalizedName = role.Name.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<Role?> ToListDb(IEnumerable<RoleDto> roles)
        {
            var rolesDb = new List<Role?>();
            foreach (var role in roles){ rolesDb.Add(ToDb(role)); }

            return rolesDb;
        }
    }
}