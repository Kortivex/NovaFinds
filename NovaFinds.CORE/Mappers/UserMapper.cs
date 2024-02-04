// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the UserMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class UserMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static UserDto? ToDomain(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                EmailConfirmed = user.EmailConfirmed,
                Nif = user.Nif,
                City = user.City,
                Country = user.Country,
                State = user.State,
                StreetAddress = user.StreetAddress,
                PhoneNumber = user.PhoneNumber!,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IsActive = user.IsActive
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<UserDto?> ToListDomain(IEnumerable<User> users)
        {
            var usersDto = new List<UserDto?>();
            foreach (var user in users){ usersDto.Add(ToDomain(user)); }

            return usersDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static User? ToDb(UserDto user)
        {
            return new User
            {
                UserName = user.UserName,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Nif = user.Nif,
                City = user.City,
                Country = user.Country,
                State = user.State,
                StreetAddress = user.StreetAddress,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                IsActive = user.IsActive
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<User?> ToListDb(IEnumerable<UserDto> users)
        {
            var usersDb = new List<User?>();
            foreach (var user in users){ usersDb.Add(ToDb(user)); }

            return usersDb;
        }
    }
}