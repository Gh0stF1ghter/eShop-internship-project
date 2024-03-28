using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Tests.UnitTests.Mocks
{
    internal class UserManagerMock : Mock<UserManager<User>>
    {
        public UserManagerMock(IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            Object.UserValidators.Add(new UserValidator<User>());
            Object.PasswordValidators.Add(new PasswordValidator<User>());

            Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        }

        public void FindByEmail(User? user) =>
            Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

        public void FindByName(User? user) =>
            Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

        public void CheckPassword(bool result) =>
            Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(result);

        public void Create(IdentityResult result) =>
            Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(result);
    }
}